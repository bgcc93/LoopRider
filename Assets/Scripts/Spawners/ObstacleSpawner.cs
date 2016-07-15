using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject[] obj; 
    public float spawnMin;
    public float spawnMax;
    public int chanceToSpawn = 0;

    private int objNum = 0;

    void Awake()
    {
        objNum = obj.GetLength(0);
        StartCoroutine(Spawn());        
    }
    IEnumerator Spawn()
    {
        while (true)
        {            
            if( chanceToSpawn < Random.Range(0, 100 ) ){
                int idx = Random.Range(0, objNum);
                Instantiate(obj[idx], transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(spawnMin, spawnMax));
        }
	}
}