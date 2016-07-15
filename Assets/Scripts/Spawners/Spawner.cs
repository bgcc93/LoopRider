using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject[] obj;
	public enum objectType{obstacles,loop};
	public objectType type;

	public void Spawn () {
		int idx = Random.Range (0, obj.GetLength(0));
		/*
		if (type == objectType.obstacles) {

		}
		*/
		Instantiate (obj[idx], transform.position,Quaternion.identity);
	}
}
