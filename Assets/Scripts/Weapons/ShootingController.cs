using UnityEngine;
using System.Collections;

public class ShootingController : MonoBehaviour {

	public Transform weaponHolder;
	private Transform[] guns;
	private Weapon[] weapons;

	private int totalGuns = 0;
	private string ID;

	private bool safetyOn = false;

	public void ToggleSafety(){ safetyOn = !safetyOn;}
	public bool GetSafety(){ return safetyOn;}


	void Awake(){
		guns = new Transform[weaponHolder.childCount];

		for( int i = 0 ; i < weaponHolder.childCount; i++ ){ 
			guns[i] = weaponHolder.GetChild(i).transform; 
			totalGuns += guns[i].childCount;
		}

		ID = transform.name;

		weapons = new Weapon[totalGuns];

		for( int i = 0 ; i < guns.GetLength(0); i++ ){
			for( int j = 0 ; j < guns[i].childCount ; j++ ){
				weapons[2*i+j] = guns[i].GetComponentsInChildren<Weapon>()[j];
				if( j == 0 ){
					weapons[2 * i + j].source = weapons[2 * i + j].GetComponent<AudioSource>();
				}else{
					weapons[2 * i + j].GetComponent<AudioSource>().enabled = false;
				}
			}
		}
	}
	public void TakeAim(int gunTier, Transform tgt){
		guns [gunTier].LookAt (tgt);
	}
	public void SetAmmo(int weaponTier, int qty){
		for( int i = 0 ; i < guns[weaponTier].childCount ; i++ ){
			weapons[2 * weaponTier + i].SetAmmo(qty);
		}
	}
	public void DepleteAmmo(int weaponTier){
		for( int i = 0 ; i < guns[weaponTier].childCount ; i++ ){
			weapons[2 * weaponTier + i].SetAmmo(0);
		}
	}
	public void DepleteAllAmmo(){
		for( int i = 0 ; i < weapons.GetLength(0) ; i++ ){
			weapons[i].SetAmmo(0);
		}
	}
	public void RefilAllAmmo(){
		for( int i = 0 ; i < weapons.GetLength(0) ; i++ ){
			weapons[i].SetAmmo(weapons[i].maxAmmo);
		}
	}
	public void ShootWeapons( int weaponTier ){
		if(!GetSafety()){
			for( int i = 0 ; i < guns[weaponTier].childCount ; i++ ){
				weapons[2 * weaponTier + i].Shoot(ID);
			}
		}
	}
	public int GetAmmoFromWeapon( int weaponTier ){
		if( weapons[weaponTier + 1] != null ){
			return weapons[weaponTier + 1].GetAmmo();
		}else{
			return 0;
		}
	}
}