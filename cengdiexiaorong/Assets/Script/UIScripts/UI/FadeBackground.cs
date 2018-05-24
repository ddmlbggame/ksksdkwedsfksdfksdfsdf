using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeBackground : MonoBehaviour {
    private bool m_IsFadeIn;
    private bool m_IsFadeOut;
    private float m_ElapseTime;
    public float m_FadeTime;
    public bool m_IsBlack = false;
    // Use this for initialization
    void Start ()
    {
        Reset(true);
    }
	void OnDisable()
    {
        if (m_IsFadeOut)
        {
            m_IsBlack = false;
            GetComponent<RawImage>().color = new Vector4(0, 0, 0, 0);
        }
        else if (m_IsFadeIn)
        {
            m_IsBlack = true;
            GetComponent<RawImage>().color = new Vector4(0, 0, 0, 1);
        }
        Reset(false);
    }
    /// <summary>
    /// 从黑屏渐变至正常屏幕
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadingOut());
    }
    public IEnumerator FadingOut()
    {
        m_ElapseTime = 0;
        m_IsFadeIn = false;
        m_IsFadeOut = true;
        GetComponent<RawImage>().color = new Vector4(0, 0, 0, 1);

        while (m_IsFadeOut)
        {
            m_ElapseTime = (m_ElapseTime + Time.deltaTime) < m_FadeTime ? (m_ElapseTime + Time.deltaTime) : m_FadeTime;
            GetComponent<RawImage>().color = Vector4.Lerp(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 0), m_ElapseTime / m_FadeTime);
            if (m_ElapseTime >= m_FadeTime)
            {
                m_IsBlack = false;
                m_IsFadeOut = false;
            }
            yield return 1;
        }


    }
    /// <summary>
    /// 从正常屏幕渐变至黑屏
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(FadingIn());
    }
    public IEnumerator FadingIn()
    {
        m_ElapseTime = 0;
        m_IsFadeIn = true;
        m_IsFadeOut = false;
        m_IsBlack = true;
        GetComponent<RawImage>().color = new Vector4(0, 0, 0, 0);
        while (m_IsFadeIn)
        {
            m_ElapseTime = (m_ElapseTime + Time.deltaTime) < m_FadeTime ? (m_ElapseTime + Time.deltaTime) : m_FadeTime;
            GetComponent<RawImage>().color = Vector4.Lerp(new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 1), m_ElapseTime / m_FadeTime);
            if (m_ElapseTime >= m_FadeTime)
            {
                m_IsBlack = true;
                m_IsFadeIn = false;
            }
            yield return 1;
        }
    }
    public void Reset(bool resetColor)
    {
        m_ElapseTime = 0;
        m_IsFadeIn = false;
        m_IsFadeOut = false;
        if (resetColor)
        {
            m_IsBlack = false;
            GetComponent<RawImage>().color = new Vector4(0, 0, 0, 0);
        }
    }

    public void BlackImage(bool isShow)
    {
        BlackingImage(isShow);
    }

    private void BlackingImage(bool isShow)
    {
        gameObject.SetActive(isShow);
        gameObject.transform.localPosition = Vector3.one;
        if (isShow)
        {
            GetComponent<RawImage>().color = new Vector4(0, 0, 0, 1);
        }
        else
        {
            GetComponent<RawImage>().color = new Vector4(0, 0, 0, 0);
        }
    }

    //private IEnumerator BlackingForSecond(float seconds)
    //{
    //    m_IsBlack = true;
    //    GetComponent<RawImage>().color = new Vector4(0, 0, 0, 1);

    //    yield return new WaitForSeconds(seconds);

    //    GetComponent<RawImage>().color = new Vector4(0, 0, 0, 0);
    //    m_IsBlack = false;

    //}
}
