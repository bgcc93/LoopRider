using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour {

	public Camera cam;

	private PlayerMovement pm;

	public float boostMax = 50f;
	public float boostRechargeRate = 20f;
	public float boostCosumptionRate = 10f;
	public float boostPower = 0f;
	public float boost;
	public bool rechargeBoost;

	private float boostProgress = 0f;

	// Use this for initialization
	void Start () {
		boost = boostMax;
		rechargeBoost = false;
		pm = GetComponent<PlayerMovement> ();
	}

	void Update(){
		if( rechargeBoost && boost <= boostMax){
			Recharge ();
		}
	}

	public void Recharge(){
		boostProgress = 0f;
		boost += boostRechargeRate * Time.deltaTime;
		if( boost >= boostMax){
			boost = boostMax;
			rechargeBoost = false;
		}
	}
	
	public void UseBoost(int breOrBoos){
		if ( boost > 0f && rechargeBoost == false && !pm.isDead()) {
			//The boost's power increases as it's used
			//Here we check if it's reached the maximum
			if( boostProgress < boostPower ){
				boostProgress += boostCosumptionRate/10;
			}
			//Here we burn the boost
			boost -= boostCosumptionRate * Time.deltaTime;


			//here we check if it's boost or break to be used
			//this first one is the boost
			if( breOrBoos > 0 ){
				//so it doesn't overspeed, with the maximum speed defined at the player's prefab
				if( pm.speed < pm.extraSpeed ){
					//Here the boost affects the speed
					pm.speed += (boostProgress * breOrBoos) / (boostCosumptionRate * 2.5f);
					Mathf.Lerp( pm.speed, pm.extraSpeed, Time.deltaTime * pm.extraSpeed / pm.speed);
				}
				//little camera shake to give it an extra somethng
				pm.shake = .3f;
				//finally we change the FoC cause it gives a nice effect
				if( cam.fieldOfView < 120 ){
					cam.fieldOfView += .5f;
				}
			}else{
				//same ol' same ol'
				if(pm.speed > pm.minBreak ){
					pm.speed += (boostProgress * breOrBoos) / (boostCosumptionRate * 2.5f);
					Mathf.Lerp(pm.speed, pm.minBreak, Time.deltaTime * pm.speed / pm.minBreak );
				}
				//camShift.z *= .5f;
				if( cam.fieldOfView > 40 ){
					cam.fieldOfView -= .5f;
				}
			}
		//most likely the boost will be empty so we etart to recharge it again
		}else{
			rechargeBoost = true;
		}
	}

}
