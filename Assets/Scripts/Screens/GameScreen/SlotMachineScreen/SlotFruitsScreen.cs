using UnityEngine;
using System.Collections;

public class SlotFruitsScreen : BaseSlotMachineScreen {

  public SlotMachine slotMachine;
  public UIButton btnBetPerLine;
  public UIButton btnLines;
  public UIButton btnMaxBet;
  public UILabel betPerLineLabel;
  public UILabel lineLabel;

  public override void Init(object[] data) {
    base.Init(data);
    slotMachine.Init();
    EventDelegate.Set(btnBetPerLine.onClick, EventBetPerLine);
    EventDelegate.Set(btnLines.onClick, EventBetLines);
    EventDelegate.Set(btnMaxBet.onClick, EventMaxBet);
    SetBetPerLine(50);
    SetNunLine(1);
  }
  
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
}
