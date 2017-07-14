using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsUI : MonoBehaviour {

	void Start() {
		Screen.orientation = ScreenOrientation.Landscape;
	}

	public void StartSimulation() {
		GameManager.Instance.LoadSimulation ();
	}

}
