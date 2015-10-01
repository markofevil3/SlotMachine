using UnityEngine;
using System.Collections;

public class SlotItemPirate : SlotItem {

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
	public const int ITEM_FREESPIN = 10;

  public static string[] spriteNames = new string[11] {"character-13", "character-04", "character-12", "character-06", "character-07", "character-03", "character-11", "character-08", "character-10", "character-05", "character-09"};
	
  public override string GetSpriteName(int index) {
    return spriteNames[index];
  }
}
