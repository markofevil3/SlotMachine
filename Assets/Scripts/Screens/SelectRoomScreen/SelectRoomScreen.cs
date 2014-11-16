using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SelectRoomScreen : BaseScreen {
  
  private string ROOM_ROW_PREFAB = Global.SCREEN_PATH + "/SelectRoomScreen/RoomRow";
  private BaseGameScreen.GameType mGameType;
  
  public BaseGameScreen.GameType gameType {
    get { return mGameType; }
    set { mGameType = value; }
  }
  
  public UIButton btnBack;
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIPopupList betFilterPopupList;
  public UILabel betFilterLabel;
  public UIButton btnCreateRoom;
  public UIDragScrollView backgroundDragScrollView;
  
  private JSONArray roomList;
  private JSONArray filteredRoomList;
  private string[] betFilterList = new string[] {"Bet_Filter_All", "Bet_Filter_under_50k", "Bet_Filter_over_100k", "Bet_Filter_over_500k", "Bet_Filter_over_1m"};
  private string crtBetFilter;
  
  public override void Init(object[] data) {
    gameType = (BaseGameScreen.GameType)data[0];
    EventDelegate.Set(btnBack.onClick, BackToSelectGame);
    EventDelegate.Set(btnCreateRoom.onClick, OpenPopupCreateRoom);
    // fake room list
    // roomList = new JSONArray();
    roomList = ((JSONObject)data[1]).GetArray("rooms");
    Debug.Log(roomList.ToString());
    // fake room min bet
    // int[] roomBet = new int[] { 10000, 100000, 520000, 2000000};
    // for (int i = 0; i < roomList.Length; i++) {
    //   JSONObject room = roomList[i].Obj;
    //   room.Add("id", i);
    //   room.Add("name", "room " + i);
    //   room.Add("minBet", roomBet[i % 4]);
    //   // roomList.Add(room);
    // }
    InitScrollViewData();
    // Set Bet Filter
    for (int i = 0; i < betFilterList.Length; i++) {
      betFilterPopupList.items.Add(betFilterList[i]);
    }
    EventDelegate.Set(betFilterPopupList.onChange, EventFilterBet);
    crtBetFilter = betFilterList[0];
  }

  private void InitScrollViewData(bool isFilter = false) {
    wrapContent.ResetChildPositions();
    scrollview.ResetPosition();
    JSONArray targetList;
    if (isFilter) {
      targetList = filteredRoomList;
    } else {
      targetList = roomList;
    }
    wrapContent.minIndex = -(targetList.Length - 1);
    wrapContent.onInitializeItem = UpdateRowDataOnScroll;
    Transform tempGameObject;
    bool canDrag = true;
    if (targetList.Length < wrapContent.transform.childCount) {
      canDrag = false;
      backgroundDragScrollView.enabled = false;
    } else {
      backgroundDragScrollView.enabled = true;
    }
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
      tempGameObject = wrapContent.transform.GetChild(i);
      RoomRowScript tempRowScript = tempGameObject.GetComponent<RoomRowScript>();
      tempRowScript.Init(scrollview);
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < targetList.Length) {
				Utils.SetActive(tempGameObject.gameObject, true);
        tempRowScript.UpdateData(targetList[i].Obj);
      } else {
				Utils.SetActive(tempGameObject.gameObject, false);
      }
    }
  }

  private void EventFilterBet() {
    betFilterLabel.SetCurrentSelection();
    string crtSelect = UIPopupList.current.value;
    Debug.Log("EventFilterBet " + crtSelect);
    if (crtSelect == crtBetFilter) {
      return;
    }
    crtBetFilter = crtSelect;
    switch (crtSelect) {
      case "Bet_Filter_All":
        InitScrollViewData();
      break;
      case "Bet_Filter_under_50k":
        filteredRoomList = new JSONArray();
        for (int i = 0; i < roomList.Length; i++) {
          if (roomList[i].Obj.GetInt("minBet") <= 50000) {
            filteredRoomList.Add(roomList[i].Obj);
          }
        }
        InitScrollViewData(true);
      break;
      case "Bet_Filter_over_100k":
        filteredRoomList = new JSONArray();
        for (int i = 0; i < roomList.Length; i++) {
          if (roomList[i].Obj.GetInt("minBet") >= 100000) {
            filteredRoomList.Add(roomList[i].Obj);
          }
        }
        InitScrollViewData(true);
      break;
      case "Bet_Filter_over_500k":
        filteredRoomList = new JSONArray();
        for (int i = 0; i < roomList.Length; i++) {
          if (roomList[i].Obj.GetInt("minBet") >= 500000) {
            filteredRoomList.Add(roomList[i].Obj);
          }
        }
        InitScrollViewData(true);
      break;
      case "Bet_Filter_over_1m":
        filteredRoomList = new JSONArray();
        for (int i = 0; i < roomList.Length; i++) {
          if (roomList[i].Obj.GetInt("minBet") >= 1000000) {
            filteredRoomList.Add(roomList[i].Obj);
          }
        }
        InitScrollViewData(true);
      break;
    }
    scrollview.panel.alpha = 0.1f;
    TweenAlpha tween = TweenAlpha.Begin(scrollview.gameObject, 0.8f, 1.0f);
  }

  public void UpdateRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (realIndex < roomList.Length) {
      go.GetComponent<RoomRowScript>().UpdateData(roomList[realIndex].Obj);
    }
  }

  public void UpdateFilteredRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (realIndex < filteredRoomList.Length) {
      go.GetComponent<RoomRowScript>().UpdateData(filteredRoomList[realIndex].Obj);
    }
  }

  private void OpenPopupCreateRoom() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_CREATE_ROOM);
  }

  public override void Open() {
    
  }

  public override void Close() {
		ScreenManager.Instance.SelectRoomScreen = null;
		base.Close();
	}

  private void BackToSelectGame() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_GAME);
  }

}
