using System.Linq;
using System.Security.Cryptography;
using System;
using System.Text;

namespace Bettery.Kiosk.Common
{
    /// <summary>
    /// Class Utils
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// To the upper string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToUpperString(object value)
        {
            if (value != null)
            {
                return value.ToString().ToUpper();
            }

            return string.Empty;
        }

        /// <summary>
        /// To the lower string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToLowerString(object value)
        {
            if (value != null)
            {
                return value.ToString().ToLower();
            }

            return string.Empty;
        }

        /// <summary>
        /// Mins the specified number items.
        /// </summary>
        /// <param name="numberItems">The number items.</param>
        /// <returns>The smallest number</returns>
        public static int Min(params int[] numberItems)
        {
            return numberItems.Min();
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }

    //  Hashing Utils
    class HashUtils
    {
        HashAlgorithm HashProvider;
        int SaltLength;

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
            SaltLength = theSaltLength;
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
            byte[] DataAndSalt = new byte[Data.Length + SaltLength];

            // Copy both the data and salt into the new array
            Array.Copy(Data, DataAndSalt, Data.Length);
            Array.Copy(Salt, 0, DataAndSalt, Data.Length, SaltLength);

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
            Salt = new byte[SaltLength];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(Salt);

            // Compute hash value of our data with the salt.
            Hash = ComputeHash(Data, Salt);
        }

        public void GetHash(byte[] Data, out byte[] Hash)
        {
            
            // Allocate memory for the salt
            byte[] Salt = new byte[SaltLength];

            Salt = GetBytes("0djk3ljd");

            // Compute hash value of our data with the salt.
            Hash = ComputeHash(Data, Salt);
        }


        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
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

        public void GetHashString(string Data, out string Hash)
        {
            byte[] HashOut;

            // Obtain the Hash and Salt for the given string
            GetHash(Encoding.UTF8.GetBytes(Data), out HashOut);

            // Transform the byte[] to Base-64 encoded strings
            Hash = Convert.ToBase64String(HashOut);
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

    }
}