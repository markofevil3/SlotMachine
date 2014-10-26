using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotFruitsScreen : BaseSlotMachineScreen {

  public override void Init(object[] data) {
		WINNING_ANIMATION_PREFAB = Global.SCREEN_PATH + "/GameScreen/WinningAnimation/FruitWinningAnimation";
    base.Init(data);
  }
}
