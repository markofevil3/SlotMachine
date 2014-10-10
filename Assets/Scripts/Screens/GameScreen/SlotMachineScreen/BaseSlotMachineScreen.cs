using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class BaseSlotMachineScreen : BaseScreen {
  
  public enum GameType {
    SLOT_FRUITS,
    SLOT_HALLOWEEN,
    TOTAL
  }
    
  public GameType gameType;
  
  [HideInInspector]
  public GameBottomBarScript bottomBarScript;
  
  public SlotMachine slotMachine;
  public UIButton btnBetPerLine;
  public UIButton btnLines;
  public UIButton btnMaxBet;
  public UILabel betPerLineLabel;
  public UILabel lineLabel;
  
  public override void Init(object[] data) {    
    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.GAME_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "GameBottomBar";
   	bottomBarScript = tempGameObject.GetComponent<GameBottomBarScript>();
   	bottomBarScript.Init(this);
   	
   	slotMachine.Init();
    EventDelegate.Set(btnBetPerLine.onClick, EventBetPerLine);
    EventDelegate.Set(btnLines.onClick, EventBetLines);
    EventDelegate.Set(btnMaxBet.onClick, EventMaxBet);
    SetBetPerLine(50);
    SetNunLine(1);
    
    base.Init(data);
  }
  
  public override void Open() {}
  
  public virtual void SetResults(JSONObject jsonData) {}
  
  public virtual void UpdateJackpot(int score) {}
  
  void SetBetPerLine(int betPerLine) {
    slotMachine.SetBetPerLine(betPerLine);
    betPerLineLabel.text = slotMachine.GetBetPerLine().ToString();
  }

  void SetNunLine(int lineVal) {
    slotMachine.SetNumLine(lineVal);
    lineLabel.text = slotMachine.GetNumLine().ToString();
  }

  void EventBetPerLine() {
    
  }
  
  void EventBetLines() {
    SetNunLine((slotMachine.GetNumLine() % SlotCombination.MAX_LINE) + 1);
  }
  
  void EventMaxBet() {
    SetNunLine(SlotCombination.MAX_LINE);
  }
  
  public PlayerSlotScript GetPlayer(int index) {
    return null;
  }

  public PlayerSlotScript GetPlayer(string username) {
    return null;
  }
  
  
  public override void Close() {
		ScreenManager.Instance.CurrentGameScreen = null;
		base.Close();
	}
}
