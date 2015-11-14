using UnityEngine;
using System.Collections;

public class ShockwavePower : Power {
	public float Distance = 25.0f;

	public Transform PowerFX;
	private Transform fxInstance = null;

	public override void Start() {
		base.Start ();
	}

	public override void Update() {
		base.Update ();
		if (durationCount > 0.0f) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			foreach (GameObject enemy in enemies) {
				//slow enemy speed to 0 over 5 seconds
				enemy.GetComponent<EnemyController>().MoveSpeed = durationCount;
			}
			if (fxInstance != null) {
				fxInstance.gameObject.transform.localScale *= 1.25f;
			}
		}
	}

	public override bool usePower() {
		if (!base.usePower ())
			return false;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) <= Distance) {
				EnemyController control = enemy.GetComponent<EnemyController>();
				control.MoveSpeed = Duration;
				control.RotateSpeed = 0.0f;
			}
		}
		if (fxInstance != null)
			Destroy (fxInstance.gameObject);
		fxInstance = (Transform)Instantiate(PowerFX, this.gameObject.transform.position, Quaternion.identity);

		return true;
	}

	protected override void onDurationEnd() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			//reset enemy speed to max
			enemy.GetComponent<EnemyController>().resetSpeed();
		}
		if (fxInstance != null)
			Destroy(fxInstance.gameObject);
	}

}
