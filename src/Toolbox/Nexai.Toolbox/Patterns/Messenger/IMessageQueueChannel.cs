// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Patterns.Messenger
{
    using Nexai.Toolbox.Abstractions.Supports;

    /// <summary>
    /// A channel is specialized in a type of message and can be extra specialized in a specific category
    /// It store the subscription and distribute the incomming message
    /// </summary>
    internal interface IMessageQueueChannel : ISupportDebugDisplayName
    {
    }
}
