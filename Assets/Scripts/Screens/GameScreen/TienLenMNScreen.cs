using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boomlagoon.JSON;

public class TienLenMNScreen : BaseGameScreen {
  
  public UIButton btnBack;
  public UIButton btnThrowCard;
  public UIGridExtent cardHolder;
  public Transform playedCardHolder;
  public UIGrid playerLeftGrid;
  public UIGrid playerRightGrid;
  
  private List<string> userCards = new List<string>() {"2C", "3C", "4C", "QH", "AC", "KD", "9D", "6D"};
  private List<SelectableCard> userSelectableCards = new List<SelectableCard>();
  private List<SelectableCard> playedCards = new List<SelectableCard>();
  private int minCardDepth = 40;
  private float cardSpaceX = 30.0f;
  private float cardMaxRotateZ = 10.0f;
  private JSONObject users = new JSONObject();
  
  public override void Init(object[] data) {
    base.Init(data);
    EventDelegate.Set(btnBack.onClick, BackToSelectRoom);
    EventDelegate.Set(btnThrowCard.onClick, EventPlayCards);
    Debug.Log(data[1]);
    JSONObject roomData = (JSONObject)data[1];
    JSONArray userList = roomData.GetObject("gameRoom").GetArray("userGames");
    for (int i = 0 ; i < userList.Length; i++) {
      users.Add(userList[i].Obj.GetInt("seatIndex").ToString(), userList[i].Obj);
    }
    // FAKE user joined
    for (int i = 0; i < 4; i++) {
      GameObject tempUser;
      if (i < 2) {
        tempUser = NGUITools.AddChild(playerLeftGrid.gameObject, Resources.Load(Global.SCREEN_PATH + "/GameScreen/TienLenMN/PlayerSlot", typeof(GameObject)) as GameObject);
      } else {
        tempUser = NGUITools.AddChild(playerRightGrid.gameObject, Resources.Load(Global.SCREEN_PATH + "/GameScreen/TienLenMN/PlayerSlot", typeof(GameObject)) as GameObject);
      }
      tempUser.name = "PlayerSlot" + (i + 1);
      PlayerSlotScript playerScript = tempUser.GetComponent<PlayerSlotScript>();
      JSONObject user = users.ContainsKey(i.ToString()) ? users.GetObject(i.ToString()) : null;
      if (user != null) {
        playerScript.Init(user.GetString("userId"), "Dan Choi" + (i + 1), (i + 1) * 200000, string.Empty);
      } else {
        playerScript.InitEmpty();
      }
      playerHolder.Add(playerScript);
    }
    
    playerLeftGrid.Reposition();
    playerRightGrid.Reposition();
    
    // for (int i = 0; i < playerHolder.Count; i++) {
    //  UIButton tempBtn = playerHolder[i].btnThrowCard;
    //  tempBtn.inputParams = new object[] {playerHolder[i]};
    //       EventDelegate.Set(tempBtn.onClick, delegate() { PlayCardFromOther((PlayerSlotScript)tempBtn.inputParams[0]); });
    //       // Test Player turn circle display
    //       StartCoroutine(DisplayPlayerTurn(i));
    // }
		
		StartNewGame();
  } 

  public override void UpdateSeats(JSONObject jsonData) {
    JSONArray userList = jsonData.GetObject("gameRoom").GetArray("userGames");
    users = new JSONObject();
    for (int i = 0 ; i < userList.Length; i++) {
      users.Add(userList[i].Obj.GetInt("seatIndex").ToString(), userList[i].Obj);
    }
    // FAKE user joined
    for (int i = 0; i < 4; i++) {
      GameObject tempUser;
      PlayerSlotScript playerScript = playerHolder[i];
      JSONObject user = users.ContainsKey(i.ToString()) ? users.GetObject(i.ToString()) : null;
      if (user != null) {
        playerScript.Init(user.GetString("userId"), "Dan Choi" + (i + 1), (i + 1) * 200000, string.Empty);
      } else {
        playerScript.InitEmpty();
      }
    }
  }
  

