using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   private void Awake()
   {
      ManagerEvent.RegEvent(EventCMD.MOVECAMERATOTARGET, MoveToTarget);
   }

   private void MoveToTarget(object e)
   {
      Vector3 target = (Vector3)e;
      LeanTween.move(this.gameObject, target, 1f).setEase(LeanTweenType.linear);
   }
}
