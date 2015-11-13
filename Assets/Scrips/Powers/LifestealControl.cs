using UnityEngine;
using System.Collections;

public class LifestealControl : MonoBehaviour {
	public float MoveSpeed = 20.0f;
	public float RotateSpeed = 1.0f;
	public int HealAmount = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject Player = GameObject.FindGameObjectWithTag ("Player");
		Vector3 toPlayer = Vector3.Normalize(Player.transform.position - gameObject.transform.position);
		Vector3 rotation = Vector3.RotateTowards (gameObject.transform.forward, toPlayer, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);
		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			other.GetComponent<MechController>().restoreHealth(HealAmount);
			Destroy(gameObject);
		}
	}
}
