//----------------------------------------------
// Unity3D common libraries and editor tools
// License: The MIT License ( http://opensource.org/licenses/MIT )
// Copyright © 2013- mulova@gmail.com
//----------------------------------------------
using System.Collections.Generic;
using System.Collections.Generic.Ex;
using mulova.commons;
using UnityEditor;
using UnityEngine;

namespace mulova.unicore
{
    [CustomEditor(typeof(LogInitializer))]
    public class LogInitializerInspector : Editor
    {
        private LogInitializer comp;
        
        void OnEnable()
        {
            comp = target as LogInitializer;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //serializedObject.Update();
            //var lifeTime = serializedObject.FindProperty("lifeTime");
            //EditorGUILayout.PropertyField(lifeTime);
            //serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying&&GUILayout.Button("Apply"))
            {
                comp.Apply();
            }
            bool changed = false;
            //if (drawer.Draw())
            //{
            //    foreach (LoggerData d in comp.snapshot.data)
            //    {
            //        if (d != null&&LogManager.HasLogger(d.name))
            //        {
            //            LogManager.GetLogger(d.name).level = d.level;
            //        }
            //    }
            //    changed = true;
            //}
            ScanLoggers();
            //changed |= EditorGUIEx.PopupEnum("Log Level", ref comp.snapshot.logLevel);
            //changed |= EditorGUIEx.Toggle("Show Name", ref comp.snapshot.showName);
            //changed |= EditorGUIEx.Toggle("Show Time", ref comp.snapshot.showTime);
            //changed |= EditorGUIEx.Toggle("Show Method", ref comp.snapshot.showMethod);
            //changed |= EditorGUIEx.Toggle("Show Level", ref comp.snapshot.showLevel);

            for (int i=0; i<comp.snapshot.stacktraceTypes.Length; ++i)
            {
                UnityEngine.LogType logType = (UnityEngine.LogType)i;
                StackTraceLogType stacktrace = (StackTraceLogType)EditorGUILayout.EnumPopup(logType.ToString(), comp.snapshot.stacktraceTypes[i]);
                if (stacktrace != comp.snapshot.stacktraceTypes[i])
                {
                    Undo.RecordObject(comp, "Set stack trace type");
                    changed = true;
                    comp.snapshot.stacktraceTypes[i] = stacktrace;
                }
            }
            
            if (changed)
            {
                EditorUtil.SetDirty(comp);
            }
        }
        
        private List<commons.ILog> loggers = new List<commons.ILog>();
        private Vector2 logScrollPos;

        private void ScanLoggers()
        {
            // Scan all loggers and remove already added
            if (GUILayout.Button("Scan"))
            {
                loggers = LogManager.GetLoggers();
                loggers.Sort(new LoggerSorter());

                foreach (LoggerData d in comp.snapshot.data)
                {
                    loggers.Remove(LogManager.GetLogger(d.name));
                }
            }
            if (!loggers.IsEmpty())
            {
                logScrollPos = EditorGUILayout.BeginScrollView(logScrollPos, GUILayout.MinHeight(100));
                for (int i=loggers.Count-1; i>=0; i--)
                {
                    commons.ILog log = loggers[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(log.ToString());
                    if (GUILayout.Button("+"))
                    {
                        //drawer.Add(new LoggerData(log.ToString(), log.level));
                        loggers.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
    
    class LoggerSorter : IComparer<commons.ILog>
    {
        int IComparer<commons.ILog>.Compare(commons.ILog x, commons.ILog y)
        {
            return y.ToString().CompareTo(x.ToString());
        }
    }
    
    class LogEntry
    {
        commons.ILog logger;
        commons.LogLevel level;
    }
}