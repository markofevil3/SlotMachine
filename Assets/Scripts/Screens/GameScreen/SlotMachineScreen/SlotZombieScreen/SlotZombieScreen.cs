using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotZombieScreen : BaseSlotMachineScreen {
	
	public UIPanel mainPanel;
	public GameObject freeSpinLeft;
	public UILabel freeSpinLeftLabel;
	public GameObject[] lampGlows;
	
	private Transform fallingLeave;
	
	public override void SpawnSkill(int type, int level, int damage, Vector3 fromPos, bool isYou) {
		GameObject tempGameObject;
		SkillFireBall skill;
		switch (type) {
			case SlotItemZombie.ITEM_BASEBALL:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillBaseBall", skillCamera.transform).gameObject;
				SkillBaseBall skillBaseBall = tempGameObject.GetComponent<SkillBaseBall>();
				skillBaseBall.Init(level, damage, bossManager);
			break;
			case SlotItemZombie.ITEM_KNIFE:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillKnife", skillCamera.transform).gameObject;
				SkillKnife skillKnife = tempGameObject.GetComponent<SkillKnife>();
				skillKnife.Init(level, damage, bossManager, fromPos);
			break;
			case SlotItemZombie.ITEM_ELECTRICGUN:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillElectricGun", skillCamera.transform).gameObject;
				SkillElectricGun skillElectricGun = tempGameObject.GetComponent<SkillElectricGun>();
				skillElectricGun.Init(level, damage, bossManager);
			break;
			case SlotItemZombie.ITEM_PISTOL:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillPistol", skillCamera.transform).gameObject;
				SkillPistol skillPistol = tempGameObject.GetComponent<SkillPistol>();
				skillPistol.Init(level, damage, bossManager);
			break;
			// TO DO: need new effect
			case SlotItemZombie.ITEM_BROOK:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillMachineGun", skillCamera.transform).gameObject;
				SkillMachineGun skillMachineGun2 = tempGameObject.GetComponent<SkillMachineGun>();
				skillMachineGun2.Init(level, damage, bossManager);
			break;
			case SlotItemZombie.ITEM_CROSSBOW:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillCrossBow", skillCamera.transform).gameObject;
				SkillCrossBow skillCrossBow = tempGameObject.GetComponent<SkillCrossBow>();
				skillCrossBow.Init(level, damage, bossManager, fromPos);
			break;
			case SlotItemZombie.ITEM_MACHINEGUN:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillMachineGun", skillCamera.transform).gameObject;
				SkillMachineGun skillMachineGun = tempGameObject.GetComponent<SkillMachineGun>();
				skillMachineGun.Init(level, damage, bossManager);
			break;
			case SlotItemZombie.ITEM_GRENADE:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillGrenade", skillCamera.transform).gameObject;
				SkillGrenade skillGrenade = tempGameObject.GetComponent<SkillGrenade>();
				skillGrenade.Init(level, damage, bossManager);
			break;
			case SlotItemZombie.ITEM_MISSILE:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillMissile", skillCamera.transform).gameObject;
				SkillMissile skillMissile = tempGameObject.GetComponent<SkillMissile>();
				skillMissile.Init(level, damage, bossManager, fromPos);
			break;
			// case SlotItemZombie.ITEM_RALLY:
			// 	tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
			// 	// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
			// 	skill = tempGameObject.GetComponent<SkillFireBall>();
			// 	skill.Init(level, damage, bossManager);
			// break;
		}
		// tempGameObject = MyPoolManager.Instance.Spawn("SkillMissile", skillCamera.transform).gameObject;
		// SkillMissile skillKnife = tempGameObject.GetComponent<SkillMissile>();
		// skillKnife.Init(level, damage, bossManager, fromPos);
	}
	
	public override void ShowFreeSpinAnimation() {
		Utils.SetActive(freeSpinLeft, true);
		fallingLeave = MyPoolManager.Instance.Spawn("FallingLeaves", skillCamera.transform);
		fallingLeave.localScale = Vector3.one * 525f;
		fallingLeave.position = new Vector3(0, mainPanel.worldCorners[1].y, 0);
		for (int i = 0; i < lampGlows.Length; i++) {
			NGUITools.SetActive(lampGlows[i], true);
		}
	}

	public override void StopFreeSpinAnimation() {
		Utils.SetActive(freeSpinLeft, false);
		if (fallingLeave != null) {
			MyPoolManager.Instance.Despawn(fallingLeave);
			fallingLeave = null;
		}
		for (int i = 0; i < lampGlows.Length; i++) {
			NGUITools.SetActive(lampGlows[i], false);
		}
	}
	
	public override void UpdateFreeSpinText(int numb) {
		freeSpinLeftLabel.text = Localization.Format("FreeSpinCountText", numb.ToString());
	}	
}
