// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Disposables
{
    public sealed class DisposableContainer<TContent> : SafeDisposable<TContent>
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableContainer{TContent}"/> class.
        /// </summary>
        public DisposableContainer(TContent content, bool disposeContent = false) 
            : base(content, disposeContent)
        {
        }

        #endregion
    }
}
