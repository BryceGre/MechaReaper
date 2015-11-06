using UnityEngine;
using System.Collections;

public class SoulController : MonoBehaviour {
	public float RotateSpeed;
	public float MoveSpeed;
	private GameObject gameController;
	private float soulTimer;
	private bool flickerActiveState;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		soulTimer = 20.0f;
		flickerActiveState = true;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0, RotateSpeed * Time.deltaTime, 0);
		soulTimer -= Time.deltaTime;
		if (soulTimer <= 0.0f) {
			Destroy (this.gameObject);
			return;
		}
		GameObject Player = gameController.GetComponent<GameController> ().getPlayer ();
		if (Vector3.Distance (Player.transform.position, this.gameObject.transform.position) < 10.0f) {
			Vector3 toPlayer = Vector3.Normalize (Player.transform.position - this.gameObject.transform.position);
			this.gameObject.GetComponent<Rigidbody> ().velocity = toPlayer * MoveSpeed;
		} else {
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			gameController.GetComponent<GameController>().incrementSoulScore();
			Destroy (this.gameObject);
		}
	}
}
