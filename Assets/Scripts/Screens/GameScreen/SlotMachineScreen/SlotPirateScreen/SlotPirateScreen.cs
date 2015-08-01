using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotPirateScreen : BaseSlotMachineScreen {

	public GameObject starHolder;
	public TweenScale octopusTween;
	public GameObject octopusFreeSpin;
	public UILabel octopusFreeSpinLabel;

	public override void SpawnSkill(int type, int level, int damage, Vector3 fromPos, bool isYou) {
		GameObject tempGameObject;
		SkillFireBall skill;
		switch (type) {
			case SlotItemPirate.ITEM_CHOPPER:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillBite", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillBite", typeof(GameObject)) as GameObject);
				SkillBite skillBite = tempGameObject.GetComponent<SkillBite>();
				skillBite.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_USOOP:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
				skill = tempGameObject.GetComponent<SkillFireBall>();
				skill.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_NAMI:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillThunder", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillThunder", typeof(GameObject)) as GameObject);
				SkillThunder thunderSkill = tempGameObject.GetComponent<SkillThunder>();
				thunderSkill.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_FRANKY:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillFlameThrower", skillCamera.transform).gameObject;
				SkillFlameThrower skillFlameThrower = tempGameObject.GetComponent<SkillFlameThrower>();
				skillFlameThrower.Init(level, damage, bossManager, fromPos, isYou);
			break;
			case SlotItemPirate.ITEM_BROOK:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillDagger", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillDagger", typeof(GameObject)) as GameObject);
				SkillDagger skillSword = tempGameObject.GetComponent<SkillDagger>();
				skillSword.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_NICO:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillDagger", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillDagger", typeof(GameObject)) as GameObject);
				SkillDagger skillDagger = tempGameObject.GetComponent<SkillDagger>();
				skillDagger.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_SANJI:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSanji", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSanji", typeof(GameObject)) as GameObject);
				SkillSanji skillSanji = tempGameObject.GetComponent<SkillSanji>();
				skillSanji.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_ZORO:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSwordBlue", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSwordBlue", typeof(GameObject)) as GameObject);
				SkillSwordBlue skillSwordBlue = tempGameObject.GetComponent<SkillSwordBlue>();
				skillSwordBlue.Init(level, damage, bossManager);
			break;
			case SlotItemPirate.ITEM_LUFFY:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillLuffy", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillLuffy", typeof(GameObject)) as GameObject);
				SkillLuffy skillLuffy = tempGameObject.GetComponent<SkillLuffy>();
				skillLuffy.Init(level, damage, bossManager);
			break;
			// case SlotItemPirate.ITEM_RALLY:
			// 	tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
			// 	// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
			// 	skill = tempGameObject.GetComponent<SkillFireBall>();
			// 	skill.Init(level, damage, bossManager);
			// break;
		}
			// tempGameObject = MyPoolManager.Instance.Spawn("SkillFlameThrower", skillCamera.transform).gameObject;
			// SkillFlameThrower skillLuffy = tempGameObject.GetComponent<SkillFlameThrower>();
			// skillLuffy.Init(1, damage, bossManager, fromPos, true);
	}
	
	public override void ShowFreeSpinAnimation() {
		TweenColor[] tweens = starHolder.GetComponentsInChildren<TweenColor>();
		if (tweens != null && tweens.Length > 0) {
			for (int i = 0; i < tweens.Length; i ++) {
				tweens[i].enabled = true;
			}
		}
		octopusTween.enabled = true;
		Utils.SetActive(octopusFreeSpin, true);
	}

	public override void StopFreeSpinAnimation() {
		TweenColor[] tweens = starHolder.GetComponentsInChildren<TweenColor>();
		if (tweens != null && tweens.Length > 0) {
			for (int i = 0; i < tweens.Length; i ++) {
				tweens[i].value = Color.white;
				tweens[i].enabled = false;
			}
		}
		octopusTween.enabled = false;
		octopusTween.value = Vector3.one;
		Utils.SetActive(octopusFreeSpin, false);
	}
	
	public override void UpdateFreeSpinText(int numb) {
		octopusFreeSpinLabel.text = Localization.Format("FreeSpinCountText", numb.ToString());
	}	
	
	// private void SpawnSkill(int damage, Vector3 startPos) { // startPos is world position
	// 	//   	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/Lighting", typeof(GameObject)) as GameObject);
	// 	// Skill skill = tempGameObject.GetComponent<Skill>();
	// 	// skill.Init(bossManager, damage, startPos);
	// 	//###########
	// 	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillThunder", typeof(GameObject)) as GameObject);
	// 	SkillThunder skill = tempGameObject.GetComponent<SkillThunder>();
	// 	skill.Init(3, damage, bossManager);
	// }
}
