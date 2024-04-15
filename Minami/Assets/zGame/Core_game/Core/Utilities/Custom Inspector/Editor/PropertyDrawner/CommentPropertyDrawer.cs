using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Editor.PropertyDrawner
{
	/// <summary>
	/// A property drawer for fields marked with the CommentAttribute. Similar to Header, but useful
	/// for longer descriptions.
	/// </summary>
	[CustomPropertyDrawer(typeof(CommentAttribute), useForChildren: true)]
	public class CommentPropertyDrawer : DecoratorDrawer
	{
		CommentAttribute CommentAttribute
		{
			get { return (CommentAttribute)attribute; }
		}

		public override float GetHeight()
		{
			return EditorStyles.whiteLabel.CalcHeight(CommentAttribute.content, Screen.width - 19);
		}

		public override void OnGUI(Rect position)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.LabelField(position, CommentAttribute.content);
			EditorGUI.EndDisabledGroup();
		}
	}
}
