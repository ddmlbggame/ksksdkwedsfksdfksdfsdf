using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuy : UIBase {

	public GameObject sure;

	public GameObject cancel;

	public static UIInfo Info = new UIInfo(UIType.Buy, UIHierarchyType.Dialog, "UI_Buy");

	public override void OnEnable()
	{
		base.OnEnable();
		EventTriggerListener.Get(this.sure).onClick = this.OnClickSure;
		EventTriggerListener.Get(this.cancel).onClick = this.OnClickCancel;
		
	}

	private void OnClickSure(GameObject obj)
	{
		SDK.Instance.ShowRewardedVideo();
		UIManager.Instance.Hide(Info);
	}

	private void OnClickCancel(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
	}

}
