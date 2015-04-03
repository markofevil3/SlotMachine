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
	
	// Partical effect for each boss
	void SpawnEffect() {
		Vector2 randomPos = Random.insideUnitCircle / 2.5f;		
		GameObject effect = CFX_SpawnSystem.GetRandom();
		effect.transform.position = new Vector3(middlePoint.position.x + randomPos.x, middlePoint.position.y + randomPos.y, 0);
		effect.transform.parent = transform;
		Invoke("SpawnEffect", Random.Range(1f, 10f));
	}
	
	
	// TEST CODe
	void Awake() {
		Invoke("SpawnEffect", Random.Range(1f, 10f));
	}
	
	public virtual void Destroy() {
		MyPoolManager.Instance.Despawn(transform);
	}
}
