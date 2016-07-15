using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float responseTIme = 0.01f;
	public float precision = 10;
	private float nextAction = 0;

	public float distanceFollow = 30;
	public float distanceTolerance = 10;
	private int followDecision = 0;
	private bool angleAttack = true;
	private Vector3 heading;
	
	//1 is for frontal guns
	//2 is for rear guns
	private bool angleReady1 = false;
	private bool fireFront = false;
	private bool fireRear = false;
	private float speedModifier = 0;
	private float targetAngle = 0;
    public float turretAcuracy = 0.5f;

	private Transform player;
	
	private ShootingController sc;
	private EnemyMovement em;
	private AutoPilot ap;
	
	private float distanceToPlayer = 0;
	
	private Transform myTransform;
	//private bool invisible = false;
	private Transform aim;

	void Awake(){
		sc = GetComponentInChildren<ShootingController>();
		em = GetComponent<EnemyMovement> ();
		ap = GetComponent<AutoPilot> ();
		myTransform = transform;
	}
	void Start(){		
		player = GameObject.Find ("Player").transform;
		aim = GameObject.Find("playerBody").transform;

	}
	
	// First we check if the player is ahead or behind the enemy.
	// if the player is ahead, we then check to see if there's ammo in the primary weapon
	// if so, the enemy matches the player's angle to fire.
	// If the enemy is ahead, it keeps a nice distance to shoot with it's rear guns
	// if the enemy is behind and there's no more ammo to the main guns, the enemy then speeds up to get ahead of the player and get in range to fire its rear secondary guns
	// the secondary is unlimitted.
	
	// Maybe I should first check if there's ammo on the primary.
	// You know, it doesn't sound very smart that the main tactic is to get ahead of the player and try and get shot.
	// ammo should be the main factor of choice here
	
	//private int followDecision :
	// has ammo:
	// 0 = is behind and will try and keep a nice distance to fire primary weapons +
	// 1 = enemy is behind but getting too close, will slow down to try to keep a nice distance +
	// 2 = enemy is ahead and will slow down to get behind the player+
	//doesn't have ammo:
	// 3 = enemy is ahead and will try to keep its distance to use its rear weapons+
	// 4 = enemy is behind and needs to get ahead+
	// 5 = enemy is ahead and too close, tries to get a nice distance
	// 6 = god knows what's happened, let's speed up just 'cause
	
	// angleAttack should determine if the enemy should match the player's angle or avoid it.
	// if the enemey has ammo, it should match, to get a line of fire.
	// if the enemy has no ammo, it should avoid, to avoid getting in the line of fire of the player.
	// if the enemy has ammo, but is ahead of the player, it should avoid as well.
	
	// private bool angleAttack:
	// 0 = avoid
	// 1 = match
	
	void DecideSpeedAngleAndWeapon( float distance ){
		if (sc.GetAmmoFromWeapon(0) > 0) {
			if ( distance < 0 ){
				if ( distance <= -1 * distanceFollow ){ followDecision = 0;}
				else{ followDecision = 1;}
				angleAttack = true;
				fireRear = false;
				fireFront = true;
			}else if( distance > 0 ){
				followDecision = 2;
				angleAttack = false;
				fireRear = false;
				fireFront = false;
			}
			
		}else{
			if (distance > distanceFollow) { followDecision = 3; }
			else if( distance < 0 ){ followDecision = 4; }
			else if( distance < distanceFollow ){ followDecision = 5; }
			else{ followDecision = 5; }
			angleAttack = false;
			fireRear = true;
			fireFront = false;
		}
	}
	
	void MatchSpeed( float distance ){
		
		speedModifier = distance / distanceFollow;
		
		if      ( followDecision == 0 ){ speedModifier *= -1; }
		else if ( followDecision == 1 ){ speedModifier *=  2.5f ; }
		else if ( followDecision == 2 ){ speedModifier *= -1; }
		else if ( followDecision == 3 ){ speedModifier *= -1; }
		else if ( followDecision == 4 ){ speedModifier *= -3; }
		else if ( followDecision == 5 ){ speedModifier *=  5; }
		
		if ( em.speed > em.minSpeed && em.speed < em.maxSpeed ){
			em.speed += speedModifier;
		}
	}
	// +1: right
	// -1: left
	void MatchAngle(bool angleAttack){
		if( !ap.dodging ){
			targetAngle = myTransform.rotation.eulerAngles.z - player.transform.rotation.eulerAngles.z + Random.Range(-precision,precision);
			int targetHelper = 1;
				
			if(!angleAttack){
				targetAngle += 180;				
			}else{
				if( targetAngle >= -precision && targetAngle <= precision ){angleReady1 = true;}
				else { angleReady1 = false; }
			}
			if( targetAngle < 0 ){
				targetHelper = -1;
			}else{
				targetHelper =  1;
			}
			if( targetAngle > 180 * targetHelper ){
				em.Turn (1);
			} 
			else if( targetAngle < 180 * targetHelper ){
				em.Turn (-1);
			}

		}
	}
	void TakeAim(){
		if(aim == null){
			aim = GameObject.FindGameObjectWithTag("playerHitBox").transform;        
		}
        Vector3 v = aim.position;
        v.x += Random.Range(0,turretAcuracy) * Random.Range(-1,1);
        v.y += Random.Range(0, turretAcuracy) * Random.Range(-1, 1);
        v.z += Random.Range(0, turretAcuracy) * Random.Range(-1, 1);
        aim.position = v;
		sc.TakeAim (1,aim);
	}
	public void KillPlayer(){
		heading = myTransform.position - player.position;
		distanceToPlayer = Vector3.Dot (heading, myTransform.forward);
		
		//tolerance gives some margin error, making it seem like it's a live driver
		float distance = distanceToPlayer + Random.Range (-distanceTolerance, distanceTolerance);
		
		DecideSpeedAngleAndWeapon( distance );
		MatchSpeed ( distance );
		MatchAngle (angleAttack);
		if (fireFront && angleReady1) {			
			if(Time.time > nextAction){
				nextAction = Time.time + responseTIme;
				sc.ShootWeapons(0);
			}
		}
		if (fireRear) {
			if(Time.time > nextAction){
				nextAction = Time.time + responseTIme;
				TakeAim();
				sc.ShootWeapons(1);
			}
		}
	}
}
