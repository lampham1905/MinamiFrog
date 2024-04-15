/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com 
 **/

using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Config.Editor
{
    public class ProfilesCollection : ScriptableObject
    {
        private static readonly string FilePath = "Assets/Editor/ProfilesCollection.asset";

        public List<DevSetting.Profile> profiles = new List<DevSetting.Profile>();

        public static ProfilesCollection LoadOrCreateCollection()
        {
            var collection = AssetDatabase.LoadAssetAtPath(FilePath, typeof(ProfilesCollection)) as ProfilesCollection;
            if (collection == null)
                collection = EditorHelper.CreateScriptableAsset<ProfilesCollection>(FilePath);
            return collection;
        }
    }
}