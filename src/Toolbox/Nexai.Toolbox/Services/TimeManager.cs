// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Services
{
    using Nexai.Toolbox.Abstractions.Services;

    using System;

    /// <inheritdoc />
    public sealed class TimeManager : ITimeManager
    {
        #region Ctor

        /// <summary>
        /// Initializes the <see cref="TimeManager"/> class.
        /// </summary>
        static TimeManager()
        {
            Instance = new TimeManager();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a instance.
        /// </summary>
        public static TimeManager Instance { get; }

        /// <inheritdoc />
        public DateOnly Today
        {
            get { return DateOnly.FromDateTime(DateTime.Today.Date); }
        }

        /// <inheritdoc />
        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }

        /// <inheritdoc />
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        /// <inheritdoc />
        public DateTime LocalNow
        {
            get { return DateTime.Now; }
        }

        #endregion
    }
}
