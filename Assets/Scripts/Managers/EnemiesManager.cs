using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {
	public GameObject enemy;
	public int spwnQty = 1;

//	private int enemiesInScene = 0;
	private Transform player;
	private float gameTime = 0;
	private float r = 0;
	private GameObject en;
	private int a = 0;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;

	}
	
	// Update is called once per frame
	void Update () {

		gameTime += Time.deltaTime;
		if( spwnQty == 0 ||  a < spwnQty ){
			if( Mathf.Ceil(gameTime) % 3 == 0){
				Vector3 v = new Vector3(0, 0, player.position.z + 45);
				r = Random.Range (0,359);

				Instantiate(enemy, v, Quaternion.Euler(0,0,r));
				a++;
				gameTime += 1;
			}
		}
	}
}
