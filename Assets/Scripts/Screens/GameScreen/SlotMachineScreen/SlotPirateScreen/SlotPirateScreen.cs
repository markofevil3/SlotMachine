using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotPirateScreen : BaseSlotMachineScreen {
	
	private JSONObject specialData;

	public Boss boss;

  public override void Init(object[] data) {
		WINNING_ANIMATION_PREFAB = Global.SCREEN_PATH + "/GameScreen/WinningAnimation/FruitWinningAnimation";
    base.Init(data);
		boss.Init(roomData.GetInt("dIndex"), roomData.GetInt("dHP"), roomData.GetInt("dMaxHP"), gameObject, "BossGetHitCallback");
  }
	
	// Set game special data after each spin success
	public override void SetSpecialData(JSONObject jsonData) {
		specialData = jsonData;
		Debug.Log("SetSpecialData " + specialData.ToString());
	}
	
	
	// Slot reel stopped, displayed result, start display winning animation if should
	public override void EventFinishSpin(bool isBigWin, int winningCash) {
		// TEST CODE -- commented for new spawn skill
		// if (winningCash > 0) {
		// 	if (specialData.GetInt("dHP") == 0) {
		// 		slotMachine.Wait();
		// 	}
		// 	SpawnSkill(winningCash, userAvatarPanel.position);
		// }
		base.EventFinishSpin(isBigWin, winningCash);
	}
	
	public override void OtherPlayerSpinResult(string username, JSONObject jsonData) {
		PlayerSlotScript playerSlot = FindUserSlot(username);
		SetSpecialData(jsonData);
		// TEST CODE: commented - should remove
		// SpawnSkill(jsonData.GetInt("totalWin"), playerSlot.transform.position);
	}
	
	public override void SpawnSkill(int type, int level, int damage) {
		// switch (type) {
		// 	case SlotItemPirate.ITEM_CHOPPER:
		// 	break;
		// 	case SlotItemPirate.ITEM_USOOP:
		// 	break;
		// 	case SlotItemPirate.ITEM_NAMI:
		// 		GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/SkillThunder", typeof(GameObject)) as GameObject);
		// 		SkillThunder skill = tempGameObject.GetComponent<SkillThunder>();
		// 		skill.Init(3, damage, boss);
		// 	break;
		// 	case SlotItemPirate.ITEM_FRANKY:
		// 	break;
		// 	case SlotItemPirate.ITEM_BROOK:
		// 	break;
		// 	case SlotItemPirate.ITEM_NICO:
		// 	break;
		// 	case SlotItemPirate.ITEM_SANJI:
		// 	break;
		// 	case SlotItemPirate.ITEM_ZORO:
		// 	break;
		// 	case SlotItemPirate.ITEM_LUFFY:
		// 	break;
		// 	case SlotItemPirate.ITEM_RALLY:
		// 	break;
		// }
		GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/SkillThunder", typeof(GameObject)) as GameObject);
		SkillThunder skill = tempGameObject.GetComponent<SkillThunder>();
		skill.Init(3, damage, boss);
	}
	
	
	private void SpawnSkill(int damage, Vector3 startPos) { // startPos is world position
		// TEST CODE -- commented
		//   	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/Lighting", typeof(GameObject)) as GameObject);
		// Skill skill = tempGameObject.GetComponent<Skill>();
		// skill.Init(boss, damage, startPos);
		//###########
		GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/SkillThunder", typeof(GameObject)) as GameObject);
		SkillThunder skill = tempGameObject.GetComponent<SkillThunder>();
		skill.Init(3, damage, boss);
	}
	
	private void BossGetHitCallback() {
		if (specialData.GetInt("dHP") == 0) {
			boss.ChangeBoss(specialData.GetObject("newBoss"), "EventFinishChangeBoss");
			int dropCash = (int)specialData.GetArray("dropItems")[0].Number;
			
			slotMachine.Wait();
			winningAnimation.SetData(dropCash);
			winningAnimation.FadeIn(true, 2f);
			
	    AccountManager.Instance.UpdateUserCash(dropCash);
	    UpdateUserCashLabel();
		}
	}
	
	private void EventFinishChangeBoss() {
		slotMachine.Resume();
	}
}
