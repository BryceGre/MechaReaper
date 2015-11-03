using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

	public Transform Target = null;
	public float MoveSpeed = 25.0f;
	public float RotateSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 toTarget = Target.transform.position - gameObject.transform.position;
		Vector3 rotation = Vector3.RotateTowards (transform.forward, toTarget, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);
		
		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
	}
}
