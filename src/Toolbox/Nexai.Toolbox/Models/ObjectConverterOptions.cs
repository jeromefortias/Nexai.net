// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ObjectConverterOptions
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConverterOptions"/> class.
        /// </summary>
        public ObjectConverterOptions()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConverterOptions"/> class.
        /// </summary>
        public ObjectConverterOptions(bool reuseConverter)
        {
            this.ReuseConverter = reuseConverter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether <see cref="ObjectConverter"/> will try to reuse the last converter that resolved the Type to Type convertion
        /// </summary>
        public bool ReuseConverter { get; }

        #endregion
    }
}
