using UnityEngine;
using Umeng;
using System.Collections.Generic;


public class GameExample : MonoBehaviour {



	void Start () {
		

				
		GA.Start();

		//调试时开启日志 发布时设置为false
		GA.SetLogEnabled (true);



				
		}


	void OnGUI() {




		if (GUI.Button(new Rect(150, 50, 500, 100), "StartLevel"))
		{

			GA.StartLevel ("level1");
		}
		if (GUI.Button(new Rect(150, 200, 500, 100), "FinishLevel"))
		{

			GA.FinishLevel("level1");

		}
		if (GUI.Button(new Rect(150, 350, 500, 100), "Bonus"))
		{

			GA.Bonus (10, GA.BonusSource.Source10);
		}
		if (GUI.Button(new Rect(150, 500, 500, 100), "Pay"))
		{
			GA.Pay (19, GA.PaySource.Source10, 10);
		}
		if (GUI.Button(new Rect(150, 650, 500, 100), "Event"))
		{
			GA.Event ("event1");


		}
		if (GUI.Button(new Rect(150, 800, 500, 100), "getSuperProperties"))
		{
			GA.Event("event1","label");

		}
		if (GUI.Button(new Rect(150, 950, 500, 100), "back"))
		{
			Application.LoadLevel ("AnalyticsEntry");

		}

	}
		

}


