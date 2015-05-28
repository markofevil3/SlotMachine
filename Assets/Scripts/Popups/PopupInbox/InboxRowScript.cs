using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class InboxRowScript : MonoBehaviour {
	
	// Inbox message data structure
	// type
	// title
	// message
	// goldVal
	// gemVal
	// fromUsername
	// createdAt

  [HideInInspector]
  public JSONObject rowData;
  
  public UILabel titleLabel;
	public UISprite iconSprite;
	public UILabel timeLabel;
  public UIEventTriggerExtent eventTrigger;
  public UIDragScrollView dragScrollView;

	private int messageTimeSeconds = 0;

  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateRowData(JSONObject data) {
    rowData = data;
		titleLabel.text = rowData.GetString("title");
		messageTimeSeconds = (int)((Utils.UTCNowMiliseconds() - data.GetLong("createdAt")) / 1000);
		timeLabel.text = Utils.GetTimePassed(messageTimeSeconds);
    EventDelegate.Set(eventTrigger.onClick, EventViewMessage);
  }
	
	void EventViewMessage() {
		if (PopupManager.Instance != null && PopupManager.Instance.PopupInbox != null) {
			PopupManager.Instance.PopupInbox.EventViewMessage(rowData);
		}
	}
	
	void Update() {
		timeLabel.text = Utils.GetTimePassed(messageTimeSeconds);
	}
}
