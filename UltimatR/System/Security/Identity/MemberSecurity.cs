
// <copyright file="MemberSecurity.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{



    /// <summary>
    /// Class MemberSecurity.
    /// Implements the <see cref="System.IMemberSecurity" />
    /// </summary>
    /// <seealso cref="System.IMemberSecurity" />
    public abstract class MemberSecurity : IMemberSecurity
    {
        #region Methods






        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>System.String.</returns>
        public virtual string CreateToken(MemberIdentity member)
        {
            string token = null;
            string key = member.Key;
            string timesalt = Convert.ToBase64String(DateTime.Now.Ticks.ToString().ToBytes(CharEncoding.ASCII));
            token = CryptoHash.Encrypt(key, timesalt);
            member.Token = token;
            DateTime time = DateTime.Now;
            member.RegisterTime = time;
            member.LifeTime = time.AddMinutes(30);
            member.LastAction = time;
            return token;
        }






        /// <summary>
        /// Gets the by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>MemberIdentity.</returns>
        public abstract MemberIdentity GetByToken(string token);






        /// <summary>
        /// Gets the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberIdentity.</returns>
        public abstract MemberIdentity GetByUserId(string userId);







        /// <summary>
        /// Registers the specified member identity.
        /// </summary>
        /// <param name="memberIdentity">The member identity.</param>
        /// <param name="encoded">if set to <c>true</c> [encoded].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Register(MemberIdentity memberIdentity, bool encoded = false);









        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="di">The di.</param>
        /// <param name="ip">The ip.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Register(string name, string key, out MemberIdentity di, string ip = "");








        /// <summary>
        /// Registers the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="di">The di.</param>
        /// <param name="ip">The ip.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Register(string token, out MemberIdentity di, string ip = "");







        /// <summary>
        /// Verifies the identity.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="checkPasswd">The check passwd.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool VerifyIdentity(MemberIdentity member, string checkPasswd)
        {
            bool verify = false;

            string hashpasswd = member.Key;
            string saltpasswd = member.Salt;
            verify = CryptoHash.Verify(hashpasswd, saltpasswd, checkPasswd);

            return verify;
        }







        /// <summary>
        /// Verifies the token.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="checkToken">The check token.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool VerifyToken(MemberIdentity member, string checkToken)
        {
            bool verify = false;

            string token = member.Token;

            if (checkToken.Equals(token))
            {
                DateTime time = DateTime.Now;
                DateTime registerTime = member.RegisterTime;
                DateTime lastAction = member.LastAction;
                DateTime lifeTime = member.LifeTime;
                if (lifeTime > time)
                    verify = true;
                else if (lastAction > time.AddMinutes(-30))
                {
                    member.LifeTime = time.AddMinutes(30);
                    member.LastAction = time;
                    verify = true;
                }
            }
            return verify;
        }

        #endregion
    }
}
