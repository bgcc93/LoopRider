using UnityEngine;
using System.Collections.Generic;

public class LoopManager : MonoBehaviour {
	public Transform[] prefab;
	public int numberOfObjects;
	public Vector3 startPosition;
	public float recycleOffset;
    public float objectsOffset = 82;

	private Transform player;
	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;
	private int idx = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
		objectQueue = new Queue<Transform>(numberOfObjects);
		nextPosition = startPosition;
		for (int i = 0; i < numberOfObjects; i++) {
			idx = Random.Range (0, prefab.GetLength(0));
			Transform o = (Transform)Instantiate(prefab[idx]);
			o.localPosition = nextPosition;
            nextPosition.z += objectsOffset;
			objectQueue.Enqueue(o);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (objectQueue.Peek().localPosition.z + recycleOffset < player.position.z) {
			Transform o = objectQueue.Dequeue();
			o.localPosition = nextPosition;
            nextPosition.z += objectsOffset;
			objectQueue.Enqueue(o);
		}

	}
}
