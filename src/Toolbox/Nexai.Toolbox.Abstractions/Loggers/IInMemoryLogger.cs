// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Loggers
{
    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;

    /// <summary>
    ///  Logger that keep in memory the logs
    /// </summary>
    public interface IInMemoryLogger : ILogger, IObservable<SimpleLog>
    {
        #region Methods

        /// <summary>
        /// Gets a copy of logs at requested time.
        /// </summary>
        IReadOnlyCollection<SimpleLog> GetLogsCopy();

        #endregion
    }
}
