using UnityEngine;
using System.Collections;

public class AscendPower : Power {
	public float Distance = 25.0f;
	
	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();
		if (this.durationCount > 0.0f) {
			float factor = Time.deltaTime * (Distance / Duration);
			Vector3 up = gameObject.transform.up * factor;
			this.gameObject.transform.Translate(up, Space.World);
		}
	}
	
	public override bool usePower() {
		return base.usePower ();
	}

	protected override void onDurationEnd() {
		float factor = (Time.deltaTime + durationCount) * (Distance / Duration);
		Vector3 up = gameObject.transform.up * factor;
		this.gameObject.transform.Translate(up, Space.World);

	}
}
