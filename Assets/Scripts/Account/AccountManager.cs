using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using Sfs2X.Entities.Data;

public class AccountManager : MonoBehaviour {
  
  public static AccountManager Instance { get; private set; }
	
  private int randomNumb = new System.Random().Next();
	
	private string mUsername = string.Empty;
	private string mPassword = string.Empty;
	private string mDisplayName = string.Empty;
	private int mCash = 0;
	private bool mIsGuest = false;
	// private JSONArray mFriends;
	
	public string username {
	  get { return mUsername; }
	  set { mUsername = value; }
	}

	public string password {
	  get { return mPassword; }
	  set { mPassword = value; }
	}
	
	public string displayName {
	  get { return mDisplayName; }
	  set { mDisplayName = value; }
	}
	
	public int cash {
	  get { return mCash ^ randomNumb; }
	  set { mCash = value ^ randomNumb; }
	}
	
	public bool isGuest {
	  get { return mIsGuest; }
	  set { mIsGuest = value; }
	}
	
	// public JSONArray friends {
	//   get { return mFriends; }
	//   set { mFriends = value; }
	// }
	
	void Awake() {
		Instance = this;
	}

	public string GetUserId() {
		return "Guest";
	}
	
	public void SetUser(JSONObject user) {
	  username = user.GetString("username");
		password = user.GetString("password");
		displayName = user.GetString("displayName");
		cash = user.GetInt("cash");
		// friends = user.GetArray("friends");
	}
	
	public void UpdateUserCash(int updateVal) {
	  cash = Mathf.Max(0, cash + updateVal);
	}
	
	public bool IsYou(string username) {
	  return username == this.username;
	}
	
	public bool IsFriend(string username) {
		return SmartfoxClient.Instance.IsContainBuddy(username);
	}
	
	private bool CanAddFriend(string username) {
		int count = SmartfoxClient.Instance.GetBuddyList().Count;
	  return count < Global.MAX_FRIEND_NUMB;
	}
	
	public void AddFriend(string username) {
	  if (CanAddFriend(username)) {
	    UserExtensionRequest.Instance.AddFriend(username);
	  } else {
      HUDManager.Instance.AddFlyText(Localization.Get(ErrorCode.USER.MAX_FRIENDS.ToString()), Vector3.zero, 40, Color.red, 0, 4f);
	  }
	}
	
	public void Register(JSONObject jsonData) {
	  SmartfoxClient.Instance.Register(jsonData);
	}
	
	public void RegisterAsGuest() {
	  SmartfoxClient.Instance.RegisterAsGuest();
	}
	
  // void HandleErrorCode(ErrorCode.USER errorCode) {
  //   Debug.Log("HandleErrorCode " + errorCode);
  // }
  // 
  // private ISFSObject CreateExtensionObject(JSONObject jsonData) {
  //     ISFSObject objOut = new SFSObject();
  //     objOut.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
  //     return objOut;
  // }
  // 
  // private ServerRequest CreateExtensionRequest(string commandId, string successCallback, JSONObject jsonData = null) {
  //  if (jsonData == null) {
  //    jsonData = new JSONObject();
  //  }
  // 
  //  ISFSObject requestData = CreateExtensionObject(jsonData);
  //  ServerRequest serverRequest = new ServerRequest(ServerRequest.Type.EXTENSION,
  //                          Command.Create(GameId.USER, commandId),
  //                          requestData,
  //                          gameObject,
  //                          successCallback);
  //  return serverRequest;
  // }
}