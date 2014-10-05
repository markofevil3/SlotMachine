using UnityEngine;
using System.Collections;

public class AndroidTools {
	
#if UNITY_ANDROID
 	public static AndroidJavaClass androidPlugin;
	public static AndroidJavaObject androidObject;
#endif
	
	public static void SetLocalNotification(int timer, string message) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
		  androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LocalNotification");
	    androidPlugin.CallStatic("SetLocalNotification", timer, message);
		}
		#endif
  }
  
  public static void CancelAllLocalNotifications() {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LocalNotification");
	    androidPlugin.CallStatic("CancelAllLocalNotifications");
		}
		#endif
  }
  
  public static void SharedPreferencesSet(string key, string value) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	    androidPlugin.CallStatic("Set", key, value);
		}
		#endif
  }
  
  public static void SharedPreferencesSet(string key, int value) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
		  androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	    androidPlugin.CallStatic("Set", key, value);
		}
		#endif
  }
  
  public static void SharedPreferencesSet(string key, float value) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	    androidPlugin.CallStatic("Set", key, value);
		}
		#endif
  }
  
  public static void SharedPreferencesSet(string key, long value) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
		  androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	    androidPlugin.CallStatic("Set", key, value);
		}
		#endif
  }
  
  public static void SharedPreferencesSet(string key, bool value) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	    androidPlugin.CallStatic("Set", key, value);
		}
		#endif
  }
  
  public static string SharedPreferencesGetString(string key) {
  	string val = "";
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	  	val = androidPlugin.CallStatic<string>("GetString", key);
		}
		#endif
		return val;
  }
    
  public static int SharedPreferencesGetInt(string key) {
  	int val = -1;
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	  	val = androidPlugin.CallStatic<int>("GetInt", key);
		}
		#endif
		return val;
  }
  
  public static long SharedPreferencesGetLong(string key) {
  	long val = 0;
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	  	val = androidPlugin.CallStatic<long>("GetLong", key);
		}
		#endif
		return val;
  }
  
  public static float SharedPreferencesGetFloat(string key) {
  	float val = 0;
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	  	val = androidPlugin.CallStatic<float>("GetFloat", key);
		}
		#endif
		return val;
  }
  
  public static bool SharedPreferencesGetBool(string key) {
  	bool val = false;
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.alleylabs.MyAndroidPlugins.LumbaSharedPreferences");
	  	val = androidPlugin.CallStatic<bool>("GetBool", key);
		}
		#endif
		return val;
  }

	public static void ShowAlert(string title, string message, string positiveButton) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			androidObject = androidPlugin.GetStatic<AndroidJavaObject>("currentActivity");
			androidObject.CallStatic("ShowAlert", title, message, positiveButton);
		}
		#endif
	}

	public static void ShowAlert(string title, string message, string positiveButton, string negativeButton) {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			androidObject = androidPlugin.GetStatic<AndroidJavaObject>("currentActivity");
			androidObject.CallStatic("ShowAlert", title, message, positiveButton, negativeButton);
		}
		#endif
	}

	public static void HideAlert() {
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			androidPlugin = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			androidObject = androidPlugin.GetStatic<AndroidJavaObject>("currentActivity");
			androidObject.CallStatic("HideAlert");
		}
		#endif
	}
}
