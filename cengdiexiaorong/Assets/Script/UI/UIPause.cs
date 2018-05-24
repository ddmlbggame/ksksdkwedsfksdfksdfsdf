using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : UIBase {

	public static UIInfo Info = new UIInfo(UIType.Pause, UIHierarchyType.Normal, "UI_Pause");

	public GameObject close;

	public GameObject sound;

	public GameObject music;

	public GameObject advertising;

	public GameObject restoradvertising;

	public GameObject soundoff;

	public GameObject soundon;

	public GameObject musicoff;

	public GameObject musicon;

	public override void OnEnable()
	{
		EventTriggerListener.Get(this.close).onClick = this._OnClose;
		EventTriggerListener.Get(this.sound).onClick = this._OnSound;
		EventTriggerListener.Get(this.music).onClick = this._OnMusic;
		EventTriggerListener.Get(this.restoradvertising).onClick = this._OnResorePurchase;
		EventTriggerListener.Get(this.advertising).onClick = this._OnAdvertising;
		SDK.PurchaseRemoveAds += RemovePurseRemoveAds;
		this._Refresh();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SDK.PurchaseRemoveAds -= RemovePurseRemoveAds;
	}

	private void RemovePurseRemoveAds(bool state)
	{
		this.advertising.SetActive(false);
	}
	private void _Refresh()
	{
		bool mute = FSoundManager.IsSoundMute;
		soundoff.SetActive(mute);
		soundon.SetActive(!mute);

		bool music_mute = FSoundManager.IsMusicMute;
		musicoff.SetActive(music_mute);
		musicon.SetActive(!music_mute);
		this.advertising.SetActive(!GameData.GetPursedRemoveAds());	
	}
	private void _OnClose(GameObject obj)
	{
		UIManager.Instance.Hide(Info);
	}
	private void _OnAdvertising(GameObject obj)
	{
		SDK.Instance.OnPurchaseClicked(SDK.Instance.AppPurchaseNoAds);
	}

	private void _OnResorePurchase(GameObject obj)
	{
		SDK.Instance.RestorePurchases();
	}

	private void _OnSound(GameObject obj)
	{
		bool mute = FSoundManager.IsSoundMute;
		FSoundManager.IsSoundMute = !mute;
		soundoff.SetActive(!mute);
		soundon.SetActive(mute);
	}

	private void _OnMusic(GameObject obj)
	{
		bool mute = FSoundManager.IsMusicMute;
		musicoff.SetActive(!mute);
		musicon.SetActive(mute);
		FSoundManager.IsMusicMute = !mute;
	}
	
}
