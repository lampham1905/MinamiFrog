/***********************************************
				EasyTouch V
	Copyright © 2014-2015 The Hedgehog Team
    http://www.thehedgehogteam.com/Forum/
		
	  The.Hedgehog.Team@gmail.com
		
**********************************************/

using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Engine{
// Represents informations on Finger for touch
// Internal use only, DO NOT USE IT
public class Finger : BaseFinger{

	public float startTimeAction;
	public Vector2 oldPosition;			
	public int tapCount;				// Number of taps.
	public TouchPhase phase;			// Describes the phase of the touch.
	public EasyTouch.GestureType gesture;		
	public EasyTouch.SwipeDirection oldSwipeType;

}
}

	



