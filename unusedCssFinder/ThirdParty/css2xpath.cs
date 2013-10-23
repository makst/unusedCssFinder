/*
 * C# port of css2xpath (JavaScript)
 * 
 * Version 1.0
 * 
 * Original version by Andrea Giammarchi (http://webreflection.blogspot.com/2009/04/vice-versa-sub-project-css2xpath.html)
 * Original covered under 'MIT License'
 * 
 * [License for this port]:
 * 
 * The MIT License

    Copyright (c) MostThingsWeb (http://mosttw.wordpress.com/)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThirdParty.MostThingsWeb
{

    /// <summary>
    /// A static utility class for transforming CSS selectors to XPath selectors.
    /// </summary>
    public static class css2xpath
    {

        private static List<Regex> patterns;
        private static List<Object> replacements;

        static css2xpath()
        {
            // Initalize list of patterns and replacements
            patterns = new List<Regex>();
            replacements = new List<Object>();

            // Generate all the rules

            // Attributes
            AddRule(new Regex(@"\[([^\]~\$\*\^\|\!]+)(=[^\]]+)?\]"), "[@$1$2]");

            // Multiple queries
            AddRule(new Regex(@"\s*,\s*"), "|");

            // Remove space around +, ~, and >
            AddRule(new Regex(@"\s*(\+|~|>)\s*"), "$1");

            //Handle *, ~, +, and >
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*])~([a-zA-Z0-9_\-\*])"), "$1/following-sibling::$2");
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*])\+([a-zA-Z0-9_\-\*])"), "$1/following-sibling::*[1]/self::$2");
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*])>([a-zA-Z0-9_\-\*])"), "$1/$2");

            // Escaping
            AddRule(new Regex(@"\[([^=]+)=([^'|" + "\"" + @"][^\]]*)\]"), "[$1='$2']");

            // All descendant or self to //
            AddRule(new Regex(@"(^|[^a-zA-Z0-9_\-\*])(#|\.)([a-zA-Z0-9_\-]+)"), "$1*$2$3");
            AddRule(new Regex(@"([\>\+\|\~\,\s])([a-zA-Z\*]+)"), "$1//$2");
            AddRule(new Regex(@"\s+\/\/"), "//");

            // Handle :first-child
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):first-child"), "*[1]/self::$1");

            // Handle :last-child
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):last-child"), "$1[not(following-sibling::*)]");

            // Handle :only-child
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):only-child"), "*[last()=1]/self::$1");

            // Handle :empty
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):empty"), "$1[not(*) and not(normalize-space())]");

            // Handle :not
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):not\(([^\)]*)\)"), new MatchEvaluator((Match m) =>
            {
                return m.Groups[1].Value + "[not(" + (new Regex("^[^\\[]+\\[([^\\]]*)\\].*$")).Replace(Transform(m.Groups[2].Value), "$1") + ")]";
            }));

            // Handle :nth-child
            AddRule(new Regex(@"([a-zA-Z0-9_\-\*]+):nth-child\(([^\)]*)\)"), new MatchEvaluator((Match m) =>
            {
                String b = m.Groups[2].Value;
                String a = m.Groups[1].Value;

                switch (b)
                {
                    case "n":
                        return a;
                    case "even":
                        return "*[position() mod 2=0 and position()>=0]/self::" + a;
                    case "odd":
                        return a + "[(count(preceding-sibling::*) + 1) mod 2=1]";
                    default:
                        // Parse out the 'n'
                        b = ((new Regex("^([0-9])*n.*?([0-9])*$")).Replace(b, "$1+$2"));

                        // Explode on + (i.e 'nth-child(2n+0)' )
                        String[] b2 = new String[2];
                        String[] splitResult = b.Split('+');

                        // The first component will always be a number
                        b2[0] = splitResult[0];

                        int buffer = 0;

                        // The second component might be missing
                        if (splitResult.Length == 2)
                            if (!int.TryParse(splitResult[1], out buffer))
                                buffer = 0;

                        b2[1] = buffer.ToString();

                        return "*[(position()-" + b2[1] + ") mod " + b2[0] + "=0 and position()>=" + b2[1] + "]/self::" + a;
                }
            }));

            // Handle :contains
            AddRule(new Regex(@":contains\(([^\)]*)\)"), new MatchEvaluator((Match m) =>
            {
                return "[contains(string(.),'" + m.Groups[1].Value + "')]";
            }));

            // != attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)\|=([^\]]+)\]"), "[@$1=$2 or starts-with(@$1,concat($2,'-'))]");

            // *= attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)\*=([^\]]+)\]"), "[contains(@$1,$2)]");

            // ~= attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)~=([^\]]+)\]"), "[contains(concat(' ',normalize-space(@$1),' '),concat(' ',$2,' '))]");

            // ^= attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)\^=([^\]]+)\]"), "[starts-with(@$1,$2)]");

            // $= attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)\$=([^\]]+)\]"), new MatchEvaluator((Match m) =>
            {
                String a = m.Groups[1].Value;
                String b = m.Groups[2].Value;
                return "[substring(@" + a + ",string-length(@" + a + ")-" + (b.Length - 3) + ")=" + b + "]";
            }));

            // != attribute
            AddRule(new Regex(@"\[([a-zA-Z0-9_\-]+)\!=([^\]]+)\]"), "[not(@$1) or @$1!=$2]");

            // ID and class
            AddRule(new Regex(@"#([a-zA-Z0-9_\-]+)"), "[@id='$1']");
            AddRule(new Regex(@"\.([a-zA-Z0-9_\-]+)"), "[contains(concat(' ',normalize-space(@class),' '),' $1 ')]");

            // Normalize filters
            AddRule(new Regex(@"\]\[([^\]]+)"), " and ($1)");
        }

        /// <summary>
        /// Adds a rule for transforming CSS to XPath.
        /// </summary>
        /// <param name="regex">A Regex for the parts of the CSS you want to transform.</param>
        /// <param name="replacement">A MatchEvaluator for converting the matched CSS parts to XPath.</param>
        /// <exception cref="ArgumentException">Thrown if regex or replacement is null.</exception>
        /// <example>
        /// <code>
        /// // Handle :contains selectors
        /// AddRule(new Regex(@":contains\(([^\)]*)\)"), new MatchEvaluator((Match m) => {
        ///     return "[contains(string(.),'" + m.Groups[1].Value + "')]";
        /// }));
        /// 
        /// // Note: Remember that m.Groups[1] refers to the first captured group; m.Groups[0] refers
        /// // to the entire match.
        /// </code>
        /// </example>
        public static void AddRule(Regex regex, MatchEvaluator replacement)
        {
            _AddRule(regex, replacement);
        }

        /// <summary>
        /// Adds a rule for transforming CSS to XPath.
        /// </summary>
        /// <param name="regex">A Regex for the parts of the CSS you want to transform.</param>
        /// <param name="replacement">A String for converting the matched CSS parts to XPath.</param>
        /// <exception cref="ArgumentException">Thrown if regex or replacement is null.</exception>
        /// <example>
        /// <code>
        /// // Replace commas (denotes multiple queries) with pipes (|)
        /// AddRule(new Regex(@"\s*,\s*"), "|");
        /// </code>
        /// </example>
        public static void AddRule(Regex regex, String replacement)
        {
            _AddRule(regex, replacement);
        }

        /// <summary>
        /// Adds a rule for transforming CSS to XPath. For internal use only.
        /// </summary>
        /// <param name="regex">A Regex for the parts of the CSS you want to transform.</param>
        /// <param name="replacement">A String or MatchEvaluator for converting the matched CSS parts to XPath.</param>
        /// <exception cref="ArgumentException">Thrown if regex or replacement is null, or if the replacement is neither a String nor a MatchEvaluator.</exception>
        private static void _AddRule(Regex regex, Object replacement)
        {
            if (regex == null)
                throw new ArgumentException("Must supply non-null Regex.", "regex");

            if (replacement == null || (!(replacement is String) && !(replacement is MatchEvaluator)))
                throw new ArgumentException("Must supply non-null replacement (either String or MatchEvaluator).", "replacement");

            patterns.Add(regex);
            replacements.Add(replacement);
        }

        /// <summary>
        /// Transforms the given CSS selector to an XPath selector.
        /// </summary>
        /// <param name="css">The CSS selector to transform into an XPath selector.</param>
        /// <returns>The resultant XPath selector.</returns>
        public static String Transform(String css)
        {
            int len = patterns.Count;

            for (int i = 0; i < len; i++)
            {
                Regex pattern = patterns[i];
                Object replacement = replacements[i];

                // Depending on what the replacement is, we need to cast it to either a String or a MatchEvaluator
                if (replacement is String)
                    css = pattern.Replace(css, (String)replacement);
                else
                    css = pattern.Replace(css, (MatchEvaluator)replacement);
            }

            return "//" + css;
        }

        /// <summary>
        /// Forces the CSS to XPath rules to be created. Not neccesary; the rules are created the first time Transform is called.
        /// </summary>
        /// <remarks>
        /// Perhaps you would want to use this in the initalization procedure of your application.
        /// </remarks>
        public static void PreloadRules()
        {
            /* Empty by design:
             * 
             * The static class initializer will be called the first time any static method, such
             * as this one, is called
             */
        }
    }
}