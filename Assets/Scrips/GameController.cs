using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EZObjectPools;

public class GameController : MonoBehaviour {
	public static float Unit = 2.5f;
	public GUIController GUI = null;
	public Canvas WaveMenu = null;
	public Canvas Victory = null;
	public Canvas Defeat = null;
	public Canvas Pause = null;
	
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
	public float WaveInterval;
	private bool GameOver = false;
	private float spawnTimer;
	private int MaxWaves = 8;
	private int currentWave = 0;

	private float waveTime = 0.0f;
	private int waveSouls = 0;

	private bool showWaveMenu = false;

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
		GameOver = false;
		Cursor.visible = false;
		createAsteroidField ();
		waveTime = 0.0f;
		waveSouls = 0;
		spawnTimer = SpawnInterval;
		waves = new Wave[MaxWaves];

		int[] enemies0 = new int[6];
		enemies0[Enemy.Ship1] = 20;
		enemies0[Enemy.Ship2] = 0;
		enemies0[Enemy.Ship3] = 0;
		enemies0[Enemy.Mech1] = 0;
		enemies0[Enemy.Mech2] = 0;
		enemies0[Enemy.Mech3] = 0;
		waves [0] = new Wave (this, enemies0, 100);

		int[] enemies1 = new int[6];
		enemies1[Enemy.Ship1] = 10;
		enemies1[Enemy.Ship2] = 0;
		enemies1[Enemy.Ship3] = 0;
		enemies1[Enemy.Mech1] = 10;
		enemies1[Enemy.Mech2] = 0;
		enemies1[Enemy.Mech3] = 0;
		waves [1] = new Wave (this, enemies1, 100);
		
		int[] enemies2 = new int[6];
		enemies2[Enemy.Ship1] = 8;
		enemies2[Enemy.Ship2] = 2;
		enemies2[Enemy.Ship3] = 0;
		enemies2[Enemy.Mech1] = 8;
		enemies2[Enemy.Mech2] = 2;
		enemies2[Enemy.Mech3] = 0;
		waves [2] = new Wave (this, enemies2, 100);
		
		int[] enemies3 = new int[6];
		enemies3[Enemy.Ship1] = 5;
		enemies3[Enemy.Ship2] = 5;
		enemies3[Enemy.Ship3] = 0;
		enemies3[Enemy.Mech1] = 5;
		enemies3[Enemy.Mech2] = 5;
		enemies3[Enemy.Mech3] = 0;
		waves [3] = new Wave (this, enemies3, 100);
		
		int[] enemies4 = new int[6];
		enemies4[Enemy.Ship1] = 2;
		enemies4[Enemy.Ship2] = 8;
		enemies4[Enemy.Ship3] = 0;
		enemies4[Enemy.Mech1] = 2;
		enemies4[Enemy.Mech2] = 8;
		enemies4[Enemy.Mech3] = 0;
		waves [4] = new Wave (this, enemies4, 100);
		
		int[] enemies5 = new int[6];
		enemies5[Enemy.Ship1] = 2;
		enemies5[Enemy.Ship2] = 5;
		enemies5[Enemy.Ship3] = 3;
		enemies5[Enemy.Mech1] = 2;
		enemies5[Enemy.Mech2] = 5;
		enemies5[Enemy.Mech3] = 3;
		waves [5] = new Wave (this, enemies5, 100);
		
		int[] enemies6 = new int[6];
		enemies6[Enemy.Ship1] = 0;
		enemies6[Enemy.Ship2] = 5;
		enemies6[Enemy.Ship3] = 5;
		enemies6[Enemy.Mech1] = 0;
		enemies6[Enemy.Mech2] = 5;
		enemies6[Enemy.Mech3] = 5;
		waves [6] = new Wave (this, enemies6, 100);
		
		int[] enemies7 = new int[6];
		enemies7[Enemy.Ship1] = 0;
		enemies7[Enemy.Ship2] = 2;
		enemies7[Enemy.Ship3] = 8;
		enemies7[Enemy.Mech1] = 0;
		enemies7[Enemy.Mech2] = 2;
		enemies7[Enemy.Mech3] = 8;
		waves [7] = new Wave (this, enemies7, 100);

