using UnityEngine;
using System.Collections;

public class CataclysmPower : Power {
	private float suckCount;

	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();
		if (suckCount > 0.0f) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject[] souls = GameObject.FindGameObjectsWithTag("Soul");
			suckCount -= Time.deltaTime;
			if (suckCount <= 0.0f) {
				foreach (GameObject enemy in enemies) {
					Vector3 toPlayer = this.gameObject.transform.position - enemy.transform.position;
					enemy.transform.Translate(toPlayer);
					enemy.GetComponent<EnemyController>().Fear = true;
				}
				foreach (GameObject soul in souls) {
					Vector3 toPlayer = this.gameObject.transform.position - soul.transform.position;
					soul.transform.Translate(toPlayer);
				}
				suckCount = 0.0f;
			} else {
				foreach (GameObject enemy in enemies) {
					Vector3 toPlayer = (this.gameObject.transform.position - enemy.transform.position) / suckCount;
					enemy.transform.Translate(toPlayer * Time.deltaTime);
				}
				foreach (GameObject soul in enemies) {
					Vector3 toPlayer = (this.gameObject.transform.position - soul.transform.position) / suckCount;
					soul.transform.Translate(toPlayer * Time.deltaTime);
				}
			}
		}
	}
	
	public override bool usePower() {
		if (!base.usePower ())
			return false;
		this.durationCount += 1.0f;
		suckCount = 1.0f;
		return true;
	}
	
	protected override void onDurationEnd() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().Fear = false;
		}
	}
}
