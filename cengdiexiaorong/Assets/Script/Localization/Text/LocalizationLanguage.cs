using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationLanguage :MonoBehaviour {

	[HideInInspector]
	public string[] localization_languages;

	public LanguageType language_type = LanguageType.Chinese;

	public static SystemLanguage lange_type = SystemLanguage.Unknown;

	private Text text;

	public void Awake()
	{
		localizationText.lange_type = Application.systemLanguage;
		if (text == null)
		{
			text = this.GetComponent<Text>();
		}
		SetText();
	}

	public void SetText()
	{
		if(this.text == null)
		{
			text = this.GetComponent<Text>();
		}
#if UNITY_EDITOR
		if (localization_languages != null && localization_languages.Length >= 2)
		{
			if (language_type == LanguageType.Chinese)
			{
				this.text.text = localization_languages[0];
			}
			else
			{
				this.text.text = localization_languages[1];
			}
		}
#else
		if (localization_languages != null && localization_languages.Length >= 2)
		{
			if (lange_type == SystemLanguage.Chinese)
			{
				this.text.text = localization_languages[0];
			}
			else
			{
				this.text.text = localization_languages[1];
			}
		}
#endif

	}
}

public enum LanguageType
{
	Chinese,
	English,
}
