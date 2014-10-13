using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class BaseSlotMachineScreen : BaseScreen {
  
  public enum GameType {
    SLOT_FRUITS,
    SLOT_HALLOWEEN,
    TOTAL
  }
    
  [HideInInspector]
  public GameBottomBarScript bottomBarScript;
  
  public GameType gameType;
  public UIButton btnBack;
  public SlotMachine slotMachine;
  public UIButton btnBetPerLine;
  public UIButton btnLines;
  public UIButton btnMaxBet;
  public UILabel betPerLineLabel;
  public UILabel lineLabel;
  public UILabel userCashLabel;
  
  public PlayerSlotScript[] otherPlayers = new PlayerSlotScript[4];
  
  private string roomId = string.Empty;
  
  public string GetRoomId() {
    return roomId;
  }
  
  public GameType GetCrtGameType() {
    return gameType;
  }
  
  public override void Init(object[] data) {    
    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.GAME_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "GameBottomBar";
   	bottomBarScript = tempGameObject.GetComponent<GameBottomBarScript>();
   	bottomBarScript.Init(this);
   	    
    EventDelegate.Set(btnBack.onClick, EventBackToSelectGame);
   	
   	slotMachine.Init();
    EventDelegate.Set(btnBetPerLine.onClick, EventBetPerLine);
    EventDelegate.Set(btnLines.onClick, EventBetLines);
    EventDelegate.Set(btnMaxBet.onClick, EventMaxBet);
    SetBetPerLine(50);
    SetNunLine(1);
    
    // Init other players if have
    Debug.Log("### " + data[1].ToString());
    JSONObject jsonData = (JSONObject)data[1];
    JSONArray otherPlayerDatas = jsonData.GetArray("otherPlayers");
    int count = 0;
    for (int i = 0; i < otherPlayerDatas.Length; i++) {
      if (!AccountManager.Instance.IsYou(otherPlayerDatas[i].Obj.GetString("username"))) {
        otherPlayers[count].Init(otherPlayerDatas[i].Obj.GetString("username"), otherPlayerDatas[i].Obj.GetString("displayName"), otherPlayerDatas[i].Obj.GetInt("cash"), null);
        count++;
      }
    }
    
    roomId = jsonData.GetString("roomId");
    
    UpdateUserCashLabel();
    
    base.Init(data);
  }
  
  private void EventBackToSelectGame() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_LEAVE_GAME, new object[] { gameType });
  }
  
  public override void Open() {}
  
  public virtual void SetResults(JSONObject jsonData) {
    slotMachine.SetResults(jsonData);
    AccountManager.Instance.UpdateUserCash(-jsonData.GetInt("cost"));
    UpdateUserCashLabel();
  }
  
  public void UpdateUserCashLabel() {
    userCashLabel.text = AccountManager.Instance.cash.ToString("N0");
  }
  
  public virtual void UpdateJackpot(int score) {
    slotMachine.UpdateJackpot(score);
  }
  
  void SetBetPerLine(int betPerLine) {
    slotMachine.SetBetPerLine(betPerLine);
    betPerLineLabel.text = slotMachine.GetBetPerLine().ToString();
  }

  void SetNunLine(int lineVal) {
    slotMachine.SetNumLine(lineVal);
    lineLabel.text = slotMachine.GetNumLine().ToString();
  }

  void EventBetPerLine() {}
  
  void EventBetLines() {
    SetNunLine((slotMachine.GetNumLine() % SlotCombination.MAX_LINE) + 1);
  }
  
  void EventMaxBet() {
    SetNunLine(SlotCombination.MAX_LINE);
  }
  
  public void OnPlayerJoinRoom(string roomId, JSONObject userData) {
    if (this.roomId == roomId) {
      PlayerSlotScript playerSlot = GetAvailableSlot(userData.GetString("username"));
      playerSlot.Init(userData.GetString("username"), userData.GetString("displayName"), userData.GetInt("cash"), null);
    } else {
      Debug.LogError("Not in this room " + this.roomId + " | " + roomId);
    }
  }
  
  public void OnPlayerLeaveRoom(string roomId, string username) {
    if (this.roomId == roomId) {
      PlayerSlotScript playerSlot = FindUserSlot(username);
      if (playerSlot != null) {
        playerSlot.InitEmpty();
      } else {
        Debug.Log("Cant find user slot " + username);
      }
    } else {
      Debug.LogError("Not in this room " + this.roomId + " | " + roomId);
    }
  }
  
  public void UpdateOtherPlayerCash(string username, int cashVal) {
    PlayerSlotScript playerSlot = FindUserSlot(username);
     if (playerSlot != null) {
       playerSlot.UpdateCash(cashVal);
     } else {
       Debug.Log("Cant find user slot " + username);
     }
  }
  
  private PlayerSlotScript GetAvailableSlot(string username) {
    PlayerSlotScript emptySlot = null;
    PlayerSlotScript alreadyJoinSlot = null;
    for (int i = 0; i < otherPlayers.Length; i++) {
      if (otherPlayers[i].IsThisUser(username)) {
        alreadyJoinSlot = otherPlayers[i];
      } else if (otherPlayers[i].IsEmpty() && emptySlot == null){
        emptySlot = otherPlayers[i];
      }
    }
    return alreadyJoinSlot != null ? alreadyJoinSlot : emptySlot;
  }
  
  public PlayerSlotScript FindUserSlot(string username) {
    for (int i = 0; i < otherPlayers.Length; i++) {
      if (otherPlayers[i].IsThisUser(username)) {
        return otherPlayers[i];
      }
    }
    return null;
  }
  
  public override void Close() {
		ScreenManager.Instance.CurrentGameScreen = null;
		base.Close();
	}
}
