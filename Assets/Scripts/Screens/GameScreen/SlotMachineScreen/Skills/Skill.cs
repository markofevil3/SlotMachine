using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

  public enum Type {
    FIRE_BALL,
    THUNDER,
		SWORD_ATTACK_BLUE,
		SWORD_DAGGER,
		BITE,
		LUFFY_HAND,
		SANJI_KICK,
		PISTOL,
		MACHINE_GUN,
		GRENADE,
		CROSSBOW,
		KNIFE,
		BOW,
		SPELL_FIRE,
		SPELL_ICE,
		METEOR
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
	public BossManager bossManager;

	public ParticleSystem[] listParticles;

	public virtual void Init() {
		listParticles = gameObject.GetComponentsInChildren<ParticleSystem>(true);
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive () {
		yield return new WaitForSeconds(0.5f);
		bool isAlive = false;
		for (int i = 0; i < listParticles.Length; i++) {
			if (listParticles[i].IsAlive(true)) {
				isAlive = true;
			}
		}
		if (!isAlive) {
			Destroy();
		} else {
			StartCoroutine("CheckIfAlive");
		}
	}

	// example of fireball 
	public virtual void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
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

	public virtual void Init(int level, int damage, BossManager bossManager) {}

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
	
	public virtual void Destroy() {
		MyPoolManager.Instance.Despawn(transform);
	}
}
