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
	
	public void Despawn(Transform prefab) {
		if (spawnPool == null) {
			spawnPool = PoolManager.Pools[poolName];
		}
		spawnPool.Despawn(prefab);
	}
}
