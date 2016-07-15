using UnityEngine;
using System.Collections;

public class EventManagerPlayer : MonoBehaviour {
	private GameObject player;

	private ShootingController sc;
	private BoostController bc;
	private PlayerMovement pm;

	private bool b_shoot1 = false;
	private bool b_shoot2 = false;
	private bool b_boost = false;
	private bool b_break = false;

	void Start(){
		do{
			player = GameObject.Find("Player");
		}while (player == null);

		sc = player.GetComponentInChildren<ShootingController>();
		bc = player.GetComponent<BoostController> ();
		pm = player.GetComponent<PlayerMovement> ();
	}

	public void Shoot1_Down(){ b_shoot1 = true ; }
	public void Shoot1_Up()  { b_shoot1 = false; }

	public void SlowMo(){ pm.ToggleSlowMo(); }
	
	public void Shoot2_Down(){ b_shoot2 = true ; }
	public void Shoot2_Up()  { b_shoot2 = false; }

	public void Boost_Down(){ b_boost = true; }
	public void Boost_Up()  { b_boost = false; bc.rechargeBoost = true; }

	public void BreaksDown(){ b_break = true; }
	public void BreaksUp()  { b_break = false; bc.rechargeBoost = true; }

	void Update(){
		if(b_shoot1) { sc.ShootWeapons(0);}
		if(b_shoot2) { sc.ShootWeapons(1); }
		if(b_boost)  { bc.UseBoost(1); }
		if(b_break)  { bc.UseBoost(-1); }
	}
}
