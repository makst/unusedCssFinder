﻿using UnusedCssFinder.CssData;

namespace UnusedCssFinder.CssData
{
    public class ElementData
    {
        public string Value { get; set; }
        public Specificity Specificity { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}