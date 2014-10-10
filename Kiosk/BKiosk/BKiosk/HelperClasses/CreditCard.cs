using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BKiosk.HelperClasses
{
    class CreditCard
    {
        public string ccName { get; set; }
        public string ccNumber { get; set; }
        public string ccExpDate { get; set;}
        public CreditCard(string CardReaderData)
        {
            bool CaretPresent = false;
            bool EqualPresent = false;
            try
            {
                CaretPresent = CardReaderData.Contains("^");
                EqualPresent = CardReaderData.Contains("=");

                if (CaretPresent)
                {
                    string[] CardData = CardReaderData.Split('^');
                    //B1234123412341234^CardUser/John^030510100000019301000000877000000? 

                    ccName = FormatName(CardData[1]);
                    ccNumber = FormatCardNumber(CardData[0]);
                    ccExpDate = CardData[2].Substring(2, 2) + CardData[2].Substring(0, 2);
                }
                else if (EqualPresent)
                {
                    string[] CardData = CardReaderData.Split('=');
                    //1234123412341234=0305101193010877? 

                    ccNumber = FormatCardNumber(CardData[0]);
                    ccExpDate = CardData[1].Substring(2, 2) + CardData[1].Substring(0, 2);
                } 

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw;                
            }
            
         
        }
        private string FormatName(string o)
        {
            string result = string.Empty;
            try
            {
                if (o.Contains("/"))
                {
                    string[] NameSplit = o.Split('/');

                    result = NameSplit[1] + " " + NameSplit[0];
                }
                else
                {
                    result = o;
                }
                return result;

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw; 
            }
        }
        private string FormatCardNumber(string o)
        {
            string result = string.Empty;
            try
            {
                result = Regex.Replace(o, "[^0-9]", string.Empty);
                return result;

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw; 
            }
        }
    }
}
