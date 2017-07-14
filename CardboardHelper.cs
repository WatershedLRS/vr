using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CardboardHelper : MonoBehaviour {

	public bool loadCardboard;

	void Start () {
		#if UNITY_IOS
		if (loadCardboard) {
			StartCoroutine (LoadDevice ("cardboard", true));
		} else {
			StartCoroutine (LoadDevice ("", false));
		}
		#endif
	}

	IEnumerator LoadDevice(string newDevice, bool enable) {
		VRSettings.LoadDeviceByName (newDevice);
		yield return null;
		VRSettings.enabled = enable;
		InputTracking.Recenter ();
	}

}
