using UnityEngine;
using System.Collections;

public class SlotItemDragon : SlotItem {
	
	public const int ITEM_WILD = 0;
	public const int ITEM_TRAP = 1;
	public const int ITEM_BOW = 2;
	public const int ITEM_SPELLWATER = 3;
	public const int ITEM_AXE = 4;
	public const int ITEM_BOMB = 5;
	public const int ITEM_SPELLLIGHTING = 6;
	public const int ITEM_SWORD = 7;
	public const int ITEM_SPELLFIRE = 8;
	public const int ITEM_METEOR = 9;
	public const int ITEM_FREESPIN = 10;

  private string[] spriteNames = new string[11] {"character-13", "1-02", "7-02", "5-02", "8-02", "2-02", "4-02", "6-02", "9-02", "meteor", "3-02"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
