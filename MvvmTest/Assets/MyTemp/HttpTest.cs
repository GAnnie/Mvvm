using UnityEngine;
using System.Collections;
using Foundation.Core;

public class HttpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(GUILayout.Button("Test"))	
		{
			StartCoroutine(Test());	
		}
	}
	
	IEnumerator Test()
	{
		var client = new HttpServiceClient();
		client.Accept = "application/json";
		
		var task = client.GetAsync("http://127.0.0.1/E%3A/player.json");
		
		yield return StartCoroutine(task.WaitRoutine());
		
		if(task.IsFaulted)
			Debug.LogException(task.Exception);
		
		var reslt = task.Result;
		
		Debug.Log(reslt);
	}
}
