using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	public float speed;
	public float selfDestructTime;
	public float damage = 1;
	public float damageModifier = 1;
	public string shotIgnores;

	//public AudioClip[] shotHitClips;
	//private int flyBy = 0;
	//private AudioSource source;


	//public GameObject weaponFiredFrom;
	private Transform myTransform;

	void Awake ()
	{
		StartCoroutine (Wait ());
		myTransform = transform;
	}
	void Start(){
		//source = GetComponent<AudioSource> ();
	}

	void Update(){
		myTransform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
	void OnTriggerEnter (Collider other){

        if (other.tag == "obstacle" || other.tag != "loop" && other.tag != shotIgnores && other.tag != "shot" && (shotIgnores == "Player" && other.tag != "hero"))
        {
			//source.PlayOneShot( shotHitClips[ (Random.Range (0, shotHitClips.Length - 1)) ] );
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	IEnumerator Wait(){
		yield return new WaitForSeconds (selfDestructTime);
		Destroy (gameObject);
	}
}
