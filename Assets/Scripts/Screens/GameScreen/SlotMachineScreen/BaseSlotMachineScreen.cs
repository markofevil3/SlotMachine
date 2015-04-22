using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class BaseSlotMachineScreen : BaseScreen {
	
  public enum GameType {
    SLOT_FRUITS,
    SLOT_HALLOWEEN,
		SLOT_DRAGON,
		SLOT_PIRATE,
		SLOT_ZOMBIE,
    TOTAL
  }
  [HideInInspector]
	public string WINNING_ANIMATION_PREFAB = "";
  [HideInInspector]
  public GameBottomBarScript bottomBarScript;
  [HideInInspector]
	public JSONObject roomData;
	
  public GameType gameType;
  public UIButton btnBack;
  public SlotMachine slotMachine;
  public UIButton btnBetPerLine;
  public UIButton btnLines;
  public UIButton btnMaxBet;
  public UILabel betPerLineLabel;
  public UILabel lineLabel;
  public UILabel userCashLabel;
	public UIEventTriggerExtent backgroundEventListener;
	public InGameChatBar inGameChatBar;
  public PlayerSlotScript[] otherPlayers = new PlayerSlotScript[4];
	public Transform userAvatarPanel;
	public GameObject skillCamera;
	public BossManager bossManager;
  public BigWinPanel bigWinPanel;
	
  private string roomId = string.Empty;
	
	private List<SpawnableSkill> listSpawnSkills = new List<SpawnableSkill>();
  private bool canSpawnSkill = true;
	private float MAX_SKILL_DURATION = 2f;
	
  public string GetRoomId() {
    return roomId;
  }
  
  public GameType GetCrtGameType() {
    return gameType;
  }
  
	// Add Skill to queue for spawn
	public void AddSkillToQueue(SpawnableSkill mSkill) {
		listSpawnSkills.Add(mSkill);
	}
	
	public void PauseSpawnSkill() {
		canSpawnSkill = false;
	}
	
	public void ResumeSpawnSkill() {
		canSpawnSkill = true;
		if (listSpawnSkills.Count == 0) {
			slotMachine.Resume();
		}
	}
	
	// Spawn skill if avaiable in queue
	void Update() {
		if (listSpawnSkills.Count > 0 && canSpawnSkill) {
			canSpawnSkill = false;
			SpawnSkill(listSpawnSkills[0]);
			listSpawnSkills.RemoveAt(0);
		}
	}
	
  public override void Init(object[] data) {    
		// TEST CODE -- commented
    // GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.GAME_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
    //    	tempGameObject.name = "GameBottomBar";
    //    	bottomBarScript = tempGameObject.GetComponent<GameBottomBarScript>();
    //    	bottomBarScript.Init(this);
   	    
    EventDelegate.Set(btnBack.onClick, EventBackToSelectGame);
   	
   	slotMachine.Init();
    EventDelegate.Set(btnBetPerLine.onClick, EventBetPerLine);
    EventDelegate.Set(btnLines.onClick, EventBetLines);
    EventDelegate.Set(btnMaxBet.onClick, EventMaxBet);
    SetBetPerLine(500);
    SetNunLine(1);
    
    // Init other players if have
    Debug.Log("### " + data[1].ToString());
    JSONObject jsonData = (JSONObject)data[1];
    JSONArray otherPlayerDatas = jsonData.GetArray("otherPlayers");
		roomData = jsonData.GetObject("roomData");
    int count = 0;
    for (int i = 0; i < otherPlayerDatas.Length; i++) {
      if (!AccountManager.Instance.IsYou(otherPlayerDatas[i].Obj.GetString("username"))) {
        otherPlayers[count].Init(otherPlayerDatas[i].Obj.GetString("username"), otherPlayerDatas[i].Obj.GetString("displayName"), otherPlayerDatas[i].Obj.GetInt("cash"), null);
        count++;
      }
    }
    
    for (int i = 0; i < otherPlayers.Length; i++) {
      if (otherPlayers[i].IsEmpty()) {
        otherPlayers[i].InitEmpty();
      }
    }
    
    roomId = jsonData.GetString("roomId");
    
    UpdateUserCashLabel(0);
    bigWinPanel.Hide();
    base.Init(data);
  }

  private void EventBackToSelectGame() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_LEAVE_GAME, new object[] { gameType });
  }
  
  public override void Open() {}
  
	// Set result after receive from server, then slot reel will display result
  public virtual void SetResults(JSONObject jsonData) {
    slotMachine.SetResults(jsonData);
		SetSpecialData(jsonData.GetObject("specials"));
    // AccountManager.Instance.UpdateUserCash(-jsonData.GetInt("cost"));
    UpdateUserCashLabel(-jsonData.GetInt("cost"));
  }
  
	public virtual void SetSpecialData(JSONObject jsonData) {}
	
	// Show animation when player got free spin
	public void DisplayFreeSpinAnimation() {
		
	}
	
	public virtual void FadeInBigWin(int numb) {
		bigWinPanel.FadeIn(numb);
	}

	public virtual void FadeInFreeSpin(int numb, bool shouldPause = true) {
		bigWinPanel.FadeInFreeSpin(numb, shouldPause);
	}

	public virtual void SpawnSkill(int type, int level, int damage) {}
	public virtual void SpawnSkill(SpawnableSkill skill) {}

	public virtual void OtherPlayerSpinResult(string username, JSONObject jsonData) {}

  public void UpdateUserCashLabel(int addValue) {
		if (addValue == 0) {
			UpdateUserCashLabelFinished();
			return;
		}
		int fromVal = AccountManager.Instance.cash;
		AccountManager.Instance.UpdateUserCash(addValue);
		Debug.Log("UpdateUserCash " + fromVal + " " + AccountManager.Instance.cash + " " + addValue);
		LeanTween.value(gameObject, UpdateUserCashLabelCallback, fromVal, AccountManager.Instance.cash, 1f).setOnComplete(UpdateUserCashLabelFinished);
  }
  
	void UpdateUserCashLabelCallback(float val) {
    userCashLabel.text = Mathf.Floor(val).ToString("N0");
	}
	
	void UpdateUserCashLabelFinished() {
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
		slotMachine.StartMachine();
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

public class SpawnableSkill {
	public int type;
	public int level;
	public int damage;
	public bool isYou;
	
	public SpawnableSkill(int type, int level, int damage, bool isYou) {
		this.type = type;
		this.level = level - 2 >= 1 ? level - 2 : 1;
		this.damage = damage;
		this.isYou = isYou;
	}
}
