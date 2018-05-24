using UnityEngine;
using Umeng;
using System.Runtime.InteropServices;


public class Dplus : MonoBehaviour {




	void OnGUI() {

		var obj = new JSONObject();
		obj["key"] = 1;
		obj["key2"] = 2;
		 


		if (GUI.Button(new Rect(150, 50, 500, 100), "track2"))
        {
			
			DplusAgent.track("name"); 
			DplusAgent.track("name",obj); 
			Debug.Log("track");
        }
		if (GUI.Button(new Rect(150, 200, 500, 100), "registerSuperProperty"))
		{
			
			DplusAgent.registerSuperProperty(obj);
			Debug.Log("registerSuperProperty");
		}
		if (GUI.Button(new Rect(150, 350, 500, 100), "clearSuperProperties"))
		{
			
			DplusAgent.clearSuperProperties();
			Debug.Log("clearSuperProperties");
		}
		if (GUI.Button(new Rect(150, 500, 500, 100), "setFirstLaunchEvent"))
		{
			DplusAgent.setFirstLaunchEvent (new[]{ "hello", "world" });
			Debug.Log("setFirstLaunchEvent");
		}
		if (GUI.Button(new Rect(150, 650, 500, 100), "getSuperProperty"))
		{
			Debug.Log("getSuperProperty:"+DplusAgent.getSuperProperty ("key"));


		}
		if (GUI.Button(new Rect(150, 800, 500, 100), "getSuperProperties"))
		{
			Debug.Log("getSuperProperties:"+DplusAgent.getSuperProperties().ToString());

		}
		if (GUI.Button(new Rect(150, 950, 500, 100), "back"))
		{
			Application.LoadLevel ("AnalyticsEntry");

		}

    }
}


