using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public Text title;
	public Text summary;

	void Start () {
		Screen.orientation = ScreenOrientation.Landscape;

		int identifiedCount = GameManager.Instance.hazardsIdentified.Count;
		string hazardTense;

		// Set title based on how many hazards were identified
		string newTitle;

		if (identifiedCount > 0) {
			newTitle = "Good Job!";
		} else { 
			newTitle = "Better Luck Next Time";
		}

		title.text = newTitle;

		// Set message based on how many hazards were identified
		if (identifiedCount == 1) {
			hazardTense = "hazard";
		} else {
			hazardTense = "hazards";
		}

		if (GameManager.Instance.allHazardsIdentified) {
			summary.text = String.Format ("You identified all {0} industrial safety hazards!", identifiedCount);
		} else {
			summary.text = String.Format("You identified {0} industrial safety {1}.", identifiedCount, hazardTense);
		}
	}

	public void RestartSimulation() {
		GameManager.Instance.ResetSimulation ();
		GameManager.Instance.LoadMenu ();
	}

}
