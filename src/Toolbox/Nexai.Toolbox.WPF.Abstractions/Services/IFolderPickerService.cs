// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions.Services
{
    public interface IFolderPickerService
    {
        /// <summary>
        /// Picks the folder path asynchronous.
        /// </summary>
        ValueTask<string?> PickFolderPathAsync(string? root = null);
    }
}
