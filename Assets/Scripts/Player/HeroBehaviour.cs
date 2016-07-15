using UnityEngine;
using System.Collections;

public class HeroBehaviour : MonoBehaviour {
	Rigidbody[] rb;
	Collider[] cols;
	// Use this for initialization
	void Start () {
		rb = GetComponentsInChildren<Rigidbody> ();
		cols = GetComponentsInChildren<Collider> ();
		foreach ( Rigidbody r in rb   )	r.isKinematic = true;
		foreach ( Collider  c in cols ) c.enabled = false;
	}
	// Update is called once per frame
	public void RagOn (float force) {
		transform.parent = null;
		foreach ( Collider  c in cols ) c.enabled = true;
		foreach (Rigidbody r in rb) {
			r.isKinematic = false;
			r.velocity = new Vector3(0,0,force);
		}
	}
}

