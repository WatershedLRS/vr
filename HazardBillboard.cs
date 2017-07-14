using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBillboard : MonoBehaviour {

	public string hazardName;
	public GameObject hazardText;
	public bool enableOnStart;

	private TextMesh hazardTextMesh;

	void Start () {
		gameObject.SetActive(enableOnStart);

		hazardTextMesh = hazardText.GetComponent<TextMesh> ();
		hazardTextMesh.text = hazardName;
	}

}
