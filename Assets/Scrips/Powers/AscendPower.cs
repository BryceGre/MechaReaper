using UnityEngine;
using System.Collections;

public class AscendPower : Power {
	public float Distance = 25.0f;
	public float Duration = 2.0f;
	private float durationCounter = 0.0f;
	
	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();
		if (durationCounter > 0.0f) {
			durationCounter -= Time.deltaTime;
			float factor = Time.deltaTime * (Distance / Duration);
			if (durationCounter <= 0.0f) {
				factor = (Time.deltaTime + durationCounter) * (Distance / Duration);
				durationCounter = 0.0f;
			}
			Vector3 up = gameObject.transform.up * factor;
			this.gameObject.transform.Translate(up, Space.World);
		}
	}
	
	public override void usePower() {
		if (this.isOnCooldown()) return;

		durationCounter = Duration;
		startCooldown ();
	}
}
