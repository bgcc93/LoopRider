using UnityEngine;
using System.Collections;

public class AutoPilot : MonoBehaviour {

	public Transform rayCastObjParent;
	private Transform[] rayCastObjs;

	public bool dodging = false;
	public int collisionCehckDistance = 250;	
	public float emergencyDistance = 80;

	private EnemyMovement em;

	void Awake(){
		em = GetComponent<EnemyMovement> ();
		rayCastObjs = rayCastObjParent.GetComponentsInChildren<Transform>();
	}
	//int side : 2 = extreme side 
	//			 0 = frontal sides
	public int checkLeftOrRight( int side ){
		RaycastHit hitR;
		RaycastHit hitL;
		
		int r = 1;
		
		Ray rayRight = new Ray( rayCastObjs[2 + side].transform.position, rayCastObjs[2 + side].TransformDirection(Vector3.forward) );
		Ray rayLeft  = new Ray( rayCastObjs[3 + side].transform.position, rayCastObjs[3 + side].TransformDirection(Vector3.forward) );
		
		bool leftRay  = Physics.Raycast (rayLeft, out hitL, (collisionCehckDistance + ((side - 2) * 15) - 20), 1 << 10);
		bool rightRay = Physics.Raycast (rayRight,out hitR,  (collisionCehckDistance + ((side - 2) * 15) - 20), 1 << 10);
		if ( leftRay || rightRay ) {
			if     ( hitR.distance  == 0 ) return 1;
			else if( hitL.distance == 0 ) return -1;
			if (hitR.distance <= hitL.distance) {
				r = -1;
			} else {
				r =  1;
			}
			if( hitR.distance == 0 && hitL.distance == 0 ){
				r = Random.Range (0,1) == 0 ? -1 : 1;
			}
			/*else if( hit.distance == hit2.distance ){
				r = 0;
			}*/
		}else{
			r = ( Random.Range (0,1) == 0 ? -1 : 1 );
		}
		return r;
	}
	
	public void CheckCollision(){
		RaycastHit hit;
		
		Ray rayCenter = new Ray( rayCastObjs[1].transform.position, rayCastObjs[1].TransformDirection(Vector3.forward) );
		Ray rayRight  = new Ray( rayCastObjs[6].transform.position, rayCastObjs[6].TransformDirection(Vector3.forward) );
		Ray rayLeft   = new Ray( rayCastObjs[7].transform.position, rayCastObjs[7].TransformDirection(Vector3.forward) );
		
		if ( Physics.Raycast( rayCenter, out hit, collisionCehckDistance, 1 << 10 ) ){
			dodging = true;
			if ( hit.distance <= emergencyDistance ){
				em.EmergencyTurn( checkLeftOrRight(0) ); //change to 2 to use the emergency raycasts
			}else if( hit.distance > emergencyDistance ){
				em.Turn( checkLeftOrRight(0) );
			}
		}
		else if ( Physics.Raycast( rayRight,  out hit, collisionCehckDistance - collisionCehckDistance / 10, 1 << 10 ) ) {
			dodging = true;
			if ( hit.distance <= emergencyDistance ){
				em.EmergencyTurn (-1);
			}else{
				em.Turn (-1);
			}
		}
		else if ( Physics.Raycast( rayLeft,   out hit, collisionCehckDistance - collisionCehckDistance / 10, 1 << 10 ) ) {
			dodging = true;
			if ( hit.distance <= emergencyDistance ){
				em.EmergencyTurn (1);
			}else{
				em.Turn (1);
			}
		}else{
			dodging = false;
			
		}
		
	}

}
