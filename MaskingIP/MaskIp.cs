using System;
using System.Text;

namespace MaskingIP
{
    public class MaskIp
    {
        private int randomKey;
        
        public MaskIp()
        {
            Random rnd = new Random();
            this.randomKey = rnd.Next(1, 256);
        }

        public string CalculateMaskedIP(string ipAddress)
        {
            string[] partsOfIpAddress = ipAddress.Split('.');
            
            return this.BuildIpAddress(partsOfIpAddress);
        }

        /// <summary>
        /// Helping method that help us building ip address
        /// </summary>
        /// <param name="partsOfIpAddress"></param>
        /// <returns></returns>
        private string BuildIpAddress(string[] partsOfIpAddress)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);

            for (int i = 0; i < partsOfIpAddress.Length; i++)
            {
                if (this.AddDotToIP(i))
                    stringBuilder.Append(".");
                stringBuilder.Append(CalculateRandom(int.Parse(partsOfIpAddress[i])).ToString());
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// For adding '.' in Ip address building
        /// </summary>
        /// <param name="partIndex"></param>
        /// <returns></returns>
        private bool AddDotToIP(int partIndex)
        {
            return partIndex > 0;
        }

        private int CalculateRandom(int number)
        {
            return (number + this.randomKey) % 256;
        }
    }
}
