using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMovement : Movement {
	protected Animator anim;

	public ParticleSystem[] collisionParticles;

	public float speed = 0.0f;
	public float extraSpeed = 250f;
	public float maxSpeed = 200f;
	public float minSpeed = 50f;
	public float minBreak = 35f;
	public float acceleration = 2f;
    public float EmergencyTurnRatio = 3.0f;

	public float handling = 0f;
	
	public string[] hitAnimations;
	private int animationToPlay = 0;

	//public Slider healthBarSlider;

	private Vector3 moveDirection = Vector3.zero;
	public float axis = 0;
	private float lastAxis = 0;

	private bool dead = false;
	private bool waitActive = false;

	private Transform player;

	private HealthController hc;
    private ShootingController sc;
	private PlayerMovement pm;
	private EnemyAI eai;
	private AutoPilot ap;
	private BikeBehaviour bb;

	private Transform myTransform;

	void Awake(){
		hc = GetComponentInChildren<HealthController> ();
		anim = GetComponentInChildren<Animator> ();

		eai = GetComponent<EnemyAI> ();
		ap = GetComponent<AutoPilot> ();	
		speed = 30;
		myTransform = transform;
	}

	void Start() {
		player = GameObject.Find ("Player").transform;
		bb = GetComponentInChildren<BikeBehaviour> ();
        sc = GetComponentInChildren<ShootingController>();
		pm = player.GetComponent<PlayerMovement> ();

        sc.DepleteAmmo(0);
	}
	void ZeroOutAxis (){
		if     ( axis > 0  ) axis -= handling;
		else if( axis < 0  ) axis += handling;

		Mathf.Lerp (axis, 0, Time.deltaTime * Mathf.Abs(axis));
	}
	public bool isDead(){return dead;}

	// Update is called once per frame
	void Update() {
		ZeroOutAxis ();
		ap.CheckCollision ();
		if (speed > maxSpeed) {
			speed -= acceleration / 25;
			Mathf.Lerp(speed,maxSpeed,Time.deltaTime * speed / extraSpeed);
		}

		if( !isDead() ){
			if ( !pm.isDead() ){
				eai.KillPlayer();
			}

			if(speed <= maxSpeed){
				speed += acceleration/100;
				Mathf.Lerp(speed,maxSpeed,Time.deltaTime*speed/maxSpeed);
			}

			moveDirection.z = speed;
			
			axis = Mathf.Lerp (lastAxis, axis, Time.deltaTime * handling);
			myTransform.Rotate(0,0, axis * -1.0f  * speed * handling * Time.deltaTime);
			
			anim.SetFloat ("f_turn", axis * -1);
			lastAxis = axis;
		}else{
			//HE DIES
			if ( speed  >= 1 ) {
				speed -= 1.0f;
			}else if ( speed >= 0 || speed < 0 ){
				speed = 0;
			}
			moveDirection.z = Mathf.Lerp ( speed, 0, Time.deltaTime * -1 );
		}
		// end of dead check
        myTransform.Translate(moveDirection * Time.deltaTime);
	}
	override public void TakeDamage(float dam, bool hardHit){
		collisionParticles[0].Play();
		collisionParticles[1].Play();
		if(hardHit){
            if (!isDead()) { 
			    animationToPlay = Random.Range (0, hitAnimations.GetLength(0));
			    anim.Play(hitAnimations[animationToPlay]);
			    speed = dam * 2;
			    if( speed <= minSpeed && !isDead() ){
				    speed = minSpeed;
			    }
            }
		}
		hc.ChangeHP( -dam );
		if (hc.GetHP() <= 0) {
			Die();
		}
	}

    //int	  side: -1 = right
    //				+1 = left
    override public void EmergencyTurn(int side)
    {
        axis = handling * EmergencyTurnRatio * side * -1;
    }
    override public void Turn(int side)
    {
        axis = handling * side * -1;
    }
    override public void TurnWithFactor(int side, float factor)
    {
        axis = handling * side * -1 * factor;
    }

	IEnumerator Wait(){
		waitActive = true;
		yield return new WaitForSeconds (4.0f);
		waitActive = false;
		Destroy (gameObject);
	}
	
	void Die(){
        if (this != null) {
		    dead = true;
		    bb.RagOn (speed);
		    if (!waitActive) {
			    StartCoroutine (Wait ());
		    }
        }
	}
}