using UnityEngine;
using System.Collections;

public class BaseSlotMachineScreen : BaseScreen {
  
  public enum GameType {
    SLOT_FRUITS,
    TOTAL
  }
    
  public GameType gameType;
  
  [HideInInspector]
  public GameBottomBarScript bottomBarScript;
  
  public override void Init(object[] data) {    
    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.GAME_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "GameBottomBar";
   	bottomBarScript = tempGameObject.GetComponent<GameBottomBarScript>();
   	bottomBarScript.Init(this);
    base.Init(data);
  }
  
  public override void Open() {}
  
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
