using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour {
	public float MoveSpeed = 25.0f;
	public float lifeTime = 10.0f;

	private float life = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		life += Time.deltaTime;
		if (life >= lifeTime) 
			Destroy (this.gameObject);

		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float[] distances = new float[enemies.Length];
		bool explode = false;
		for (int i=0; i<enemies.Length; i++) {
			distances[i] = Vector3.Distance (gameObject.transform.position, enemies[i].transform.position) / GameController.Unit;
			if (distances[i] <= 3.0f) {
				explode = true;
			}
		}

		if (explode) {
			for (int i=0; i<enemies.Length; i++) {
				int damage = Mathf.RoundToInt(-1.0f*Mathf.Pow(distances[i], 2.0f) + 20.0f);
				if (damage > 0)
					enemies[i].GetComponent<EnemyController> ().applyDamage (damage);
			}
			Destroy(gameObject);
		}
	}
}
