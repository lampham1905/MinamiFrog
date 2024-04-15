using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Common.Debug
{
    public class DebugDrawHandlesExample : MonoBehaviour
    {
        public Rect rect;
        public Bounds bounds;
        public CustomProgressBar bar;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DebugDrawHandlesExample))]
    public class DebugDrawHandlesExampleEditor : Editor
    {
        DebugDrawHandlesExample mTarget;

        private void OnEnable()
        {
            mTarget = (DebugDrawHandlesExample)target;
        }

        void OnSceneGUI()
        {
            if (mTarget == null)
                return;

            mTarget.rect = DebugDrawHandles.DrawHandlesRectangleXY(mTarget.transform.position * 2, mTarget.rect, Color.red);
            mTarget.bounds = DebugDrawHandles.DrawHandlesRectangleXY(mTarget.transform.position * -2, mTarget.bounds, Color.red);
        }
    }
#endif
}