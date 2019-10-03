﻿using System.Collections.Generic;
using System.Collections.Generic.Ex;
using System.IO;
using mulova.commons;
using mulova.comunity;
using mulova.preprocess;
using mulova.unicore;
using UnityEditor;
using UnityEngine;

namespace mulova.audio
{
    public class AudioGroupBuildProcessor : ComponentBuildProcess
    {
        protected override void VerifyComponent(Component comp)
        {
            FindMissingAudioClips(comp as AudioGroup);
        }

        protected override void PreprocessComponent(Component comp)
        {
        }

        protected override void PreprocessOver(Component c)
        {
        }

        public override System.Type compType
        {
            get
            {
                return typeof(AudioGroup);
            }
        }

        private void FindMissingAudioClips(AudioGroup g)
        {
            string clipFolder = AssetDatabase.GUIDToAssetPath(g.assetDir.guid);
            var paths = EditorAssetUtil.ListAssetPaths(clipFolder, FileType.Audio, true);
            HashSet<string> local = GetHashSet(paths);

            foreach (AudioClipData a in g.data)
            {
                if (!local.Contains(a.path))
                {
                    log.LogFormat("Missing clip '{0}', Check if filename is upper case.", a.path);
                } else
                {
                    local.Remove(a.path);
                }
            }
            if (!local.IsEmpty())
            {
                log.LogFormat("Missing clip data {0} in {1}", local.Join(","), g.csv.GetEditorPath());
            }
        }

        /// <summary>
        /// change filename to lowercase
        /// </summary>
        /// <returns>The hash set.</returns>
        /// <param name="paths">Paths.</param>
        private HashSet<string> GetHashSet(string[] paths)
        {
            HashSet<string> set = new HashSet<string>();
            foreach (string p in paths)
            {
                string dir = PathUtil.GetDirectory(p);
                string filename = Path.GetFileName(p).ToLower();
                set.Add(PathUtil.Combine(dir, filename));
            }
            return set;
        }
    }
}
