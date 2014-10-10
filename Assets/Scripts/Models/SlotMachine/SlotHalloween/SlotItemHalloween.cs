using UnityEngine;
using System.Collections;

public class SlotItemHalloween : SlotItem {

  private string[] spriteNames = new string[10] {"wild", "pumpkin", "bat", "dracula", "frankenstein", "halloween_cat", "hat", "jack_skellington", "scream", "witch"};

  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
