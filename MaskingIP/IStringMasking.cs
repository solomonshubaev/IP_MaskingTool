using System;
using System.Collections.Generic;
using System.Text;

namespace MaskingIP
{
    public interface IStringMasking
    {
        /// <summary>
        /// Replacing all matches regex with new string
        /// </summary>
        /// <param name="regex">regex pattern</param>
        /// <param name="newString">new string to replace with - optional</param>
        public void ReplaceMatchingStringInString(string regexPattern, string newString);

    }
}
