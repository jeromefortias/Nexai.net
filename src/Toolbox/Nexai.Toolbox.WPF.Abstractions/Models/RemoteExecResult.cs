// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public record class RemoteExecResultRemoteExecResult(bool Success, string StandardLog, string ErrorLog, int ExitCode, string ExceptionMessage);
}
