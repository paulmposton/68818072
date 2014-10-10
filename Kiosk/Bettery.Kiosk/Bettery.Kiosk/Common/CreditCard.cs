using System.Text.RegularExpressions;

namespace Bettery.Kiosk.Common
{
    /// <summary>
    /// Class Creadit Card
    /// </summary>
    public class CreditCard
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the exp date.
        /// </summary>
        /// <value>
        /// The exp date.
        /// </value>
        public string ExpDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="cardReaderData">The card reader data.</param>
        public CreditCard(string cardReaderData)
        {
            bool caretPresent = cardReaderData.Contains("^");
            bool equalPresent = cardReaderData.Contains("=");

            if (caretPresent)
            {
                string[] cardData = cardReaderData.Split('^');
                //B1234123412341234^CardUser/John^030510100000019301000000877000000? 

                Name = FormatName(cardData[1]);
                Number = FormatCardNumber(cardData[0]);
                ExpDate = cardData[2].Substring(2, 2) + cardData[2].Substring(0, 2);
            }
            else if (equalPresent)
            {
                string[] cardData = cardReaderData.Split('=');
                //1234123412341234=0305101193010877? 

                Number = FormatCardNumber(cardData[0]);
                ExpDate = cardData[1].Substring(2, 2) + cardData[1].Substring(0, 2);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="cardReaderData">The card reader data.</param>
        /// <param name="expireDateFormat">The expire date format.</param>
        public CreditCard(string cardReaderData, ExpireDateFormat expireDateFormat)
        {
            bool caretPresent = cardReaderData.Contains("^");
            bool equalPresent = cardReaderData.Contains("=");

            if (caretPresent)
            {
                string[] cardData = cardReaderData.Split('^');
                //B1234123412341234^CardUser/John^030510100000019301000000877000000? 

                Name = FormatName(cardData[1]);
                Number = FormatCardNumber(cardData[0]);
                //ExpDate = cardData[2].Substring(2, 2) + cardData[2].Substring(0, 2);
                if (expireDateFormat == ExpireDateFormat.MMYY)
                {
                    ExpDate = cardData[2].Substring(2, 2) + cardData[2].Substring(0, 2);
                }
                else if (expireDateFormat == ExpireDateFormat.YYYY_MM)
                {
                    ExpDate = "20" + cardData[2].Substring(0, 2) + "-" + cardData[2].Substring(2, 2);
                }
            }
            else if (equalPresent)
            {
                string[] cardData = cardReaderData.Split('=');
                //1234123412341234=0305101193010877? 

                Number = FormatCardNumber(cardData[0]);
                if (expireDateFormat == ExpireDateFormat.MMYY)
                {
                    ExpDate = cardData[1].Substring(2, 2) + cardData[1].Substring(0, 2);
                }
                else if (expireDateFormat == ExpireDateFormat.YYYY_MM)
                {
                    ExpDate =  "20" + cardData[1].Substring(0, 2) + "-" + cardData[1].Substring(2, 2);
                }
            }
        }

        /// <summary>
        /// Formats the name.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        private string FormatName(string o)
        {
            string result = string.Empty;

            if (o.Contains("/"))
            {
                string[] nameSplit = o.Split('/');

                result = nameSplit[1] + " " + nameSplit[0];
            }
            else
            {
                result = o;
            }

            return result;
        }

        /// <summary>
        /// Formats the card number.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        private string FormatCardNumber(string o)
        {
            string result = string.Empty;
            result = Regex.Replace(o, "[^0-9]", string.Empty);

            return result;
        }

        /// <summary>
        /// Expire Date Format
        /// </summary>
        public enum ExpireDateFormat
        {
            MMYY = 0,
            //YYYY-MM
            YYYY_MM = 1
        }
    }
}
