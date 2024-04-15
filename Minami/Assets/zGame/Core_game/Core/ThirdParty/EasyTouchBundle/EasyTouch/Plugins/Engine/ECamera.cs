/***********************************************
				EasyTouch V
	Copyright © 2014-2015 The Hedgehog Team
    http://www.thehedgehogteam.com/Forum/
		
	  The.Hedgehog.Team@gmail.com
		
**********************************************/

using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouch.Plugins.Engine{
[System.Serializable]
public class ECamera{

	public Camera camera;
	public bool guiCamera;
	
	public ECamera( Camera cam,bool gui){
		this.camera = cam;
		this.guiCamera = gui;
	}
	
}
}
