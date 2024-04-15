#if UNITY_EDITOR
using Lam.zGame.Core_game.Core.Utilities.AntiCheat.ObscuredTypes;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.AntiCheat.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ObscuredInt))]
    internal class ObscuredIntDrawer : ObscuredPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            var hiddenValue = prop.FindPropertyRelative("hiddenValue");
            SetBoldIfValueOverridePrefab(prop, hiddenValue);

            var cryptoKey = prop.FindPropertyRelative("currentCryptoKey");
            var inited = prop.FindPropertyRelative("inited");

            var currentCryptoKey = cryptoKey.intValue;
            var val = 0;

            if (!inited.boolValue)
            {
                if (currentCryptoKey == 0)
                {
                    currentCryptoKey = cryptoKey.intValue = ObscuredInt.cryptoKeyEditor;
                }
                hiddenValue.intValue = ObscuredInt.Encrypt(0, currentCryptoKey);
                inited.boolValue = true;
            }
            else
            {
                val = ObscuredInt.Decrypt(hiddenValue.intValue, currentCryptoKey);
            }

            EditorGUI.BeginChangeCheck();
            val = EditorGUI.IntField(position, label, val);
            if (EditorGUI.EndChangeCheck())
            {
                hiddenValue.intValue = ObscuredInt.Encrypt(val, currentCryptoKey);
            }

            ResetBoldFont();
        }
    }
}
#endif