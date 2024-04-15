using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Editor.PropertyDrawner;
using UnityEditor;

namespace Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Editor
{
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(ObjectColorDictionary))]
    [CustomPropertyDrawer(typeof(ObjectObjectDictionary))]
    public class SerializableDictionaryDrawer : SerializableDictionaryPropertyDrawer { }
}