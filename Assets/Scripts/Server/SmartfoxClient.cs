using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Requests.Game;
using Sfs2X.Requests.Buddylist;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Invitation;
using Sfs2X.Util;

using Boomlagoon.JSON;

public class SmartfoxClient : MonoBehaviour {
	
	public static SmartfoxClient Instance { get; private set; }
	
	private const string DEFAULT_PASSWORD = "default";
	private const string DEFAULT_GUEST_NAME = "I am Rich";
	
	private SmartFox client;
	private ServerRequest currentRequest;
	private string userId;
	private bool isRequesting;
	private bool isManualLogin = false;
	private bool isConnected = false;
	private bool isLoggedIn = false;

	private bool shouldCallRegisterAsGuest = false;
	private bool shouldCallLoginUsingFB = false;
	private bool shouldCallLoginUser = false;
	private JSONObject jsonDataToSend;

  public bool IsLoggedIn {
    get { return isLoggedIn; }
  }

	void Awake() {
		Instance = this;
		ServerRequestQueue.Init();
	}

	void Start() {
		// userId = AccountManager.Instance.GetUserId();
		if (FileManager.GetFromKeyChain("username") != string.Empty || FileManager.GetFromKeyChain("guestId") != string.Empty) {
	    Connect(null);
		}
	}

  public void Register(JSONObject user) {
    if (client != null) {
      PopupManager.Instance.ShowLoadingPopup("LoadingText_Register");
      user.Add("isRegister", true);
      user.Add("isGuest", false);
      ISFSObject loginData = new SFSObject();
  		loginData.PutByteArray("jsonData", Utils.ToByteArray(user.ToString()));
      client.Send(new LoginRequest(user.GetString("username"), user.GetString("password"), "Gamble", loginData));
    } else {
      Utils.Log("NO CONNECTION!");
    }
  }

  public void RegisterAsGuest() {
		if (isConnected) {
	    PopupManager.Instance.ShowLoadingPopup("LoadingText_LoginGuest");
	    JSONObject user = new JSONObject();
	    string userId = FileManager.GetFromKeyChain("guestId");
	    if (userId == string.Empty) {
	      userId = Guid.NewGuid().ToString().Replace("-", "");
	      user.Add("isRegister", true);
	      Utils.Log("RegisterAsGuest " + userId);
	    } else {
	      user.Add("isRegister", false);
	      Utils.Log("LoginAsGuest " + userId);
	    }
    
	    // FAKE create fake user -- remove when publish
	    #if UNITY_EDITOR
	      userId = Guid.NewGuid().ToString().Replace("-", "");
	      user.Add("isRegister", true);
	    # endif
	    //
    
	    user.Add("isGuest", true);
	    user.Add("username", userId);
	    user.Add("password", DEFAULT_PASSWORD);
	    user.Add("displayName", DEFAULT_GUEST_NAME);
	    ISFSObject loginData = new SFSObject();
	    loginData.PutByteArray("jsonData", Utils.ToByteArray(user.ToString()));
	    client.Send(new LoginRequest(userId, DEFAULT_PASSWORD, "Gamble", loginData));
		} else {
			shouldCallRegisterAsGuest = true;
			Connect(null);
		}
  }

	public void LoginUsingFB(string mFBId, string mDisplayName, string mAvatarLink, string mEmail) {
    JSONObject jsonData = new JSONObject();
    jsonData.Add("isFBLogin", true);
    jsonData.Add("guestId", FileManager.GetFromKeyChain("guestId"));
    jsonData.Add("username", FileManager.GetFromKeyChain("username"));
    jsonData.Add("password", DEFAULT_PASSWORD);
    jsonData.Add("displayName", mDisplayName);
    jsonData.Add("avatar", mAvatarLink);
    jsonData.Add("email", mEmail);
    jsonData.Add("fbId", mFBId);
		if (isConnected) {
			Utils.Log("LoginUsingFB--- " + jsonData.ToString());
	    PopupManager.Instance.ShowLoadingPopup("LoadingText_Login");
	    ISFSObject loginData = new SFSObject();
	    loginData.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
	    client.Send(new LoginRequest(mFBId, DEFAULT_PASSWORD, "Gamble", loginData));
		} else {
			shouldCallLoginUsingFB = true;
			Connect(jsonData);
		}
	}

