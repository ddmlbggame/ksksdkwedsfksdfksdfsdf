using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFinish : UIBase {

	public  static UIInfo Info = new UIInfo(UIType.Finish, UIHierarchyType.Dialog, "UI_Finish");
	[SerializeField]
	private GameObject _back;
	[SerializeField]
	private GameObject _restart;
	[SerializeField]
	private GameObject _next;
	[SerializeField]
	private Text hight_record;
	[SerializeField]
	private Text beyond_record;
	[SerializeField]
	private Text current_record;

	public static int chanllange_level = 0;
	public override void OnEnable()
	{
		base.OnEnable();
		EventTriggerListener.Get(this._back).onClick = this._OnClickBack;
		EventTriggerListener.Get(this._restart).onClick = this.OnClickRestart;
		EventTriggerListener.Get(this._next).onClick = this.OnClickNext;
		_Refresh();
	}

	private void _Refresh()
	{
		if(GameControl.Instance.game_data._current_game_type == GameType.Custom)
		{
			int max_level = GameControl.Instance.game_data.GetLevelDatas(GameControl.Instance.game_data.Current_Difficulty).Count;
			if (GameControl.Instance.game_data.currentGameLevel < max_level)
			{
				this._next.SetActive(true);
				this._restart.SetActive(false);
			}else
			{
				this._next.SetActive(false);
				this._restart.SetActive(true);
			}
			int record_time = GameData.GetRecord(GameControl.Instance.game_data.Current_Difficulty, GameControl.Instance.game_data.currentGameLevel);
			if (record_time == 0)
			{
				this.hight_record.text = "无纪录";
				GameData.SetRecord(GameControl.Instance.game_data.Current_Difficulty, GameControl.Instance.game_data.currentGameLevel, UIMain.custom_cost_time);
			}
			else
			{
				this.hight_record.text = GameControl.SetTimeFormat(record_time);
			}
			if(UIMain.custom_cost_time< record_time)
			{
				GameData.SetRecord(GameControl.Instance.game_data.Current_Difficulty, GameControl.Instance.game_data.currentGameLevel, UIMain.custom_cost_time);
			}
			this.current_record.text = GameControl.SetTimeFormat(UIMain.custom_cost_time);
			this.beyond_record.text = GameControl.BeatPeple(UIMain.custom_cost_time, GameControl.Instance.game_data._current_game_type);
		}
		else
		{
			this._next.SetActive(false);
			this._restart.SetActive(true);
			this.current_record.text = GameControl.Instance.game_data.ChallangePassedNumber.ToString();
			if (chanllange_level == 0)
			{
				this.hight_record.text = "无纪录";
			}
			else
			{
				this.hight_record.text = chanllange_level.ToString();
			}
			this.beyond_record.text = GameControl.BeatPeple(UIMain.custom_cost_time, GameControl.Instance.game_data._current_game_type);
		}
		

	}
	private void _OnClickBack(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
		UIManager.Instance.PopShow();
		bool showrate = false;
		int level = GameData.GetPassedLevel(GameControl.Instance.game_data.Current_Difficulty);
		if (Random.Range(0,100) >=70 || (GameControl.Instance.game_data.currentGameLevel ==3 && level==4) )
		{
			showrate = true;
			SDK.Instance.GoToCommnet();
		}
		if (!showrate)
		{
			SDK.Instance.ShowInterstitial();
		}
	}
		

	private void OnClickRestart(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
		GameScene.Instance.SetGameStart();
		GameControl.Instance.game_data.ResetChallangeData();
		GameControl.HandleRestartEvent();
		SDK.Instance.StartLevel(string.Format("重新开始，模式{0}",GameControl.Instance.game_data._current_game_type));
	}

	private void OnClickNext(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
		GameControl.Instance.game_data.currentGameLevel++;
		GameScene.Instance.SetGameStart(true);
		GameControl.HandleRestartEvent();

	}
}
