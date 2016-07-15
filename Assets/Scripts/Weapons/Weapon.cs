using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float damage;

	public AudioClip shotAudio;	
	public AudioSource source;
	private float lowPitchRange = .7F;
	private float highPitchRange = .9F;
	//private float volRand;

	public ParticleSystem[] shootingParticles;
	
	public GameObject projectile;
	public Transform shotSpawn;
	
	public float fireRate;	
	private float nextFire;

	public int maxAmmo;
	private int ammo;
	public bool unlimitted = false;

	void Awake(){
		ammo = maxAmmo;
	}
	void Start(){
		if(source != null){
			source.clip = shotAudio;
		}
	}
	public void ShotSound(){
		source.pitch = Random.Range (lowPitchRange,highPitchRange);
//		volRand = Random.Range(.9f,1.8f);
		source.Play ();
	}

	public void Shoot (string ID) {
		GameObject shot;
		if( ammo > 0 ){
			if ( Time.time > nextFire ){
				foreach( ParticleSystem p in shootingParticles ){ p.Emit(1); }

				nextFire = Time.time + fireRate;

				shot = Instantiate(projectile, shotSpawn.position, shotSpawn.rotation) as GameObject;
				shot.GetComponent<ProjectileBehaviour>().damage = damage;
				shot.GetComponent<ProjectileBehaviour>().shotIgnores = ID;
				if(source != null){
					ShotSound();
				}
				if(!unlimitted){
					ammo--;
				}

			}
		}
	}
	public int GetAmmo(){
		return ammo;
	}
	public void SetAmmo(int a){
		ammo = a;
	}
}