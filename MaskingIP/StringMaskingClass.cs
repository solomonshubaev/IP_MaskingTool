using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MaskingIP
{
    /// <summary>
    /// Masking 
    /// </summary>
    public class StringMaskingClass : IStringMasking
    {

        private string text;
        private MaskIp maskIp;

        public StringMaskingClass(string text)
        {
            this.text = text;
            this.maskIp = new MaskIp();
        }

        public string Text { get => text; set => text = value; }

        
        public void ReplaceMatchingStringInString(string regexPattern, string newString = "")
        {
            if (this.text == string.Empty)
            {
                Console.WriteLine("File text is empty.");
                return;
            }
            Regex regex = new Regex(regexPattern);
            StringBuilder stringBuilder = new StringBuilder(this.text);
            if (regex.IsMatch(this.text))
            {
                MatchCollection matchCollection = regex.Matches(this.text);
                foreach (Match match in matchCollection)
                {

                    string matchValue = match.Value;
                    if (IsIpValid(matchValue)) //check if match is a valid ip
                    {
                        string maskedIp = this.maskIp.GetMaskedIp(matchValue);
                        maskedIp = matchValue[0] + maskedIp + matchValue[matchValue.Length - 1]; //string to keep everything as it was
                        stringBuilder.Replace(match.Value, maskedIp);
                        this.text = stringBuilder.ToString();
                    }
                }
                Console.WriteLine("IP addresses replaced");
            }
        }

        /// <summary>
        /// Checks if the ip address is valid
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IsIpValid(string ip)
        {
            string[] parts = ip.Split('.');
            int valueOfPart; // integer value of string part
            foreach (string part in parts)
            {
                valueOfPart = int.Parse(part);
                if (valueOfPart > 255)
                    return false;
            }
            return true;
        }
    }
}
