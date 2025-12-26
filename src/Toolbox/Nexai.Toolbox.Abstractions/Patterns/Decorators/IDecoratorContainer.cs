// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Patterns.Decorators
{
    /// <summary>
    /// Relation key between a source of data linked by <typeparamref name="TSourceKey"/> and a external data <typeparamref name="TData"/>
    /// </summary>
    public interface IDecoratorContainer<out TData, out TSourceKey>
    {
        /// <summary>
        /// Gets the data decorating.
        /// </summary>
        TData Data { get; }

        /// <summary>
        /// Gets the source key decorate by this data.
        /// </summary>
        TSourceKey SourceKey { get; }
    }
}
