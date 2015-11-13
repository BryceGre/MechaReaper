using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour {

	public int swordDamage = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<EnemyController>().applyDamage(swordDamage);
		}
	}
}
