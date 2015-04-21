using UnityEngine;
using System.Collections;

public class BossCrocodile : Boss {
	
	public GameObject lightGlow;
	
	private Vector3[] lightPosition = new Vector3[3];
	
	public override Vector3 GetPosition() {
		return new Vector3(0, 160f, 0);
	}
	
	public override void Init() {
		base.Init();
		lightPosition[0] = new Vector3(-100f, 160f, 0);
		lightPosition[1] = new Vector3(-51f, 102f, 0);
		lightPosition[2] = new Vector3(42f, 143f, 0);
		StartCoroutine(StartGlowEffect());
	}
	
	IEnumerator StartGlowEffect() {
		yield return new WaitForSeconds(Random.Range(1f, 5f));
		lightGlow.transform.localScale = Vector3.zero;
		lightGlow.transform.localPosition = lightPosition[Random.Range(0, 3)];
		LeanTween.scale(lightGlow, Vector3.one, 1.5f).setOnComplete(ScaleDownGlowEffect);
	}
	
	void ScaleDownGlowEffect() {
		LeanTween.scale(lightGlow, Vector3.zero, 1f).setOnComplete(MoveGlowEffect);
	}
	
	void MoveGlowEffect() {
		if (gameObject != null && gameObject.activeSelf) {
			StartCoroutine(StartGlowEffect());
		}
	}
	
	public override void Destroy() {
		StopCoroutine(StartGlowEffect());
		base.Destroy();
	}
}
