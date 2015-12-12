using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	public GameObject Garage = null;
	public GameObject About = null;
	public GameObject Controls = null;
	public Font Font;
	public AudioClip MouseOverClip;
	public AudioClip MouseOutClip;
	public AudioClip ClickClip;

	private string scrollText = "Somewhere in the Milky Way Galaxy, lies a dark presence hidden among a dark and lost asteroid belt.  This presence, known to the race of humanity as the infamous “Mecha Reaper”, has long feasted on human souls and has taken the lives of thousands as a result.  On one fateful morning, a soul taken by the Mecha Reaper managed to escape and make its way towards Earth.  After making contact with information on the Mecha Reaper, the humans managed to locate its residing place.  After much debate, the human race decided to rid the universe of the evil Mecha Reaper and avenge their long lost brethren with their own mecha technologies.  After a long journey in space, they find the Mecha Reaper but as they approach, they find him ready and waiting for battle!";
	private Rect scrollRect = new Rect(0, 0, 0, 0);
	private GUIStyle scrollStyle;
	private float scrollSpeed = 25.0f;

	// Use this for initialization
	void Start () {
		scrollStyle = new GUIStyle ();
		scrollStyle.font = Font;
		scrollStyle.fontStyle = FontStyle.Normal;
		scrollStyle.fontSize = (int)(64.0f * (float)(Screen.width)/1920.0f); //scale size font;
		scrollStyle.richText = true;
		scrollStyle.alignment = TextAnchor.UpperCenter;
		scrollStyle.wordWrap = true;
		scrollStyle.normal.textColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		if (scrollRect.width != 0 && scrollRect.height != 0) {
			if (Input.GetButtonDown ("Submit")) {
				scrollRect.y = -scrollRect.height;
			}
		}
	}

	void OnGUI() {
		if (scrollRect.width != 0 && scrollRect.height != 0) {
			scrollRect.y -= Time.deltaTime * scrollSpeed;
			if (scrollRect.y < -scrollRect.height) {
				scrollRect = new Rect(0, 0, 0, 0);
				//Camera.main.clearFlags = CameraClearFlags.SolidColor;
				this.transform.Find("ContainerPanel").gameObject.SetActive (true);
				Application.LoadLevel ("space");
			} else {
				GUI.Label(scrollRect, scrollText, scrollStyle);
			}
		}
	}

	public void ClickPlayGame() {
		//Camera.main.clearFlags = CameraClearFlags.Skybox;
		this.transform.Find("ContainerPanel").gameObject.SetActive (false);
		Rect rect = this.GetComponent<RectTransform> ().rect;
		scrollRect = new Rect (0, 0, rect.width, rect.height);
		scrollRect.y = this.GetComponent<RectTransform> ().rect.height;

		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}

	public void ClickGarage() {
		this.gameObject.SetActive (false);
		About.SetActive (false);
		Controls.SetActive (false);

		Garage.SetActive (true);
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}
	
	public void ClickAbout() {
		this.gameObject.SetActive (false);
		Garage.SetActive (false);
		Controls.SetActive (false);

		About.SetActive (true);
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}

	public void ClickControls() {
		this.gameObject.SetActive (false);
		Garage.SetActive (false);
		About.SetActive (false);

		Controls.SetActive (true);
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}

	public void ClickQuit() {
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);

		Application.Quit ();
	}

	public void ClickHome() {
		Garage.SetActive (false);
		About.SetActive (false);
		Controls.SetActive (false);

		this.gameObject.SetActive (true);
		
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (ClickClip);
	}

	public void MouseOver() {
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (MouseOverClip);
	}

	public void MouseOut() {
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (MouseOutClip);
	}
}