		waves [currentWave].createPools ();
	}

	// Update is called once per frame
	void Update () {
		if (GameOver)
			return;
		Canvas over = null;
		if (currentWave >= waves.Length) {
			GameOver = true;
			over = Victory;
		} else if (Player.GetComponent<MechController> ().health <= 0) {
			GameOver = true;
			over = Defeat;
		}
		if (GameOver) {
			GUI.GUICanvas.gameObject.SetActive(false);
			over.gameObject.SetActive(true);
			Cursor.visible = true;

			int souls = Player.GetComponent<MechController>().getSoulScore();
			float time = Time.timeSinceLevelLoad;
			over.transform.Find("Panel2").Find ("SoulsCount").GetComponent<Text>().text = souls.ToString("D3") + "/" + (currentWave * 100).ToString("D3");
			over.transform.Find("Panel2").Find ("TimeCount").GetComponent<Text>().text = Mathf.FloorToInt(time / 60).ToString("00") + ":" + Mathf.RoundToInt(time % 60).ToString("00");

			bool insert = false;
			string outSouls = "";
			string outTime = "";
			for (int i=0; i<10; i++) {
				int highSouls = PlayerPrefs.GetInt ("high-souls-" + i);
				float highTime = PlayerPrefs.GetFloat("high-time-" + i);
				if (insert == false) {
					if (souls > highSouls || (souls == highSouls && time > highTime)) {
						for (int j=9; j>i; j--) {
							PlayerPrefs.SetInt("high-souls-" + j, PlayerPrefs.GetInt ("high-souls-" + (j-1)));
							PlayerPrefs.SetFloat("high-time-" + j, PlayerPrefs.GetFloat ("high-souls-" + (j-1)));
							PlayerPrefs.SetInt("high-wave-" + j, PlayerPrefs.GetInt ("high-wave-" + (j-1)));
						}
						PlayerPrefs.SetInt("high-souls-" + i, souls);
						PlayerPrefs.SetFloat("high-time-" + i, time);
						PlayerPrefs.SetInt("high-wave-" + i, currentWave);
						PlayerPrefs.Save();
						insert = true;
					}
				}
				outSouls += PlayerPrefs.GetInt ("high-souls-" + i).ToString("D3") + "/" + (PlayerPrefs.GetInt ("high-wave-" + i) * 100).ToString("D3") + "\n";
				outTime += Mathf.FloorToInt(PlayerPrefs.GetFloat("high-time-" + i) / 60).ToString("00") + ":" + Mathf.RoundToInt(PlayerPrefs.GetFloat("high-time-" + i) % 60).ToString("00") + "\n";
			}

			over.transform.Find("Panel3").Find("SoulsCount").GetComponent<Text>().text = outSouls;
			over.transform.Find("Panel3").Find("TimeCount").GetComponent<Text>().text = outTime;

			return;
		}
		if (Input.GetButtonDown("Pause")) {
			if (Time.timeScale == 0.0f) {
				this.unPause();
			} else if (Cursor.visible == false) {
				this.pause();
			}
			return;
		}
		if (Cursor.visible == true) 
			return;
		if (Input.GetButtonDown ("Cheat")) {
			forceNextWave();
			for (int i=0; i<100; i++)
				Player.GetComponent<MechController>().incrementSoulScore();
			return;
		}
		if (waves [currentWave].isWaveOver ()) {
			if (showWaveMenu == true) {
				spawnTimer -= Time.deltaTime;
				if (spawnTimer <= 0.0f) {
					GUI.GUICanvas.gameObject.SetActive(false);
					WaveMenu.gameObject.SetActive(true);
					Cursor.visible = true;
					WaveMenu.transform.Find ("Panel1").Find("WaveText").GetComponent<Text>().text = "Wave " + (currentWave+1) + " Complete";
					int souls = Player.GetComponent<MechController>().getSoulScore() - waveSouls;
					WaveMenu.transform.Find ("Panel2").Find("SoulsCount").GetComponent<Text>().text = souls.ToString("D3") + "/100";
					WaveMenu.transform.Find ("Panel2").Find("TimeCount").GetComponent<Text>().text = Mathf.FloorToInt(waveTime / 60).ToString("00") + ":" + Mathf.RoundToInt(waveTime % 60).ToString("00");
				}
			} else {
				showWaveMenu = true;
				Debug.Log ("Wave " + currentWave + "Complete");
				spawnTimer = WaveInterval;
			}
		} else {
			//spawn enemies
			spawnTimer -= Time.deltaTime;
			while (spawnTimer <= 0.0f) {
				spawnTimer += SpawnInterval;
				waves [currentWave].spawnNext ();
			}
			waveTime += Time.deltaTime;
		}
		//update GUI
		MechController mech = Player.GetComponent<MechController> ();
		float health = (float)mech.health / (float)mech.getMaxHealth();
		float shield = (float)mech.shield / (float)mech.getMaxShield();
		GUI.health.sizeDelta = new Vector2(GUI.getHealthWidth() * health, GUI.health.rect.height);
		GUI.shield.sizeDelta = new Vector2(GUI.getShieldWidth() * shield, GUI.shield.rect.height);
	}

	public void nextWave() {
		if (!waves [currentWave].isWaveOver ())
			return; 

		WaveMenu.gameObject.SetActive (false);
		GUI.GUICanvas.gameObject.SetActive(true);
		Cursor.visible = false;
		showWaveMenu = false;

		waves[currentWave].destroyPools();
		currentWave++;
		if (currentWave < waves.Length) {
			waves [currentWave].createPools ();
			waveTime = 0.0f;
			waveSouls = Player.GetComponent<MechController> ().getSoulScore ();
			spawnTimer = SpawnInterval;
		}
	}

	public void goToMainMenu() {
		Application.LoadLevel ("menu");
	}

	public void pause() {
		Time.timeScale = 0.0f;
		Camera.main.GetComponent<AudioSource> ().Pause ();
		Cursor.visible = true;
		Pause.gameObject.SetActive(true);
	}

	public void unPause() {
		Pause.gameObject.SetActive (false);
		Cursor.visible = false;
		Camera.main.GetComponent<AudioSource> ().UnPause ();
		Time.timeScale = 1.0f;

	}

	private void forceNextWave() {
		WaveMenu.gameObject.SetActive (false);
		GUI.GUICanvas.gameObject.SetActive(true);
		Cursor.visible = false;
		showWaveMenu = false;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.SetActive(false);
		}

		waves[currentWave].destroyPools();
		currentWave++;
		if (currentWave < waves.Length) {
			waves [currentWave].createPools ();
			waveTime = 0.0f;
			waveSouls = Player.GetComponent<MechController> ().getSoulScore ();
			spawnTimer = SpawnInterval;
		}
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

		public void createPools() {
			Pools[Enemy.Ship1] = EZObjectPool.CreateObjectPool(Control.EnemyShip1Prefab, "SHIP1_WAVE"+Num, Enemies[Enemy.Ship1], false, true, false);
			Pools[Enemy.Ship2] = EZObjectPool.CreateObjectPool(Control.EnemyShip2Prefab, "SHIP2_WAVE"+Num, Enemies[Enemy.Ship2], false, true, false);
			Pools[Enemy.Ship3] = EZObjectPool.CreateObjectPool(Control.EnemyShip3Prefab, "SHIP3_WAVE"+Num, Enemies[Enemy.Ship3], false, true, false);
			Pools[Enemy.Mech1] = EZObjectPool.CreateObjectPool(Control.EnemyMech1Prefab, "MECH1_WAVE"+Num, Enemies[Enemy.Mech1], false, true, false);
			Pools[Enemy.Mech2] = EZObjectPool.CreateObjectPool(Control.EnemyMech2Prefab, "MECH2_WAVE"+Num, Enemies[Enemy.Mech2], false, true, false);
			Pools[Enemy.Mech3] = EZObjectPool.CreateObjectPool(Control.EnemyMech3Prefab, "MECH3_WAVE"+Num, Enemies[Enemy.Mech3], false, true, false);
			Num++;
		}

		public void destroyPools() {
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
