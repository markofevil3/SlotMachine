using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class OpenUDID {
  static public string Get() {
  	#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return GetOpenUDID();
			}
			return "";
	   #else
	   	return "";
	   #endif
  }

  //---------------------------------------------------------------------------
  //  IOS NATIVE INTERFACE
  //---------------------------------------------------------------------------
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	extern static private string GetOpenUDID();
#endif // UNITY_IPHONE
}
