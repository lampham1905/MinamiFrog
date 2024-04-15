/***********************************************
				EasyTouch V
	Copyright © 2014-2015 The Hedgehog Team
    http://www.thehedgehogteam.com/Forum/
		
	  The.Hedgehog.Team@gmail.com
		
**********************************************/

using Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Engine;
using UnityEngine;
using UnityEngine.Events;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Components{
[AddComponentMenu("EasyTouch/Quick Swipe")]
public class QuickSwipe : QuickBase {

	#region Events
	[System.Serializable] public class OnSwipeAction : UnityEvent<Gesture>{}

	[SerializeField] 
	public OnSwipeAction onSwipeAction;
	#endregion

	#region enumeration
	public enum ActionTriggering {InProgress,End}
	public enum SwipeDirection {Vertical, Horizontal, DiagonalRight,DiagonalLeft,Up,UpRight, Right,DownRight,Down,DownLeft, Left,UpLeft,All};
	#endregion

	#region Members
	public bool allowSwipeStartOverMe = true;
	public ActionTriggering actionTriggering;
	public SwipeDirection swipeDirection = SwipeDirection.All;
	private float axisActionValue = 0;
	public bool enableSimpleAction = false;
	#endregion

	#region MonoBehaviour callback
	public QuickSwipe(){
		quickActionName = "QuickSwipe" + System.Guid.NewGuid().ToString().Substring(0,7);
	}

	public override void OnEnable(){
		base.OnEnable();
		Engine.EasyTouch.On_Drag += On_Drag;
		Engine.EasyTouch.On_Swipe += On_Swipe;
		Engine.EasyTouch.On_DragEnd += On_DragEnd;
		Engine.EasyTouch.On_SwipeEnd += On_SwipeEnd;
	}

	public override void OnDisable(){
		base.OnDisable();
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		Engine.EasyTouch.On_Drag -= On_Drag;
		Engine.EasyTouch.On_Swipe -= On_Swipe;
		Engine.EasyTouch.On_DragEnd -= On_DragEnd;
		Engine.EasyTouch.On_SwipeEnd -= On_SwipeEnd;
	}
	#endregion

	#region EasyTouch Event
	void On_Swipe (Gesture gesture){

		if (gesture.touchCount ==1 && ((gesture.pickedObject != gameObject && !allowSwipeStartOverMe) || allowSwipeStartOverMe)){
			fingerIndex = gesture.fingerIndex;
			if (actionTriggering == ActionTriggering.InProgress){
				if (isRightDirection(gesture)){
					onSwipeAction.Invoke( gesture);
					if (enableSimpleAction){
						DoAction(gesture);
					}
				}
			}
		}
	}

	void On_SwipeEnd (Gesture gesture){
			if (actionTriggering == ActionTriggering.End && isRightDirection(gesture) ){
			onSwipeAction.Invoke( gesture);
			if (enableSimpleAction){
				DoAction(gesture);
			}
		}

		if (fingerIndex ==  gesture.fingerIndex){
			fingerIndex =-1;
		}
	}

	void On_DragEnd (Gesture gesture){
		if (gesture.pickedObject == gameObject && allowSwipeStartOverMe){
			On_SwipeEnd( gesture);
		}
	}
	
	void On_Drag (Gesture gesture){
		if (gesture.pickedObject == gameObject && allowSwipeStartOverMe){
			On_Swipe( gesture);
		}
	}
	#endregion

	#region Private methods
	bool isRightDirection(Gesture gesture){
		float coef = -1;
		if ( inverseAxisValue){
			coef = 1;
		}

		axisActionValue = 0;
		switch (swipeDirection){
		case SwipeDirection.All:
			axisActionValue = gesture.deltaPosition.magnitude*coef;
			return true;
		case SwipeDirection.Horizontal:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Left || gesture.swipe == Engine.EasyTouch.SwipeDirection.Right){
				axisActionValue = gesture.deltaPosition.x *coef;
				return true;
			}
			break;
		case SwipeDirection.Vertical:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Up || gesture.swipe == Engine.EasyTouch.SwipeDirection.Down){
				axisActionValue = gesture.deltaPosition.y*coef;
				return true;
			}
			break;
		case SwipeDirection.DiagonalLeft:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.UpLeft || gesture.swipe == Engine.EasyTouch.SwipeDirection.DownRight){
				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}
			break;
		case SwipeDirection.DiagonalRight:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.UpRight || gesture.swipe == Engine.EasyTouch.SwipeDirection.DownLeft){
				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}

			break;
		case SwipeDirection.Left:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Left){
				axisActionValue = gesture.deltaPosition.x*coef;
				return true;
			}
			break;
		case SwipeDirection.Right:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Right){
				axisActionValue = gesture.deltaPosition.x*coef;
				return true;
			}
			break;
		case SwipeDirection.DownLeft:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.DownLeft){
				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}
			break;
		case SwipeDirection.DownRight:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.DownRight){

				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}
			break;
		case SwipeDirection.UpLeft:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.UpLeft){

				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}
			break;
		case SwipeDirection.UpRight:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.UpRight){
				axisActionValue = gesture.deltaPosition.magnitude*coef;
				return true;
			}
			break;
		case SwipeDirection.Up:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Up){
				axisActionValue = gesture.deltaPosition.y*coef;
				return true;
			}
			break;
		case SwipeDirection.Down:
			if (gesture.swipe == Engine.EasyTouch.SwipeDirection.Down){
				axisActionValue = gesture.deltaPosition.y*coef;
				return true;
			}
			break;
		}

		axisActionValue = 0;
		return false;
	}

	void DoAction(Gesture gesture){

		switch (directAction){
		case DirectAction.Rotate:
		case DirectAction.RotateLocal:
			axisActionValue *= sensibility;
			break;
		case DirectAction.Translate:
		case DirectAction.TranslateLocal:
		case DirectAction.Scale:
			axisActionValue /= Screen.dpi;
			axisActionValue *= sensibility;
			break;
		}

		DoDirectAction( axisActionValue);
	
	}
	#endregion
}
}
