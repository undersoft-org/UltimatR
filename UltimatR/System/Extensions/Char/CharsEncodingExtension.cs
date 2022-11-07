// ***********************************************************************
// Assembly         : UltimatR.Core
// Authors          : darisuz.hanc < undersoft.org >
// Participants
// Patronate        : m.krzetowski (architect), k.reszka (team-leader)
// Contribution     : d.hanc (r&d.soft.developer), p.grys (senior.soft.engineer)
// Development      : p.gasowski (jr.soft.developer)
// Business         : k.golos (po) m.rafalski (pm), m.korzeniewski (analyst) 
// QA               : a.urbanek
// DevOps           : k.manikowski        
// Created          : 02-05-2022
//
// Last Modified By : dariusz.hanc
// Last Modified On : 02-07-2022
// ***********************************************************************
// <copyright file="CharsEncodingExtension.cs" company="UltimatR.Core">
//     Copyright (c) . All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
/// <include file="UltimatR.Core.xml" path="doc/members/member[@name='N:System']" />
namespace System
{
    using System.Text;

    #region Enums

    /// <summary>
    /// Enum CharEncoding
    /// </summary>
    /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='T:System.CharEncoding']" />
    public enum CharEncoding
    {
        /// <summary>
        /// The ASCII
        /// </summary>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='F:System.CharEncoding.ASCII']" />
        ASCII,
        /// <summary>
        /// The ut f8
        /// </summary>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='F:System.CharEncoding.UTF8']" />
        UTF8,
        /// <summary>
        /// The unicode
        /// </summary>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='F:System.CharEncoding.Unicode']" />
        Unicode
    }

    #endregion




    /// <summary>
    /// Class CharsEncodingExtension.
    /// </summary>
    /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='T:System.CharsEncodingExtension']" />
    public static class CharsEncodingExtension
    {
        #region Methods







        /// <summary>
        /// Converts to bytes.
        /// </summary>
        /// <param name="ca">The ca.</param>
        /// <param name="tf">The tf.</param>
        /// <returns>Byte[].</returns>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='M:System.CharsEncodingExtension.ToBytes(Char,CharEncoding)']" />
        public static Byte[] ToBytes(this Char ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(new char[] { ca });
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(new char[] { ca });
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(new char[] { ca });
                default:
                    return Encoding.ASCII.GetBytes(new char[] { ca });
            }
        }







        /// <summary>
        /// Converts to bytes.
        /// </summary>
        /// <param name="ca">The ca.</param>
        /// <param name="tf">The tf.</param>
        /// <returns>Byte[].</returns>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='M:System.CharsEncodingExtension.ToBytes(Char[],CharEncoding)']" />
        public static Byte[] ToBytes(this Char[] ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(ca);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(ca);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(ca);
                default:
                    return Encoding.ASCII.GetBytes(ca);
            }
        }







        /// <summary>
        /// Converts to bytes.
        /// </summary>
        /// <param name="ca">The ca.</param>
        /// <param name="tf">The tf.</param>
        /// <returns>Byte[].</returns>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='M:System.CharsEncodingExtension.ToBytes(String,CharEncoding)']" />
        public static Byte[] ToBytes(this String ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(ca);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(ca);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(ca);
                default:
                    return Encoding.ASCII.GetBytes(ca);
            }
        }







        /// <summary>
        /// Converts to chars.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <param name="tf">The tf.</param>
        /// <returns>Char[].</returns>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='M:System.CharsEncodingExtension.ToChars(Byte,CharEncoding)']" />
        public static Char[] ToChars(this Byte ba, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetChars(new byte[] { ba });
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetChars(new byte[] { ba });
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetChars(new byte[] { ba });
                default:
                    return Encoding.ASCII.GetChars(new byte[] { ba });
            }
        }







        /// <summary>
        /// Converts to chars.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <param name="tf">The tf.</param>
        /// <returns>Char[].</returns>
        /// <include file="UltimatR.Core.xml" path="doc/members/member[@name='M:System.CharsEncodingExtension.ToChars(Byte[],CharEncoding)']" />
        public static Char[] ToChars(this Byte[] ba, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetChars(ba);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetChars(ba);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetChars(ba);
                default:
                    return Encoding.ASCII.GetChars(ba);
            }
        }

        #endregion
    }
}
