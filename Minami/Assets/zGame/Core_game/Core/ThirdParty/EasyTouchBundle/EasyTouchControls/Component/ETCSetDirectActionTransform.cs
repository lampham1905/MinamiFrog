using Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouchControls.Plugins;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.ThirdParty.EasyTouchBundle.EasyTouchControls.Component
{
	[AddComponentMenu("EasyTouch Controls/Set Direct Action Transform ")]
	public class ETCSetDirectActionTransform : MonoBehaviour {

		public string axisName1;
		public string axisName2;

		void Start(){
			if (!string.IsNullOrEmpty(axisName1)){
				ETCInput.SetAxisDirecTransform(axisName1, transform);
			}

			if (!string.IsNullOrEmpty(axisName2)){
				ETCInput.SetAxisDirecTransform(axisName2, transform);
			}
		}
	}
}
