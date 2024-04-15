using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Demo.Script
{
    [System.Serializable]
    public class PrefabsList : AssetsList<GameObject>
    {
        public override bool showBox => false;
        public override bool @readonly => false;
    }

    [CreateAssetMenu(fileName = "ExampleAssetsCollection", menuName = "RUtilities/Assets Collection Example")]
    public class ExampleAssetsCollection : ScriptableObject
    {
        public SpritesList icons;
        public List<GameObject> gameobjects;
        public PrefabsList prefabs;

#if UNITY_EDITOR
        [CustomEditor(typeof(ExampleAssetsCollection))]
        public class ExampleAssetsCollectionEditor : Editor
        {
            private ExampleAssetsCollection mScript;

            private void OnEnable()
            {
                mScript = target as ExampleAssetsCollection;
            }

            public override void OnInspectorGUI()
            {
                var currentTab = EditorHelper.Tabs(mScript.name, "Default", "Custom");
                switch (currentTab)
                {
                    case "Default":
                        base.OnInspectorGUI();
                        break;
                    case "Custom":
                        mScript.icons.DrawInEditor("Icons");
                        EditorHelper.ListObjects(ref mScript.gameobjects, "Prefabs 1", true);
                        mScript.prefabs.DrawInEditor("Prefabs 2");
                        break;
                }
            }
        }
#endif
    }
}