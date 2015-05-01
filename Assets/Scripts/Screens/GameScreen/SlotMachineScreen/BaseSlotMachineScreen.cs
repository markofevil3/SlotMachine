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
  [HideInInspector]
	public bool isChangingBoss = false;
	
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
	
	// private List<SpawnableSkill> listSpawnSkills = new List<SpawnableSkill>();
	private List<SpinData> listSpinDatas = new List<SpinData>();
  private bool canSpawnSkill = true;
	private double MAX_SKILL_DURATION = 2.0;
	private double lastProcessSpin = 0;
	
  public string GetRoomId() {
    return roomId;
  }
  
  public GameType GetCrtGameType() {
    return gameType;
  }
  
	public void AddSpinDataToQueue(SpinData mSpinData) {
		listSpinDatas.Add(mSpinData);
	}
	
	IEnumerator ProcessSpinData(SpinData spinData) {
		Debug.Log("ProcessSpinData " + spinData.spawnSkills.Count);
		if (spinData.newBossData != null) {
			Debug.Log("ProcessSpinData~~~~~~~~~ ");
			PauseSpawnSkill();
		}
		// Show Glow on user slot if not you
		if (!spinData.isYou) {
			PlayerSlotScript playerSlot = FindUserSlot(spinData.username);
			if (playerSlot != null) {
				playerSlot.ShowGlow();
			}
		}
		for (int i = 0; i < spinData.spawnSkills.Count; i++) {
			SpawnSkill(spinData.spawnSkills[i]);
			yield return new WaitForSeconds(0.5f);
		}
		if (spinData.newBossData != null) {
			yield return new WaitForSeconds(2f);
			DisplayBossDrop(spinData.dropCash, spinData.dropGem, spinData.newBossData);
		}
	}
	
	public virtual void DisplayBossDrop(int dropCash, int dropGem, JSONObject newBossData) {
		Debug.Log("DisPlayBossDrop " + dropCash + " " + dropGem);
		bigWinPanel.FadeInTreasure(dropCash, dropGem, newBossData);
		StartCoroutine(DisplayBossDropCallback(dropCash, dropGem, newBossData));
	}
	
	IEnumerator DisplayBossDropCallback(int dropCash, int dropGem, JSONObject newBossData) {
		yield return new WaitForSeconds(2.5f);
		// TO DO: update user gem
    UpdateUserCashLabel(dropCash);
		isChangingBoss = true;
		bossManager.ChangeBoss(newBossData, "EventFinishChangeBoss");
	}
	
	public virtual void EventFinishChangeBoss() {
		ResumeSpawnSkill();
		isChangingBoss = false;
	}
	
	// Add Skill to queue for spawn
	// public void AddSkillToQueue(SpawnableSkill mSkill) {
	// 	listSpawnSkills.Add(mSkill);
	// }
	
	public void PauseSpawnSkill() {
		Debug.Log("PauseSpawnSkill");
		canSpawnSkill = false;
		slotMachine.Wait();
	}
	
	public void ResumeSpawnSkill() {
		Debug.Log("ResumeSpawnSkill");
		
		canSpawnSkill = true;
		slotMachine.Resume();
		// if (listSpawnSkills.Count == 0) {
		// 	slotMachine.Resume();
		// }
	}
	
	// Spawn skill if avaiable in queue
	void Update() {
		// TEST CODE - commented to use new SpinData queue
		// if (listSpawnSkills.Count > 0 && canSpawnSkill) {
		// 	canSpawnSkill = false;
		// 	SpawnSkill(listSpawnSkills[0]);
		// 	listSpawnSkills.RemoveAt(0);
		// }
		
		if (listSpinDatas.Count > 0 && canSpawnSkill) {
		// if (listSpinDatas.Count > 0 && (canSpawnSkill || Network.time - lastProcessSpin > MAX_SKILL_DURATION)) {
			lastProcessSpin = Network.time;
			StartCoroutine(ProcessSpinData(listSpinDatas[0]));
			listSpinDatas.RemoveAt(0);
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
    // AccountManager.Instance.UpdateUserCash(-jsonData.GetInt("cost"));
    UpdateUserCashLabel(-jsonData.GetInt("cost"));
  }
  	
	// Show animation when player got free spin
	public void DisplayFreeSpinAnimation() {
		
	}
	
	public virtual void FadeInBigWin(int numb) {
		bigWinPanel.FadeInBigWin(numb);
	}

	public virtual void FadeInFreeSpin(int numb, bool shouldPause = true) {
		bigWinPanel.FadeInFreeSpin(numb, shouldPause);
	}

	public virtual void SpawnSkill(int type, int level, int damage) {}
	public virtual void SpawnSkill(SpawnableSkill skill) {}

	public virtual void OtherPlayerSpinResult(string username, JSONObject jsonData) {}
	
	public virtual void ProcessSpinData(string username, JSONObject jsonData) {
		
	}

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

// Used for spawn skills of each player
public class SpinData {

	public bool isYou;
	public string username;
	public int totalDamage = 0;

	public JSONObject newBossData = null;
	public int dropCash = 0;
	public int dropGem = 0;
	public List<SpawnableSkill> spawnSkills = new List<SpawnableSkill>();
		
		
	public SpinData(string username, JSONObject jsonData, bool isYou) {
		Debug.Log("SpinData: " + jsonData.ToString());
		this.isYou = isYou;
		this.username = username;
		JSONArray resultsData = jsonData.GetArray("items");
		JSONObject extraData = SlotCombination.CalculateCombination(resultsData);
	  JSONArray winningCount = extraData.GetArray("wCount");  
	  JSONArray winningType = extraData.GetArray("wType");
    JSONArray winningGold = jsonData.GetArray("wGold");
		
    for (int i = 0; i < winningGold.Length; i++) {
      totalDamage += (int)winningGold[i].Number;
    }
		
		if (jsonData.ContainsKey("newBoss")) {
			newBossData = jsonData.GetObject("newBoss");
			JSONArray bossDrops = jsonData.GetArray("dropItems");
			dropCash = (int)bossDrops[0].Number;
			dropGem = (int)bossDrops[1].Number;
			bossDrops = null;
		}
		

		for (int i = 0; i < winningCount.Length; i++) {
			if (winningCount[i].Number >= 3 || ((int)winningType[i].Number == (int)SlotItem.Type.TILE_1 && winningCount[i].Number >= 2)) {
				spawnSkills.Add(new SpawnableSkill((int)winningType[i].Number, (int)winningCount[i].Number, (int)winningGold[i].Number, isYou));
			}
		}
		extraData = null;
		resultsData = null;
		winningCount = null;
		winningType = null;
		winningGold = null;
	}
	
}
