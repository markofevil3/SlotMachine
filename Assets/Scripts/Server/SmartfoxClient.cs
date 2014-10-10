using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
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

  public bool IsLoggedIn {
    get { return isLoggedIn; }
  }

	void Awake() {
		Instance = this;
		ServerRequestQueue.Init();
	}

	void Start() {
		// userId = AccountManager.Instance.GetUserId();
    Connect();
	}

	public void ManualConnect(string userId) {
		this.userId = userId;
		Connect();
	}

  public void Register(JSONObject user) {
    if (client != null) {
      user.Add("isRegister", true);
      user.Add("isGuest", false);
      ISFSObject loginData = new SFSObject();
  		loginData.PutByteArray("jsonData", Utils.ToByteArray(user.ToString()));
      client.Send(new LoginRequest(user.GetString("username"), user.GetString("password"), "Gamble", loginData));
    } else {
      Debug.Log("NO CONNECTION!");
    }
  }

  public void RegisterAsGuest() {
    JSONObject user = new JSONObject();
    string userId = FileManager.GetFromKeyChain("guestId");
    if (userId == string.Empty) {
      userId = Guid.NewGuid().ToString().Replace("-", "");
      user.Add("isRegister", true);
      Debug.Log("RegisterAsGuest " + userId);
    } else {
      user.Add("isRegister", false);
      Debug.Log("LoginAsGuest " + userId);
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
  }

  public void LoginUser(string username, string password) {
    JSONObject jsonData = new JSONObject();
    jsonData.Add("isRegister", false);
    jsonData.Add("isGuest", false);
    ISFSObject loginData = new SFSObject();
    loginData.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
    isManualLogin = true;
    client.Send(new LoginRequest(username, password, "Gamble", loginData));
  }
  
  public void LogoutUser() {
    if (client != null) {
      client.Send(new LogoutRequest());
    }
  }
  
	public void Connect() {
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
    // client.Connect("54.255.173.193", 9933);
    #if UNITY_EDITOR
    client.Connect("127.0.0.1", 9933);
    # else
    client.Connect("113.190.10.207", 9933);
    # endif
    // walk around for custom error code from server
    SFSErrorCodes.SetErrorMessage(2, "{0}");
	}

	void OnConnection(BaseEvent e) {
		if ((bool)e.Params["success"]) {
		  isConnected = true;
		  Debug.Log("Connect Success " + AccountManager.Instance.username + " " + AccountManager.Instance.password);
		  if (AccountManager.Instance.username != string.Empty) {
        // PopupManager.Instance.ShowLoadingPopup();
		    LoginUser(AccountManager.Instance.username, AccountManager.Instance.password);
		  } else {
		    if (FileManager.GetFromKeyChain("username") != string.Empty) {
          // PopupManager.Instance.ShowLoadingPopup();
		      LoginUser(FileManager.GetFromKeyChain("username"), FileManager.GetFromKeyChain("password"));
		    }
		  }
		} else {
		  Debug.Log("Connect FAIL!");
		}
	}

	void OnConnectionLost(BaseEvent e) {
	  isConnected = false;
	  Debug.Log("Connection is lost, reason:" + e.Params["reason"]);
	  HUDManager.Instance.AddFlyText("Connection is lost, reason:" + e.Params["reason"], Vector3.zero, 40, Color.red, 0, 4f);
	}

	void OnLogin(BaseEvent e) {
	  PopupManager.Instance.CloseLoadingPopup();
	  isLoggedIn = true;
		ISFSObject loginData = (ISFSObject)e.Params["data"];
		JSONObject user = JSONObject.Parse(Utils.FromByteArray(loginData.GetByteArray("jsonData")));
		AccountManager.Instance.SetUser(user);
		if (ScreenManager.Instance.LobbyScreen != null) {
		  ScreenManager.Instance.LobbyScreen.EventLoggedIn();
		}
		
		if (user.GetBoolean("isRegister") || isManualLogin) {
      bool mIsGuest = user.GetBoolean("isGuest");
      string mUsername = user.GetString("username");
		  FileManager.SaveToKeyChain("username", mUsername);
		  FileManager.SaveToKeyChain("password", user.GetString("password"));
		  FileManager.SaveToKeyChain("isGuest", mIsGuest.ToString());
		  if (mIsGuest && !isManualLogin) {
		    FileManager.SaveToKeyChain("guestId", mUsername);
		  }
		  isManualLogin = false;
		}
		
		Debug.Log("OnLogin---" + user.ToString());
	}

	void OnLoginError(BaseEvent e) {
	  isLoggedIn = false;
	  PopupManager.Instance.CloseLoadingPopup();
	  ISFSObject objIn = (SFSObject)e.Params["data"];
	  int errorCode = int.Parse((string)e.Params["errorMessage"]);
	  
	  HUDManager.Instance.AddFlyText(((ErrorCode.USER)errorCode).ToString(), Vector3.zero, 40, Color.red);
    if (ScreenManager.Instance.LobbyScreen != null) {
      ScreenManager.Instance.LobbyScreen.EventLoginFail();
		}
		
		Debug.Log("LogIn error: " + ((ErrorCode.USER)errorCode));
	}
  
  void OnLogout(BaseEvent e) {
    isLoggedIn = false;
    PopupManager.Instance.CloseLoadingPopup();
    if (ScreenManager.Instance.LobbyScreen != null) {
      ScreenManager.Instance.LobbyScreen.EventLogoutSuccess();
    }
	  
    Debug.Log("Log out");
  }
  
	void OnJoinRoom(BaseEvent e) {

	}

	void OnJoinRoomError(BaseEvent e) {

	}

	void OnPublicMessage(BaseEvent e) {
	  foreach (DictionaryEntry entry in e.Params) {
	    Debug.Log(entry.Key + " " + entry.Value);
  	}
  	User sender = ((User)e.Params["sender"]);
  	if (ScreenManager.Instance.CurrentGameScreen != null && !AccountManager.Instance.IsYou(sender.Name)) {
  	  Debug.Log("#######");
  	  ScreenManager.Instance.CurrentGameScreen.bottomBarScript.DisplayBubbleChat(e.Params["message"].ToString(), sender.Name);
  	}
    // Debug.Log("OnPublicMessage + " + e.Params.ToString());
    //    JSONObject messageJson = JSONObject.Parse(e.Params["message"].ToString());
    //    Debug.Log("OnPublicMessage " + messageJson.ToString());
	}

	void OnPrivateMessage(BaseEvent e) {

	}

	void OnModeratorMessage(BaseEvent e) {
		string cmd = (string)e.Params["message"];
		string commandId, gameId;
		ParseCmd(cmd, out gameId, out commandId);
		ISFSObject objIn = (SFSObject)e.Params["data"];
		JSONObject jsonData = JSONObject.Parse(Utils.FromByteArray(objIn.GetByteArray("jsonData")));
		Debug.Log("OnModeratorMessage:" + gameId + "." + commandId + " " + jsonData);

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
    // Debug.Log(crtRoom.GetVariable("jackpot").GetIntValue());
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
    Debug.Log("SendPublicMessage");
		client.Send(new PublicMessageRequest(currentRequest.requestData.GetUtfString("message"), currentRequest.requestData));
  }

	private void SendRequest() {
		isRequesting = true;
    // Debug.Log("SendRequest " + currentRequest.commandId);
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
}