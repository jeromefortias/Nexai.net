// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Disposables
{
    using System;

    /// <summary>
    /// Token used as security temporary key
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ISecureContextToken : IDisposable
    {
        /// <summary>
        /// Gets the secure context identifier.
        /// </summary>
        Guid SecureContextId { get; }
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Embed content
    /// </remarks>
    public interface ISecureContextToken<TContent> : ISecureContextToken
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        TContent Token { get; }
    }
}
