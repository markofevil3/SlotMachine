using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using Sfs2X.Entities;

public class MyFacebook : MonoBehaviour {
	
	public static MyFacebook Instance;

	public bool IsLoggedIn {
		get {
			return FB.IsLoggedIn;
		}
	}

	void Awake() {
		Instance = this;
	}

	public void Init() {
		if (!FB.IsInitialized) {
	    FB.Init(OnInitComplete);
		}
	}

	void OnInitComplete() {
    Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		if (FB.IsLoggedIn) {
			// LoadUserProfile();
		} else {
		}
	}
	
	// {"id":"10202684447992635","name":"Bui Phi Quan","picture":{"data":{"is_silhouette":false,"url":"https:\/\/fbcdn-profile-a.akamaihd.net\/hprofile-ak-xpf1\/v\/t1.0-1\/p50x50\/11165318_10202675741974990_4092081472674314476_n.jpg?oh=63d166916d7e176464a0787efe97832f&oe=55D27A28&__gda__=1439402958_a03252d9e3042d454b93f3b0b0257106"}}}
	
	
	public void Login() {
    PopupManager.Instance.ShowLoadingPopup("LoadingText_Login");
    FB.Login("public_profile,email,user_friends", LoginCallback);
	}
	
	public void LogOut() {
		FB.Logout();
	}
	
	void LoginCallback(FBResult result) {
		string lastResponse = "";
    if (result.Error != null) {
	    lastResponse = "Error Response:\n" + result.Error;
		  PopupManager.Instance.CloseLoadingPopup();
		} else if (!FB.IsLoggedIn) {
      lastResponse = "Login cancelled by Player";
		  PopupManager.Instance.CloseLoadingPopup();
    } else {
      lastResponse = "Login was successful! " + FB.UserId;;
			AccountManager.Instance.fbId = FB.UserId;
			LoadUserProfile();
    }
		Debug.Log(lastResponse);
	}
	
	public void LoadUserProfile() {
    FB.API("/me?fields=id,name,birthday,picture.width(128).height(128),email", Facebook.HttpMethod.GET, LoadUserProfileCallback);     
	}
	
	void LoadUserProfileCallback(FBResult result) {
    if (result.Error != null) {
			Debug.Log("LoadUserProfileCallback " + result.Error);
		  PopupManager.Instance.CloseLoadingPopup();
			return;
		}  
		JSONObject userData = JSONObject.Parse(result.Text);
		AccountManager.Instance.avatarLink = userData.GetObject("picture").GetObject("data").GetString("url").Replace("\\", "");
		AccountManager.Instance.displayName = userData.GetString("name");
		AccountManager.Instance.email = userData.ContainsKey("email") ? userData.GetString("email") : string.Empty;
		AccountManager.Instance.LoginUsingFB(FB.UserId, AccountManager.Instance.displayName, AccountManager.Instance.avatarLink, AccountManager.Instance.email);
		userData = null;
	}
	
	public void InviteFriends() {
		if (FB.IsLoggedIn) {
	    PopupManager.Instance.ShowLoadingPopup("LoadingText_GettingFbFriends");
      List<object> friendSelectorFilters = new List<object>();
      friendSelectorFilters.Add("app_non_users");
	    FB.AppRequest(
        to: null,
        filters : friendSelectorFilters,
        excludeIds : null,
        message: "Friend Smash is smashing! Check it out.",
        title: "Play Friend Smash with me!",
        callback:InviteFriendsCallback
	    );   
		} else {
			HUDManager.Instance.AddFlyText(Localization.Get("InviteFriend_AskForLogin"), Vector3.zero, 40, Color.red, 0, 2f);
		}
	}
	
	void InviteFriendsCallback(FBResult result) {
	  PopupManager.Instance.CloseLoadingPopup();
    if (result.Error == null) {
      HUDManager.Instance.AddFlyText(Localization.Get("InviteFriend_Success"), Vector3.zero, 40, Color.green, 0, 2f);
		}
		Debug.Log("InviteFriendsCallback " + result.Text);
	}
	
	public void LoadFBFriends() {
		FB.API("/me?fields=friends.limit(50).fields(id,name)", Facebook.HttpMethod.GET, LoadFBFriendsCallback);  
	}
	
	void LoadFBFriendsCallback(FBResult result) {
		if (result.Error != null)  {
      HUDManager.Instance.AddFlyText(Localization.Get("LoadFBFriend_Fail"), Vector3.zero, 40, Color.red, 0, 4f);
		} else {
			JSONObject friendsData = JSONObject.Parse(result.Text);
			JSONArray ids = friendsData.GetObject("friends").GetArray("data");
			JSONArray fbIds = new JSONArray();
			List<Buddy> buddyList = SmartfoxClient.Instance.GetBuddyList();
			for (int i = 0; i < ids.Length; i++) {
				string fbId = ids[i].Obj.GetString("id");
				bool added = false;
				for (int j = 0; j < buddyList.Count; j++) {
					if (fbId == buddyList[j].GetVariable("$facebookId").GetStringValue()) {
						added = true;
					}
				}
				if (!added) {
					fbIds.Add(fbId);
				}
			}
			if (fbIds.Length > 0) {
				UserExtensionRequest.Instance.AddFbFriends(fbIds);
			}
			friendsData = null;
			ids = null;
			fbIds = null;
		}        
	}
	
	
	
	
	//   public delegate void LoadPictureCallback (string fbId, string url);
	//
	// public void GetAvatarByFbId(string fbId, LoadPictureCallback callback) {
	// 	Debug.Log("GetAvatarByFbId----------- " + fbId);
	// 	string url = GetPictureURL(fbId, 128, 128);
	//     FB.API(url,Facebook.HttpMethod.GET,result => {
	//     if (result.Error != null) {
	//       Debug.Log("LoadAvatarByFbId " + result.Error);
	//       return;
	//     }
	// 		JSONObject jsonData = JSONObject.Parse(result.Text);
	//       callback(fbId, jsonData.GetObject("data").GetString("url").Replace("\\", ""));
	//     });
	// }
	//
	// IEnumerator LoadPictureEnumerator(string url, UITexture uiTexture) {
	//     WWW www = new WWW(url);
	//     yield return www;
	// 	if (www.texture != null) {
	// 		uiTexture.mainTexture = www.texture;
	// 	}
	// 	www.Dispose();
	// }
	//
	//   private string GetPictureURL(string fbId, int? width = null, int? height = null, string type = null) {
	//     string url = string.Format("/{0}/picture", fbId);
	//     string query = width != null ? "&width=" + width.ToString() : "";
	//     query += height != null ? "&height=" + height.ToString() : "";
	//     query += type != null ? "&type=" + type : "";
	//     query += "&redirect=false";
	//     if (query != "") url += ("?g" + query);
	//     return url;
	//   }
}
