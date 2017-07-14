using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationUI : MonoBehaviour {
	
	public void ExitSimulation() {
		GameManager.Instance.CloseSimulation ();
	}

}
