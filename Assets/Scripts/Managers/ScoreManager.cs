using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	PersistentData pd;
	GameObject persistentDataObject;

	public Text lbl_distance;
	public Text lbl_time;

	private Transform player;
	private float gameTime;
	private float distanceTraveled;

	void Start () {
		lbl_time.text = "0 secs";
		lbl_distance.text = "0 Km";
		distanceTraveled = 0;
		gameTime = 0;

        player = GameObject.FindWithTag("hero").transform;

        persistentDataObject = GameObject.FindWithTag ("data");
		pd = persistentDataObject.GetComponent<PersistentData> ();
		
	}
	
	void Update () {
		gameTime += Time.deltaTime;
		lbl_time.text = gameTime.ToString ("F2") + "secs" ;
		distanceTraveled = player.position.z;
		lbl_distance.text = (distanceTraveled / (300 * 12)).ToString ("F1") + "Km";
		if( distanceTraveled >= pd._maxScore )
			pd._maxScore = distanceTraveled;
		if( gameTime >= pd._maxTime )
			pd._maxTime = gameTime;	
	}
}