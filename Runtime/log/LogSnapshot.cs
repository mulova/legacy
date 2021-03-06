﻿//----------------------------------------------
// Unity3D common libraries and editor tools
// License: The MIT License ( http://opensource.org/licenses/MIT )
// Copyright © 2013- mulova@gmail.com
//----------------------------------------------

using System;

using UnityEngine;
using mulova.commons;
using System.Ex;

namespace mulova.unicore {
	[Serializable]
	public class LogSnapshot {
        public commons.LogLevel logLevel = commons.LogLevel.Warning;
        public LoggerData[] data = new LoggerData[0];
		public bool showName = false;
		public bool showTime = false;
		public bool showMethod = false;
		public bool showLevel = false;
        public StackTraceLogType[] stacktraceTypes = new StackTraceLogType[Enum.GetValues(typeof(UnityEngine.LogType)).Length];

		public LogSnapshot() {}

		public LogSnapshot(LogSnapshot src) {
			logLevel = src.logLevel;
			data = src.data.CloneElements();
			showName = src.showName;
			showTime = src.showTime;
			showMethod = src.showMethod;
			showLevel = src.showLevel;
		}

		public void Apply() {
			LogManager.ResetLevel(logLevel);
			foreach (LoggerData d in data) {
                LogManager.GetLogger(d.name).level = d.level;
			}
			SimpleLogFormatter f = LogManager.GetLogFormatter() as SimpleLogFormatter;
			if (f != null) {
				f.ShowName = showName;
				f.ShowTime = showTime;
				f.ShowMethod = showMethod;
				f.ShowLevel = showLevel;
			}
		}
	}
}
