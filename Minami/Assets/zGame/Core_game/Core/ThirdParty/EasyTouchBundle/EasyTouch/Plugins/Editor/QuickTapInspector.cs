﻿using Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Components;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Editor
{
	[CustomEditor(typeof(QuickTap))]
	public class QuickTapInspector : UnityEditor.Editor {

		public override void OnInspectorGUI(){
		
			QuickTap t = (QuickTap)target;
		
			EditorGUILayout.Space();
		
			t.quickActionName = EditorGUILayout.TextField("Name",t.quickActionName);
		
			EditorGUILayout.Space();
		
			t.is2Finger = EditorGUILayout.Toggle("2 fingers gesture",t.is2Finger);
			t.actionTriggering = (QuickTap.ActionTriggering)EditorGUILayout.EnumPopup("Action triggering",t.actionTriggering);
		
			EditorGUILayout.Space();

			t.enablePickOverUI = EditorGUILayout.ToggleLeft("Allow over UI Element",t.enablePickOverUI);
		
			serializedObject.Update();
			SerializedProperty touch = serializedObject.FindProperty("onTap");
			EditorGUILayout.PropertyField(touch, true, null);
			serializedObject.ApplyModifiedProperties();
		
			if (GUI.changed){
				EditorUtility.SetDirty(t);
#if UNITY_5_3_OR_NEWER
				EditorSceneManager.MarkSceneDirty( EditorSceneManager.GetActiveScene());
#endif
			}
		}
	}
}