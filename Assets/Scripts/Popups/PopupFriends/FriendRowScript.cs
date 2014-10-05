using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class FriendRowScript : MonoBehaviour {

  [HideInInspector]
  public JSONObject rowData;
  
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UILabel currentGameLabel;
  public UIButton btnSendGift;
  public UITexture avatarTexture;
  public UIDragScrollView dragScrollView;

  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateRowData(JSONObject data) {
    rowData = data;
    usernameLabel.text = data.GetString("displayName");
    cashLabel.text = data.GetInt("cash").ToString("N0");
    EventDelegate.Set(btnSendGift.onClick, EventOpenGiftPopup);
  }
  
  void EventOpenGiftPopup() {
    
  }
}
