/***********************************************
				EasyTouch V
	Copyright © 2014-2015 The Hedgehog Team
    http://www.thehedgehogteam.com/Forum/
		
	  The.Hedgehog.Team@gmail.com
		
**********************************************/

using System.Collections.Generic;
using Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Engine;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Components{
[AddComponentMenu("EasyTouch/Trigger")]
[System.Serializable]
public class EasyTouchTrigger : MonoBehaviour {

	public enum ETTParameter{ None,Gesture, Finger_Id,Touch_Count, Start_Position, Position, Delta_Position, Swipe_Type, Swipe_Length, Swipe_Vector,Delta_Pinch, Twist_Anlge, ActionTime, DeltaTime, PickedObject, PickedUIElement }
	public enum ETTType {Object3D,UI};

	[System.Serializable]
	public class EasyTouchReceiver{
		public bool enable;
		public ETTType triggerType;
		public string name;
		public bool restricted;
		public GameObject gameObject;
		public bool otherReceiver;
		public GameObject gameObjectReceiver;
		public Engine.EasyTouch.EvtType eventName;
		public string methodName;
		public ETTParameter parameter; 
	}

	[SerializeField]
	public List<EasyTouchReceiver> receivers = new List<EasyTouchReceiver>();

	#region Monobehaviour Callback
	void Start(){
		Engine.EasyTouch.SetEnableAutoSelect( true);

	}

	void OnEnable(){
		SubscribeEasyTouchEvent();
	}
		
	void OnDisable(){
		UnsubscribeEasyTouchEvent();
	}

	void OnDestroy(){
		UnsubscribeEasyTouchEvent();
	}

	private void SubscribeEasyTouchEvent(){

		// Touch
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Cancel))
			Engine.EasyTouch.On_Cancel += On_Cancel;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchStart))
			Engine.EasyTouch.On_TouchStart += On_TouchStart;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchDown))
			Engine.EasyTouch.On_TouchDown += On_TouchDown;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchUp))
			Engine.EasyTouch.On_TouchUp += On_TouchUp;

		// Tap & long tap
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SimpleTap))
			Engine.EasyTouch.On_SimpleTap += On_SimpleTap;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTapStart))
			Engine.EasyTouch.On_LongTapStart += On_LongTapStart;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTap))
			Engine.EasyTouch.On_LongTap += On_LongTap;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTapEnd))
			Engine.EasyTouch.On_LongTapEnd += On_LongTapEnd;

		// Double tap
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DoubleTap))
			Engine.EasyTouch.On_DoubleTap += On_DoubleTap;

		// Drag
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DragStart))
			Engine.EasyTouch.On_DragStart += On_DragStart;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Drag))
			Engine.EasyTouch.On_Drag += On_Drag;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DragEnd))
			Engine.EasyTouch.On_DragEnd += On_DragEnd;

		// Swipe
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SwipeStart))
			Engine.EasyTouch.On_SwipeStart += On_SwipeStart;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Swipe))
			Engine.EasyTouch.On_Swipe += On_Swipe;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SwipeEnd))
			Engine.EasyTouch.On_SwipeEnd += On_SwipeEnd;

		// Tap 2 fingers
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchStart2Fingers))
			Engine.EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchDown2Fingers))
			Engine.EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TouchUp2Fingers))
			Engine.EasyTouch.On_TouchUp2Fingers += On_TouchUp2Fingers;

		// Tap & Long tap 2 fingers
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SimpleTap2Fingers))
			Engine.EasyTouch.On_SimpleTap2Fingers+= On_SimpleTap2Fingers;

		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTapStart2Fingers))
			Engine.EasyTouch.On_LongTapStart2Fingers += On_LongTapStart2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTap2Fingers))
			Engine.EasyTouch.On_LongTap2Fingers += On_LongTap2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_LongTapEnd2Fingers))
			Engine.EasyTouch.On_LongTapEnd2Fingers += On_LongTapEnd2Fingers;

		// double tap fingers
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DoubleTap2Fingers))
			Engine.EasyTouch.On_DoubleTap2Fingers += On_DoubleTap2Fingers;

		// Swipe
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SwipeStart2Fingers))
			Engine.EasyTouch.On_SwipeStart2Fingers += On_SwipeStart2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Swipe2Fingers))
			Engine.EasyTouch.On_Swipe2Fingers += On_Swipe2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_SwipeEnd2Fingers))
			Engine.EasyTouch.On_SwipeEnd2Fingers += On_SwipeEnd2Fingers;

		// Drag
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DragStart2Fingers))
			Engine.EasyTouch.On_DragStart2Fingers += On_DragStart2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Drag2Fingers))
			Engine.EasyTouch.On_Drag2Fingers += On_Drag2Fingers;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_DragEnd2Fingers))
			Engine.EasyTouch.On_DragEnd2Fingers += On_DragEnd2Fingers;

		// Pinch
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Pinch))
			Engine.EasyTouch.On_Pinch += On_Pinch;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_PinchIn))
			Engine.EasyTouch.On_PinchIn += On_PinchIn;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_PinchOut))
			Engine.EasyTouch.On_PinchOut += On_PinchOut;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_PinchEnd))
			Engine.EasyTouch.On_PinchEnd += On_PinchEnd;

		// Twist
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_Twist))
			Engine.EasyTouch.On_Twist += On_Twist;
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_TwistEnd))
			Engine.EasyTouch.On_TwistEnd += On_TwistEnd;

		// Unity UI
		if (IsRecevier4( Engine.EasyTouch.EvtType.On_OverUIElement))
			Engine.EasyTouch.On_OverUIElement += On_OverUIElement;

		if (IsRecevier4( Engine.EasyTouch.EvtType.On_UIElementTouchUp))
			Engine.EasyTouch.On_UIElementTouchUp += On_UIElementTouchUp;

	}

	private void UnsubscribeEasyTouchEvent(){
		Engine.EasyTouch.On_Cancel -= On_Cancel;
		Engine.EasyTouch.On_TouchStart -= On_TouchStart;
		Engine.EasyTouch.On_TouchDown -= On_TouchDown;
		Engine.EasyTouch.On_TouchUp -= On_TouchUp;

		Engine.EasyTouch.On_SimpleTap -= On_SimpleTap;
		Engine.EasyTouch.On_LongTapStart -= On_LongTapStart;
		Engine.EasyTouch.On_LongTap -= On_LongTap;
		Engine.EasyTouch.On_LongTapEnd -= On_LongTapEnd;
		
		Engine.EasyTouch.On_DoubleTap -= On_DoubleTap;
		
		Engine.EasyTouch.On_DragStart -= On_DragStart;
		Engine.EasyTouch.On_Drag -= On_Drag;
		Engine.EasyTouch.On_DragEnd -= On_DragEnd;
		
		Engine.EasyTouch.On_SwipeStart -= On_SwipeStart;
		Engine.EasyTouch.On_Swipe -= On_Swipe;
		Engine.EasyTouch.On_SwipeEnd -= On_SwipeEnd;

		Engine.EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
		Engine.EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
		Engine.EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;

		Engine.EasyTouch.On_SimpleTap2Fingers-= On_SimpleTap2Fingers;
		Engine.EasyTouch.On_LongTapStart2Fingers -= On_LongTapStart2Fingers;
		Engine.EasyTouch.On_LongTap2Fingers -= On_LongTap2Fingers;
		Engine.EasyTouch.On_LongTapEnd2Fingers -= On_LongTapEnd2Fingers;
		
		Engine.EasyTouch.On_DoubleTap2Fingers -= On_DoubleTap2Fingers;

		Engine.EasyTouch.On_SwipeStart2Fingers -= On_SwipeStart2Fingers;
		Engine.EasyTouch.On_Swipe2Fingers -= On_Swipe2Fingers;
		Engine.EasyTouch.On_SwipeEnd2Fingers -= On_SwipeEnd2Fingers;

		Engine.EasyTouch.On_DragStart2Fingers -= On_DragStart2Fingers;
		Engine.EasyTouch.On_Drag2Fingers -= On_Drag2Fingers;
		Engine.EasyTouch.On_DragEnd2Fingers -= On_DragEnd2Fingers;

		Engine.EasyTouch.On_Pinch -= On_Pinch;
		Engine.EasyTouch.On_PinchIn -= On_PinchIn;
		Engine.EasyTouch.On_PinchOut -= On_PinchOut;
		Engine.EasyTouch.On_PinchEnd -= On_PinchEnd;
		
		Engine.EasyTouch.On_Twist -= On_Twist;
		Engine.EasyTouch.On_TwistEnd -= On_TwistEnd;

		Engine.EasyTouch.On_OverUIElement += On_OverUIElement;
		Engine.EasyTouch.On_UIElementTouchUp += On_UIElementTouchUp;

	}
	#endregion
	
	#region One Finger EasyTouch Callback

	void On_TouchStart (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchStart,gesture);
	}

	void On_TouchDown (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchDown,gesture);
	}

	void On_TouchUp (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchUp,gesture);
	}

	void On_SimpleTap (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SimpleTap,gesture);
	}

	void On_DoubleTap (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DoubleTap,gesture);
	}

	void On_LongTapStart (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTapStart,gesture);
	}

	void On_LongTap (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTap,gesture);
	}
	
	void On_LongTapEnd (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTapEnd,gesture);
	}

	void On_SwipeStart (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SwipeStart,gesture);
	}

	void On_Swipe (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Swipe,gesture);
	}

	void On_SwipeEnd (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SwipeEnd,gesture);
	}
	
	void On_DragStart (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DragStart,gesture);
	}

	void On_Drag (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Drag,gesture);
	}

	void On_DragEnd (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DragEnd,gesture);
	}
		
	void On_Cancel (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Cancel,gesture);
	}

	#endregion

	#region Two Finger EasyTouch Callback
	void On_TouchStart2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchStart2Fingers,gesture);
	}
	
	void On_TouchDown2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchDown2Fingers,gesture);
	}
	
	void On_TouchUp2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TouchUp2Fingers,gesture);
	}
	
	void On_LongTapStart2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTapStart2Fingers,gesture);
	}
	
	void On_LongTap2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTap2Fingers,gesture);
	}
	
	void On_LongTapEnd2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_LongTapEnd2Fingers,gesture);
	}

	void On_DragStart2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DragStart2Fingers,gesture);
	}
	
	void On_Drag2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Drag2Fingers,gesture);
	}
	
	void On_DragEnd2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DragEnd2Fingers,gesture);
	}

	void On_SwipeStart2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SwipeStart2Fingers,gesture);
	}

	void On_Swipe2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Swipe2Fingers,gesture);
	}

	void On_SwipeEnd2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SwipeEnd2Fingers,gesture);
	}
			
	void On_Twist (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Twist,gesture);
	}

	void On_TwistEnd (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_TwistEnd,gesture);
	}

	void On_Pinch (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_Pinch,gesture);
	}

	void On_PinchOut (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_PinchOut,gesture);
	}
	
	void On_PinchIn (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_PinchIn,gesture);
	}
	
	void On_PinchEnd (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_PinchEnd,gesture);
	}

	void On_SimpleTap2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_SimpleTap2Fingers,gesture);
		
	}

	void On_DoubleTap2Fingers (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_DoubleTap2Fingers,gesture);
	}
	#endregion

	#region UI Event
	void On_UIElementTouchUp (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_UIElementTouchUp,gesture);
	}
	
	void On_OverUIElement (Gesture gesture){
		TriggerScheduler(Engine.EasyTouch.EvtType.On_OverUIElement,gesture);	
	}
	#endregion

	#region Public Method
	public void AddTrigger(Engine.EasyTouch.EvtType ev){
		EasyTouchReceiver r = new EasyTouchReceiver();
		r.enable = true;
		r.restricted = true;
		r.eventName = ev;
		r.gameObject =null;
		r.otherReceiver = false;
		r.name = "New trigger";
		receivers.Add( r );

		if (Application.isPlaying){
			UnsubscribeEasyTouchEvent();
			SubscribeEasyTouchEvent();
		}

	}

	public bool SetTriggerEnable(string triggerName,bool value){

		EasyTouchReceiver r =GetTrigger( triggerName);

		if (r!=null){
			r.enable = value;
			return true;
		}
		else{
			return false;
		}
	}

	public bool GetTriggerEnable(string triggerName){

		EasyTouchReceiver r =GetTrigger( triggerName);
		
		if (r!=null){
			return r.enable;
		}
		else{
			return false;
		}
	}

	#endregion

	#region Private Method
	private void TriggerScheduler(Engine.EasyTouch.EvtType evnt, Gesture gesture){

		foreach( EasyTouchReceiver receiver in receivers){

			if (receiver.enable && receiver.eventName == evnt){
				if (
					(receiver.restricted && ( (gesture.pickedObject == gameObject && receiver.triggerType == ETTType.Object3D ) || ( gesture.pickedUIElement == gameObject && receiver.triggerType == ETTType.UI )  )) 

					|| (!receiver.restricted && (receiver.gameObject == null || ((receiver.gameObject == gesture.pickedObject && receiver.triggerType == ETTType.Object3D ) || ( gesture.pickedUIElement == receiver.gameObject && receiver.triggerType == ETTType.UI ) ) ))

					){

					GameObject sender = gameObject;
					if (receiver.otherReceiver && receiver.gameObjectReceiver!=null){
						sender = receiver.gameObjectReceiver;
					}
					switch (receiver.parameter){
						case ETTParameter.None:
							sender.SendMessage( receiver.methodName,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.ActionTime:
							sender.SendMessage( receiver.methodName,gesture.actionTime,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Delta_Pinch:
							sender.SendMessage( receiver.methodName,gesture.deltaPinch,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Delta_Position:
							sender.SendMessage( receiver.methodName,gesture.deltaPosition,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.DeltaTime:
							sender.SendMessage( receiver.methodName,gesture.deltaTime,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Finger_Id:
							sender.SendMessage( receiver.methodName,gesture.fingerIndex,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Gesture:
							sender.SendMessage( receiver.methodName,gesture,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.PickedObject:
							if (gesture.pickedObject!=null){
								sender.SendMessage( receiver.methodName,gesture.pickedObject,SendMessageOptions.DontRequireReceiver);
							}
							break;
						case ETTParameter.PickedUIElement:
							if (gesture.pickedUIElement!=null){
								sender.SendMessage( receiver.methodName,gesture.pickedObject,SendMessageOptions.DontRequireReceiver);
							}
							break;
						case ETTParameter.Position:
							sender.SendMessage( receiver.methodName,gesture.position,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Start_Position:
							sender.SendMessage( receiver.methodName,gesture.startPosition,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Swipe_Length:
							sender.SendMessage( receiver.methodName,gesture.swipeLength,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Swipe_Type:
							sender.SendMessage( receiver.methodName,gesture.swipe,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Swipe_Vector:
							sender.SendMessage( receiver.methodName,gesture.swipeVector,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Touch_Count:
							sender.SendMessage( receiver.methodName,gesture.touchCount,SendMessageOptions.DontRequireReceiver);
							break;
						case ETTParameter.Twist_Anlge:	
							sender.SendMessage( receiver.methodName,gesture.twistAngle,SendMessageOptions.DontRequireReceiver);
							break;

					}
				}
			}
		}
	}

	private bool IsRecevier4(Engine.EasyTouch.EvtType evnt){

		int result = receivers.FindIndex(
			delegate(EasyTouchTrigger.EasyTouchReceiver e){
			return  e.eventName == evnt;
		}
		);

		if (result>-1){
			return true;
		}
		else{
			return false;
		}
	}

	private EasyTouchReceiver GetTrigger(string triggerName){
		EasyTouchTrigger.EasyTouchReceiver t = receivers.Find(
			delegate(EasyTouchTrigger.EasyTouchReceiver n){
			return  n.name == triggerName;
		}
		);

		return t;
	}

	#endregion
}
}

