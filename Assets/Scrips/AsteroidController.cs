using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

	public float range;
	public Transform player = null;
	private float distance;
	public GameObject asteroidExplosion;
	public int health = 50;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {

		if (health <= 0) {
			this.explodeAsteroid ();
		}
		distance = Vector3.Distance (player.transform.position, this.transform.position);
		if (distance > range) {
			this.transform.position = (player.transform.position - this.transform.position) + player.transform.position;

		}

	}
	public void applyDamage(int damage)
	{
		health -= damage;
	}

	public void explodeAsteroid()
	{
		Instantiate (asteroidExplosion, this.transform.position, this.transform.rotation);
		gameObject.SetActive(false);

	}
}
