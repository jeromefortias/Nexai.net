// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectorService<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the value based on configured data
        /// </summary>
        TValue? GetValue(in TKey key);
    }
}
