using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnter : MonoBehaviour {
	void Awake()
	{
		//LTLocalization.Init();
		DontDestroyOnLoad(this.gameObject);
		UIManager.Instance._Init(this.transform);
		UIManager.Instance.PushShow(UILogin.Info, true);
		GameControl.Instance.Init(this.transform);
	}

}
