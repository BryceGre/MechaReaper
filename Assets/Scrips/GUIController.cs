using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {
	public RectTransform health;
	public RectTransform shield;
	public Text souls;
	public Text missiles;
	public Canvas GUICanvas;
	private float healthWidth;
	private float shieldWidth;
	
	// Use this for initialization
	void Start () {
		healthWidth = health.rect.width;
		shieldWidth = shield.rect.width;
	}

	public float getHealthWidth() { return healthWidth; }
	public float getShieldWidth() { return shieldWidth; }
}
