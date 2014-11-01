using UnityEngine;
using System.Collections;

public class SlotItemHalloween : SlotItem {

  private string[] spriteNames = new string[11] {"wild", "pumpkin", "bat", "dracula", "frankenstein", "halloween_cat", "hat", "jack_skellington", "scream", "witch", "coins"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
