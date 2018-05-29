using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : UIBase {

	public static UIInfo Info = new UIInfo(UIType.Login, UIHierarchyType.Normal,  "UI_LogIn");

	public GameObject _custom_game;

	public GameObject _race_game;

	public GameObject _setting;

	public Text max_level;

	public override void OnEnable()
	{
		base.OnEnable();
		EventTriggerListener.Get(this._custom_game).onClick = this._OnClickCustom;
		EventTriggerListener.Get(this._race_game).onClick = this._OnClickRace;
		EventTriggerListener.Get(this._setting).onClick = this._OnClickSetting;
		Refresh();
	}

	private void Refresh()
	{
		int level = GameData.GetChanllangeLevel();
		max_level.text = string.Format(LTLocalization.GetText("1"), level);
	}

	private void _OnClickCustom(GameObject obj)
	{
		UIManager.Instance.PushShow(UILevel.Info,true);
	}

	private void _OnClickRace(GameObject obj)
	{
		GameControl.Instance.PlayGame( GameType.challenge, LevelDifficulty.Simple ,1);
		SDK.Instance.StartLevel("进入挑战模式");
		int max_level = GameData.GetChanllangeLevel();
		UIFinish.chanllange_level = max_level;
	}

	private void _OnClickSetting(GameObject obj)
	{
		UIManager.Instance.PushShow(UIPause.Info);
	}
	
}
