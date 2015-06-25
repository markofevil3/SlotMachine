using UnityEngine;
using System.Collections;

public class BossHomeless : Boss {
	
	public override Vector3 GetPosition() {
		return new Vector3(-36, 2f, 0);
	}
	
	public override void Init() {
		base.Init();
	}
	
	public override void Destroy() {
		base.Destroy();
	}
}
