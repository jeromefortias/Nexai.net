// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Services
{
    using System;

    /// <summary>
    /// Handler used to provide time information through
    /// </summary>
    public interface ITimeManager
    {
        #region Properties

        /// <summary>
        /// Gets the date of today.
        /// </summary>
        DateOnly Today { get; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        DateTime LocalNow { get; }

        #endregion
    }
}
