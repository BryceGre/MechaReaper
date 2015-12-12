using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GarageController : MonoBehaviour {
	public Text GunText = null;
	public Text MeleeText = null;
	public Text MissileText = null;
	public Text RocketText = null;
	public AudioClip ClickClip;

	private readonly string[] Guns = new string[] {
		"Autocannon",
		"Railgun",
		"Machine Gun"
	};
	
	private readonly string[] Melees = new string[] {
		"Sword",
		"Longsword",
		"Dagger"
	};
	
	private readonly string[] Missiles = new string[] {
		"Standard"
	};
	
	private readonly string[] Rockets = new string[] {
		"Standard"
	};

	// Use this for initialization
	void Start () {
		GunText.text = Guns[PlayerPrefs.GetInt("gun")];
		MeleeText.text = Melees[PlayerPrefs.GetInt("melee")];
		MissileText.text = Missiles[PlayerPrefs.GetInt("missile")];
		RocketText.text = Rockets[PlayerPrefs.GetInt("rocket")];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GunNext() {
		PlayerPrefs.SetInt ("gun", PlayerPrefs.GetInt ("gun") + 1);
		if (PlayerPrefs.GetInt ("gun") == Guns.Length)
			PlayerPrefs.SetInt ("gun", 0);
		GunText.text = Guns[PlayerPrefs.GetInt("gun")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void GunPrev() {
		PlayerPrefs.SetInt ("gun", PlayerPrefs.GetInt ("gun") - 1);
		if (PlayerPrefs.GetInt ("gun") < 0)
			PlayerPrefs.SetInt ("gun", Guns.Length - 1);
		GunText.text = Guns[PlayerPrefs.GetInt("gun")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void MeleeNext() {
		PlayerPrefs.SetInt ("melee", PlayerPrefs.GetInt ("melee") + 1);
		if (PlayerPrefs.GetInt ("melee") == Melees.Length)
			PlayerPrefs.SetInt ("melee", 0);
		MeleeText.text = Melees[PlayerPrefs.GetInt("melee")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void MeleePrev() {
		PlayerPrefs.SetInt ("melee", PlayerPrefs.GetInt ("melee") - 1);
		if (PlayerPrefs.GetInt ("melee") < 0)
			PlayerPrefs.SetInt ("melee", Melees.Length - 1);
		MeleeText.text = Melees[PlayerPrefs.GetInt("melee")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void MissileNext() {
		PlayerPrefs.SetInt ("missile", PlayerPrefs.GetInt ("missile") + 1);
		if (PlayerPrefs.GetInt ("missile") == Missiles.Length)
			PlayerPrefs.SetInt ("missile", 0);
		MissileText.text = Missiles[PlayerPrefs.GetInt("missile")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void MissilePrev() {
		PlayerPrefs.SetInt ("missile", PlayerPrefs.GetInt ("missile") - 1);
		if (PlayerPrefs.GetInt ("missile") < 0)
			PlayerPrefs.SetInt ("missile", Missiles.Length - 1);
		MissileText.text = Missiles[PlayerPrefs.GetInt("missile")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void RocketNext() {
		PlayerPrefs.SetInt ("rocket", PlayerPrefs.GetInt ("rocket") + 1);
		if (PlayerPrefs.GetInt ("rocket") == Rockets.Length)
			PlayerPrefs.SetInt ("rocket", 0);
		RocketText.text = Rockets[PlayerPrefs.GetInt("rocket")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void RocketPrev() {
		PlayerPrefs.SetInt ("rocket", PlayerPrefs.GetInt ("rocket") - 1);
		if (PlayerPrefs.GetInt ("rocket") < 0)
			PlayerPrefs.SetInt ("rocket", Rockets.Length - 1);
		RocketText.text = Rockets[PlayerPrefs.GetInt("rocket")];
		PlayerPrefs.Save ();
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
}
