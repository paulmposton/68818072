using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MvcWebRole1.Common
{
    public static class Generators
    {
        private const string AllowedChars = "ABCDEFGHJKLMNPQRTUVWXY2346789";
        private static readonly int AllowedCharsLength = AllowedChars.Length;

        private static readonly RNGCryptoServiceProvider RngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public static string GeneratePassCode(int size)
        {
            var token = new char[size];

            var data = new byte[size];
            RngCryptoServiceProvider.GetBytes(data);

            // map each random byte to a character, ensure only allowed characters are used
            for (var i = 0; i < size; ++i)
                token[i] = AllowedChars[data[i] % AllowedCharsLength];

            return new string(token);
        }
    }
    //  Hashing Utils
    class HashUtils
    {
        HashAlgorithm HashProvider;
        int SalthLength;

        private int UniqueKeyMaxSize;

        //private void HashUtils()
        //{
        //    bool isSuccess = Int32.TryParse(ConfigurationManager.AppSettings["UniqueKeyMaxSize"], out UniqueKeyMaxSize);
        //}

        /// <summary>
        /// The constructor takes a HashAlgorithm as a parameter.
        /// </summary>
        /// <param name="HashAlgorithm">
        /// A <see cref="HashAlgorithm"/> HashAlgorihm which is derived from HashAlgorithm. C# provides
        /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
        /// </param>

        public HashUtils(HashAlgorithm HashAlgorithm, int theSaltLength)
        {
            HashProvider = HashAlgorithm;
            SalthLength = theSaltLength;
        }

        /// <summary>
        /// Default constructor which initialises the SaltedHash with the SHA256Managed algorithm
        /// and a Salt of 4 bytes ( or 4*8 = 32 bits)
        /// </summary>

        public HashUtils()
            : this(new SHA256Managed(), 4)
        {
        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="Data">A byte array of the Data to Hash</param>
        /// <param name="Salt">A byte array of the Salt to add to the Hash</param>
        /// <returns>A byte array with the calculated hash</returns>

        private byte[] ComputeHash(byte[] Data, byte[] Salt)
        {
            // Allocate memory to store both the Data and Salt together
            byte[] DataAndSalt = new byte[Data.Length + SalthLength];

            // Copy both the data and salt into the new array
            Array.Copy(Data, DataAndSalt, Data.Length);
            Array.Copy(Salt, 0, DataAndSalt, Data.Length, SalthLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return HashProvider.ComputeHash(DataAndSalt);
        }

        /// <summary>
        /// Given a data block this routine returns both a Hash and a Salt
        /// </summary>
        /// <param name="Data">
        /// A <see cref="System.Byte"/>byte array containing the data from which to derive the salt
        /// </param>
        /// <param name="Hash">
        /// A <see cref="System.Byte"/>byte array which will contain the hash calculated
        /// </param>
        /// <param name="Salt">
        /// A <see cref="System.Byte"/>byte array which will contain the salt generated
        /// </param>

        public void GetHashAndSalt(byte[] Data, out byte[] Hash, out byte[] Salt)
        {
            // Allocate memory for the salt
            Salt = new byte[SalthLength];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(Salt);

            // Compute hash value of our data with the salt.
            Hash = ComputeHash(Data, Salt);
        }

        /// <summary>
        /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
        /// from the required byte arrays to strings. Both the Hash and Salt are returned as Base-64 encoded strings.
        /// </summary>
        /// <param name="Data">
        /// A <see cref="System.String"/> string containing the data to hash
        /// </param>
        /// <param name="Hash">
        /// A <see cref="System.String"/> base64 encoded string containing the generated hash
        /// </param>
        /// <param name="Salt">
        /// A <see cref="System.String"/> base64 encoded string containing the generated salt
        /// </param>

        public void GetHashAndSaltString(string Data, out string Hash, out string Salt)
        {
            byte[] HashOut;
            byte[] SaltOut;

            // Obtain the Hash and Salt for the given string
            GetHashAndSalt(Encoding.UTF8.GetBytes(Data), out HashOut, out SaltOut);

            // Transform the byte[] to Base-64 encoded strings
            Hash = Convert.ToBase64String(HashOut);
            Salt = Convert.ToBase64String(SaltOut);
        }

        /// <summary>
        /// This routine verifies whether the data generates the same hash as we had stored previously
        /// </summary>
        /// <param name="Data">The data to verify </param>
        /// <param name="Hash">The hash we had stored previously</param>
        /// <param name="Salt">The salt we had stored previously</param>
        /// <returns>True on a succesfull match</returns>

        public bool VerifyHash(byte[] Data, byte[] Hash, byte[] Salt)
        {
            byte[] NewHash = ComputeHash(Data, Salt);

            //  No easy array comparison in C# -- we do the legwork
            if (NewHash.Length != Hash.Length) return false;

            for (int Lp = 0; Lp < Hash.Length; Lp++)
                if (!Hash[Lp].Equals(NewHash[Lp]))
                    return false;

            return true;
        }

        /// <summary>
        /// This routine provides a wrapper around VerifyHash converting the strings containing the
        /// data, hash and salt into byte arrays before calling VerifyHash.
        /// </summary>
        /// <param name="Data">A UTF-8 encoded string containing the data to verify</param>
        /// <param name="Hash">A base-64 encoded string containing the previously stored hash</param>
        /// <param name="Salt">A base-64 encoded string containing the previously stored salt</param>
        /// <returns></returns>

        public bool VerifyHashString(string Data, string Hash, string Salt)
        {
            byte[] HashToVerify = Convert.FromBase64String(Hash);
            byte[] SaltToVerify = Convert.FromBase64String(Salt);
            byte[] DataToVerify = Encoding.UTF8.GetBytes(Data);
            return VerifyHash(DataToVerify, HashToVerify, SaltToVerify);
        }

        private string GetUniqueKey() 
        { 
             
            char[] chars = new char[62]; 
            string a; a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; 
            chars = a.ToCharArray();
            int size = UniqueKeyMaxSize; byte[] data = new byte[1]; 
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider(); 
            crypto.GetNonZeroBytes(data);
            size = UniqueKeyMaxSize; 
            data = new byte[size]; 
            crypto.GetNonZeroBytes(data); 
            StringBuilder result = new StringBuilder(size); 
            foreach (byte b in data) 
            { 
                result.Append(chars[b % (chars.Length - 1)]); 
            } 
            return result.ToString(); }
    }
    class TemplateUtils
    {
        
        /// <summary>
        /// Retrieves the contents of a file (your email template, for example).
        /// </summary>
        /// <param name="path">The path to the file on a local disk, or a remote URL (starting http:// or https://).</param>
        /// <returns></returns>
        public static string GetEmailTemplate(string path)
        {
            if (path.StartsWith("http://") || path.StartsWith("https://")) return GetEmailTemplate(new Uri(path));
            using (var sr = new StreamReader(path)) return sr.ReadToEnd();
        }
        public static string GetEmailTemplate(Uri remoteAddress)
        {
            var result = String.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(remoteAddress);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var sr = new StreamReader(response.GetResponseStream())) result = sr.ReadToEnd();
                }
            }
            catch
            {
                throw; // Technically not pretty; but in practice you would probably want to handle this yourself :)
            }
            return result;
        }        
        
        /// <summary>
        /// Sends an email using the parameters you provide, substituting tokens in a collection with values you supply.
        /// </summary>
        /// <param name="to">The receipient of the message.</param>
        /// <param name="from">The sender of the message.</param>
        /// <param name="cc">An array of CC addresses (optional).</param>
        /// <param name="bcc">An array of BCC addresses (optional).</param>
        /// <param name="subject">The subject of the email (also parsed for tokens).</param>
        /// <param name="replacements">A dictionary of replacements to be made.</param>
        /// <param name="body">The body of the message to be sent.</param>
        /// <param name="isBodyHtml">Indicates whether the content of the template is HTML (true) or plain-text (false) and is used to set the IsBodyHtml property of the MailMessage.</param>
        public static void Send(MailAddress to, MailAddress from, string subject, IEnumerable<KeyValuePair<string, string>> replacements, string body, bool isBodyHtml = true)
        {

            using (MailMessage message = new MailMessage())
            {
                // Assign message recipient
                message.To.Add(to);
                message.From = from;

                // Set body type
                message.IsBodyHtml = isBodyHtml;

                // Replace tokens in the message body and subject line
                foreach (KeyValuePair<string, string> token in replacements)
                {
                    body = body.Replace(token.Key, token.Value);
                    subject = subject.Replace(token.Key, token.Value);
                }

                // Assign message body and subject
                message.Body = body;
                message.Subject = subject;

                // Send the message
                System.Net.Mail.SmtpClient client = new SmtpClient("mail.authsmtp.com", 2525);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;

                // TODO: needs to use AuthSMTP
                client.Credentials = new NetworkCredential("ac61276", "bnhvcjb4dbuqyf");
                client.Send(message);
            }
        }
    }
    public static class Enum<T>
    {
        /// <summary>
        /// Get the custom Display attribute's Name parameter for an enum value.
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns>The display name for the enum value, or null if the enum value has no display attribute.</returns>
        public static string GetDisplayName(T value)
        {
            var type = typeof(T);
            var field = type.GetField(value.ToString());
            var displayAttributes = ((DisplayAttribute[])field.GetCustomAttributes(typeof(DisplayAttribute), false));

            return (displayAttributes.Length == 0)
              ? null
              : displayAttributes[0].Name;
        }


        /// <summary>
        /// Get an enumerable collection of an enum's value-display name pairs
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, string>> GetValueDisplayNameCollection()
        {
            var collection = new List<Tuple<int, string>>();
            foreach (var value in Enum.GetValues(typeof(T)))
                collection.Add(new Tuple<int, string>((int)value, GetDisplayName((T)value)));

            return collection;
        }
    }

}