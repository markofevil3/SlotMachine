using UnityEngine;
using System.Collections;

public class SlotItemPirate : SlotItem {

  private string[] spriteNames = new string[11] {"character-13", "character-03", "character-04", "character-05", "character-06", "character-07", "character-08", "character-10", "character-11", "character-12", "character-09"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
