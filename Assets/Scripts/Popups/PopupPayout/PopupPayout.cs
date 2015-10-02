using UnityEngine;
using System.Collections;

public class PopupPayout : Popup {

	public PayoutItemScript[] payoutItems;

  public override void Init(object[] data) {
    base.Init(data);
		for (int i = 0; i < payoutItems.Length; i++) {
			payoutItems[i].Init(i, SlotItem.GetItemSpriteName(ScreenManager.Instance.CurrentSlotScreen.gameType, i));
		}
  }
}
