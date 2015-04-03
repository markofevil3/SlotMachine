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
  public WinningAnimation winningAnimation {
  	get { 
			if (mWinningAnimation == null) {
		    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(WINNING_ANIMATION_PREFAB, typeof(GameObject)) as GameObject);
		   	tempGameObject.name = "WinningAnimation";
		   	mWinningAnimation = tempGameObject.GetComponent<WinningAnimation>();
		   	mWinningAnimation.Init(slotMachine);
			}
			return mWinningAnimation;
		}
  }
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
  
  private string roomId = string.Empty;
	private WinningAnimation mWinningAnimation;
	
	private List<SpawnableSkill> listSpawnSkills = new List<SpawnableSkill>();
  private bool canSpawnSkill = true;
  private float lastSkillTime;
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
	
	// Spawn skill if avaiable in queue
	void Update() {
		if (listSpawnSkills.Count > 0 && (canSpawnSkill || Time.time - lastSkillTime > MAX_SKILL_DURATION)) {
			lastSkillTime = Time.time;
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
    
    UpdateUserCashLabel();
    
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
    AccountManager.Instance.UpdateUserCash(-jsonData.GetInt("cost"));
    UpdateUserCashLabel();
  }
  
	public virtual void SetSpecialData(JSONObject jsonData) {}
	
	// Show animation when player got free spin
	public void DisplayFreeSpinAnimation() {
		
	}
	
	// Slot reel stopped, displayed result, start display winning animation if should
	public virtual void EventFinishSpin(bool isBigWin, int winningCash) {
		// TEST CODE -- commented, should refine
		// if (isBigWin) {
		// 	slotMachine.Wait();
		// 	winningAnimation.SetData(winningCash);
		// 	winningAnimation.FadeIn(true, 2f);
		// }
	}

	public virtual void SpawnSkill(int type, int level, int damage) {}
	public virtual void SpawnSkill(SpawnableSkill skill) {}

	public virtual void OtherPlayerSpinResult(string username, JSONObject jsonData) {}

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
		this.level = level - 2;
		this.damage = damage;
		this.isYou = isYou;
	}
}
