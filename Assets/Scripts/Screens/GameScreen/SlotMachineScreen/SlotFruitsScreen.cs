using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotFruitsScreen : BaseSlotMachineScreen {

  public override void Init(object[] data) {
    base.Init(data);
  }
  
  public override void SetResults(JSONObject jsonData) {
    slotMachine.SetResults(jsonData);
  }
  
  public override void UpdateJackpot(int score) {
    slotMachine.UpdateJackpot(score);
  }
}
