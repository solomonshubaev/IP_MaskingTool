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

        private string readedText;
        private string filePath;
        private int maxFileLength;
        public FileManager(string filePath, int maxFileLength = 5000000)
        {
            this.maxFileLength = maxFileLength;
            this.readedText = "";
            this.filePath = filePath;
            ReadFromFile();
        }

        

        public void ReadFromFile()
        {
            try
            {
                this.filePath = RemoveApostrophes(this.filePath);
                if (File.Exists(this.filePath))
                {
                    if (IsFileSizeValid() && IsFileExtensionValid()) //check if input is valid
                        this.readedText = File.ReadAllText(this.filePath);
                }
                else
                    Console.WriteLine("File not found");
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
                StringMaskingClass stringMasking = new StringMaskingClass(readedText);
                stringMasking.ReplaceMatchingStringInString(Definitions.IP4_REGEX);
                string outputPath = GetOutputPathString(this.filePath);
                File.WriteAllText(outputPath, stringMasking.Text);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override string ToString()
        {
            return this.readedText;
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

        /// <summary>
        /// Replacing input file name to output file name
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="outputFileName">File's new name</param>
        /// <param name="combineToOriginalFileName">Combine the name and add it to input file's name</param>
        /// <returns>New path with new file string</returns>
        private string GetOutputPathString(string fullPath
            ,string outputFileName = "OUTPUT",bool combineToOriginalFileName = false)
        {
            try
            {
                string path;
                if (combineToOriginalFileName)
                {
                    path = Path.GetFileNameWithoutExtension(fullPath);
                }
                else
                {
                    path = Directory.GetParent(fullPath).ToString();
                }

                path = Path.Combine(path, outputFileName);
                return Path.ChangeExtension(path, ".log");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            throw new Exception("ERROR IN GetOutputPathString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True if string between " "</returns>
        private bool CheckIfStringInsideApostrophes(string str)
        {
            return str[0] == '"' || str[str.Length - 1] == '"';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>New string without ""</returns>
        private string RemoveApostrophes(string str)
        {
            if (CheckIfStringInsideApostrophes(str))
                //str = str.Trim('"');
                str = str.Substring(1,str.Length-2);
            return str;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if file length is smaller or equal to max length</returns>
        private bool IsFileSizeValid()
        {
            long fileSize = new FileInfo(this.filePath).Length;
            if (fileSize <= this.maxFileLength)
                return true;
            throw new Exception("File size is too big. max size is " + this.maxFileLength + " (Bytes)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if extension is *.log</returns>
        private bool IsFileExtensionValid()
        {
            if (Path.GetExtension(this.filePath) == ".log")
                return true;
            throw new Exception("Invalid file input: only file with extension .log are valid.");
        }
    }
}
