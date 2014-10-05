using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boomlagoon.JSON;

public class FileManager : MonoBehaviour {

  private const string LOCAL_KEYCHAIN_FILE = "cardGameData";

  public static void SaveToKeyChain(string keyName, string mValue) {
    #if UNITY_IPHONE
      if (Application.platform == RuntimePlatform.IPhonePlayer) {
        KeyChainBindings.SetStringForKey(keyName, mValue);
      }
		#endif
		#if UNITY_ANDROID
      if (Application.platform == RuntimePlatform.Android) {
        AndroidTools.SharedPreferencesSet(keyName, mValue);
      }
		#endif
		#if UNITY_EDITOR
		  string filePath = Path.Combine(Application.persistentDataPath, LOCAL_KEYCHAIN_FILE);
		  JSONObject data;
		  if (File.Exists(filePath)) {
  			string readText = File.ReadAllText(filePath);
  			data = JSONObject.Parse(readText);
  			data.Add(keyName, mValue);
  			File.WriteAllText(filePath, data.ToString());
  		} else {
  		  data = new JSONObject();
  		  data.Add(keyName, mValue);
  		  File.WriteAllText(filePath, data.ToString());
  		}
    #endif
  }

  public static string GetFromKeyChain(string keyName) {
    #if UNITY_IPHONE
      if (Application.platform == RuntimePlatform.IPhonePlayer) {
        string val = KeyChainBindings.GetStringForKey(keyName);
        if (val == "undefined") {
          val = string.Empty;
        }
        return val;
      }
		#endif
		#if UNITY_ANDROID
      if (Application.platform == RuntimePlatform.Android) {
       return AndroidTools.SharedPreferencesGetString(keyName);
      }
		#endif
		#if UNITY_EDITOR
		  string filePath = Path.Combine(Application.persistentDataPath, LOCAL_KEYCHAIN_FILE);
		  JSONObject data;
		  if (File.Exists(filePath)) {
  			string readText = File.ReadAllText(filePath);
  			data = JSONObject.Parse(readText);
  			return data.GetString(keyName);
  		} else {
  		  return string.Empty;
  		}
    #endif
    return string.Empty;
  }
  
}
