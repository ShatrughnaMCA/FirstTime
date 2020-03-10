﻿using System;

namespace NoidaAuthority.PMS.Common.Logger
{
    public interface ILogService
    {
        void LogInformation(string message, Exception exception = null);
        void LogInformationFormat(string format, params object[] args);
        void LogDebug(string message, Exception exception = null);
        void LogDebugFormat(string format, params object[] args);
        void LogWarning(string message, Exception exception = null);
        void LogWarningFormat(string format, params object[] args);
        void LogError(string message, Exception exception = null);
        void LogErrorFormat(string format, params object[] args);
        void LogException(string message, Exception exception = null);
        void LogExceptionFormat(string format, params object[] args);
        bool IsDebugEnabled { get; }
    }
}