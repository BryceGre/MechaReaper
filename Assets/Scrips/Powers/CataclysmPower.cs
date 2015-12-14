using UnityEngine;
using System.Collections;

public class CataclysmPower : Power {
	private Vector3 cameraPos;
	
	public Transform PowerFX;
	private Transform fxInstance = null;

	private const float moveSpeed = 100.0f;

	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();
		
		if (durationCount > 0.0f) {
			Camera.main.transform.localPosition = cameraPos + (Random.insideUnitSphere / 10);
		}

		if (fxInstance != null) {
			fxInstance.gameObject.transform.localScale *= 1.25f;
		}
	}

	void LateUpdate() {
		if (durationCount > 0.0f) {
			GameObject[] souls = GameObject.FindGameObjectsWithTag("Soul");
			foreach (GameObject soul in souls) {
				Vector3 toPlayer = Vector3.Normalize(this.gameObject.transform.position - soul.transform.position);
				soul.GetComponent<Rigidbody> ().velocity = (toPlayer * moveSpeed);
			}
		}
	}
	
	public override bool usePower() {
		if (!base.usePower ())
			return false;
		
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().destroyEnemy();
		}

		cameraPos = Camera.main.transform.localPosition;

		if (fxInstance != null)
			Destroy (fxInstance.gameObject);
		fxInstance = (Transform)Instantiate(PowerFX, this.gameObject.transform.position, Quaternion.identity);

		return true;
	}

	
	protected override void onDurationEnd() {
		Camera.main.transform.localPosition = cameraPos;
		if (fxInstance != null)
			Destroy(fxInstance.gameObject);
	}
}
