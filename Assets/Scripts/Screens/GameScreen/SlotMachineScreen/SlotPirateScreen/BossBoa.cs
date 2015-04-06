using UnityEngine;
using System.Collections;

public class BossBoa : Boss {
	
	public UITexture snake;
	public Transform snakeMovePoint;
	
	private string[] effectName = new string[2] { "CFXM2_PickupHeart2", "CFXM2_PickupHeart3" };
	
	public override Vector3 GetPosition() {
		return new Vector3(0, 250f, 0);
	}

	public override void Init() {
		base.Init();
		StartCoroutine(SpawnEffect());
	}
	
	// Partical effect for each boss
	IEnumerator SpawnEffect() {
		yield return new WaitForSeconds(Random.Range(1f, 5f));
		Vector2 randomPos = Random.insideUnitCircle / 2.5f;
		Transform effect = MyPoolManager.Instance.Spawn(effectName[Random.Range(0, 2)], transform);
		effect.position = new Vector3(middlePoint.position.x + randomPos.x, middlePoint.position.y + randomPos.y, 0);
		StartCoroutine(SpawnEffect());
	}
	
	Vector3 targetPos = Vector3.zero;
	float speed = 0.05f;
	float step;
	Vector2 randomSnakePos;
	
	void FindTarget() {
		randomSnakePos = Random.insideUnitCircle / 15f;
		targetPos = new Vector3(snakeMovePoint.position.x + randomSnakePos.x, snakeMovePoint.position.y + randomSnakePos.y, 0);
	}

  void Update() {
    step = speed * Time.deltaTime;
		if (targetPos == Vector3.zero || targetPos == snake.transform.position) {
			FindTarget();
		}
    snake.transform.position = Vector3.MoveTowards(snake.transform.position, targetPos, step);
  }
	
	public override void Destroy() {
		StopCoroutine(SpawnEffect());
		base.Destroy();
	}
}
