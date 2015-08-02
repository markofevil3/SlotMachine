using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotDragonScreen : BaseSlotMachineScreen {
	
	public GameObject freeSpinLeft;
	public UILabel freeSpinLeftLabel;
	
	public override void SpawnSkill(int type, int level, int damage, Vector3 fromPos, bool isYou) {
		GameObject tempGameObject;
		SkillFireBall skill;
		switch (type) {
			case SlotItemDragon.ITEM_TRAP:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillBite", skillCamera.transform).gameObject;
				SkillBite skillBite = tempGameObject.GetComponent<SkillBite>();
				skillBite.Init(level, damage, bossManager);
			break;
			case SlotItemDragon.ITEM_BOW:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillBow", skillCamera.transform).gameObject;
				SkillBow skillBow = tempGameObject.GetComponent<SkillBow>();
				skillBow.Init(level, damage, bossManager, fromPos);
			break;
			case SlotItemDragon.ITEM_SPELLWATER:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSpellIce", skillCamera.transform).gameObject;
				SkillSpellIce skillSpellIce = tempGameObject.GetComponent<SkillSpellIce>();
				skillSpellIce.Init(level, damage, bossManager, fromPos);
			break;
			case SlotItemDragon.ITEM_AXE:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillAxe", skillCamera.transform).gameObject;
				SkillAxe skillAxe = tempGameObject.GetComponent<SkillAxe>();
				skillAxe.Init(level, damage, bossManager);
			break;
			case SlotItemDragon.ITEM_BOMB:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSkullBomb", skillCamera.transform).gameObject;
				SkillSkullBomb skillBomb = tempGameObject.GetComponent<SkillSkullBomb>();
				skillBomb.Init(level, damage, bossManager);
			break;
			case SlotItemDragon.ITEM_SPELLLIGHTING:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillDagger", skillCamera.transform).gameObject;
				SkillDagger skillDagger = tempGameObject.GetComponent<SkillDagger>();
				skillDagger.Init(level, damage, bossManager);
			break;
			case SlotItemDragon.ITEM_SWORD:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSanji", skillCamera.transform).gameObject;
				SkillSanji skillSanji = tempGameObject.GetComponent<SkillSanji>();
				skillSanji.Init(level, damage, bossManager);
			break;
			case SlotItemDragon.ITEM_SPELLFIRE:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillSpellFire", skillCamera.transform).gameObject;
				SkillSpellFire skillSpellFire = tempGameObject.GetComponent<SkillSpellFire>();
				skillSpellFire.Init(level, damage, bossManager, fromPos);
			break;
			case SlotItemDragon.ITEM_METEOR:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillMeteor", skillCamera.transform).gameObject;
				SkillMeteor skillMeteor = tempGameObject.GetComponent<SkillMeteor>();
				skillMeteor.Init(level, damage, bossManager, fromPos);
			break;
			// case SlotItemDragon.ITEM_SPELLRUBY:
			// 	tempGameObject = MyPoolManager.Instance.Spawn("SkillIcePurple", skillCamera.transform).gameObject;
			// 	SkillIcePurple skillIcePurple = tempGameObject.GetComponent<SkillIcePurple>();
			// 	skillIcePurple.Init(level, damage, bossManager);
			// break;
		}
		// tempGameObject = MyPoolManager.Instance.Spawn("SkillSkullBomb", skillCamera.transform).gameObject;
		// SkillSkullBomb skillAxe = tempGameObject.GetComponent<SkillSkullBomb>();
		// skillAxe.Init(3, damage, bossManager);
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
