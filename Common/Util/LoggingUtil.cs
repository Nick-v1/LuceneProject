﻿using System;
using NLog;

namespace Common.Util
{

    public class LoggingUtil
    {
        public static void ConfigureNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var console =
                    new NLog.Targets.ConsoleTarget("console")
                    {
                        Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception:format=tostring}"
                    };
            config.AddTarget(console);
            config.AddRuleForAllLevels(console);

            LogManager.Configuration = config;
        }
    }
}
