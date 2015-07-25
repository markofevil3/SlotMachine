using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotDragonScreen : BaseSlotMachineScreen {
	
	public GameObject freeSpinLeft;
	public UILabel freeSpinLeftLabel;
	
	public override void SpawnSkill(int type, int level, int damage, Vector3 fromPos) {
		GameObject tempGameObject;
		SkillFireBall skill;
		// switch (type) {
		// 	case SlotItemDragon.ITEM_TRAP:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillBite", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillBite", typeof(GameObject)) as GameObject);
		// 		SkillBite skillBite = tempGameObject.GetComponent<SkillBite>();
		// 		skillBite.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_BOW:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillBow", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSwordBlue", typeof(GameObject)) as GameObject);
		// 		SkillBow skillBow = tempGameObject.GetComponent<SkillBow>();
		// 		skillBow.Init(level, damage, bossManager, fromPos);
		// 	break;
		// 	case SlotItemDragon.ITEM_SPELLWATER:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillSpellIce", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSwordBlue", typeof(GameObject)) as GameObject);
		// 		SkillSpellIce skillSpellIce = tempGameObject.GetComponent<SkillSpellIce>();
		// 		skillSpellIce.Init(level, damage, bossManager, fromPos);
		// 	break;
		// 	case SlotItemDragon.ITEM_AXE:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
		// 		skill = tempGameObject.GetComponent<SkillFireBall>();
		// 		skill.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_BOMB:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillDagger", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillDagger", typeof(GameObject)) as GameObject);
		// 		SkillDagger skillSword = tempGameObject.GetComponent<SkillDagger>();
		// 		skillSword.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_SPELLLIGHTING:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillDagger", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillDagger", typeof(GameObject)) as GameObject);
		// 		SkillDagger skillDagger = tempGameObject.GetComponent<SkillDagger>();
		// 		skillDagger.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_SWORD:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillSanji", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSanji", typeof(GameObject)) as GameObject);
		// 		SkillSanji skillSanji = tempGameObject.GetComponent<SkillSanji>();
		// 		skillSanji.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_SPELLFIRE:
		// 	tempGameObject = MyPoolManager.Instance.Spawn("SkillSpellFire", skillCamera.transform).gameObject;
		// 	// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSwordBlue", typeof(GameObject)) as GameObject);
		// 	SkillSpellFire skillSpellFire = tempGameObject.GetComponent<SkillSpellFire>();
		// 	skillSpellFire.Init(level, damage, bossManager, fromPos);
		// 	break;
		// 	case SlotItemDragon.ITEM_SPELLRUBY:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillLuffy", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillLuffy", typeof(GameObject)) as GameObject);
		// 		SkillLuffy skillLuffy = tempGameObject.GetComponent<SkillLuffy>();
		// 		skillLuffy.Init(level, damage, bossManager);
		// 	break;
		// 	case SlotItemDragon.ITEM_METEOR:
		// 		tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
		// 		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
		// 		skill = tempGameObject.GetComponent<SkillFireBall>();
		// 		skill.Init(level, damage, bossManager);
		// 	break;
		// }
		tempGameObject = MyPoolManager.Instance.Spawn("SkillMeteor", skillCamera.transform).gameObject;
		// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSwordBlue", typeof(GameObject)) as GameObject);
		SkillMeteor skillKnife = tempGameObject.GetComponent<SkillMeteor>();
		skillKnife.Init(3, damage, bossManager, fromPos);
	}
	
	public override void ShowFreeSpinAnimation() {
		Utils.SetActive(freeSpinLeft, true);
	}

	public override void StopFreeSpinAnimation() {
		Utils.SetActive(freeSpinLeft, false);
	}
	
	public override void UpdateFreeSpinText(int numb) {
		freeSpinLeftLabel.text = Localization.Format("FreeSpinCountText", numb.ToString());
	}	
}
