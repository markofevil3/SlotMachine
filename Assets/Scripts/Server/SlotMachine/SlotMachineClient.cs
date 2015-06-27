using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Boomlagoon.JSON;

using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class SlotMachineClient : MonoBehaviour {

  public static SlotMachineClient Instance { get; private set; }
  private static JSONObject EMPTY_JSON_OBJECT = new JSONObject();
  
  void Awake() {
   Instance = this;
  }
  
  public static string GetCommandByGameType(BaseSlotMachineScreen.GameType gameType) {
    switch (gameType) {
      case BaseSlotMachineScreen.GameType.SLOT_FRUITS:
        return Command.SLOT_MACHINE.SLOT_TYPE_FRUITS;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN:
        return Command.SLOT_MACHINE.SLOT_TYPE_HALLOWEEN;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_DRAGON:
        return Command.SLOT_MACHINE.SLOT_TYPE_DRAGON;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_PIRATE:
        return Command.SLOT_MACHINE.SLOT_TYPE_PIRATE;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_ZOMBIE:
        return Command.SLOT_MACHINE.SLOT_TYPE_ZOMBIE;
      break;
      default:
        return string.Empty;
    }
  }
  
  public static BaseSlotMachineScreen.GameType GetGameTypeByCommand(string gameTypeCommand) {
    switch (gameTypeCommand) {
      case Command.SLOT_MACHINE.SLOT_TYPE_FRUITS:
        return BaseSlotMachineScreen.GameType.SLOT_FRUITS;
      break;
      case Command.SLOT_MACHINE.SLOT_TYPE_HALLOWEEN:
        return BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN;
      break;
      case Command.SLOT_MACHINE.SLOT_TYPE_DRAGON:
        return BaseSlotMachineScreen.GameType.SLOT_DRAGON;
      break;
      case Command.SLOT_MACHINE.SLOT_TYPE_PIRATE:
        return BaseSlotMachineScreen.GameType.SLOT_PIRATE;
      break;
      case Command.SLOT_MACHINE.SLOT_TYPE_ZOMBIE:
        return BaseSlotMachineScreen.GameType.SLOT_ZOMBIE;
      break;
      default:
        return BaseSlotMachineScreen.GameType.SLOT_FRUITS;
    }
  }
  
  // Join user to game lobby room and join available room
	public void JoinRoom(BaseSlotMachineScreen.GameType gameType, string roomName = null) {
	  PopupManager.Instance.ShowLoadingPopup("LoadingText_JoinRoom");
	  JSONObject jsonData = new JSONObject();
		jsonData.Add("gameType", GetCommandByGameType(gameType));
		jsonData.Add("roomName", roomName);
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.SLOT_MACHINE.SLOT_JOIN_ROOM, jsonData));
	}

  // Move to Select room if join success
	private void OnJoinRoom(JSONObject jsonData) {
	  Debug.Log("OnJoinLobby " +jsonData.ToString());
	  PopupManager.Instance.CloseLoadingPopup();
	  ScreenManager.Instance.SetScreen(BaseScreen.Type.SLOT_GAME_SCREEN, new object[]{GetGameTypeByCommand(jsonData.GetString("gameType")), jsonData});
    // ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_ROOM, new object[]{(int)BaseGameScreen.GameType.TIEN_LEN_MB, jsonData});
	}

  public void Play(int betPerLineIndex, int numLines) {
    JSONObject jsonData = new JSONObject();
		jsonData.Add("betPerLine", betPerLineIndex);
		jsonData.Add("numLines", numLines);
		jsonData.Add("gameType", GetCommandByGameType(ScreenManager.Instance.CurrentSlotScreen.GetCrtGameType()));
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.SLOT_MACHINE.SLOT_PLAY, jsonData));
  }

  public void OnPlay(JSONObject jsonData) {
    Debug.Log(jsonData.ToString());
    if (ScreenManager.Instance.CurrentSlotScreen != null) {
      ScreenManager.Instance.CurrentSlotScreen.SetResults(jsonData);
    }
  }

  public void LeaveGame() {
	  PopupManager.Instance.ShowLoadingPopup("LoadingText_LeaveRoom");
    JSONObject jsonData = new JSONObject();
    jsonData.Add("gameType", GetCommandByGameType(ScreenManager.Instance.CurrentSlotScreen.GetCrtGameType()));
		SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.SLOT_MACHINE.SLOT_LEAVE, jsonData));
  }

  public void OnLeaveGame(JSONObject jsonData) {
		Debug.Log("OnLeaveGame " + jsonData.ToString());
	  PopupManager.Instance.CloseLoadingPopup();
    ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_GAME, null, true);
  }

  // public void Create() {
  //  TLMBGameConfig gameConfig = TLMBGameConfig.CreateCountGame(10);
  //  JSONObject jsonData = new JSONObject();
  //  jsonData.Add("gameConfig", gameConfig.ToJsonObject());
  //  jsonData.Add("seatIndex", 0);
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.CREATE, jsonData));
  // }
  // 
  //   // Move to Game screen if Create success
  // private void OnCreate(JSONObject jsonData) {
  //   ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, jsonData});
  // }
  // 
  // public void StartGame() {
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.START));
  // }
  // 
  // private void OnStartGame(JSONObject jsonData) {
  //   Debug.Log("OnStartGame " + jsonData);
  //  // TLMBGameData.LoadRoom(jsonData.GetObject("gameRoom"));
  // }
  // 
  // public void Join(string roomId) {
  //  JSONObject jsonData = new JSONObject();
  //  jsonData.Add("roomId", roomId);
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.JOIN, jsonData));
  // }
  // 
  // private void OnJoin(JSONObject jsonData) {
  //   Debug.Log("OnJoin " + jsonData);
  //     if (ScreenManager.Instance.SelectRoomScreen != null) {
  //       ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, jsonData});
  //     }
  // }
  // 
  // public void Drop(string cardsString) {
  //  JSONObject jsonData = new JSONObject();
  //  jsonData.Add("cardsString", cardsString);
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.DROP, jsonData)); 
  // }
  // 
  // private void OnDrop(JSONObject jsonData) {
  // }
  // 
  // public void Fold() {
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.FOLD)); 
  // }
  // 
  // private void OnFold(JSONObject jsonData) {
  // 
  // }
  // 
  // public void Sit(int seatIndex) {
  //  JSONObject jsonData = new JSONObject();
  //  jsonData.Add("seatIndex", seatIndex);
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.SIT, jsonData));
  // }
  // 
  // private void OnSit(JSONObject jsonData) {
  //   Debug.Log("OnSit " + jsonData.ToString());
  //   if (ScreenManager.Instance.CurrentGameScreen != null) {
  //      Debug.Log("#######");
  //      ScreenManager.Instance.CurrentGameScreen.UpdateSeats(jsonData);
  //    }
  // }
  // 
  // public void Standup() {
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.STANDUP));
  // }
  // 
  // private void OnStandup(JSONObject jsonData) {
  // 
  // }
  // 
  // public void Leave() {
  //   Debug.Log("Leave");
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.LEAVE));
  // }
  // 
  // private void OnLeave(JSONObject jsonData) {
  //   Debug.Log("OnLeave");
  //     TLMBClient.Instance.JoinLobby();
  // }
  // 
  // public void Quit() {
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.TLMB.QUIT));
  // }
  // 
  // private void OnQuit(JSONObject jsonData) {
  // 
  // }
  // 
  // public void Kick() {
  // }
  // 
  // private void OnKick(JSONObject jsonData) {
  // 
  // }

	public void UpdateState(JSONObject jsonData) {
		
	}

	public void OnModerateMessage(string commandId, JSONObject jsonData) {
		// Debug.Log("OnModerateMessage:" + commandId + " " + jsonData);
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
			case Command.SLOT_MACHINE.SLOT_JOIN_ROOM: OnJoinRoom(jsonData); break;
			case Command.SLOT_MACHINE.SLOT_PLAY: OnPlay(jsonData); break;
			case Command.SLOT_MACHINE.SLOT_LEAVE: OnLeaveGame(jsonData); break;
      // case Command.TLMB.CREATE: OnCreate(jsonData); break;
      // case Command.TLMB.START: OnStartGame(jsonData); break;
      // case Command.TLMB.JOIN: OnJoin(jsonData); break;
      // case Command.TLMB.SIT: OnSit(jsonData); break;
      // case Command.TLMB.STANDUP: OnStandup(jsonData); break;
      // case Command.TLMB.DROP: OnDrop(jsonData); break;
      // case Command.TLMB.FOLD: OnFold(jsonData); break;
      // case Command.TLMB.LEAVE: OnLeave(jsonData); break;
      // case Command.TLMB.QUIT: OnQuit(jsonData); break;
      // case Command.TLMB.KICK: OnKick(jsonData); break;
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
														Command.Create(GameId.SLOT_MACHINE, commandId),
														requestData,
														gameObject,
														"OnExtensionCallback");
		return serverRequest;
	}
}
