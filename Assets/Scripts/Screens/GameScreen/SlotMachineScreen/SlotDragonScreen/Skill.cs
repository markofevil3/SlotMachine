using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

	public Animator animator;
	public SpriteRenderer sprite;
	
	private Vector3 size = new Vector3(50f, 50f, 1);
	private Vector3 rotation = new Vector3(0, 0, 90f);
	private int damage;
	private Boss boss;

	public void Init(Boss boss, int damage) {
		transform.localScale = size;
		transform.localRotation = Quaternion.Euler(rotation);
		this.boss = boss;
		this.damage = damage;
		sprite.color = new Color(1f,1f,1f,0f);
		LeanTween.move(gameObject, boss.bossSprite.transform.position, 0.6f).setEase(LeanTweenType.easeInQuad).setOnComplete(Explode);
		LeanTween.value(gameObject, UpdateSpriteAlpha, 0, 1f, 0.2f);
	}

	// Fade in skill
	private void UpdateSpriteAlpha(float val) {
		sprite.color = new Color(1f,1f,1f,val);
	}

	public void Explode() {
		boss.GetHit(damage);
		animator.SetBool("explode", true);
	}

	public void Destroy() {
		Destroy(gameObject);
	}
}
