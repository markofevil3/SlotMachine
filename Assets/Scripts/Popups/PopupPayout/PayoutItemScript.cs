using UnityEngine;
using System.Collections;

public class PayoutItemScript : MonoBehaviour {

	public UISprite itemIcon;
	public UILabel[] winCountLabels;
	public UILabel[] winMultiLabels;
	public UILabel specialText;

	public void Init(int type, string iconSpriteName) {
		if (ScreenManager.Instance != null && ScreenManager.Instance.CurrentSlotScreen != null) {
			itemIcon.atlas = AtlasManager.Instance.GetAtlasReferenceBySlotType(ScreenManager.Instance.CurrentSlotScreen.gameType);
			itemIcon.spriteName = iconSpriteName;
			if (specialText != null) {
				specialText.text = Localization.Get("PopupPayout_specialText_" + ScreenManager.Instance.CurrentSlotScreen.gameType.ToString());
			}
			if (type == (int)SlotItem.Type.WILD) {
			} else {
				int gap = SlotCombination.NUM_REELS - winCountLabels.Length;
				for (int i = 0; i < winCountLabels.Length; i++) {
					winCountLabels[i].text = i + gap + 1 + "";
					winMultiLabels[i].text = ScreenManager.Instance.CurrentSlotScreen.PAYOUTS[type, i + gap].ToString();
				}
			}
		}
	}
}
