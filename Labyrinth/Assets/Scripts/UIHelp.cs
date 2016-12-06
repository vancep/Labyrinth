using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class UIHelp
{
	public static T getAccessTo<T>(string myTag)
	{
		GameObject gameObj = GameObject.FindWithTag(myTag);

		if(gameObj != null)
		{
			return gameObj.GetComponent<T>();
		}
		else
		{
			return default(T);
		}

	}

	public static Dropdown getAccessToDropdown(string myTag)
	{
		GameObject gameObj = GameObject.FindWithTag(myTag);

		if(gameObj != null)
		{
			return gameObj.GetComponent<Dropdown>();
		}
		else
		{
			return null;
		}
	}

	public static Toggle getAccessToToggle(string myTag)
	{
		GameObject gameObj = GameObject.FindWithTag(myTag);

		if(gameObj != null)
		{
			return gameObj.GetComponent<Toggle>();
		}
		else
		{
			return null;
		}
	}


}
