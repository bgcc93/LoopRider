using UnityEngine;
using System.Collections;

public class OffScreenFollow : MonoBehaviour {

	private Transform target;
	public float distance = 10.0f;
	public int forward;

	void Start(){
		target = GameObject.FindWithTag ("Player").transform;
	}
	// Update is called once per frame
	void Update () {
		if (!target)
			return;
		transform.position = new Vector3(0,0,target.position.z + (distance * forward));
	}

}
