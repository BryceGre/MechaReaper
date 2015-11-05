﻿using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public RectTransform health;
	public RectTransform shield;
	private float healthWidth;
	private float shieldWidth;
	
	// Use this for initialization
	void Start () {
		healthWidth = health.rect.width;
		shieldWidth = shield.rect.width;
		Debug.Log(healthWidth);
	}

	public float getHealthWidth() { return healthWidth; }
	public float getShieldWidth() { return shieldWidth; }
}