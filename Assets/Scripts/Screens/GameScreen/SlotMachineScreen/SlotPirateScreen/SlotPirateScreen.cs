using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotPirateScreen : BaseSlotMachineScreen {
	
	private JSONObject specialData;	

  public override void Init(object[] data) {
		WINNING_ANIMATION_PREFAB = Global.SCREEN_PATH + "/GameScreen/WinningAnimation/FruitWinningAnimation";
    base.Init(data);
		bossManager.Init(roomData.GetInt("dIndex"), roomData.GetInt("dHP"), roomData.GetInt("dMaxHP"), this, "BossGetHitCallback");
		// TEST CODE - spawn a skill
		// InvokeRepeating("Test", 2f, 3f);
  }
	
	// int count = 0;
	//
	// void Test() {
	// 	SpawnSkill(SlotItemPirate.ITEM_RALLY, 1, 10);
	// 	count++;
	// }
	
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
		Debug.Log("OtherPlayerSpinResult " + jsonData.ToString());
		// TEST CODE: commented - should remove
		// SpawnSkill(jsonData.GetInt("totalWin"), playerSlot.transform.position);
	}
	
	public override void SpawnSkill(SpawnableSkill skill) {
		SpawnSkill(skill.type, skill.level, skill.damage);
	}
	
	public override void SpawnSkill(int type, int level, int damage) {
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
				tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
				skill = tempGameObject.GetComponent<SkillFireBall>();
				skill.Init(level, damage, bossManager);
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
			case SlotItemPirate.ITEM_RALLY:
				tempGameObject = MyPoolManager.Instance.Spawn("SkillFireBall", skillCamera.transform).gameObject;
				// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillFireBall", typeof(GameObject)) as GameObject);
				skill = tempGameObject.GetComponent<SkillFireBall>();
				skill.Init(level, damage, bossManager);
			break;
		}
			// tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillSanji", typeof(GameObject)) as GameObject);
			// SkillSanji skillLuffy = tempGameObject.GetComponent<SkillSanji>();
			// skillLuffy.Init(level, damage, bossManager);
	}
	
	
	private void SpawnSkill(int damage, Vector3 startPos) { // startPos is world position
		// TEST CODE -- commented
		//   	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/Lighting", typeof(GameObject)) as GameObject);
		// Skill skill = tempGameObject.GetComponent<Skill>();
		// skill.Init(bossManager, damage, startPos);
		//###########
		GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SkillThunder", typeof(GameObject)) as GameObject);
		SkillThunder skill = tempGameObject.GetComponent<SkillThunder>();
		skill.Init(3, damage, bossManager);
	}
	
	private void BossGetHitCallback() {
		if (specialData != null && specialData.ContainsKey("dHP") && specialData.GetInt("dHP") == 0) {
			bossManager.ChangeBoss(specialData.GetObject("newBoss"), "EventFinishChangeBoss");
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
