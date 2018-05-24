using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class localizationText : Text {
	[HideInInspector]
	public string[] localization_languages;
	public static SystemLanguage lange_type;

	protected override void Awake()
	{
		base.Awake();
		if(lange_type == SystemLanguage.Unknown)
		{
			localizationText.lange_type = Application.systemLanguage;
		}
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		SetText();
	}

	public override void SetLayoutDirty()
	{
		base.SetLayoutDirty();
#if UNITY_EDITOR
		SetText();
#endif
	}

	private void SetText()
	{
#if UNITY_EDITOR
		//if(localization_languages != null && localization_languages.Length>=2)
		//{
		//	if (Application.systemLanguage == SystemLanguage.Chinese)
		//	{
		//		this.m_Text = localization_languages[0];
		//	}
		//	else if (Application.systemLanguage == SystemLanguage.English)
		//	{
		//		this.m_Text = localization_languages[1];
		//	}
		//}
#else
		if (lange_type == SystemLanguage.Chinese)
		{
			this.m_Text = localization_languages[0];
		}
		else
		{
			this.m_Text = localization_languages[1];
		}
#endif

	}
}
