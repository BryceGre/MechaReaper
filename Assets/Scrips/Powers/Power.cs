using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Power : MonoBehaviour {
	public float Cooldown;
	public RectTransform Icon;
	public float Duration;
	protected float durationCount;
	protected float cooldownCount;

	public virtual void Start() {
		cooldownCount = 0.0f;
		durationCount = 0.0f;
		Icon.Find ("CDImage").GetComponent<Image> ().fillAmount = 0.0f;
		Icon.Find ("CDText").GetComponent<Text> ().text = "";
	}

	public virtual void Update() {
		if (durationCount > 0.0f) {
			durationCount -= Time.deltaTime;
			if (durationCount <= 0.0f) {
				this.onDurationEnd();
				durationCount = 0.0f;
				cooldownCount = Cooldown;
				if (Cooldown > 0.0f) {
					Icon.Find ("CDImage").GetComponent<Image> ().fillAmount = 1.0f;
					Icon.Find ("CDText").GetComponent<Text> ().text = cooldownCount.ToString ("F0");
				}
			}
		} else if (cooldownCount > 0.0f) {
			cooldownCount -= Time.deltaTime;
			if (cooldownCount <= 0.0f) {
				this.onCooldownEnd();
				cooldownCount = 0.0f;
				Icon.Find ("CDImage").GetComponent<Image> ().fillAmount = 0.0f;
				Icon.Find ("CDText").GetComponent<Text> ().text = "";
			} else {
				Icon.Find ("CDImage").GetComponent<Image> ().fillAmount = cooldownCount / Cooldown;
				Icon.Find ("CDText").GetComponent<Text> ().text = cooldownCount.ToString ("F0");
			}
		}
	}

	protected virtual void onDurationEnd () {
		return;
	}

	protected virtual void onCooldownEnd () {
		return;
	}

	public virtual bool usePower() {
		if (this.durationCount > 0.0f || this.cooldownCount > 0.0f)
			return false;
		durationCount = Duration;
		Icon.Find ("CDImage").GetComponent<Image> ().fillAmount = 1.0f;
		Icon.Find ("CDText").GetComponent<Text> ().text = "";
		return true;
	}
}
