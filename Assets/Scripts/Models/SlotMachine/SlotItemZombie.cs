using UnityEngine;
using System.Collections;

public class SlotItemZombie : SlotItem {
	
	public const int ITEM_WILD = 0;
	public const int ITEM_BASEBALL = 1;
	public const int ITEM_KNIFE = 2;
	public const int ITEM_ELECTRICGUN = 3;
	public const int ITEM_PISTOL = 4;
	public const int ITEM_BROOK = 5;
	public const int ITEM_CROSSBOW = 6;
	public const int ITEM_MACHINEGUN = 7;
	public const int ITEM_GRENADE = 8;
	public const int ITEM_LUFFY = 9;
	public const int ITEM_FREESPIN = 10;

  private string[] spriteNames = new string[11] {"character-13", "9-02", "3-02", "electricGun", "2-02", "8-02", "crossBow", "7-02", "1-02", "4-02", "6-02"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
