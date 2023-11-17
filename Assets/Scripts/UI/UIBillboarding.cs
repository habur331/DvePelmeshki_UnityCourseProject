using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
   private Camera _mainCamera;

   private void Start()
   {
      _mainCamera = Camera.main;
   }

   private void Update()
   {
      transform.forward = _mainCamera.transform.forward;
   }
}
