// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Builders
{
    /// <summary>
    /// Builder used to setup Globalization resources
    /// </summary>
    public interface IGlobalizationStringResourceBuilder
    {
        /// <summary>
        /// Add RESXs the string resource.
        /// </summary>
        IGlobalizationStringResourceBuilder RESXStringResource<TResource>();
    }
}
