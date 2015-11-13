using UnityEngine;
using System.Collections;
using EZObjectPools;

public class GameController : MonoBehaviour {
	public static float Unit = 2.5f;
	public GUIController GUI = null;
	
	public Transform Player = null;
	public GameObject EnemyShip1Prefab = null;
	public GameObject EnemyShip2Prefab = null;
	public GameObject EnemyShip3Prefab = null;
	public GameObject EnemyMech1Prefab = null;
	public GameObject EnemyMech2Prefab = null;
	public GameObject EnemyMech3Prefab = null;
	public Transform asteroid1Prefab = null;
	public Transform asteroid2Prefab = null;
	public Transform asteroid3Prefab = null;
	public float asteroidRange;
	
	public float SpawnInterval;
	private float spawnTimer;
	public int MaxWaves;
	private int currentWave = 1;

	private bool inProgress = true;
	private int totalSoulScore;
	
	private static class Enemy {
		public const int Ship1 = 0;
		public const int Mech1 = 1;
		public const int Ship2 = 2;
		public const int Mech2 = 3;
		public const int Ship3 = 4;
		public const int Mech3 = 5;
	}
	
	private Wave[] waves;

	// Use this for initialization
	void Start () {
		//Spawn ();
		createAsteroidField ();
		totalSoulScore = 0;
		spawnTimer = 0;
		waves = new Wave[MaxWaves];

		int[] enemies0 = new int[6];
		enemies0[Enemy.Ship1] = 20;
		enemies0[Enemy.Ship2] = 0;
		enemies0[Enemy.Ship3] = 0;
		enemies0[Enemy.Mech1] = 0;
		enemies0[Enemy.Mech2] = 0;
		enemies0[Enemy.Mech3] = 0;
		waves [0] = new Wave (this, enemies0, 200);

		int[] enemies1 = new int[6];
		enemies1[Enemy.Ship1] = 10;
		enemies1[Enemy.Ship2] = 0;
		enemies1[Enemy.Ship3] = 0;
		enemies1[Enemy.Mech1] = 10;
		enemies1[Enemy.Mech2] = 0;
		enemies1[Enemy.Mech3] = 0;
		waves [1] = new Wave (this, enemies1, 200);
	}

	// Update is called once per frame
	void Update () {
		//spawn enemies
		spawnTimer += Time.deltaTime;
		while (spawnTimer >= SpawnInterval) {
			spawnTimer -= SpawnInterval;
			waves[currentWave].spawnNext();
		}
		//update GUI
		GUI.souls.text = totalSoulScore.ToString("D4");
		MechController mech = Player.GetComponent<MechController> ();
		float health = (float)mech.health / (float)mech.getMaxHealth();
		float shield = (float)mech.shield / (float)mech.getMaxShield();
		GUI.health.sizeDelta = new Vector2(GUI.getHealthWidth() * health, GUI.health.rect.height);
		GUI.shield.sizeDelta = new Vector2(GUI.getShieldWidth() * shield, GUI.shield.rect.height);
	}

	void createAsteroidField (){
		for (int i = 0; i<500; i++) {
			Transform asteroid1 = (Transform)Instantiate (asteroid1Prefab, Player.transform.position + (Random.insideUnitSphere * asteroidRange), Random.rotation);
			float magnitudeX = Random.Range (2, 25);
			float magnitudeY = Random.Range (2, 25);
			float magnitudeZ = Random.Range (2, 25);
			asteroid1.GetComponent<AsteroidController>().player = Player;
			asteroid1.GetComponent<AsteroidController>().range = asteroidRange;
			asteroid1.transform.localScale = new Vector3(magnitudeX, magnitudeY, magnitudeZ);


		}
		for (int i = 0; i<500; i++) {
			Transform asteroid2 = (Transform)Instantiate (asteroid2Prefab, Player.transform.position + (Random.insideUnitSphere * asteroidRange), Random.rotation);
			float magnitudeX = Random.Range (2, 25);
			float magnitudeY = Random.Range (2, 25);
			float magnitudeZ = Random.Range (2, 25);
			asteroid2.GetComponent<AsteroidController>().player = Player;
			asteroid2.GetComponent<AsteroidController>().range = asteroidRange;
			asteroid2.transform.localScale = new Vector3(magnitudeX, magnitudeY, magnitudeZ);
			
		}
		for (int i = 0; i<500; i++) {
			Transform asteroid3 = (Transform)Instantiate (asteroid3Prefab, Player.transform.position + (Random.insideUnitSphere * asteroidRange), Random.rotation);
			float magnitudeX = Random.Range (2, 25);
			float magnitudeY = Random.Range (2, 25);
			float magnitudeZ = Random.Range (2, 25);
			asteroid3.GetComponent<AsteroidController>().player = Player;
			asteroid3.GetComponent<AsteroidController>().range = asteroidRange;
			asteroid3.transform.localScale = new Vector3(magnitudeX, magnitudeY, magnitudeZ);
			
		}
	}

