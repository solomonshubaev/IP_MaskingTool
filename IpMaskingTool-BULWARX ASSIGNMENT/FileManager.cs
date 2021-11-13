using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MaskingIP;

namespace IpMaskingTool_BULWARX_ASSIGNMENT
{
    class FileManager
    {
        private const string FILE_TYPE_TEXT = ".txt";
        private const string OUTPUT_FILE_NAME = "outputFile" + FILE_TYPE_TEXT;

        private readonly string inputFileName;
        private string readedText;
        private string filePath;
        private MaskIp maskIp;
        public FileManager(string inputFileName)
        {
            this.readedText = "";
            string currentPath = Directory.GetCurrentDirectory();
            this.filePath = Directory.GetParent(currentPath).Parent.Parent.FullName + "/";
            this.inputFileName = inputFileName;
            this.maskIp = new MaskIp();
            try
            {
                if (!this.IsFileNameHasType())
                    this.inputFileName += FILE_TYPE_TEXT; // add .txt to file name
                ReadFromFile();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        

        public void ReadFromFile()
        {
            StringBuilder stringBuilder = new StringBuilder(this.filePath);
            stringBuilder.Append(this.inputFileName);
            string finalFilePath = stringBuilder.ToString();
            try
            {
                if (File.Exists(finalFilePath))
                    this.readedText = File.ReadAllText(finalFilePath); 
                else
                    Console.WriteLine("File not found");
            }
            catch(PathTooLongException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CreateNewFileAfterIpMask()
        {
            try
            {
                ReplaceIpInString(Definitions.IP4_REGEX);
                File.WriteAllText(this.filePath + OUTPUT_FILE_NAME, this.readedText);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Replacing valid IPs in the log file
        /// </summary>
        /// <param name="regexPattern"></param>
        public void ReplaceIpInString(string regexPattern)
        {
            Regex regex = new Regex(regexPattern);
            StringBuilder stringBuilder = new StringBuilder(this.readedText);
            if (regex.IsMatch(this.readedText))
            {
                MatchCollection matchCollection = regex.Matches(this.readedText);
                foreach (Match match in matchCollection)
                {
                    if (this.IsValidIP(match.Value)) //check if matched ip is valid
                    {
                        string maskedIp = this.maskIp.CalculateMaskedIP(match.Value);
                        stringBuilder.Replace(match.Value, maskedIp);
                        this.readedText = stringBuilder.ToString();
                    }
                }
                Console.WriteLine("IP addresses replaced");
            }
        }

        public override string ToString()
        {
            return this.readedText;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if file is valid</returns>
        private bool IsFileNameHasType()
        {
            string[] fileParts = this.inputFileName.Split('.');
            if (this.GetFileNamePartsAmount(fileParts.Length) == 2) //(name | type)
                return true;
            else if (this.GetFileNamePartsAmount(fileParts.Length) == 1)
                return false;
            else
                throw new Exception("Invalid File Input");
        }

        private int GetFileNamePartsAmount(int lengthOfArr)
        {
            return lengthOfArr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>True if the ip is valid</returns>
        private bool IsValidIP(string ip)
        {
            string[] ipParts = ip.Split('.');
            foreach(string part in ipParts)
            {
                if (this.IsStringStartWithZero(part))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True if string start with "0" and no more characters after it</returns>
        private bool IsStringStartWithZero(string str)
        {
            if (str == string.Empty)
                throw new Exception("str is empty");
            return str[0] == '0' && str.Length > 1;
        }
    }
}
