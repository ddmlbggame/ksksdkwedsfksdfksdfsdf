using UnityEngine;
using Umeng;
using System.Collections.Generic;


public class AnalyticsEntry : MonoBehaviour {



	void Start () {
		

				
		GA.Start();

		//调试时开启日志 发布时设置为false
		GA.SetLogEnabled (true);



				
		}


	void OnGUI() {


		if (GUI.Button (new Rect (150, 100, 500, 100), "Dplus")) {

			Application.LoadLevel ("Dplus");


		}

		if (GUI.Button (new Rect (150, 300, 500, 100), "Game")) {

			Application.LoadLevel ("Game");


		}

		if (GUI.Button (new Rect (150, 500, 500, 100), "back")) {

			Application.LoadLevel ("UmengDemo");

		}







    }





}