	public static Vector3 randomPointOnSphere(float radius) {
		float u = Random.Range(0.0f, 2.0f);
		float v = Random.Range(-1.0f, 1.0f);
		float theta = Mathf.PI * u;
		float phi = Mathf.Acos(v);
		float x = (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
		float y = (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
		float z = (radius * Mathf.Cos(phi));
		return new Vector3(x, y, z);
	}

	public void incrementSoulScore()
	{
		totalSoulScore++;
	}

	public GameObject getPlayer() {
		return Player.gameObject;
	}
	
	private class Wave {
		private static int Num = 0;
		private GameController Control;
		private int[] Enemies;
		private int EnemyCount;
		private EZObjectPool[] Pools;
		private int lastSpawn = 0;

		public Wave(GameController control, int[] enemies, int enemyCount) {
			if (enemies.Length != 6) throw new UnityException("Must have 6 enemy types!");
			Control = control;
			Enemies = enemies;
			EnemyCount = enemyCount;
			Pools = new EZObjectPool[6];
			createPools();
		}

		~Wave() {
			destroyPools();
		}

		public bool isWaveOver() {
			if (EnemyCount > 0)
				return false;
			for (int i=0; i<Pools.Length; i++) {
				if (Pools[i].ActiveObjectCount() > 0)
					return false;
			}
			return true;
		}

		private void createPools() {
			Pools[Enemy.Ship1] = EZObjectPool.CreateObjectPool(Control.EnemyShip1Prefab, "SHIP1_WAVE"+Num, Enemies[Enemy.Ship1], false, true, false);
			Pools[Enemy.Ship2] = EZObjectPool.CreateObjectPool(Control.EnemyShip2Prefab, "SHIP2_WAVE"+Num, Enemies[Enemy.Ship2], false, true, false);
			Pools[Enemy.Ship3] = EZObjectPool.CreateObjectPool(Control.EnemyShip3Prefab, "SHIP3_WAVE"+Num, Enemies[Enemy.Ship3], false, true, false);
			Pools[Enemy.Mech1] = EZObjectPool.CreateObjectPool(Control.EnemyMech1Prefab, "MECH1_WAVE"+Num, Enemies[Enemy.Mech1], false, true, false);
			Pools[Enemy.Mech2] = EZObjectPool.CreateObjectPool(Control.EnemyMech2Prefab, "MECH2_WAVE"+Num, Enemies[Enemy.Mech2], false, true, false);
			Pools[Enemy.Mech3] = EZObjectPool.CreateObjectPool(Control.EnemyMech3Prefab, "MECH3_WAVE"+Num, Enemies[Enemy.Mech3], false, true, false);
			Num++;
		}

		private void destroyPools() {
			for (int i=0; i<Pools.Length; i++) {
				Pools[i].DeletePool(true);
			}
		}
		
		public void spawnNext() {
			if (EnemyCount <= 0)
				return;
			GameObject obj = null;
			for (int i=0; i<Pools.Length; i++) {
				lastSpawn++;
				if (lastSpawn >= Pools.Length) lastSpawn -= Pools.Length;
				if (Pools[lastSpawn].PoolSize > 0)  {
					if (Pools[lastSpawn].TryGetNextObject(Control.Player.transform.position + randomPointOnSphere(50.0f), Quaternion.identity, out obj)) {
						obj.GetComponent<EnemyController>().Player = Control.Player;
						EnemyCount--;
						return;
					}
				}
			}
		}
	}
}
