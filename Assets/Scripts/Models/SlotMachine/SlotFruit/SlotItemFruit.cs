using UnityEngine;
using System.Collections;

public class SlotItemFruit : SlotItem {

  private string[] spriteNames = new string[10] {"wild", "apple", "strawberry", "radish", "broccoli", "eggplant", "bell_pepper", "chili_pepper", "mushroom", "fruits"};

  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
