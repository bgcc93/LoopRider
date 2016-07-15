using UnityEngine;
using System.Collections;

public class ObstacleSpwnrManager : MonoBehaviour {
	public float spawnMin = 0, spawnMax = 4;
	public int difuculty = 2;
	public GameObject[] spwnrs;
	private int[] spwnrsCalled;
	private int spwnrsToCall;
	private int nextSpawn;
	// Use this for initialization
	void Start () {

		spwnrsCalled = new int[spwnrs.GetLength(0)];

		for( int i = 0; i < spwnrsCalled.GetLength(0); i++ ){
			spwnrsCalled[i] = 0;
		}
		StartSpawn ();
	}
	public void StartSpawn(){
		spwnrsToCall = Random.Range (0, spwnrs.GetLength(0) / difuculty);
		for( int i = 0; i < spwnrsToCall; i++ ){
			if( spwnrsCalled[i] == 0 ){
				nextSpawn = Random.Range (0, spwnrs.GetLength(0));
				spwnrs[nextSpawn].SendMessage ("Spawn");
				spwnrsCalled[nextSpawn]= 1;
			}
		}
		for( int i = 0; i < spwnrsCalled.GetLength(0); i++ ){
			spwnrsCalled[i] = 0;
		}

		Invoke ("StartSpawn", Random.Range (spawnMin,spawnMax));
	}
}
