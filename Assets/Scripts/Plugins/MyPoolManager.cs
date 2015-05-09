using UnityEngine;
using System.Collections;
using PathologicalGames;

public class MyPoolManager : MonoBehaviour {

	public static MyPoolManager Instance { get; private set; }

	private string poolName = "SlotMachine";
	private SpawnPool spawnPool;
	

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	public Transform Spawn(string prefabName) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		return spawnPool.Spawn(prefabName);
	}
	
	public Transform Spawn(string prefabName, Transform parent) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		return spawnPool.Spawn(prefabName, parent);
	}
	
  public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot, Transform parent) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		return spawnPool.Spawn(prefabName, pos, rot, parent);
  }
	
	
	public void Despawn(Transform prefab) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		spawnPool.Despawn(prefab, spawnPool.transform);
	}
	
  public void Despawn(Transform prefab, float seconds) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		spawnPool.Despawn(prefab, seconds, spawnPool.transform);
	}
	
}
