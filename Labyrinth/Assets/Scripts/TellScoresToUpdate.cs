using UnityEngine;
using System.Collections;

public class TellScoresToUpdate : MonoBehaviour {

	PlayerScoresScript pss;

	// Use this for initialization
	void Start () {
		pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
		TellScores();
	}
		
	public void TellScores()
	{
		pss.UpdatePlayerScores();
	}
}
