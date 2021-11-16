using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MaskingIP
{
    public class MaskIp
    {
        private int randomKey;
        private Dictionary<string, string> ipDictionary;
        
        public MaskIp()
        {
            this.ipDictionary = new Dictionary<string, string>();
        }

        public string GetMaskedIp(string ipAddress)
        {            
            return this.BuildIpAddress(ipAddress);
        }

        /// <summary>
        /// Helping method that help us building ip address
        /// </summary>
        /// <param name="partsOfIpAddress"></param>
        /// <returns></returns>
        private string BuildIpAddress(string ipAddress)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            string networkAddress = GetNetworkAddress(ipAddress);
            string maskedNetworkAddress;
            if (CheckIfIpIsAlreadyMasked(networkAddress))
            {
                maskedNetworkAddress = this.ipDictionary[networkAddress];
            }
            else // need to mask network address
            {
                maskedNetworkAddress = CalculateMaskedIp(networkAddress);
                this.ipDictionary[networkAddress] = maskedNetworkAddress;
            }
            stringBuilder.Append(maskedNetworkAddress); // appeand the network mask
            string hostAddressTypeC = ipAddress.Split('.')[3]; // the line is very specific to our project!
            stringBuilder.Append(".");
            stringBuilder.Append(CalculateMaskedIp(hostAddressTypeC));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>String of masked IP</returns>
        private string CalculateMaskedIp(string ip)
        {

            return BuildStringWithBuffer(ip, '.', true);            
        }

        /// <summary>
        /// Building new string from string's part with a buffer between parts
        /// </summary>
        /// <param name="buffer">char between parts</param>
        /// <param name="toMask">If true the returned IP will be masked</param>
        /// <returns></returns>
        private string BuildStringWithBuffer(string ip, char bufferChar, bool toMask = false)
        {
            string[] parts = ip.Split('.');
            StringBuilder stringBuilder = new StringBuilder(String.Empty);

            for (int i = 0; i < parts.Length; i++)
            {
                if (this.CheckIfAddBufferToString(i))
                    stringBuilder.Append(bufferChar.ToString());
                if(toMask)// check if need to mask IP
                {
                    Random random = new Random();
                    //calculate new masked value
                    parts[i] = ((int.Parse(parts[i]) + random.Next(0, 256)) % 256).ToString();
                }
                stringBuilder.Append(CalculateRandom(int.Parse(parts[i])).ToString());                
            }
            return stringBuilder.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullIpAddress"></param>
        /// <param name="networkClass">Network class - default values is 2 (class C)</param>
        /// <returns>String that represent network address</returns>
        public string GetNetworkAddress(string fullIpAddress,int networkClass = 2)
        {
            string[] ipParts = fullIpAddress.Split('.');
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            for (int i = 0; i <= networkClass; i++)
            {
                if (this.CheckIfAddBufferToString(i))
                    stringBuilder.Append(".");
                stringBuilder.Append(ipParts[i]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkAddress"></param>
        /// <returns>True if networkAddress already masked in the dictionary</returns>
        public bool CheckIfIpIsAlreadyMasked(string networkAddress)
        {
            return (this.ipDictionary.ContainsKey(networkAddress));
        }

        /// <summary>
        /// For adding '.' in Ip address building
        /// </summary>
        /// <param name="partIndex">the buffer will be adding after the first part of string</param>
        /// <returns></returns>
        private bool CheckIfAddBufferToString(int partIndex)
        {
            return partIndex > 0;
        }

        private int CalculateRandom(int number)
        {
            return (number + this.randomKey) % 256;
        }
    }
}
