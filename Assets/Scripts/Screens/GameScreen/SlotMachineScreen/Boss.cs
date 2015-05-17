using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class Boss : MonoBehaviour {

	public UITexture bossSprite;
	public Transform middlePoint;

	private int type;
	
	public virtual Vector3 GetPosition() {
		return Vector3.zero;
	}
	
	public virtual void Init() {
		transform.localPosition = GetPosition();
	}
	
	public virtual void Destroy() {
		MyPoolManager.Instance.Despawn(transform);
	}
}
