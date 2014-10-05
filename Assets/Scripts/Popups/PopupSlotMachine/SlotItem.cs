using UnityEngine;
using System.Collections;

public class SlotItem : MonoBehaviour {

  public enum Type {
    WILD,               // 0
    APPLE,              // 1
    STRAWBERRY,         // 2
    RADISH,             // 3
    BROCCOLI,           // 4
    EGGPLANT,           // 5
    BELL_PEPPER,        // 6
    CHILI_PEPPER,       // 7
    MUSHROOM,           // 8
    FRUIT_BASKET,       // 9
    TOTAL
  }
  
  public UISprite sprite;
  
  private string[] spriteNames = new string[10] {"wild", "apple", "strawberry", "radish", "broccoli", "eggplant", "bell_pepper", "chili_pepper", "mushroom", "fruits"};
  private Type type;
  private int index;

  public void Init(Type type, int index) {
    this.type = type;
    this.index = index;
    sprite.spriteName = spriteNames[(int)type];
  }
  
  public bool IsTarget(int targetIndex) {
    return targetIndex == index;
  }
}
