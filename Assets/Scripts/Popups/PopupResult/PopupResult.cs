using UnityEngine;
using System.Collections;

public class PopupResult : Popup {

  public UIButton btnPlayAgain;

  public override void Init(object[] data) {
    base.Init(data);
    EventDelegate.Set(btnPlayAgain.onClick, PlayAgain);
  }
  
  private void PlayAgain() {
    ScreenManager.Instance.CurrentGameScreen.ResetGame();
    CloseNoAnimation();
  }
}
