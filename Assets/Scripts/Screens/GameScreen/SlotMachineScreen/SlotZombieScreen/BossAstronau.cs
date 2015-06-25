using UnityEngine;
using System.Collections;

public class BossAstronau : Boss {
	
	public override Vector3 GetPosition() {
		return new Vector3(0, 81f, 0);
	}
	
	public override void Init() {
		base.Init();
	}
	
	public override void Destroy() {
		base.Destroy();
	}
}
