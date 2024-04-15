using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Editor.PropertyDrawner
{
    [CustomPropertyDrawer(typeof(SingleLayerAttribute))]
    class LayerAttributeEditor : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // One line of  oxygen free code.
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}