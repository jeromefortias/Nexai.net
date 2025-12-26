// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Services
{
    using Nexai.Toolbox.Abstractions.Services;

    using System.Security.Cryptography;

    /// <summary>
    /// Service in charge to provide SHA256 hashage 
    /// </summary>
    /// <seealso cref="IHashService" />
    public class HashSHA256Service : HashBaseService
    {
        #region Ctor

        /// <summary>
        /// Initializes the <see cref="HashSHA256Service"/> class.
        /// </summary>
        static HashSHA256Service()
        {
            Instance = new HashSHA256Service();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashSHA256Service"/> class.
        /// </summary>
        public HashSHA256Service()
            : base(SHA256.Create)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static HashSHA256Service Instance { get; }

        #endregion
    }
}
