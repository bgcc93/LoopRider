using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : Movement {
	PersistentData pd;
	GameObject persistentDataObject;
	

	public AudioClip engineRev;
    public float EmergencyTurnRatio = 3.0f;
    private AudioSource source;


	private float enginePitch;
	
	private float volRand;

	protected Animator[] anim;
	
	public float shake = 0f;
	public float shakeAmount = 0.7f;
	public float shakeDecreaseFactor = 1f;

	public ParticleSystem[] collisionParticles;
	
	public Camera cam;
	private Transform camHolder;

	public float speed = 0.0f;
	public float extraSpeed = 250f;
	public float maxSpeed = 200f;
	public float minSpeed = 50f;
	public float minBreak = 35f;
	public float acceleration = 2f;

	public float handling = 0f;
	
	public string[] hitAnimations;
	private int animationToPlay = 0;
	
	public Slider healthBarSlider;
	public Slider speedSlider;
	public Slider extraSpeedSlider;
	public Slider BoostBarSlider;

	public Slider[] Ammo1;
	public Slider[] Ammo2;

    public controlEnum controlMode;
    public  enum controlEnum{kb, cel, auto};

	private float sensitivity;
	
	private Vector3 moveDirection = Vector3.zero;
	private float axis = 0;
	private float lastAxis = 0;

	private bool slowMoActive = false;

	private bool dead = false;
	private bool waitActive = false;
	
	private ShootingController sc;
	private BoostController bc;
	private HealthController hc;
	private HeroBehaviour hb;
	private BikeBehaviour bb;
    


	private float camFovInit = 90;

	private Transform myTransform;
    private Transform bikeTransform;

	void Awake(){
		if( persistentDataObject = GameObject.Find ("PersistentData") ){
			pd = persistentDataObject.GetComponent<PersistentData> ();
			sensitivity = pd._sensitivity;
		}else{ sensitivity = 4; }
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		myTransform = transform;

		speed = 50;
	}

	void Start () {
		camFovInit = cam.fieldOfView;
		camHolder = GameObject.Find ("CamHolder").transform;

		sc = GetComponentInChildren<ShootingController>();
		bc = GetComponent<BoostController> ();
		hb = GetComponentInChildren<HeroBehaviour> ();
		bb = GetComponentInChildren<BikeBehaviour> ();
		hc = GetComponentInChildren<HealthController> ();

		anim = GetComponentsInChildren<Animator> ();
		source = GetComponentInChildren<AudioSource>();

		BoostBarSlider.maxValue = bc.boostMax;
		healthBarSlider.maxValue = hc.GetHP();
		speedSlider.maxValue = maxSpeed;
		extraSpeedSlider.minValue = maxSpeed;
		extraSpeedSlider.maxValue = extraSpeed;
		for (int i = 0; i < Ammo1.GetLength(0); i++) {
			Ammo1[i].maxValue = sc.GetAmmoFromWeapon(0);
		}
		
		for (int i = 0; i < Ammo2.GetLength(0); i++) {
			Ammo2[i].maxValue = sc.GetAmmoFromWeapon(1);
		}
        foreach (Transform child in transform)
        {
            if (child.CompareTag("bike")) {
                bikeTransform = child.transform;
                break;
            }
        }
	}

	public bool isDead(){return dead;}

	void Sound(){
		//volRand = speed / maxSpeed + 2f;
		source.pitch = speed / maxSpeed / 5;
		source.PlayOneShot (engineRev);
	}
    void CameraUpdates()
    {
        if (shake > 0)
        {
            cam.transform.localPosition = Random.insideUnitSphere * shakeAmount / 10;
            shake -= Time.deltaTime * shakeDecreaseFactor;

        }
        else
        {
            shake = 0.0f;
            cam.transform.localPosition = Vector3.zero;
        }

        if (cam.fieldOfView <= camFovInit)
        {
            cam.fieldOfView += .2f;
        }
        else if (cam.fieldOfView >= camFovInit)
        {
            cam.fieldOfView -= .2f;
        }

        if (speed > maxSpeed)
        {
            speed -= acceleration / 75;
            Mathf.Lerp(speed, maxSpeed, Time.deltaTime * speed / extraSpeed);
        }
    }

	void Update() {
		//Sound ();
		if (Time.frameCount % 30 == 0)
		{
			System.GC.Collect();
		}

        CameraUpdates();

		if( !isDead() ){
            //keyboard controls
            if( controlMode == controlEnum.kb ){
                axis = Input.GetAxis("Horizontal") * -1;
            }
            //Android device controls
            else if (controlMode == controlEnum.cel)
            {
                axis = Input.acceleration.x * sensitivity * 2;

                //set maximum axis
                if (axis > 1)
                {
                    axis = 1;
                }
                else if (axis < -1)
                {
                    axis = -1;
                }
                axis *= -1;
            }
            
			if(speed <= maxSpeed){
				speed += acceleration/100;
				Mathf.Lerp(speed,maxSpeed,Time.deltaTime*speed/maxSpeed);
			}

			moveDirection.z = speed;
			
			axis = Mathf.Lerp (lastAxis, axis, Time.deltaTime * handling);
			myTransform.Rotate(0,0, axis * -1.0f * speed * handling * Time.deltaTime);
			foreach(Animator a in anim){
				a.SetFloat ("f_turn", axis * -1 );
			}
			lastAxis = axis ;
		}else{
			//HE DIES
			if ( speed  >= 1 ) {
				speed -= 1.0f;
			}else if ( speed >= 0 || speed < 0 ){
				speed = 0;
			}
			moveDirection.z = Mathf.Lerp ( speed, 0, Time.deltaTime * -1 );
            camHolder.position = new Vector3(-2.5f, bikeTransform.position.y / 2, bikeTransform.position.z - 8);

		}
		// end of dead check
        
        myTransform.Translate(moveDirection * Time.deltaTime);

		UpdateSliders ();
	}

	void UpdateSliders(){
		BoostBarSlider.value = bc.boost;
		
		for (int i = 0; i < Ammo1.GetLength(0); i++) {
			Ammo1[i].value = sc.GetAmmoFromWeapon(0);
		}
		
		for (int i = 0; i < Ammo2.GetLength(0); i++) {
			Ammo2[i].value = sc.GetAmmoFromWeapon(1);
		}

		speedSlider.value = speed;
		extraSpeedSlider.value = speed;
		healthBarSlider.value = hc.GetHP();
	}

	public void ToggleSlowMo(){
        if( !isDead() ){
            if (!slowMoActive)
            { 
			    Time.timeScale = .3f;
			    handling *= 3;
			    slowMoActive = true;
		    }else{
			    slowMoActive = false;
			    handling /= 3;
			    Time.timeScale = 1;
		    }
        }
	}
	override public void TakeDamage(float dam, bool hardHit){
        collisionParticles[0].Play();
        collisionParticles[1].Play();
		if( hardHit ){
			shake = 1;
			animationToPlay = Random.Range (0, hitAnimations.GetLength(0));
			anim[0].Play(hitAnimations[animationToPlay]);
			speed -= dam * 2;
			if( speed <= minSpeed && !isDead() ){
				speed = minSpeed;
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
		Application.LoadLevel (0);
	}
	
	void Die(){
		dead = true;
        slowMoActive = true;
        ToggleSlowMo();
		camHolder.parent = null;
		hb.RagOn (speed);
		bb.RagOn (speed);

		Destroy(GameObject.Find ("Dstr"));
        Destroy(GameObject.Find("ObstaclesSpawners"));
		foreach(Animator a in anim){
			a.enabled = false;
		}
		if (!waitActive) {
			StartCoroutine (Wait ());
		}
	}
}