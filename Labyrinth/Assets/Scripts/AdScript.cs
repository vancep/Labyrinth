using UnityEngine;
using UnityEngine.Advertisements;

public class AdScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ShowAd();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowAd()
	{
		Debug.Log("Attempting to Show Ad");

		if(Advertisement.IsReady())
		{
			Debug.Log("Showing Ad");
			Advertisement.Show();
		}
		else
		{
			Debug.Log("Not Showing Ad");
		}
	}
}
