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
[AddComponentMenu("EasyTouch/Quick Tap")]
public class QuickTap : QuickBase {

	#region Events
	[System.Serializable] public class OnTap : UnityEvent<Gesture>{}
	
	[SerializeField] 
	public OnTap onTap;
	#endregion

	#region Enumeration
	public enum ActionTriggering {Simple_Tap,Double_Tap};
	#endregion

	#region Members
	public ActionTriggering actionTriggering;
	private Gesture currentGesture;
	#endregion

	#region Monobehavior CallBack
	public QuickTap(){
			quickActionName = "QuickTap"+ System.Guid.NewGuid().ToString().Substring(0,7);
	}

	void Update(){
		currentGesture = Engine.EasyTouch.current;
		
		if (!is2Finger){
			if (currentGesture.type == Engine.EasyTouch.EvtType.On_DoubleTap && actionTriggering == ActionTriggering.Double_Tap){
				DoAction(currentGesture);
			}
			
			if (currentGesture.type == Engine.EasyTouch.EvtType.On_SimpleTap && actionTriggering== ActionTriggering.Simple_Tap){
				DoAction(currentGesture);
			}
			
		}
		else{
			
			if (currentGesture.type == Engine.EasyTouch.EvtType.On_DoubleTap2Fingers && actionTriggering== ActionTriggering.Double_Tap){
				DoAction(currentGesture);
			}
			
			if (currentGesture.type == Engine.EasyTouch.EvtType.On_SimpleTap2Fingers && actionTriggering== ActionTriggering.Simple_Tap){
				DoAction(currentGesture);
			}
		}
	}
	#endregion

	void DoAction(Gesture gesture){
		if ( realType == GameObjectType.UI){
			if (gesture.isOverGui ){
				if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf( transform))){
					onTap.Invoke( gesture);
				}
			}
		}
		else{
			if ((!enablePickOverUI && gesture.pickedUIElement == null) || enablePickOverUI){
				if (Engine.EasyTouch.GetGameObjectAt( gesture.position,is2Finger) == gameObject){
					onTap.Invoke( gesture);
				}
			}
		}
	}
}
}
