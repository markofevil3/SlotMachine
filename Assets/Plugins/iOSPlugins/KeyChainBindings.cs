using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class KeyChainBindings {
	static public void SetStringForKey(string keyName, string value) {
		#if UNITY_IPHONE
    IOSSetStringForKey(value, keyName);
    #endif
  }
  
  static public string GetStringForKey(string keyName) {
  	#if UNITY_IPHONE
	    return IOSGetStringForKey(keyName);
	   #else
	   	return "";
	   #endif
  }
  
  static public void RemoveObjectForKey(string keyName) {
  	#if UNITY_IPHONE
	    IOSRemoveObjectForKey(keyName);
	   #endif
  }

  //---------------------------------------------------------------------------
  //  IOS NATIVE INTERFACE
  //---------------------------------------------------------------------------
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	extern static private void IOSSetStringForKey(string value, string keyName);
  
  [DllImport ("__Internal")]
  extern static private string IOSGetStringForKey(string keyName);
  
  [DllImport ("__Internal")]
  extern static private void IOSRemoveObjectForKey(string keyName);
#endif // UNITY_IPHONE
}
