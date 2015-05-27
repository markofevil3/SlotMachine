using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Boomlagoon.JSON;

using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class PokerClient : MonoBehaviour {
  
  // public static PokerClient Instance { get; private set; }
  // private static JSONObject EMPTY_JSON_OBJECT = new JSONObject();
  // 
  // void Awake() {
  //  Instance = this;
  //     // PokerClient.Init();
  // }
  // 
  // public void JoinLobby() {
  //  SmartfoxClient.Instance.HandleServerRequest(CreateExtensionRequest(Command.POKER.LOBBY));
  // }
  // 
  //   // Move to Select room if join success
  // private void OnJoinLobby(JSONObject jsonData) {
  //   PopupManager.Instance.CloseLoadingPopup();
  //   Debug.Log("OnJoinLobby " +jsonData.ToString());
  //   ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_ROOM, new object[]{(int)BaseGameScreen.GameType.POKER, jsonData});
  // }
  // 
  // private ISFSObject CreateExtensionObject(JSONObject jsonData) {
  //     ISFSObject objOut = new SFSObject();
  //     objOut.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
  //     return objOut;
  // }
  // 
  // private ServerRequest CreateExtensionRequest(string commandId, JSONObject jsonData = null) {
  //  if (jsonData == null) {
  //    jsonData = EMPTY_JSON_OBJECT;
  //  }
  // 
  //  ISFSObject requestData = CreateExtensionObject(jsonData);
  //  ServerRequest serverRequest = new ServerRequest(ServerRequest.Type.EXTENSION,
  //                          Command.Create(GameId.POKER, commandId),
  //                          requestData,
  //                          gameObject,
  //                          "OnExtensionCallback");
  //  return serverRequest;
  // }
}
