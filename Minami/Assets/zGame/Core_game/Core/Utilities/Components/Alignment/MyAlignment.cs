using System;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.Alignment
{
    public class MyAlignment : MonoBehaviour
    {

        public virtual void Initialize()
        {

        }

        public virtual void Align()
        {

        }

        public virtual void AlignByTweener(Action onFinish)
        {

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MyAlignment), true)]
    public class MyAlignmentEditor : UnityEditor.Editor
    {
        private MyAlignment mScript;

        private void OnEnable()
        {
            mScript = (MyAlignment)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Align"))
                mScript.Align();


            if (GUILayout.Button("Align By Tweener"))
            {
                if (!Application.isPlaying)
                {
                    Debug.Log("Can run only in Playing mode");
                    return;
                }
                mScript.AlignByTweener(null);
            }
        }
    }
#endif
}