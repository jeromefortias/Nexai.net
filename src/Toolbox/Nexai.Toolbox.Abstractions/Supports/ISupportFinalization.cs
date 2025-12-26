// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Supports
{
    using System.Threading.Tasks;

    /// <summary>
    /// Define an instance that support finalization
    /// </summary>
    public interface ISupportFinalization
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is finalizing.
        /// </summary>
        bool IsFinalizing { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is finaized.
        /// </summary>
        bool IsFinalized { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Finalize the instance
        /// </summary>
        ValueTask FinalizeAsync(CancellationToken token = default);

        #endregion
    }
}
