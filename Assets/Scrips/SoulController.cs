using UnityEngine;
using System.Collections;

public class SoulController : MonoBehaviour {
	public float RotateSpeed;
	public float MoveSpeed;
	private GameObject player;
	private float soulTimer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		soulTimer = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0, RotateSpeed * Time.deltaTime, 0);
		soulTimer -= Time.deltaTime;
		if (soulTimer <= 0.0f) {
			Destroy (this.gameObject);
			return;
		}
		if (Vector3.Distance (player.transform.position, this.gameObject.transform.position) < 10.0f) {
			Vector3 toPlayer = Vector3.Normalize (player.transform.position - this.gameObject.transform.position);
			this.gameObject.GetComponent<Rigidbody> ().velocity = toPlayer * MoveSpeed;
		} else {
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			player.GetComponent<MechController>().incrementSoulScore();
			Destroy (this.gameObject);
		}
	}
}
