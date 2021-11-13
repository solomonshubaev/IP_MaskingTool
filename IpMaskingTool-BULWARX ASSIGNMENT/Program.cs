using System;

namespace IpMaskingTool_BULWARX_ASSIGNMENT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Your file input(Default type is .txt): ");
            string userInput = Console.ReadLine();
            FileManager fileManager = new FileManager(userInput);
            fileManager.CreateNewFileAfterIpMask();

        }
    }
}
