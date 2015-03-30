using UnityEngine;
using System.Collections;

public class BossPirate : Boss {

	private string[] bossSpriteNames = new string[3] {"boa", "crocodile", "hawkEye"};

	// TEST CODE - check UHD and HD
	public override string GetBossImage(int type) {
		return "Atlas/SlotMachinePirateUHD/Bosses/" + bossSpriteNames[type];
	}
}
