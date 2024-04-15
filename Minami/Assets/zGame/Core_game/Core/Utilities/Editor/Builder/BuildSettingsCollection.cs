﻿/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com - 2019
 **/

using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Editor.Builder
{
    public class BuildSettingsCollection : ScriptableObject
    {
        private static readonly string FilePath = "Assets/Editor/BuildSettingsCollection.asset";

        public List<BuildProfile> profiles = new List<BuildProfile>();

        public static BuildSettingsCollection LoadOrCreateSettings()
        {
            var collection = AssetDatabase.LoadAssetAtPath(FilePath, typeof(BuildSettingsCollection)) as BuildSettingsCollection;
            if (collection == null)
                collection = EditorHelper.CreateScriptableAsset<BuildSettingsCollection>(FilePath);
            if (collection.profiles.Count == 0)
            {
                var settings = new BuildProfile();
                settings.Reset();
                collection.profiles.Add(settings);
            }
            return collection;
        }
    }
}