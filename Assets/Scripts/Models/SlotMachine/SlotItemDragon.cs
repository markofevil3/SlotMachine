using UnityEngine;
using System.Collections;

public class SlotItemDragon : SlotItem {
	
	public const int ITEM_WILD = 0;
	public const int ITEM_CHOPPER = 1;
	public const int ITEM_USOOP = 2;
	public const int ITEM_NAMI = 3;
	public const int ITEM_FRANKY = 4;
	public const int ITEM_BROOK = 5;
	public const int ITEM_NICO = 6;
	public const int ITEM_SANJI = 7;
	public const int ITEM_ZORO = 8;
	public const int ITEM_LUFFY = 9;
	public const int ITEM_RALLY = 10;

  private string[] spriteNames = new string[11] {"character-13", "1-02", "2-02", "3-02", "4-02", "5-02", "6-02", "7-02", "8-02", "9-02", "meteor"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
