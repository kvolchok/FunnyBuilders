using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
   [SerializeField] private Animator _doorAnimator;
   [SerializeField] private float _doorOpenSpeed = 1.5f;
   [SerializeField] private float _doorCloseSpeed = 0.3f;
   private static readonly int IsOpen = Animator.StringToHash("_isOpen");
  
   private void OnTriggerEnter(Collider other)
   {
      _doorAnimator.speed =  _doorOpenSpeed ; 
      _doorAnimator.SetBool(IsOpen,true);
   }

   private void OnTriggerExit(Collider other)
   {
      _doorAnimator.speed = _doorCloseSpeed;
      _doorAnimator.SetBool(IsOpen,false);
   }
}