  public void LoginUser(string username, string password, bool isManual = true) {
    JSONObject jsonData = new JSONObject();
    jsonData.Add("isRegister", false);
    jsonData.Add("isGuest", false);
    jsonData.Add("username", username);
    jsonData.Add("password", password);
		if (isConnected) {
	    PopupManager.Instance.ShowLoadingPopup("LoadingText_Login");
	    ISFSObject loginData = new SFSObject();
	    loginData.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
	    isManualLogin = isManual;
	    client.Send(new LoginRequest(username, password, "Gamble", loginData));
		} else {
			shouldCallLoginUser = true;
			Connect(jsonData);
		}
  }
  
  public void LogoutUser() {
    if (client != null) {
      client.Send(new LogoutRequest());
    }
		
		// TEST CODE - logout facebook
		if (MyFacebook.Instance.IsLoggedIn) {
			MyFacebook.Instance.LogOut();
		}
  }
  
	public void Connect(JSONObject jsonData) {
		jsonDataToSend = jsonData;
		PopupManager.Instance.ShowLoadingPopup("LoadingText_Connecting");
		client = new SmartFox();
		client.ThreadSafeMode = true;
		client.UseBlueBox = false;
		client.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		client.AddEventListener(SFSEvent.LOGIN, OnLogin);
		client.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		client.AddEventListener(SFSEvent.LOGOUT, OnLogout);
		client.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
		client.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnJoinRoomError);
		client.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		client.AddEventListener(SFSEvent.PRIVATE_MESSAGE, OnPrivateMessage);
		client.AddEventListener(SFSEvent.MODERATOR_MESSAGE, OnModeratorMessage);
		client.AddEventListener(SFSEvent.ADMIN_MESSAGE, OnAdminMessage);
		client.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		client.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		client.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, OnRoomVarsUpdate);
		client.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		client.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		client.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVarsUpdate);
		client.AddEventListener(SFSEvent.INVITATION, OnInvitationReceived);
		client.AddEventListener(SFSBuddyEvent.BUDDY_LIST_INIT, OnBuddyInited);
    client.AddEventListener(SFSBuddyEvent.BUDDY_VARIABLES_UPDATE, OnBuddyVariablesUpdate);
    
		
    // client.Connect("54.255.173.193", 9933);
    #if UNITY_EDITOR
    client.Connect("127.0.0.1", 9933);
    # else
    client.Connect("14.162.92.31", 9933);
    # endif
    // walk around for custom error code from server
    SFSErrorCodes.SetErrorMessage(2, "{0}");
	}

	void OnConnection(BaseEvent e) {
		if ((bool)e.Params["success"]) {
		  isConnected = true;
		  Utils.Log("Connect Success " + AccountManager.Instance.username + " " + AccountManager.Instance.password);
			if (shouldCallRegisterAsGuest) {
				shouldCallRegisterAsGuest = false;
				RegisterAsGuest();
				return;
			}
			if (shouldCallLoginUsingFB) {
				shouldCallLoginUsingFB = false;
				LoginUsingFB(jsonDataToSend.GetString("fbId"), jsonDataToSend.GetString("displayName"), jsonDataToSend.GetString("avatar"), jsonDataToSend.GetString("email"));
				jsonDataToSend = null;
				return;
			}
			if (shouldCallLoginUser) {
				shouldCallLoginUser = false;
				LoginUser(jsonDataToSend.GetString("username"), jsonDataToSend.GetString("password"));
				jsonDataToSend = null;
				return;
			}
		  if (AccountManager.Instance.username != string.Empty) {
		    LoginUser(AccountManager.Instance.username, AccountManager.Instance.password, false);
		  } else {
				Utils.Log("OnConnection " + FileManager.GetFromKeyChain("username"));
		    if (FileManager.GetFromKeyChain("username") != string.Empty) {
		      LoginUser(FileManager.GetFromKeyChain("username"), FileManager.GetFromKeyChain("password"), false);
				} else {
				  PopupManager.Instance.CloseLoadingPopup();
				}
		  }
		} else {
		  PopupManager.Instance.CloseLoadingPopup();
	    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_RELOAD_GAME);
		  Utils.Log("Connect FAIL!");
		}
	}

	void OnConnectionLost(BaseEvent e) {
	  isConnected = false;
	  Debug.LogError("Connection is lost, reason:" + e.Params["reason"]);
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_RELOAD_GAME);
	  // HUDManager.Instance.AddFlyText("Connection is lost, reason:" + e.Params["reason"], Vector3.zero, 40, Color.red, 0, 4f);
	}

	public void Restart() {
		isManualLogin = false;
		isLoggedIn = false;
		shouldCallRegisterAsGuest = false;
		shouldCallLoginUsingFB = false;
		shouldCallLoginUser = false;
		jsonDataToSend = null;
		PopupManager.Instance.Restart();
		ScreenManager.Instance.Restart();
		AccountManager.Instance.LogOut();
		Connect(null);
	}

	void OnLogin(BaseEvent e) {
	  PopupManager.Instance.CloseLoadingPopup();
	  isLoggedIn = true;
		ISFSObject loginData = (ISFSObject)e.Params["data"];
		JSONObject user = JSONObject.Parse(Utils.FromByteArray(loginData.GetByteArray("jsonData")));
		Utils.Log("OnLogin------ " + user.ToString());
		AccountManager.Instance.SetUser(user);
		if (ScreenManager.Instance.LobbyScreen != null) {
		  ScreenManager.Instance.LobbyScreen.EventLoggedIn();
		}
		if (user.GetBoolean("isRegister") || isManualLogin) {
      bool mIsGuest = user.ContainsKey("isGuest") && user.GetBoolean("isGuest");
      string mUsername = user.GetString("username");
		  FileManager.SaveToKeyChain("username", mUsername);
		  FileManager.SaveToKeyChain("password", user.GetString("password"));
		  FileManager.SaveToKeyChain("fbId", AccountManager.Instance.fbId);
		  FileManager.SaveToKeyChain("isGuest", mIsGuest.ToString());
		  if (mIsGuest && !isManualLogin) {
		    FileManager.SaveToKeyChain("guestId", mUsername);
		  }
		  isManualLogin = false;
		}
		
		// Load fb friend to add to user friend list
		if (AccountManager.Instance.fbId != string.Empty && MyFacebook.Instance.IsLoggedIn) {
			MyFacebook.Instance.LoadFBFriends();
		}
	}

	void OnLoginError(BaseEvent e) {
	  isLoggedIn = false;
	  PopupManager.Instance.CloseLoadingPopup();
	  ISFSObject objIn = (SFSObject)e.Params["data"];
	  int errorCode = int.Parse((string)e.Params["errorMessage"]);
	  
	  HUDManager.Instance.AddFlyText(((ErrorCode.USER)errorCode).ToString(), Vector3.zero, 40, Color.red);
    if (ScreenManager.Instance.LobbyScreen != null) {
			Utils.Log("############");
      ScreenManager.Instance.LobbyScreen.EventLoginFail();
		}
		
		Utils.Log("LogIn error: " + ((ErrorCode.USER)errorCode));
	}
  
  void OnLogout(BaseEvent e) {
    Utils.Log("Log out");
    isLoggedIn = false;
	  if (AccountManager.Instance != null) {
	  	AccountManager.Instance.LogOut();
	  }
    PopupManager.Instance.CloseLoadingPopup();
    if (ScreenManager.Instance.LobbyScreen != null) {
      ScreenManager.Instance.LobbyScreen.EventLogoutSuccess();
    }
  }
  
	void OnJoinRoom(BaseEvent e) {
    Utils.Log("#### OnJoinRoom");
	}

	void OnJoinRoomError(BaseEvent e) {
    Utils.Log("#### OnJoinRoomError");
	}

  void OnUserEnterRoom(BaseEvent e) {
    User enterUser = ((User)e.Params["user"]);
    Room room = (Room)e.Params["room"];
  	if (!enterUser.IsItMe && room.GroupId != "lobby" && ScreenManager.Instance.CurrentSlotScreen != null) {
  	  JSONObject userData = new JSONObject();
  	  userData.Add("username", enterUser.Name);
  	  userData.Add("cash", enterUser.GetVariable("cash").GetIntValue());
  	  userData.Add("displayName", enterUser.GetVariable("displayName").GetStringValue());
  	  Utils.Log("OnUserEnterRoom --- " + userData.ToString());
  	  
  	  ScreenManager.Instance.CurrentSlotScreen.OnPlayerJoinRoom(room.Name, userData);
  	}
  }

  void OnUserExitRoom(BaseEvent e) {
    User leaveUser = ((User)e.Params["user"]);
    Room room = (Room)e.Params["room"];
    
  	if (!leaveUser.IsItMe && room.GroupId != "lobby" && ScreenManager.Instance.CurrentSlotScreen != null) {
  	  ScreenManager.Instance.CurrentSlotScreen.OnPlayerLeaveRoom(room.Name, leaveUser.Name);
  	}
    Utils.Log("OnUserExitRoom " + e.Params["room"] + " " + e.Params["user"]);
  }

	void OnPublicMessage(BaseEvent e) {
	  // foreach (DictionaryEntry entry in e.Params) {
	  //   Utils.Log(entry.Key + " " + entry.Value);
	  //   	}
  	User sender = ((User)e.Params["sender"]);
		ISFSObject objIn = (SFSObject)e.Params["data"];
		string cmd = objIn.GetUtfString("cmd");
		switch(cmd) {
			case Command.USER.CHAT_IN_ROOM:
		  	if (ScreenManager.Instance.CurrentSlotScreen != null && !sender.IsItMe) {
		      JSONObject data = new JSONObject();
		      data.Add("message", e.Params["message"].ToString());
		      data.Add("senderId", sender.Name);
		      data.Add("senderName", sender.GetVariable("displayName").GetStringValue());
		  	  ScreenManager.Instance.CurrentSlotScreen.inGameChatBar.AddChatToList(data);
		  	}
			break;
			case Command.SLOT_MACHINE.SLOT_PLAY:
				Room room = e.Params["room"] as Room;
				JSONObject jsonData = JSONObject.Parse(Utils.FromByteArray(objIn.GetByteArray("jsonData")));
		  	if (ScreenManager.Instance.CurrentSlotScreen != null && !sender.IsItMe && room.Name == ScreenManager.Instance.CurrentSlotScreen.GetRoomId()) {
					ScreenManager.Instance.CurrentSlotScreen.OtherPlayerSpinResult(sender.Name, jsonData);
				}
			break;
		}
    // Utils.Log("OnPublicMessage + " + e.Params.ToString());
    //    JSONObject messageJson = JSONObject.Parse(e.Params["message"].ToString());
    //    Utils.Log("OnPublicMessage " + messageJson.ToString());
	}

	void OnPrivateMessage(BaseEvent e) {

	}

	void OnModeratorMessage(BaseEvent e) {
		string cmd = (string)e.Params["message"];
		string commandId, gameId;
		ParseCmd(cmd, out gameId, out commandId);
		ISFSObject objIn = (SFSObject)e.Params["data"];
		JSONObject jsonData = JSONObject.Parse(Utils.FromByteArray(objIn.GetByteArray("jsonData")));
		Utils.Log("OnModeratorMessage:" + gameId + "." + commandId + " " + jsonData);

		switch (gameId) {
			case GameId.TLMB:
				TLMBClient.Instance.OnModerateMessage(commandId, jsonData);
			break;
		}
	}

	void OnAdminMessage(BaseEvent e) {

	}

  void OnRoomVarsUpdate(BaseEvent e) {
    Room crtRoom = e.Params["room"] as Room;
    if (ScreenManager.Instance.CurrentSlotScreen != null) {
      ScreenManager.Instance.CurrentSlotScreen.UpdateJackpot(crtRoom.GetVariable("jackpot").GetIntValue());
    }
  }

  void OnUserVarsUpdate(BaseEvent e) {
    User user = ((User)e.Params["user"]);
  	if (!user.IsItMe && ScreenManager.Instance.CurrentSlotScreen != null) {
  	  ScreenManager.Instance.CurrentSlotScreen.UpdateOtherPlayerCash(user.Name, user.GetVariable("cash").GetIntValue());
  	}
    // Utils.Log("OnUserVarsUpdate " + user.GetVariable("cash").GetIntValue());
  }

  void OnInvitationReceived(BaseEvent e) {
  	Invitation invitation = e.Params["invitation"] as SFSInvitation;
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_INVITE_TO_GAME_CONFIRM, new object[] { SlotMachineClient.GetGameTypeByCommand(invitation.Params.GetUtfString("gameType")),
                                                                                            invitation.Params.GetUtfString("roomName"),
                                                                                            invitation.Inviter.Name,
                                                                                            invitation.Params.GetUtfString("message")});
  	Utils.Log(invitation.Inviter + " | " + invitation.Params.GetUtfString("message") + " | " + invitation.Params.GetUtfString("roomName"));
  }
  
  public void SendInviteToGame(List<object> invitedUsers, ISFSObject parameters) {
    client.Send( new InviteUsersRequest(invitedUsers, 20, parameters) );
  }
  
  public User GetUserByName(string mUsername) {
    return client.UserManager.GetUserByName(mUsername);
  }
  
  void OnBuddyInited(BaseEvent e) {
    
  }

	void OnBuddyVariablesUpdate(BaseEvent e) {
	}

	public bool IsContainBuddy(string mUsername) {
		return client.BuddyManager.ContainsBuddy(mUsername);
	}

  public List<Buddy> GetBuddyList() {
    return client.BuddyManager.BuddyList;
  }
  
	void ResetAll() {
		ServerRequestQueue.Clear();
		ResetServerRequest();
	}

	void ResetServerRequest() {
		isRequesting = false;
		currentRequest = null;
	}

	void Update () {		
		if (client != null) {
			client.ProcessEvents();
		}
	}

	public void Disconnect() {
		if (client != null) {
			client.Disconnect();
		}

		client = null;
	}

	public bool IsConnected() {
		return client != null && client.IsConnected;
	}
	
	void OnExtensionResponse(BaseEvent e) {
		if (currentRequest != null) {
			string cmd = (string)e.Params["cmd"];
			ISFSObject objIn = (SFSObject)e.Params["params"];
			JSONObject jsonData = JSONObject.Parse(Utils.FromByteArray(objIn.GetByteArray("jsonData")));
			jsonData.Add("commandId", cmd);
			currentRequest.handler.SendMessage(currentRequest.callback, jsonData, SendMessageOptions.DontRequireReceiver);
		}

		ResetServerRequest();
		HandleServerRequest();
	}
	
	public void HandleServerRequest(ServerRequest newRequest = null, bool shouldRequestImmediately = false) {
		if (newRequest != null) {
			if (shouldRequestImmediately) {
				ServerRequestQueue.QueueOnTop(newRequest);
			} else {
				ServerRequestQueue.Queue(newRequest);
			}
		}

		currentRequest = ServerRequestQueue.Dequeue();

		if (CanSendRequest()) { 
			switch (currentRequest.type) {
				case ServerRequest.Type.PUBLIC_MESSAGE:
     			SendPublicMessage();
 				break;
				case ServerRequest.Type.EXTENSION:
					SendRequest();
				break;
			}
		}
	}

  private void SendPublicMessage() {
		client.Send(new PublicMessageRequest(currentRequest.requestData.GetUtfString("message"), currentRequest.requestData));
  }

	private void SendRequest() {
		isRequesting = true;
    // Utils.Log("SendRequest " + currentRequest.commandId);
		client.Send(new ExtensionRequest(currentRequest.commandId, currentRequest.requestData));
	}

  private ISFSObject GetExtensionRequestSendObject(int commandId) {
    ISFSObject objOut = new SFSObject();
    // objOut.PutUtfString("userId", userId);
    return objOut;
	}

	private bool CanSendRequest() {
		return currentRequest != null && IsConnected() && !isRequesting;
	}

	private void ParseCmd(string cmd, out string gameId, out string commandId) {
		gameId = "";
		commandId = "";
		string[] arr = cmd.Split('.');

		if (arr.Length == 2) {
			gameId = arr[0];
			commandId = arr[1];
		}
	}

	public bool IsYou(string userId) {
		return this.userId == userId;
	}
	
	public List<User> GetListUsers() {
	  return client.UserManager.GetUserList();
	}
}