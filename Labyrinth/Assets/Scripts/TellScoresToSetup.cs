using UnityEngine;
using System.Collections;

public class TellScoresToSetup : MonoBehaviour {

	PlayerScoresScript pss;

	// Use this for initialization
	void Start () {
		pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
		pss.SetupStoredScores();
	}
}
