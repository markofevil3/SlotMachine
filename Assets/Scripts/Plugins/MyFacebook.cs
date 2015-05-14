using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

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
    FB.Login("public_profile,email,user_friends", LoginCallback);
	}
	
	void LoginCallback(FBResult result) {
		string lastResponse = "";
    if (result.Error != null) {
	    lastResponse = "Error Response:\n" + result.Error;
		} else if (!FB.IsLoggedIn) {
      lastResponse = "Login cancelled by Player";
    } else {
      lastResponse = "Login was successful! " + FB.UserId;;
			AccountManager.Instance.fbId = FB.UserId;
			LoadUserProfile();
    }
		Debug.Log(lastResponse);
	}
	
	public void LoadUserProfile() {
    FB.API("/me?fields=id,name,birthday,picture,email", Facebook.HttpMethod.GET, LoadUserProfileCallback);     
	}
	
	void LoadUserProfileCallback(FBResult result) {
    if (result.Error != null) {
			Debug.Log("LoadUserProfileCallback " + result.Error);
			return;
		}  
		JSONObject userData = JSONObject.Parse(result.Text);
		AccountManager.Instance.avatarLink = userData.GetObject("picture").GetObject("data").GetString("url").Replace("\\", "");
		AccountManager.Instance.displayName = userData.GetString("name");
		AccountManager.Instance.email = userData.ContainsKey("email") ? userData.GetString("email") : string.Empty;
		// TO DO - send request login or register to server
		AccountManager.Instance.LoginUsingFB(FB.UserId, AccountManager.Instance.displayName, AccountManager.Instance.avatarLink, AccountManager.Instance.email);
		
		userData = null;
	}
}
