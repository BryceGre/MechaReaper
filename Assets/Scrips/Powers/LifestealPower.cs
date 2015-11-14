using UnityEngine;
using System.Collections;

public class LifestealPower : Power {
	public float Distance = 10.0f;
	public int DrainAmount = 2;
	public float DrainInterval = 0.5f;
	private float intervalTimer = 0.0f;

	public Transform PowerFX;

	
	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();
		if (durationCount > 0.0f) {
			intervalTimer += Time.deltaTime;
			if (intervalTimer >= DrainInterval) {
				GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
				while (intervalTimer >= DrainInterval) {
					intervalTimer -= DrainInterval;
					foreach (GameObject enemy in enemies) {
						if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) <= Distance) {
							enemy.GetComponent<EnemyController>().applyDamage(DrainAmount);
							Instantiate(PowerFX, enemy.transform.position, enemy.transform.rotation);
						}
					}
				}
			}
		} else {
			intervalTimer = 0.0f;
		}
	}
	
	public override bool usePower() {
		return base.usePower ();
	}
}
