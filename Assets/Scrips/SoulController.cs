using UnityEngine;
using System.Collections;

public class SoulController : MonoBehaviour {

	public float speed;
	public GameObject gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0, speed * Time.deltaTime, 0);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {

			gameController.GetComponent<GameController>().incrementSoulScore();
			Destroy (this.gameObject);

		}
	}
}
