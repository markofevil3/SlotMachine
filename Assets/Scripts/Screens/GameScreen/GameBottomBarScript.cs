using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

public class GameBottomBarScript : MonoBehaviour {

  public UIButton btnOpenChat;
  public UIButton btnSendChat;
  public GameObject chatPanel;
  public UIInput chatInput;
  
  private BaseSlotMachineScreen currentScreen;

  public void Init(BaseSlotMachineScreen currentScreen) {
    this.currentScreen = currentScreen;
    EventDelegate.Set(btnOpenChat.onClick, ShowChatPanel);
    EventDelegate.Set(btnSendChat.onClick, SendChat);
    chatPanel.SetActive(false);
  }
  
  // Show bubble chat at player avatar
  private void DisplayBubbleChat(string text, PlayerSlotScript player) {
    text = Utils.ChatUnescape(text);
    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SpeechBubble", typeof(GameObject)) as GameObject);
   	tempGameObject.name = "SpeechBubble";
   	// fake chat alway from player 1;
   	tempGameObject.transform.position = player.bubbleChatPos;
   	SpeechBubble bubble = tempGameObject.GetComponent<SpeechBubble>();
   	bubble.SetText(text);
  }
  
  public void DisplayBubbleChat(string text, string username) {
    DisplayBubbleChat(text, currentScreen.GetPlayer(username));
  }
  
  // Event when hit send chat button
  private void SendChat() {
    // TO DO: verify chat before send
    if (chatInput.value != string.Empty) {
      string escapedString = Utils.ChatEscape(chatInput.value);
      Debug.Log("###### " + escapedString + " ---- " + Utils.ChatUnescape(escapedString));
      DisplayBubbleChat(escapedString, currentScreen.GetPlayer(AccountManager.Instance.username));
      JSONObject data = new JSONObject();
      data.Add("message", escapedString);
      data.Add("senderId", AccountManager.Instance.username);
      data.Add("senderName", AccountManager.Instance.displayName);
      SmartfoxClient.Instance.HandleServerRequest(CreatePublicMessageRequest(Command.USER.CHAT_IN_ROOM, string.Empty, data));
      chatInput.value = string.Empty;
    }
    HideChatPanel();
  }
  
  // open chat panel
  private void ShowChatPanel() {
    chatPanel.SetActive(true);
    TweenPosition.Begin(chatPanel, 0.2f, new Vector3(0, 265.0f,0));
  }
  
  // close chat panel
  private void HideChatPanel() {
    TweenPosition tween = TweenPosition.Begin(chatPanel, 0.2f, Vector3.zero);
    EventDelegate.Add(tween.onFinished, HideChatPanelCallback, true);
    
  }
  
  // disable chat panel when finish animation
  private void HideChatPanelCallback() {
    chatPanel.SetActive(false);
  }
  
  private ISFSObject CreatePublicMessageObject(JSONObject jsonData) {
    ISFSObject objOut = new SFSObject();
    objOut.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
    objOut.PutUtfString("message", jsonData.GetString("message"));
    return objOut;
  }
  
  private ServerRequest CreatePublicMessageRequest(string commandId, string successCallback, JSONObject jsonData = null) {
   if (jsonData == null) {
     jsonData = new JSONObject();
   }
  
   ISFSObject requestData = CreatePublicMessageObject(jsonData);
   ServerRequest serverRequest = new ServerRequest(ServerRequest.Type.PUBLIC_MESSAGE,
                           Command.Create(GameId.USER, commandId),
                           requestData,
                           gameObject,
                           successCallback);
   return serverRequest;
  }
}
