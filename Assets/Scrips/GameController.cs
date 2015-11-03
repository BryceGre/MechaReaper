using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int MaxEnemies = 25;
	public Transform Player = null;
	public Transform EnemyShipPrefab = null;
	public Transform EnemyMechPrefab = null;

	private bool inProgress = true;
	// Use this for initialization
	void Start () {
		Spawn ();
	}

	void Spawn() {
		for (int i=0; i<MaxEnemies; i++) {
			Transform enemy = (Transform) Instantiate(EnemyShipPrefab, Player.transform.position + randomPointOnSphere(50.0f), Quaternion.identity);
			enemy.GetComponent<EnemyShipController>().Player = Player;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	Vector3 randomPointOnSphere(float radius) {
		float u = Random.Range(0.0f, 2.0f);
		float v = Random.Range(-1.0f, 1.0f);
		float theta = Mathf.PI * u;
		float phi = Mathf.Acos(v);
		float x = (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
		float y = (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
		float z = (radius * Mathf.Cos(phi));
		return new Vector3(x, y, z);
	}


	void hitScanHit(GameObject enemyObject)
	{
	

	}
}
