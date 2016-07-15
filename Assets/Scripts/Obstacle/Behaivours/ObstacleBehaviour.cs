using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {
	public float Damage = 2;    

	public GameObject obstacleDestroyed;
    private HealthController hc;
    private float thoughness = 0;

	// Use this for initialization
	void Awake () {
        hc = GetComponent<HealthController>();
        thoughness = hc.thoughness;
	}
	void OnTriggerEnter (Collider other){
        if (other.tag == "shot")
        {
            hc.ChangeHP(-other.GetComponent<ProjectileBehaviour>().damage);
        }
        else if(other.tag == "hero"){
            hc.ChangeHP(-10);
        }
		else if (other.tag == "bike" ) {
            float t = other.GetComponent<HealthController>().thoughness;
            hc.ChangeHP(thoughness - (t + 20));
            //if (t > hc.thoughness)
            //{
            //    hc.ChangeHP(hc.thoughness - t);
            //}
            //else
            //{
            //    hc.ChangeHP( t - hc.thoughness );
            //}
		}
		if (hc.GetHP() < 0) {
			DestroyObstacle ();
		}
	}
	void DestroyObstacle(){
		Instantiate (obstacleDestroyed, transform.position, Quaternion.EulerRotation(0,0,0));
		Destroy (gameObject);
	}


}
