using Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Components;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Editor
{
	[CustomEditor(typeof(QuickTouch))]
	public class QuickTouchInspector : UnityEditor.Editor {

		public override void OnInspectorGUI(){
		
			QuickTouch t = (QuickTouch)target;

			EditorGUILayout.Space();

			t.quickActionName = EditorGUILayout.TextField("Name",t.quickActionName);

			EditorGUILayout.Space();

			t.is2Finger = EditorGUILayout.Toggle("2 fingers gesture",t.is2Finger);
			t.actionTriggering = (QuickTouch.ActionTriggering)EditorGUILayout.EnumPopup("Action triggering",t.actionTriggering);

			EditorGUILayout.Space();

			if (!t.is2Finger){
				t.isMultiTouch = EditorGUILayout.ToggleLeft("Allow multi-touch",t.isMultiTouch);
			}
			t.enablePickOverUI = EditorGUILayout.ToggleLeft("Allow over UI Element",t.enablePickOverUI);

			serializedObject.Update();
			SerializedProperty touch = serializedObject.FindProperty("onTouch");
			EditorGUILayout.PropertyField(touch, true, null);
			serializedObject.ApplyModifiedProperties();

			if (t.actionTriggering == QuickTouch.ActionTriggering.Up){
				touch = serializedObject.FindProperty("onTouchNotOverMe");
				EditorGUILayout.PropertyField(touch, true, null);
				serializedObject.ApplyModifiedProperties();
			}

			if (GUI.changed){
				EditorUtility.SetDirty(t);
#if UNITY_5_3_OR_NEWER
				EditorSceneManager.MarkSceneDirty( EditorSceneManager.GetActiveScene());
#endif
			}
		}
	}
}
