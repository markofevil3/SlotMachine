using UnityEngine;
using System.Collections;

public class TestTLMB : MonoBehaviour {
	public static TestTLMB Instance { get; private set; }
	public UILabel dataLbl;
	public UILabel commandInput;
	public UILabel userIdInput;
	public UILabel cardsInput;
	public UILabel extraInput;
	public UIEventTrigger buttonSend;

	void Awake() {
		Instance = this;
	}

	void Start() {
		EventDelegate.Set(buttonSend.onClick, EventSend);
	}

	public void EventSend() {
		Debug.Log("Send");
		
	    switch (commandInput.text) {
			case "connect":
				SmartfoxClient.Instance.ManualConnect(userIdInput.text);
			break;
			case "lobby":
				TLMBClient.Instance.JoinLobby();
			break;
			case "create":
				TLMBClient.Instance.Create();
			break;
			case "join":
				TLMBClient.Instance.Join(cardsInput.text);
			break;
			case "sit":
				TLMBClient.Instance.Sit(int.Parse(cardsInput.text));
			break;
			case "standup":
				TLMBClient.Instance.Standup();
			break;
			case "start":
				TLMBClient.Instance.StartGame();
			break;
			case "fold":
				TLMBClient.Instance.Fold();
			break;
			case "drop":
				TLMBClient.Instance.Drop(cardsInput.text);
			break;
	    }
  	}

 	public void Display(string data) {
    	dataLbl.text = data;
  	}
}