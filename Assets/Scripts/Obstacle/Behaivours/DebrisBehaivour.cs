using UnityEngine;
using System.Collections;

public class DebrisBehaivour : MonoBehaviour {
    //private Rigidbody[] r;

	public AudioClip[] crashClips;
	private AudioSource source;

	void Awake () {
		StartCoroutine (SelfDestruct ());
	}
	void Start(){
		source = GetComponent<AudioSource> ();
		source.PlayOneShot ( crashClips[ (Random.Range (0, crashClips.Length - 1)) ] );
        /*
        r = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody ri in r){
            ri.AddForce(Random.Range(-10, 10), Random.Range(-10, 10),30, ForceMode.Impulse);
        }
         * */
         
	}
	// Update is called once per frame
    /*
	void Update () {
		if (speed > 0) {
			speed -= 0.1f;
			myTransform.Translate(Vector3.forward  * speed);
		}
	}
    */
	// Update is called once per frame
    IEnumerator SelfDestruct()
    {
		yield return new WaitForSeconds (4.0f);
		Destroy (gameObject);
	}
}
