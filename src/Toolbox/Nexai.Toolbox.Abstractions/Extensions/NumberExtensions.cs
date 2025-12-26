// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Determines whether the specified value is between minimum included and maximum excluded
        /// </summary>
        public static bool IsBetween<TSource>(this TSource source, in TSource minIncluded, in TSource maxIncluded)
            where TSource : IComparable
        {
            return minIncluded.CompareTo(source) >= 0 && source.CompareTo(maxIncluded) <= 0;
        }
    }
}
