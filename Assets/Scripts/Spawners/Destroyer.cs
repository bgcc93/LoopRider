using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
	//public GameObject spwnr;

	void OnTriggerEnter(Collider other){
		if(other.tag == "enemy"){

		}
		else if (other.gameObject.transform.parent)
			Destroy (other.gameObject.transform.parent.gameObject);
		else
			Destroy (other.gameObject);
		/*
		if(other.tag == "loop"){
			spwnr = GameObject.Find ("Spawner");
			spwnr.SendMessage ("Spawn");	
		}
		*/
	}

}
