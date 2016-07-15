using UnityEngine;
using System.Collections;

public class MainMenuEventManager : MonoBehaviour {
	DataIO dio;
	GameObject dataIoObj;

	void Start(){

		dataIoObj = GameObject.Find ("Main Camera");
		dio = dataIoObj.GetComponent<DataIO> ();
	}

	public void StartGame(){ Application.LoadLevel (1); }
	public void SetGraphsLow(){ QualitySettings.SetQualityLevel(0); }
	public void SetGraphsMed(){ QualitySettings.SetQualityLevel(2); }
	public void SetGraphsHig(){ QualitySettings.SetQualityLevel(4); }
	public void SetGraphsUlt(){ QualitySettings.SetQualityLevel(5); }
}
