using UnityEngine;
using System.Collections;

public class MechController : MonoBehaviour {
	public float moveSpeed = 0.5f;
	public float lookSpeed = 2.0f;

	private CharacterController controller;
	private GameObject boostLeft;
	private GameObject boostRight;
	private GameObject boostTop;
	private GameObject boostBottom;
	
	private Vector3 moveDir = Vector3.zero;



	//Raycasting Variables
	public string fireButtonName = "Fire1";
	private bool readyToFire = true;
	public LayerMask layerMask = -1;



	// Use this for initialization
	void Start () {
		controller =	gameObject.GetComponent<CharacterController>();
		boostLeft =		gameObject.transform.Find ("Body").Find ("Boost_Left").gameObject;
		boostRight =	gameObject.transform.Find ("Body").Find ("Boost_Right").gameObject;
		boostTop =		gameObject.transform.Find ("Body").Find ("Boost_Top").gameObject;
		boostBottom =	gameObject.transform.Find ("Body").Find ("Boost_Bottom").gameObject;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		float lookX = lookSpeed * Input.GetAxis ("Mouse X");
		float lookY = lookSpeed * Input.GetAxis ("Mouse Y");
		
		transform.Rotate (lookY, lookX, 0);

		Vector3 forward =	transform.TransformDirection(Vector3.forward);
		Vector3 right =		transform.TransformDirection (Vector3.right);

		float horiInput = -Input.GetAxisRaw ("Horizontal");
		float vertInput = -Input.GetAxisRaw ("Vertical");
		float speedMod = 1.0f;
		if (Input.GetButton ("Jump"))
			speedMod = 2.0f;
		float horizontal =	(moveSpeed * speedMod) * horiInput;
		float vertical =	(moveSpeed * speedMod) * vertInput;

		moveDir = (vertical * forward) + (horizontal * right);
		controller.Move (moveDir);

		if (horiInput < 0)
			boostLeft.SetActive (true);
		else
			boostLeft.SetActive (false);

		if (horiInput > 0)
			boostRight.SetActive (true);
		else 
			boostRight.SetActive (false);

		if (vertInput < 0) {
			boostTop.SetActive (true);
			boostBottom.SetActive (true);
		} else {
			boostTop.SetActive (false);
			boostBottom.SetActive (false);
		}


		if (Input.GetButtonDown (fireButtonName) && readyToFire) 
		{
			RaycastHit hit;
			if(Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask))
			{
				if(hit.collider.gameObject.CompareTag("Enemy"))
				{
					SendMessageUpwards ("hitScanHit", hit.collider.gameObject);
				}
			}



		}

	}
}
