using UnityEngine;
using System.Collections;

public class SceneFaderInOut : MonoBehaviour {
	/*
	public float fadeSpeed = 1.5f;

	private bool sceneStarting = true;

	void Awake(){
		guiTexture.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
	}

	void Update(){
		if (sceneStarting)
			StartScene ();
	}

	void FadeToClear(){
		//guiTexture.color = Mathf.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	void FadeToBlack(){
		//guiTexture.color = Mathf.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	void StartScene(){
		FadeToClear ();
		if (guiTexture.color.a <= 0.05f){
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			sceneStarting = false;
		}
	}
	public void EndScene(){
		sceneStarting = true;
		FadeToBlack ();
		if (guiTexture.color.a <= 0.05f) {
			//Application.loadedLevel(1);
		}
	}
	*/
}
