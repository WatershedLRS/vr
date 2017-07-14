using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationExit : MonoBehaviour {

	public TextMesh timerText;

	private float timer;

	void Start () {
		timer = 10;
	}

	void Update () {
		string seconds = Mathf.Floor(timer % 60).ToString();

		timerText.text = String.Format ("Closing simulation in {0}", seconds);

		timer -= Time.deltaTime;

		if (timer < 0) {
			GameManager.Instance.EndSimulation ();
		}
	}

}
