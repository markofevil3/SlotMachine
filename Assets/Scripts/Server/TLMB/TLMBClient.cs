using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Boomlagoon.JSON;

using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class TLMBClient : MonoBehaviour {
	public static TLMBClient Instance { get; private set; }
	private static JSONObject EMPTY_JSON_OBJECT = new JSONObject();

	void Awake() {
		Instance = this;
		TLMBGameData.Init();
	}

	public void JoinLobby() {
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.LOBBY));
	}

  // Move to Select room if join success
	private void OnJoinLobby(JSONObject jsonData) {
	  PopupManager.Instance.CloseLoadingPopup();
	  Utils.Log("OnJoinLobby " +jsonData.ToString());
	  ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_ROOM, new object[]{(int)BaseGameScreen.GameType.TIEN_LEN_MB, jsonData});
	}

	public void Create() {
		TLMBGameConfig gameConfig = TLMBGameConfig.CreateCountGame(10);
		JSONObject jsonData = new JSONObject();
		jsonData.Add("gameConfig", gameConfig.ToJsonObject());
		jsonData.Add("seatIndex", 0);
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.CREATE, jsonData));
	}

  // Move to Game screen if Create success
	private void OnCreate(JSONObject jsonData) {
	  ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, jsonData});
	}

	public void StartGame() {
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.START));
	}

	private void OnStartGame(JSONObject jsonData) {
	  Utils.Log("OnStartGame " + jsonData);
		// TLMBGameData.LoadRoom(jsonData.GetObject("gameRoom"));
	}

	public void Join(string roomId) {
		JSONObject jsonData = new JSONObject();
		jsonData.Add("roomId", roomId);
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.JOIN, jsonData));
	}

	private void OnJoin(JSONObject jsonData) {
	  Utils.Log("OnJoin " + jsonData);
    if (ScreenManager.Instance.SelectRoomScreen != null) {
      ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, jsonData});
    }
	}

	public void Drop(string cardsString) {
		JSONObject jsonData = new JSONObject();
		jsonData.Add("cardsString", cardsString);
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.DROP, jsonData));	
	}

	private void OnDrop(JSONObject jsonData) {
	}

	public void Fold() {
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.FOLD));	
	}

	private void OnFold(JSONObject jsonData) {

	}

	public void Sit(int seatIndex) {
		JSONObject jsonData = new JSONObject();
		jsonData.Add("seatIndex", seatIndex);
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.SIT, jsonData));
	}

	private void OnSit(JSONObject jsonData) {
	  Utils.Log("OnSit " + jsonData.ToString());
	  if (ScreenManager.Instance.CurrentGameScreen != null) {
  	  Utils.Log("#######");
  	  ScreenManager.Instance.CurrentGameScreen.UpdateSeats(jsonData);
  	}
	}

	public void Standup() {
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.STANDUP));
	}

	private void OnStandup(JSONObject jsonData) {

	}

	public void Leave() {
	  Utils.Log("Leave");
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.LEAVE));
	}

	private void OnLeave(JSONObject jsonData) {
	  Utils.Log("OnLeave");
    TLMBClient.Instance.JoinLobby();
	}

	public void Quit() {
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.QUIT));
	}

	private void OnQuit(JSONObject jsonData) {

	}

	public void Kick() {
	}

	private void OnKick(JSONObject jsonData) {

	}

	public void UpdateState(JSONObject jsonData) {
		
	}

	public void OnModerateMessage(string commandId, JSONObject jsonData) {
		// Utils.Log("OnModerateMessage:" + commandId + " " + jsonData);
		switch (commandId) {
			case Command.TLMB.UPDATE:
				UpdateState(jsonData);
			break;
			case Command.TLMB.START:
				TLMBGameData.LoadRoom(jsonData);
				TestTLMB.Instance.Display(jsonData.ToString());
			break;
		}
	}

	private void OnExtensionCallback(JSONObject jsonData) {
		string commandId = jsonData.GetString("commandId");

		switch (commandId) {
			case Command.TLMB.LOBBY: OnJoinLobby(jsonData); break;
			case Command.TLMB.CREATE: OnCreate(jsonData); break;
			case Command.TLMB.START: OnStartGame(jsonData); break;
			case Command.TLMB.JOIN: OnJoin(jsonData); break;
			case Command.TLMB.SIT: OnSit(jsonData); break;
			case Command.TLMB.STANDUP: OnStandup(jsonData); break;
			case Command.TLMB.DROP: OnDrop(jsonData); break;
			case Command.TLMB.FOLD: OnFold(jsonData); break;
			case Command.TLMB.LEAVE: OnLeave(jsonData); break;
			case Command.TLMB.QUIT: OnQuit(jsonData); break;
			case Command.TLMB.KICK: OnKick(jsonData); break;
		}

    // if (jsonData.GetInt("errorCode") == (int)ErrorCode.TLMB.NULL) {
    //  TestTLMB.Instance.Display(jsonData.ToString());
    // }
	}

	private ISFSObject CreateExtensionObject(JSONObject jsonData) {
	    ISFSObject objOut = new SFSObject();
	    objOut.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
	    return objOut;
	}

	private ServerRequest CreateExtensionRequest(string commandId, JSONObject jsonData = null) {
		if (jsonData == null) {
			jsonData = EMPTY_JSON_OBJECT;
		}

		ISFSObject requestData = CreateExtensionObject(jsonData);
		ServerRequest serverRequest = new ServerRequest(ServerRequest.Type.EXTENSION,
														Command.Create(GameId.TLMB, commandId),
														requestData,
														gameObject,
														"OnExtensionCallback");
		return serverRequest;
	}
}