﻿using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotDragonScreen : BaseSlotMachineScreen {
	
	private JSONObject specialData;

  public override void Init(object[] data) {
    base.Init(data);
		bossManager.Init(roomData.GetInt("dIndex"), roomData.GetInt("dHP"), roomData.GetInt("dMaxHP"), this, "BossGetHitCallback");
  }
	
	// // Slot reel stopped, displayed result, start display winning animation if should
	// public override void EventFinishSpin(bool isBigWin, int winningCash) {
	// 	if (winningCash > 0) {
	// 		if (specialData.GetInt("dHP") == 0) {
	// 			slotMachine.Wait();
	// 		}
	// 		SpawnSkill(winningCash, userAvatarPanel.position);
	// 	}
	// 	base.EventFinishSpin(isBigWin, winningCash);
	// }
	
	public override void OtherPlayerSpinResult(string username, JSONObject jsonData) {
		PlayerSlotScript playerSlot = FindUserSlot(username);
		SpawnSkill(jsonData.GetInt("totalWin"), playerSlot.transform.position);
	}
	
	
	private void SpawnSkill(int damage, Vector3 startPos) { // startPos is world position
  	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/Lighting", typeof(GameObject)) as GameObject);
		Skill skill = tempGameObject.GetComponent<Skill>();
		// skill.Init(bossManager, damage, startPos);
		skill.Init(3, damage, bossManager);
	}
	
	private void BossGetHitCallback() {
		Debug.Log("BossGetHitCallback");
		if (specialData.GetInt("dHP") == 0) {
			Debug.Log("Boss DEAD");
			bossManager.ChangeBoss(specialData.GetObject("newBoss"), "EventFinishChangeBoss");
			int dropCash = (int)specialData.GetArray("dropItems")[0].Number;
			
			slotMachine.Wait();
			
	    // AccountManager.Instance.UpdateUserCash(dropCash);
	    UpdateUserCashLabel(dropCash);
		}
	}
	
	private void EventFinishChangeBoss() {
		slotMachine.Resume();
	}
}
