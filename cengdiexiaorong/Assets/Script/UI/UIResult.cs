using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResult : UIBase
{
	public static UIInfo Info = new UIInfo(UIType.Result, UIHierarchyType.Top, "UI_Result");

	public GameObject close;

	public GameObject success;

	public Animator success_ani;

	public GameObject defeat;

	public Animator defeat_ani;

	public static bool result;

	public override void OnEnable()
	{
		EventTriggerListener.Get(this.close).onClick = this._OnClose;
		this._Refresh();
	}

	private void _Refresh()
	{
		if (result)
		{
			success_ani.Play("UI_ChuangGuan",-1,0);
			
		}
		else
		{
			defeat_ani.Play("UI_CGSB",-1,0);
		}
		this.success.SetActive(result);
		this.defeat.SetActive(!result);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.success.SetActive(false);
		this.defeat.SetActive(false);
	}

	private void _OnClose(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
		if (GameControl.Instance.game_data._current_game_type == GameType.challenge)
		{
			if (result)
			{
				GameControl.HandleFinishChallangeOneEvent();
				GameControl.Instance.game_data.SetRandomLevel();
				GameScene.Instance.SetGameStart(false);
			}
			else
			{
				UIManager.Instance.PushShow(UIFinish.Info);
			}
		}else
		{
			UIManager.Instance.PushShow(UIFinish.Info);
		}
	}
}
