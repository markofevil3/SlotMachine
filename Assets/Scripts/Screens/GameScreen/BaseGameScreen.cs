using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class BaseGameScreen : BaseScreen {
  
  public enum GameType {
    TIEN_LEN_MB,
    PHOM,
    OM_BA_CAY,
    BLACKJACK,
    POKER,
    STRIP_POKER,
    TOTAL
  }
    
  public GameType gameType;
  public Sprite[] cardTextures;

  [HideInInspector]
  public GameBottomBarScript bottomBarScript;
  [HideInInspector]
  public List<PlayerSlotScript> playerHolder = new List<PlayerSlotScript>();

  public PlayerSlotScript GetPlayer(int index) {
    return playerHolder[index - 1];
  }

  public PlayerSlotScript GetPlayer(string username) {
    for (int i = 0; i < playerHolder.Count; i++) {
      if (playerHolder[i].username == username) {
        return playerHolder[i];
      }
    }
    Debug.LogError("Cant find player " + username);
    return null;
  }

  public override void Init(object[] data) {    
    // cardTextures = Resources.LoadAll<Sprite>("Atlas/Cards");
    // GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.GAME_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
    //    	tempGameObject.name = "GameBottomBar";
    //    	bottomBarScript = tempGameObject.GetComponent<GameBottomBarScript>();
    // bottomBarScript.Init(this);
    base.Init(data);
  }

  public virtual void StartNewGame() {}

  public virtual void SelectCard(SelectableCard card) {
    card.SelectCard();
  }

  public Sprite GetCardTexture(string cardName) {
    for (int i = 0; i < cardTextures.Length; i++) {
      if (cardTextures[i].name == cardName) {
        return cardTextures[i];
      }
    }
    Debug.Log("Cant find card " + cardName);
    return null;
  }

  public virtual void ResetGame() {}

  public virtual void UpdateSeats(JSONObject jsonData) {}

  public override void Open() {
    
  }

  public override void Close() {
		ScreenManager.Instance.CurrentGameScreen = null;
		base.Close();
	}

}
