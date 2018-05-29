using System;
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
		
		this._Refresh();
	}

	private void _Refresh()
	{
		if (result)
		{
			success_ani.Play("UI_ChuangGuan",-1,0);
			
			WaitUntil(_GetAnimationClipLength(success_ani , "UI_ChuangGuan"), _OnClose);
		}
		else
		{
			defeat_ani.Play("UI_CGSB",-1,0);
			WaitUntil(_GetAnimationClipLength(defeat_ani, "UI_CGSB"), _OnClose);
		}
		
		this.success.SetActive(result);
		this.defeat.SetActive(!result);
	}

	private float _GetAnimationClipLength(Animator success_ani ,string clip)
	{
		if (success_ani == null)
		{
			return 0;
		}
		var clips = success_ani.runtimeAnimatorController.animationClips;
		for (int i = 0; i < clips.Length; i++)
		{
			if (clips[i].name.Equals(clip))
			{
				return clips[i].length;
			}
		}
		return 0;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.success.SetActive(false);
		this.defeat.SetActive(false);
		if (waitie != null)
		{
			StopCoroutine(waitie);
			waitie = null;
		}
		
	}

	private void _OnClose()
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

	public void WaitUntil(float time, Action callback)
	{
		if (waitie == null)
		{
			waitie = wait(time, callback);
		}
		else
		{
			StopCoroutine(waitie);
		}
		StartCoroutine(waitie);
	}

	IEnumerator waitie = null;
	private IEnumerator wait(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		if (callback != null)
		{
			callback();
		}
	}
}
