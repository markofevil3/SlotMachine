using UnityEngine;
using System.Collections;

public class BossMan : Boss {
	
	public override Vector3 GetPosition() {
		return new Vector3(14f, -50f, 0);
	}
	
	public override void Init() {
		base.Init();
	}
	
	public override void Destroy() {
		base.Destroy();
	}
}
