using UnityEngine;
using System.Collections;

public class PayoutItemScript : MonoBehaviour {

	public UISprite itemIcon;
	public UILabel[] winCountLabels;
	public UILabel[] winMultiLabels;
	public UILabel specialText;

	public void Init(string iconSpriteName) {
		if (ScreenManager.Instance != null && ScreenManager.Instance.CurrentSlotScreen != null) {
			itemIcon.atlas = AtlasManager.Instance.GetAtlasReferenceBySlotType(ScreenManager.Instance.CurrentSlotScreen.gameType);
			itemIcon.spriteName = iconSpriteName;
			if (specialText != null) {
				specialText.text = Localization.Get("PopupPayout_specialText_" + ScreenManager.Instance.CurrentSlotScreen.gameType.ToString());
			}
		}
	}
}
