// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.UI.Exceptions
{
    using Nexai.Toolbox.WPF.UI.Resources;

    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Raised when some needed part in the template are missing
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class TemplatePartMissingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatePartMissingException"/> class.
        /// </summary>
        public TemplatePartMissingException(Type partType, string partName, Control control)
            : base(ExceptionSR.TemplatePartMissingException.WithArguments(partName, partType, control))
        {

        }
    }
}
