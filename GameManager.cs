using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TinCan;
using TinCan.LRSResponses;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CielaSpike;

public class GameManager : GenericSingleton<GameManager> {

	[HideInInspector] public string userName;
	[HideInInspector] public string userEmail;
	[HideInInspector] public List<HazardType> hazardsIdentified;
	[HideInInspector] public bool allHazardsIdentified;
	[HideInInspector] public int totalHazardTypes;

	private RemoteLRS lrs;

	void Start () {
		hazardsIdentified = new List<HazardType> ();
		totalHazardTypes = System.Enum.GetValues (typeof(HazardType)).Length;
		ResetSimulation();

		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		lrs = new RemoteLRS (WatershedCredentials.ENDPOINT, WatershedCredentials.KEY, WatershedCredentials.SECRET);
	}

	public void LoadMenu() {
		SceneManager.LoadScene ("Menu");
	}

	public void LoadInstructions() {
		SceneManager.LoadScene ("Instructions");
	}

	public void LoadSimulation() {
		SceneManager.LoadScene ("Simulation");
	}

	public void CloseSimulation() {
		SceneManager.LoadScene ("SimulationExit");
	}

	public void EndSimulation() {
		SceneManager.LoadScene ("Score");
	}

	public void ResetSimulation() {
		userName = "";
		userEmail = "";
		hazardsIdentified.Clear ();
		allHazardsIdentified = false;
	}

	public bool LogHazardIdentification(HazardType hazardType, string hazardName) {
		bool hazardAlreadyIdentified = hazardsIdentified.Contains (hazardType);

		if (hazardAlreadyIdentified) {
			return false;
		} else {
			hazardsIdentified.Add (hazardType);
			this.StartCoroutineAsync(SendStatement(hazardName));

			if (hazardsIdentified.Count == totalHazardTypes) {
				allHazardsIdentified = true;
			}

			return true;
		}
	}

	private IEnumerator SendStatement(string hazardName) {
		Statement statement = ConstructStatement (
			String.IsNullOrEmpty(GameManager.Instance.userName) ? "VR User" : GameManager.Instance.userName,
			String.IsNullOrEmpty(GameManager.Instance.userEmail) ? "vr@foo.bar" : GameManager.Instance.userEmail,
			"http://foo.bar/identified",
			"identified",
			"http://foo.bar/identified",
			hazardName
		);

		StatementLRSResponse lrsResponse;
		yield return lrsResponse = lrs.SaveStatement (statement);

		if (lrsResponse.success) {
			Debug.Log ("Statement saved: " + lrsResponse.content.id);
		} else {
			Debug.Log ("Failed to save statment: " + lrsResponse.errMsg);
		}
	}

	private Statement ConstructStatement(string actorName, string actorEmail, string verbUrl, string verbName, string activityUrl, string activityName) {
		Agent actor;
		Verb verb;
		Activity activity;
		Statement statement;

		actor = new Agent ();
		actor.mbox = "mailto:" + actorEmail;
		actor.name = actorName;

		verb = new Verb ();
		verb.id = new Uri(verbUrl);
		verb.display = new LanguageMap();
		verb.display.Add("en-US", verbName);

		activity = new Activity ();
		activity.id = activityUrl;
		activity.definition = new TinCan.ActivityDefinition ();
		activity.definition.description = new LanguageMap();
		activity.definition.description.Add("en-us", activityName);

		statement = new Statement ();
		statement.actor = actor;
		statement.verb = verb;
		statement.target = activity;

		return statement;
	}

	private bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i=0; i<chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}

}

public enum HazardType {
	BadWiring,
	ImproperStacking,
	UnsecuredMachinery,
	UnsecuredMaterial
};