  public override void StartNewGame() {
    // Init user cards
    for (int i = 0; i < userCards.Count; i++) {
      GameObject tempCard = NGUITools.AddChild(cardHolder.gameObject, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SelectableCard", typeof(GameObject)) as GameObject);
      tempCard.name = i.ToString();
      // UI2DSprite card = tempCard.GetComponent<UI2DSprite>();
      // card.sprite2D = GetCardTexture(userCards[i]);
      SelectableCard tempSelectableCard = tempCard.GetComponent<SelectableCard>();
      tempSelectableCard.Init(GetCardTexture(userCards[i]), true);
      userSelectableCards.Add(tempSelectableCard);
      tempSelectableCard.eventTrigger.inputParams = new object[] {tempSelectableCard};
      EventDelegate.Set(tempSelectableCard.eventTrigger.onClick, delegate() { SelectCard((SelectableCard)tempSelectableCard.eventTrigger.inputParams[0]); });
    }
    // Sort card depth to display (card on the right is on top)
    SortCardDepth(userSelectableCards);
    cardHolder.Reposition();
    for (int i = 0; i < userSelectableCards.Count; i++) {
      userSelectableCards[i].gameObject.SetActive(false);
      StartCoroutine(ShowCard(i));
    }
  }

  //########## TEST
  IEnumerator ShowCard(int index) {
    yield return new WaitForSeconds((index + 1) * 0.2f); 
    userSelectableCards[index].RotateShow();
  }
  
  IEnumerator DisplayPlayerTurn(int index) {
    yield return new WaitForSeconds((index + 1) * 5.0f);
    // playerHolder[index].ShowCircle(5f);
  }
  //##########
  
  // Play card - move from selectable list to played list
  private void EventPlayCards() {
    List<SelectableCard> canPlayCards = new List<SelectableCard>();
    for (int i = 0; i < userSelectableCards.Count; i++) {
      if (userSelectableCards[i].isSelected) {
        canPlayCards.Add(userSelectableCards[i]);
        playedCards.Add(userSelectableCards[i]);
      }
    }
    
    int multipler = (int)Mathf.Ceil(canPlayCards.Count / 2);
    
    // canPlayCards = canPlayCards.OrderBy(x => x.number).ToList();
    
    for (int i = 0; i < canPlayCards.Count; i++) {
      float rotateZ = Random.Range(0, cardMaxRotateZ);
      if (i < multipler) {
        canPlayCards[i].PlayCard(playedCardHolder, new Vector3(-(multipler - i) * cardSpaceX, 0, 0), new Vector3(0, 0, rotateZ));
      } else {
        canPlayCards[i].PlayCard(playedCardHolder, new Vector3((i - multipler) * cardSpaceX, 0, 0), new Vector3(0, 0, -rotateZ));
      }
    }
    
    userSelectableCards.RemoveAll(x => x.isSelected == true);
    SortCardDepth(playedCards);
    SortCardDepth(userSelectableCards);
    cardHolder.Reposition();
    
    // Test Popup Result
    if (userSelectableCards.Count == 0) {
      Invoke("EndGame", 1.0f);
    }
  }
  
  // Play other player cards
  private void EventPlayOPCards(Transform playerTransform, string[] cards) {

    int multipler = (int)Mathf.Ceil(cards.Length / 2);
    
    for (int i = 0; i < cards.Length; i++) {
			SelectableCard card = NGUITools.AddChild(playedCardHolder.gameObject,
																							 Resources.Load(Global.SCREEN_PATH + "/GameScreen/SelectableCard", typeof(GameObject)) as GameObject).GetComponent<SelectableCard>();
			playedCards.Add(card);
			card.Init(GetCardTexture(cards[i]));
			card.transform.position = playerTransform.position;
      float rotateZ = Random.Range(0, cardMaxRotateZ);
      if (i < multipler) {
        card.PlayCard(playedCardHolder, new Vector3(-(multipler - i) * cardSpaceX, 0, 0), new Vector3(0, 0, rotateZ), false);
      } else {
        card.PlayCard(playedCardHolder, new Vector3((i - multipler) * cardSpaceX, 0, 0), new Vector3(0, 0, -rotateZ), false);
      }
    }
    SortCardDepth(playedCards);
  }

	private void PlayCardFromOther(PlayerSlotScript player) {
	  // TEST STOP Player turn's circle if played
    // player.StopCircle();
	  //##########
		EventPlayOPCards(player.transform, new string[] {"6C", "7C", "8C"});
	}
  
  // Open popup result when game finish
  private void EndGame() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_RESULT);
  }
  
  // Sort card depth
  private void SortCardDepth(List<SelectableCard> sortList) {
    for (int i = 0; i < sortList.Count; i++) {
      sortList[i].SetCardDepth(minCardDepth + i);
    }
  }
  
  // Reset game table to start new game
  public override void ResetGame() {
    foreach(Transform child in playedCardHolder) {
      Destroy(child.gameObject);
    }
    foreach(Transform child in cardHolder.transform) {
      Destroy(child.gameObject);
    }
    Init(null);
  }
  
  // Leave room
  void BackToSelectRoom() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_LEAVE_GAME, new object[] { gameType });
    // ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_ROOM, new object[]{gameType});
  }
  
  public override void Close() {
		base.Close();
	}
  
}
