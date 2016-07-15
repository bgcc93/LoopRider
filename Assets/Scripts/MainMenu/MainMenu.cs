using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	PersistentData pd;
	GameObject persistentDataObject;

	public Text lbl_time;
	public Text lbl_dist;
	public Slider sli_sens;

	// Use this for initialization
	void Start () {
		persistentDataObject = GameObject.Find ("PersistentData");
		pd = persistentDataObject.GetComponent<PersistentData> ();
		lbl_time.text = pd._maxTime.ToString("F2") + " Secs";
		lbl_dist.text = (pd._maxScore / (300 * 12)).ToString ("F1") + " Km";
		sli_sens.value = pd._sensitivity;
	}
}
