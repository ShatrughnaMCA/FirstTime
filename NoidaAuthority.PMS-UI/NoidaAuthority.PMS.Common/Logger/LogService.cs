﻿using System;
using System.Globalization;
using log4net;

namespace NoidaAuthority.PMS.Common.Logger
{
    public class LogService : ILogService
    {
        private readonly ILog logger;

        public LogService()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger(GetType());
        }

        public void LogInformation(string message, Exception exception = null)
        {
            if (logger.IsInfoEnabled)
            {
                if (exception == null)
                {
                    logger.Info(message);
                }
                else
                {
                    logger.Info(message, exception);
                }
            }
        }

        public void LogInformationFormat(string format, params object[] args)
        {
            if (logger.IsInfoEnabled)
            {
                this.logger.InfoFormat(CultureInfo.InvariantCulture, format, args);
            }
        }

        public void LogDebug(string message, Exception exception = null)
        {
            if (logger.IsDebugEnabled)
            {
                if (exception == null)
                {
                    logger.Debug(message);
                }
                else
                {
                    logger.Debug(message, exception);
                }
            }
        }

        public void LogDebugFormat(string format, params object[] args)
        {
            if (logger.IsDebugEnabled)
            {
                this.logger.DebugFormat(CultureInfo.InvariantCulture, format, args);
            }
        }

        public void LogWarning(string message, Exception exception = null)
        {
            if (logger.IsWarnEnabled)
            {
                if (exception == null)
                {
                    logger.Warn(message);
                }
                else
                {
                    logger.Warn(message, exception);
                }
            }
        }

        public void LogWarningFormat(string format, params object[] args)
        {
            if (logger.IsWarnEnabled)
            {
                this.logger.WarnFormat(CultureInfo.InvariantCulture, format, args);
            }
        }

        public void LogError(string message, Exception exception = null)
        {
            if (logger.IsErrorEnabled)
            {
                if (exception == null)
                {
                    logger.Error(message);
                }
                else
                {
                    logger.Error(message, exception);
                }
            }
        }

        public void LogErrorFormat(string format, params object[] args)
        {
            if (logger.IsErrorEnabled)
            {
                this.logger.ErrorFormat(CultureInfo.InvariantCulture, format, args);
            }
        }

        public void LogException(string message, Exception exception = null)
        {
            if (logger.IsFatalEnabled)
            {
                if (exception == null)
                {
                    logger.Fatal(message);
                }
                else
                {
                    logger.Fatal(message, exception);
                };
            }
        }
        public void LogExceptionFormat(string format, params object[] args)
        {
            if (logger.IsFatalEnabled)
            {
                this.logger.FatalFormat(CultureInfo.InvariantCulture, format, args);
            }
        }

        public bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }


        }
    }
}