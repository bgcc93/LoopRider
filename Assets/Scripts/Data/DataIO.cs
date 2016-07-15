using UnityEngine;
using System.Collections;

public class DataIO : MonoBehaviour {
	PersistentData pd;
	GameObject persistentDataObject;

	private string keySens = "sens";
	private string keyDist = "bestDistance";
	private string keyTime = "bestTime";

	void Start () {	
		persistentDataObject = GameObject.Find ("PersistentData");
		pd = persistentDataObject.GetComponent<PersistentData> ();
		DoIO (); 
	}
	
	void DoIO () {
		float savedDist = 0f;
		float savedTime = 0f;
		//float savedSens = 2f;

		//get saved data
		if( PlayerPrefs.HasKey(keyDist) ){ savedDist = PlayerPrefs.GetFloat(keyDist); }
		if( PlayerPrefs.HasKey(keyTime) ){ savedTime = PlayerPrefs.GetFloat(keyTime); }
		//if( PlayerPrefs.HasKey(keySens) ){ savedSens = PlayerPrefs.GetFloat(keyDist); }

		//if saved score is lower than session score and overwrite it
		//else set the session's scores to be the saved ones
		if ( savedDist < pd._maxScore ) { PlayerPrefs.SetFloat ( keyDist, pd._maxScore ); } else{ pd._maxScore = savedDist;}
		if ( savedTime < pd._maxTime  ) { PlayerPrefs.SetFloat ( keyTime, pd._maxTime  ); } else{ pd._maxTime = savedTime; }
		//if ( savedSens != pd._sensitivity){ pd._sensitivity = savedSens; } 

	}

	public void SaveSens(){	PlayerPrefs.SetFloat(keySens, pd._sensitivity); }
}
