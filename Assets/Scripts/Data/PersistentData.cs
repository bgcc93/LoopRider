using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {
	public float _sensitivity;
	public float _maxScore;
	public float _maxTime;

	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	public void changeSensitivity(float v){ _sensitivity = v; }
}
