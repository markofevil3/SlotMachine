using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

public class InGameChatBar : MonoBehaviour {
	// Ipad keyboard height: 634
	// Iphone keyboard height: 528
	// Editor: -534f
	public UITextList textList;
	public UIInput chatInput;
	public UIEventTriggerExtent btnOpenChat;
	public UIAnchorExtent chatPanelAnchor;
	public UILabel recentChatMes;
	public UISprite background;
	public UISprite textListBackground;
	public UILabel totalBetLabel;
	public UISprite totalBetGoldIcon;

	private bool isOpen = false;
	private Vector3 closePos;
	private Vector3 openPos = Vector3.zero;
	private float keyboardHeight;

	void Start() {
		Init();
	}

	public void Init() {
    EventDelegate.Set(chatInput.onSubmit, SendChat);
    EventDelegate.Set(btnOpenChat.onClick, Open);
		chatPanelAnchor.Reset();
		closePos = transform.localPosition;
		#if UNITY_EDITOR
			chatInput.transform.GetComponent<UISprite>().depth = 2;
			chatInput.label.depth = 20;
		#else
			chatInput.transform.GetComponent<UISprite>().depth = -1;
			chatInput.label.depth = -1;
		#endif
	}
	
	public void SendChat() {
    if (chatInput.value != string.Empty) {
      string escapedString = Utils.ChatEscape(chatInput.value);
      // DisplayBubbleChat(escapedString, currentScreen.FindUserSlot(AccountManager.Instance.username));
      JSONObject data = new JSONObject();
      data.Add("message", escapedString);
      data.Add("senderId", AccountManager.Instance.username);
      data.Add("senderName", AccountManager.Instance.displayName);
      SmartfoxClient.Instance.HandleServerRequest(CreatePublicMessageRequest(Command.USER.CHAT_IN_ROOM, string.Empty, data));
			AddChatToList(data);
      chatInput.value = string.Empty;
			StartCoroutine(KeepKeyboardOpen());
    }
	}
	
	IEnumerator KeepKeyboardOpen() {
		yield return null;
		chatInput.isSelected = true;		
	}
	
	public void AddChatToList(JSONObject jsonData) {
		string message = "";
		string newestMes = "";
    if (AccountManager.Instance.IsYou(jsonData.GetString("senderId"))) {
			message += "[999999]You:[-] ";
			newestMes += "[999999]You:[-] ";
		} else {
			message += "[999999]" + jsonData.GetString("senderName") + ":[-] ";
			newestMes += "[999999]" + jsonData.GetString("senderName") + ":[-] ";
		}
		message += "[000000]" + Utils.ChatUnescape(jsonData.GetString("message")) + "[-]";
		newestMes += "[FFFFFF]" + Utils.ChatUnescape(jsonData.GetString("message")) + "[-]";
		recentChatMes.text = newestMes;
		textList.Add(message);
	}
	
	public void Open() {
		chatInput.isSelected = true;		
		// isOpen = true;
		#if UNITY_EDITOR
		if (!isOpen) {
			isOpen = true;
			// float pos = Mathf.Abs(transform.localPosition.y * 2) * 634f / 2048f;
			// openPos = new Vector3(transform.localPosition.x, pos - Mathf.Abs(transform.localPosition.y) + 150f, transform.localPosition.z);
			openPos = new Vector3(transform.localPosition.x, NGUIMath.ScreenToPixels(new Vector2(0, TouchScreenKeyboard.area.height), ScreenManager.Instance.camera.transform).y + textListBackground.height + 72f, transform.localPosition.z);
			TweenPosition tween = TweenPosition.Begin(gameObject, 0.3f, openPos, false);
		} else {
			isOpen = false;
			TweenPosition tween = TweenPosition.Begin(gameObject, 0.2f, closePos, false);
		}
		// #else
		// TweenPosition tween = TweenPosition.Begin(gameObject, 0.3f, new Vector3(transform.localPosition.x, transform.localPosition.y + TouchScreenKeyboard.area.height, 0), false);
		#endif 
		// static public TweenPosition Begin (GameObject go, float duration, Vector3 pos, bool worldSpace);
		
	}
	
	public void UpdateTotalBet(int totalBet) {
		totalBetLabel.text = Localization.Format("TotalBet_Text", totalBet.ToString("N0"));
		totalBetGoldIcon.UpdateAnchors();
	}
	
	void Update() {
    #if UNITY_IPHONE || UNITY_ANDROID
    if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			if (chatInput.isSelected && TouchScreenKeyboard.area.height > 0) {
				if (!isOpen) {
					keyboardHeight = TouchScreenKeyboard.area.height;
					isOpen = true;
					openPos = new Vector3(transform.localPosition.x, NGUIMath.ScreenToPixels(new Vector2(0, TouchScreenKeyboard.area.height), ScreenManager.Instance.camera.transform).y + textListBackground.height, transform.localPosition.z);
					TweenPosition tween = TweenPosition.Begin(gameObject, 0.2f, openPos, false);
				} else if (TouchScreenKeyboard.area.height != keyboardHeight && TouchScreenKeyboard.area.height > keyboardHeight) {
					keyboardHeight = TouchScreenKeyboard.area.height;
					openPos = new Vector3(transform.localPosition.x, NGUIMath.ScreenToPixels(new Vector2(0, TouchScreenKeyboard.area.height), ScreenManager.Instance.camera.transform).y + textListBackground.height, transform.localPosition.z);
					TweenPosition tween = TweenPosition.Begin(gameObject, 0.1f, openPos, false);
				}
			}
			if (isOpen && !TouchScreenKeyboard.visible) {
				isOpen = false;
				TweenPosition tween = TweenPosition.Begin(gameObject, 0.2f, closePos, false);
			}
		}

		#endif
	}
	
	public void Close() {
		isOpen = false;
	}
	
  private ISFSObject CreatePublicMessageObject(JSONObject jsonData, string commandId) {
    ISFSObject objOut = new SFSObject();
    objOut.PutByteArray("jsonData", Utils.ToByteArray(jsonData.ToString()));
    objOut.PutUtfString("message", jsonData.GetString("message"));
    objOut.PutUtfString("cmd", commandId);
    return objOut;
  }
  
  private ServerRequest CreatePublicMessageRequest(string commandId, string successCallback, JSONObject jsonData = null) {
   if (jsonData == null) {
     jsonData = new JSONObject();
   }
  
   ISFSObject requestData = CreatePublicMessageObject(jsonData, commandId);
   ServerRequest serverRequest = new ServerRequest(ServerRequest.Type.PUBLIC_MESSAGE,
                           Command.Create(GameId.USER, commandId),
                           requestData,
                           gameObject,
                           successCallback);
   return serverRequest;
  }
}
