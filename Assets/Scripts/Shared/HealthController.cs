using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	public float MaxHP;
    public float thoughness;
	private float HP;

	// Use this for initialization
	void Awake () {
		HP = MaxHP;
	}
	void Start(){
	}
	public float GetHP(){
		return HP;
	}
	public void ChangeHP(float val){
		HP += val;
	}
}
