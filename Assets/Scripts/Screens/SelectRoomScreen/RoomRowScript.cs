using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class RoomRowScript : MonoBehaviour {
  
  public UIStretchExtent backgroundStretch;
  public UIAnchorExtent roomNumberAnchor;
  public UIAnchorExtent sitStatusAnchor;
  public UILabel roomNumberLabel;
  public UILabel minBetLabel;
  public UILabel sitStatusLabel;
  public UIEventTriggerExtent eventTrigger;
  public UIDragScrollView dragScrollView;
  
  public JSONObject roomData;
  
  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateData(JSONObject data) {
    roomData = data;
    roomNumberLabel.text = data.GetString("name");
    minBetLabel.text = data.GetInt("minBet").ToString("N0");
    sitStatusLabel.text = data.GetInt("numUsers") + "/4";
    eventTrigger.inputParams = new object[] {roomData.GetString("id")};
    EventDelegate.Set(eventTrigger.onClick, delegate() { EventJoinSelectedRoom((string)eventTrigger.inputParams[0]); });
  }
  
  private void EventJoinSelectedRoom(string roomId) {
    Utils.Log("EventJoinSelectedRoom " + roomId);
		TLMBClient.Instance.Join(roomId);
    // if (ScreenManager.Instance.SelectRoomScreen != null) {
    //   ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, roomId});
    // }
  }
}
