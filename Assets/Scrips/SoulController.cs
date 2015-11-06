using UnityEngine;
using System.Collections;

public class SoulController : MonoBehaviour {

	public float speed;
	public GameObject gameController;
	public int soulTimer;
	public bool flickerActiveState;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		soulTimer = 1000;
		flickerActiveState = true;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0, speed * Time.deltaTime, 0);
		if (soulTimer > 0) 
		{
			soulTimer--;
		}

		if (soulTimer <= 0) 
		{
			Destroy(this.gameObject);
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
