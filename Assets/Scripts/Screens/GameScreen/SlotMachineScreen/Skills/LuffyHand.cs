﻿using UnityEngine;
using System.Collections;

public class LuffyHand : MonoBehaviour {

	private float iter;
	public LeanTweenPath ltPath;
	public GameObject hand;
	public GameObject hitEffect;
	public float delay;

	private LTSpline s;
	private Boss boss;
	private int damage;

	void Start () {
		s = new LTSpline( ltPath.splineVector() );
		LeanTween.moveSpline( hand, s.pts, .4f).setDelay(delay).setOnComplete(HitTarget);
	}
	
	public void Init(Boss boss, int damage) {
		this.boss = boss;
		this.damage = damage;
	}
	
	void HitTarget() {
		hitEffect.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 0.15f, hand.transform.position.z);
		hitEffect.SetActive(true);
		boss.Shake();
		boss.GetHit(damage);
		StartCoroutine(HideHand());
	}
	
	IEnumerator HideHand() {
		yield return new WaitForSeconds(0.2f);
		hand.SetActive(false);
	}
}