using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class SlotDragonScreen : BaseSlotMachineScreen {

	public Boss boss;

  public override void Init(object[] data) {
		WINNING_ANIMATION_PREFAB = Global.SCREEN_PATH + "/GameScreen/WinningAnimation/FruitWinningAnimation";
    base.Init(data);
		boss.Init(roomData.GetInt("dIndex"), roomData.GetInt("dHP"), roomData.GetInt("dMaxHP"));
  }
	
	// Slot reel stopped, displayed result, start display winning animation if should
	public override void EventFinishSpin(bool isBigWin, int winningCash) {
		SpawnSkill(winningCash, userAvatarPanel.position);
		base.EventFinishSpin(isBigWin, winningCash);
	}
	
	public override void OtherPlayerSpinResult(string username, JSONObject jsonData) {
		PlayerSlotScript playerSlot = FindUserSlot(username);
		SpawnSkill(jsonData.GetInt("totalWin"), playerSlot.transform.position);
	}
	
	
	private void SpawnSkill(int damage, Vector3 startPos) { // startPos is world position
  	GameObject tempGameObject = NGUITools.AddChild(skillCamera, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/FireBall", typeof(GameObject)) as GameObject);
		Skill skill = tempGameObject.GetComponent<Skill>();
		skill.Init(boss, damage, startPos);
	}
}
