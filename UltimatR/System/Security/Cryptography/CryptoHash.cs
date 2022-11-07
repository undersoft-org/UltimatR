﻿
// <copyright file="CryptoHash.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Extract;
    using System.Security.Cryptography;
    using System.Text;
    using System.Uniques;




    /// <summary>
    /// Class CryptoHash.
    /// </summary>
    public static class CryptoHash
    {
        /// <summary>
        /// The algorithm
        /// </summary>
        private static readonly KeyedHashAlgorithm ALGORITHM = new HMACSHA512();

        #region Methods








        /// <summary>
        /// Encrypts the specified pass.
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt(string pass, string salt)
        {

            byte[] bIn = pass.GetBytes();
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            KeyedHashAlgorithm kha = ALGORITHM;

            if(kha.Key.Length == bSalt.Length)
            {
                kha.Key = bSalt;
            }
            else if(kha.Key.Length < bSalt.Length)
            {
                byte[] bKey = new byte[kha.Key.Length];
                bKey.CopyBlock(bSalt, (uint)bKey.Length);
                kha.Key = bKey;
            }
            else
            {
                byte[] bKey = new byte[kha.Key.Length];
                for(int iter = 0; iter < bKey.Length;)
                {
                    int len = Math.Min(bSalt.Length, bKey.Length - iter);
                    bKey.CopyBlock(bSalt, (uint)iter, (uint)bKey.Length);
                    iter += len;
                }

                kha.Key = bKey;
            }

            bRet = kha.ComputeHash(bIn);

            return Convert.ToBase64String(bRet);
        }





        /// <summary>
        /// Salts this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string Salt()
        {
            return Convert.ToBase64String(Unique.New.GetBytes());
        }








        /// <summary>
        /// Verifies the specified hashed password.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="hashedSalt">The hashed salt.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Verify(string hashedPassword, string hashedSalt, string providedPassword)
        {
            string salt = hashedSalt;
            if (String.Equals(Encrypt(providedPassword, salt), hashedPassword, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        #endregion
    }
}
