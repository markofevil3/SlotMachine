using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class Utils : MonoBehaviour {
  
  public static bool IsTablet() {
		#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return SystemInfo.deviceModel.IndexOf("ipad", StringComparison.OrdinalIgnoreCase) > -1;
			}
		#endif
		#if UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android) {
				DisplayMetricsAndroid.Init();
				return DisplayMetricsAndroid.IsTablet();
			}
		#endif
		return true;
	}
  
  public static bool IsUHD() {
    if (Screen.width >= 2048) {
      return true;
    } else {
      return false;
    }
  }
  
  public static bool IsSD() {
    if (Screen.width <= 480) {
      return true;
    } else {
      return false;
    }
  }

  public static ByteArray ToByteArray(string stringInput) {
    return new ByteArray(Encoding.UTF8.GetBytes(stringInput));
  }
  
  public static string FromByteArray(ByteArray byteArray) {
    return Encoding.UTF8.GetString(byteArray.Bytes);
  }
  
  public static string GetSortCharacters(int num) {
		if (num < 10) {
			return "00" + num;
		} else if (num >= 10 && num < 100){
			return "0" + num;
		} else {
			return num + "";
		}
	}
	
	public static DateTime CurrentTime() {
	  return DateTime.UtcNow;
	}
	
	public static string ChatEscape(string text) {
	  return Uri.EscapeDataString(text);
	}
	
	public static string ChatUnescape(string text) {
	  return Uri.UnescapeDataString(text);
	}
	
	private static string chars = "abcdefghijklmnopqrstuvwxyz_";
	private static System.Random random = new System.Random();
	
	public static string RandomUsername(int length) {
	  string username = "";
	  for (int i = 0; i < length; i++) {
      username += chars[random.Next(chars.Length)];
    }
    return username;
	}
	
	public static string ArrIntToString(int[] arr) {
	  string log = "";
	  for (int i = 0; i < arr.Length; i++) {
	    log += arr[i] + ((i == arr.Length - 1) ? "" : ",");
	  }
	  return log;
	}
}

static class MyExtensions
{
	// shuffle a list
	public static void Shuffle<T>(this IList<T> list)  {
    int n = list.Count;  
    while (n > 1) {  
      n--;  
      int k = Global.systemRandom.Next(n + 1);  
      T value = list[k];  
      list[k] = list[n];  
      list[n] = value;  
    }  
  }
}