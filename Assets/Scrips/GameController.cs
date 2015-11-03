using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int MaxEnemies = 25;
	public Transform Player = null;
	public Transform EnemyShipPrefab = null;
	public Transform EnemyMechPrefab = null;
	private Transform[] enemyList = null;

	private int railgunDamage = 12;
	private float railgunCooldown = 1.0f;
	private int autocannonDamage = 6;
	private float autocannonCooldown = 0.5f;
	private int machinegunDamage = 3;
	private float machinegunCooldown = 0.25f;


	private bool inProgress = true;
	// Use this for initialization
	void Start () {
		enemyList = new Transform[MaxEnemies];
		Spawn ();
	}

	void Spawn() {
		for (int i=0; i<MaxEnemies; i++) {
			Transform enemy = (Transform) Instantiate(EnemyShipPrefab, Player.transform.position + randomPointOnSphere(50.0f), Quaternion.identity);
			enemy.GetComponent<EnemyShipController>().Player = Player;
			enemyList[i] = enemy;
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
		//enemyObject.GetComponent<EnemyShipController> ().applyDamage (autocannonDamage);
		enemyObject.SendMessage ("applyDamage", autocannonDamage);
	}
}
