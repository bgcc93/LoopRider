using UnityEngine;
using System.Collections;

public class BikeBehaviour : MonoBehaviour {
	Collider col;
	Rigidbody r;
	HealthController hc;
	Movement mo;
	Transform myTransform;

    float myRot = 0;
    float otherRot = 0;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		col = GetComponent<Collider> ();
		r = GetComponent<Rigidbody> ();
		hc = GetComponent<HealthController> ();
		mo = GetComponentInParent<Movement> ();

		r.isKinematic = true;
		r.useGravity = false;
		col.isTrigger = true;

	}
	void OnTriggerEnter(Collider other){
		float hpMod = 0;
		bool hardHit = false;
        if (other.tag == "bike")
        {/*
            mo.bounceOff = true;
            mo.nextCol = Time.time + mo.colRecoveryTime;
            hardHit = true;

            otherRot = other.transform.eulerAngles.z;
            myRot = mo.transform.eulerAngles.z;

            Debug.Log(otherRot);
            Debug.Log(myRot);

            if (otherRot >= myRot) {
                if (otherRot - myRot > 300) { mo.bounceOffSide = 1; }
                else { mo.bounceOffSide = -1; }
            }
            else {
                if (otherRot - myRot > 300) { mo.bounceOffSide = -1; }
                else { mo.bounceOffSide = 1; }
            }
          * */
            float t = other.GetComponentInParent<HealthController>().thoughness;
            if ( hc.thoughness < t )
            {
                hpMod = hc.thoughness - t;
                hardHit = true;
            }
            else if (hc.thoughness > t)
            {
                hpMod = t * .2f;
                hardHit = true;
            }
            else {
                hpMod = t * .5f;
                hardHit = true;
            }

        }
		if( other.tag == "obstacle" ){
            
            float t = other.GetComponent<HealthController>().thoughness;
            hardHit = true;
            
            hpMod = (hc.thoughness - t) * t / hc.thoughness;
            
            
		}
		if (other.tag == "shot") {
			ProjectileBehaviour pb = other.GetComponent<ProjectileBehaviour>();
			if(pb != null){
				if(pb.shotIgnores != myTransform.name){
					hpMod = pb.damage;
				}
			}
		}
        
		if (hpMod != 0) {
			mo.TakeDamage(hpMod, hardHit);
		}
	}	

	public void RagOn(float force){
		transform.parent = null;
		r.isKinematic = false;
		r.useGravity = true;
		r.velocity = new Vector3(Random.Range(-5,5),Random.Range(-5,5),force * 1f);
        r.AddForce(Random.Range(-2, 2), Random.Range(0, 4), Random.Range(-2, 2), ForceMode.Impulse);

		col.isTrigger = false;
		StartCoroutine (Wait ());
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds (7.0f);
		Destroy (gameObject);
	}
}
