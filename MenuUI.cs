using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class MenuUI : MonoBehaviour {

	public InputField nameField;
	public InputField emailField;
	public Text errorText;

	void Start () {
		errorText.enabled = false;
		Screen.orientation = ScreenOrientation.Landscape;
	}

	public void ViewInstructions() {
		bool nameReady;
		bool emailReady;

		nameReady = CheckName (nameField.text);
		emailReady = CheckEmail (emailField.text);

		if (nameReady && emailReady) {
			SaveName ();
			SaveEmail ();
			GameManager.Instance.LoadInstructions ();
		} else {
			errorText.enabled = true;
		}
	}

	void SaveName() {
		GameManager.Instance.userName = nameField.text;
	}

	void SaveEmail() {
		GameManager.Instance.userEmail = emailField.text;
	}

	bool CheckName(string name) {
		return !String.IsNullOrEmpty (name);
	}

	bool CheckEmail(string email) {
		if (String.IsNullOrEmpty (email))
			return false;

		string emailRegex =
			@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
			@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

		return Regex.IsMatch (email, emailRegex);
	}

}
