using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotDragonScreen : BaseSlotMachineScreen {

	public Boss boss;

  public override void Init(object[] data) {
		WINNING_ANIMATION_PREFAB = Global.SCREEN_PATH + "/GameScreen/WinningAnimation/FruitWinningAnimation";
    base.Init(data);
		boss.Init(roomData.GetInt("dIndex"), roomData.GetInt("dHP"));
  }
	
	// Slot reel stopped, displayed result, start display winning animation if should
	public override void EventFinishSpin(bool isBigWin, int winningCash) {
		boss.GetHit(winningCash);
		base.EventFinishSpin(isBigWin, winningCash);
	}
}
