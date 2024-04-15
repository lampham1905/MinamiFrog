﻿#if UNITY_EDITOR
using Lam.zGame.Core_game.Core.Utilities.AntiCheat.ObscuredTypes;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.AntiCheat.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ObscuredBool))]
    internal class ObscuredBoolDrawer : ObscuredPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            var hiddenValue = prop.FindPropertyRelative("hiddenValue");
            SetBoldIfValueOverridePrefab(prop, hiddenValue);

            var cryptoKey = prop.FindPropertyRelative("currentCryptoKey");
            var inited = prop.FindPropertyRelative("inited");

            var currentCryptoKey = cryptoKey.intValue;
            var val = false;

            if (!inited.boolValue)
            {
                if (currentCryptoKey == 0)
                {
                    currentCryptoKey = cryptoKey.intValue = ObscuredBool.cryptoKeyEditor;
                }
                inited.boolValue = true;
                hiddenValue.intValue = ObscuredBool.Encrypt(false, (byte)currentCryptoKey);
            }
            else
            {
                val = ObscuredBool.Decrypt(hiddenValue.intValue, (byte)currentCryptoKey);
            }

            EditorGUI.BeginChangeCheck();
            val = EditorGUI.Toggle(position, label, val);
            if (EditorGUI.EndChangeCheck())
            {
                hiddenValue.intValue = ObscuredBool.Encrypt(val, (byte)currentCryptoKey);
            }

            ResetBoldFont();
        }
    }
}
#endif