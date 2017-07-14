using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	public String hazardName;
	public HazardType hazardType;
	public GameObject hazardBillboard;

	public void HazardIdentified() {
		if (GameManager.Instance.LogHazardIdentification (hazardType, hazardName)) {
			Debug.Log ("User identified hazard: " + hazardName);
			hazardBillboard.SetActive (true);
		}
	}

}
