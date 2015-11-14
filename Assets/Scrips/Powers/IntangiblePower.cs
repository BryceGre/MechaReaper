using UnityEngine;
using System.Collections;

public class IntangiblePower : Power {
	public float Duration = 15.0f;
	private float durationCounter = 0.0f;
	public Material TransparentBody;
	public Material TransparentBlack;

	private Material OpaqueBody;
	private Material OpaqueBlack;

	public override void Start() {
		base.Start ();
		Physics.IgnoreLayerCollision (0, 2, true);
	}
	
	public override void Update() {
		base.Update ();
		if (durationCounter > 0.0f) {
			durationCounter -= Time.deltaTime;
			if (durationCounter <= 0.0f) {
				durationCounter = 0.0f;

				this.gameObject.layer = 8;

				this.transform.Find ("Reaper").Find ("Body").GetComponent<SkinnedMeshRenderer> ().material = OpaqueBody;
				this.transform.Find ("Reaper").Find ("Head").GetComponent<SkinnedMeshRenderer> ().material = OpaqueBlack;
				for (int i=0; i<4; i++)
					this.transform.Find ("Reaper").Find ("Cloth" + i).GetComponent<SkinnedMeshRenderer> ().material = OpaqueBlack;
			}
		}
	}
	
	public override void usePower() {
		if (this.isOnCooldown()) return;

		this.gameObject.layer = 2;

		SkinnedMeshRenderer body = this.transform.Find ("Reaper").Find ("Body").GetComponent<SkinnedMeshRenderer> ();
		OpaqueBody = body.material;
		body.material = TransparentBody;
		SkinnedMeshRenderer head = this.transform.Find ("Reaper").Find ("Head").GetComponent<SkinnedMeshRenderer> ();
		OpaqueBlack = head.material;
		head.material = TransparentBlack;
		for (int i=0; i<4; i++) {
			SkinnedMeshRenderer cloth = this.transform.Find ("Reaper").Find ("Cloth" + i).GetComponent<SkinnedMeshRenderer> ();
			cloth.material = TransparentBlack;
		}

		durationCounter = Duration;
		startCooldown ();
	}
}
