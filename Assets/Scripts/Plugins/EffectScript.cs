using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour {

	private ParticleSystem ps;

	void OnEnable() {
		if (ps == null) {
			ps = this.GetComponent<ParticleSystem>();
		}
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive () {
		while(true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if(!ps.IsAlive(true)) {
				MyPoolManager.Instance.Despawn(transform);
				break;
			}
		}
	}
}
