using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

	public float range;
	public Transform player = null;
	private float distance;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance (player.transform.position, this.transform.position);
		if (distance > range) {
			this.transform.position = (player.transform.position - this.transform.position) + player.transform.position;

		}
	
	}
}
