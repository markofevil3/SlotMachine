using UnityEngine;
using System.Collections;

public class Global {
  
  public enum AtlasType {
    UHD,
    HD,
    SD
  }
  
	// Variables
	public static string UI_HD_PATH = "Atlas/UI_HD";
	public static string UI_UHD_PATH = "Atlas/UI_UHD";
	public static string UI_SD_PATH = "Atlas/UI_SD";
	public static string SCREEN_PATH = "Prefabs/Screens";
	public static string POPUP_PATH = "Prefabs/Popups";
	public static string USER_BOTTOM_BAR_PREFAB = Global.SCREEN_PATH + "/UserBottomBar";
  
  public static bool isOpenPopup = false;
  public static bool isAnimatingPopup = false;
  public static bool isTablet = false;
  
  public static int MAX_FRIEND_NUMB = 50;
	public static int DAILY_REWARD_MILI = 300000;
  public static System.Random systemRandom;
  public static UnityEngine.Random unityRandom;
	
	public static void Init() {
	  systemRandom = new System.Random();
	  unityRandom = new UnityEngine.Random();
	}
	
	public string GetAtlasPath() {
	  if (Utils.IsUHD()) {
	    return UI_UHD_PATH;
	  }
	  if (Utils.IsSD()) {
	    return UI_SD_PATH;
	  }
	  return UI_HD_PATH;
	}
	
	public static void Reset() {
	  isOpenPopup = false;
	  isAnimatingPopup = false;
	}
}