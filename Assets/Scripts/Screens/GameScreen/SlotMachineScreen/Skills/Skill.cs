using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

  public enum Type {
    FIRE_BALL,
    THUNDER,
		SWORD_ATTACK_BLUE,
		SWORD_DAGGER,
		BITE,
		LUFFY_HAND
  }

	public Type type;
	// public Animator animator;
	// public SpriteRenderer sprite;
	
  [HideInInspector]
	public Vector3 size = new Vector3(50f, 50f, 1);
  [HideInInspector]
	public int damage;
  [HideInInspector]
	public int level;
  [HideInInspector]
	public Boss boss;

	// example of fireball 
	public virtual void Init(int level, int damage, Boss boss) {
		// transform.localScale = size;
		// transform.position = startPos;
		// Vector3 target = boss.bossSprite.transform.position;
		// // Rotate skill to look at target
		// transform.rotation = Quaternion.Euler(0 , 0, Mathf.Atan2((target.y - transform.position.y), (target.x - transform.position.x))*Mathf.Rad2Deg);
		// this.boss = boss;
		// this.damage = damage;
		// sprite.color = new Color(1f,1f,1f,0f);
		// LeanTween.move(gameObject, target, 0.6f).setEase(LeanTweenType.easeInQuad).setOnComplete(Explode);
		// LeanTween.value(gameObject, UpdateSpriteAlpha, 0, 1f, 0.2f);
	}

	// Fade in skill
	// private void UpdateSpriteAlpha(float val) {
	// 	sprite.color = new Color(1f,1f,1f,val);
	// }
	//
	// public virtual void Explode() {
	// 	boss.GetHit(damage);
	// 	animator.SetBool("explode", true);
	// }
	//
	// public void Destroy() {
	// 	Destroy(gameObject);
	// }
}
