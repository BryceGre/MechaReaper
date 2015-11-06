using UnityEngine;
using System.Collections;

public abstract class Power : MonoBehaviour {
	public float Cooldown;
	protected float cooldownCount;

	public abstract void usePower();

	public virtual void Start() {
		cooldownCount = 0.0f;
	}

	public virtual void Update() {
		cooldownCount -= Time.deltaTime;
		if (cooldownCount < 0.0f)
			cooldownCount = 0.0f;
	}

	public bool isOnCooldown() {
		return (cooldownCount > 0.0f);
	}
}
