using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class DieJiaControl : MonoBehaviour
{
	public GameObject preKuaiImage;

	public GameObject preNewKuaiImage;

	[HideInInspector]
	public List<ImageControl> imageList = new List<ImageControl>();

	public Transform panTransfrom;

	[HideInInspector]
	public List<GameObject> dianList = new List<GameObject>();

	[HideInInspector]
	public GameObject showTextureGo;

	[HideInInspector]
	public int newTextureWidth;

	[HideInInspector]
	public int newTextureHeight;

	private int halfNewTextureWidth;

	private int halfNewTextureHeight;

	private Texture2D texture;

	[HideInInspector]
	public Color[] newColors;

	//private List<Color> newColors;

	private int maxNewColorIndex;

	private int maxNewTextureX;

	private int maxNewTextureY;

	private Coroutine reDoDragEndCoroutine;

	private void Awake()
	{
		this.InitDaKuai();
	}

	private IEnumerator ReDoDragEnd(ImageControl ic)
	{
		yield return new WaitForSeconds(0.2f);
		this.DoDragEnd(ic, false);
		this.reDoDragEndCoroutine = null;
	}


	public void InitDaKuai()
	{
		this.showTextureGo = UnityEngine.Object.Instantiate<GameObject>(this.preNewKuaiImage, base.transform);
		this.showTextureGo.name = "showTextureGo";
		this.newTextureWidth = CommonConfiguration.Operational_Figure_Length;
		this.newTextureHeight = CommonConfiguration.Operational_Figure_Length;
		this.halfNewTextureWidth = this.newTextureWidth / 2;
		this.halfNewTextureHeight = this.newTextureHeight / 2;
		this.maxNewColorIndex = this.newTextureWidth * this.newTextureHeight - 1;
		this.maxNewTextureX = this.newTextureWidth - 2;
		this.maxNewTextureY = this.newTextureHeight - 2;
		this.showTextureGo.GetComponent<RectTransform>().sizeDelta =
			new Vector2((float)this.newTextureWidth, (float)this.newTextureHeight);
		//this.showTextureGo.transform.localPosition = GameScene.gameSceneInsta.caoZuoPanTransfrom.localPosition;
		this.showTextureGo.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		this.texture = new Texture2D(this.newTextureWidth, this.newTextureHeight);
		this.newColors = new Color[this.newTextureWidth * this.newTextureHeight];
		//this.newColors = new List<Color>();
		this.RefreshDaKuai();
	}

	public ImageControl CreateBaseImage(ImageData data ,int baseImageIndex, Vector2 position, bool isShiLi)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.preKuaiImage, base.transform);
		gameObject.name = baseImageIndex.ToString();
		ImageControl component = gameObject.GetComponent<ImageControl>();
		component.SetImageData(data);
		component.SetImage(baseImageIndex, this);
		gameObject.transform.localPosition = position;
		this.imageList.Add(component);
		this.DoGame();
		return component;
		//this.DoDragEnd(component, isShiLi);
	}

	private void InitColors()
	{
		foreach (ImageControl current in this.imageList)
		{
			current.InitImage();
		}
	}

	public void DoGame()
	{
		Profiler.BeginSample("-----clear newcolors");
		//for (int i = 0; i < this.newColors.Length; i++)
		//{
		//	this.newColors[i].a = 0;
		//}
		this.newColors = new Color[this.newTextureWidth * this.newTextureHeight];
		Profiler.EndSample();
		Profiler.BeginSample("--------dictionary");
		foreach (ImageControl current in this.imageList)
		{
			int show_texture_height = current.showImageTexture.height;
			int show_texture_width = current.showImageTexture.width;
			int current_half_height = current.halfHeight;
			int current_half_width = current.halfWidth;
			float currentlocalpositionx = current.transform.localPosition.x;
			float currentlocalpositiony = current.transform.localPosition.y;
			float showtexturelocalpositionx = this.showTextureGo.transform.localPosition.x;
			float showtexturelocalpositiony = this.showTextureGo.transform.localPosition.y;
			for (int i = 0; i < show_texture_width; i++)
			{
				for (int j = 0; j < show_texture_height; j++)
				{
					int num = j * show_texture_width + i;
					Color color = current.showImageColors[num];
					if (color.a > 0f)
					{
						int num2 = (int)currentlocalpositionx + i - current_half_width;
						int num3 = (int)currentlocalpositiony + j - current_half_height;
						num2 = num2 - (int)showtexturelocalpositionx + this.halfNewTextureWidth;
						num3 = num3 - (int)showtexturelocalpositiony + this.halfNewTextureHeight;
						int num4 = num3 * this.newTextureWidth + num2;
						if (num2 >= 2 && num2 <= this.maxNewTextureX && num3 >= 2 && num3 <= this.maxNewTextureY)
						{
							if (num4 <= this.maxNewColorIndex)
							{
								this.newColors[num4].a = this.newColors[num4].a > 0f ? 0f : 255f;
							}
						}
					}
				}
			}
		}
		Profiler.EndSample();
		Profiler.BeginSample("-------");
		this.RefreshDaKuai();
		Profiler.EndSample();
	}

	//public void DoGame()
	//{
	//	this.newColors = new Color[this.newTextureWidth * this.newTextureHeight];
	//	Dictionary<int, byte> dictionary = new Dictionary<int, byte>();
	//	foreach (ImageControl current in this.imageList)
	//	{
	//		dictionary.Clear();
	//		for (int i = 0; i < current.showImageTexture.width; i++)
	//		{
	//			for (int j = 0; j < current.showImageTexture.height; j++)
	//			{
	//				int num = j * current.showImageTexture.width + i;
	//				Color color = current.showImageColors[num];
	//				if (color.a > 0f)
	//				{
	//					int num2 = (int)current.transform.localPosition.x + i - current.halfWidth;
	//					int num3 = (int)current.transform.localPosition.y + j - current.halfHeight;
	//					num2 = num2 - (int)this.showTextureGo.transform.localPosition.x + this.halfNewTextureWidth;
	//					num3 = num3 - (int)this.showTextureGo.transform.localPosition.y + this.halfNewTextureHeight;
	//					int num4 = num3 * this.newTextureWidth + num2;
	//					if (num2 >= 2 && num2 <= this.maxNewTextureX && num3 >= 2 && num3 <= this.maxNewTextureY)
	//					{
	//						if (num4 <= this.maxNewColorIndex)
	//						{
	//							if (!dictionary.ContainsKey(num4))
	//							{
	//								dictionary.Add(num4, 0);
	//								if (this.newColors[num4].a > 0f)
	//								{
	//									this.newColors[num4].a = 0f;
	//								}
	//								else
	//								{
	//									this.newColors[num4].a = 255f;
	//								}
	//							}
	//						}
	//					}
	//				}
	//			}
	//		}
	//	}
	//	Profiler.BeginSample("-------");
	//	this.RefreshDaKuai();
	//	Profiler.EndSample();
	//}

	public void RefreshDaKuai()
	{
		Profiler.BeginSample("-------SetPixels");
		this.texture.SetPixels(this.newColors);
		Profiler.EndSample();
		Profiler.BeginSample("--------Apply");
		this.texture.Apply();
		Profiler.EndSample();
		Profiler.BeginSample("-------Sprite.Create");
		this.showTextureGo.GetComponent<Image>().sprite = Sprite.Create(this.texture,
			new Rect(0f, 0f, (float)this.texture.width, (float)this.texture.height), Vector2.zero, 100F, 0, SpriteMeshType.FullRect);
		Profiler.EndSample();
	}

	public void DoDragEnd(ImageControl ic, bool isShiLi)
	{

		float num = 3.40282347E+38f;
		Vector3 localPosition = Vector3.zero;
		foreach (GameObject current in this.dianList)
		{
			float num2 = Vector3.Distance(ic.transform.localPosition, current.transform.localPosition);
			if (num2 < num)
			{
				num = num2;
				localPosition = current.transform.localPosition;
			}
		}
		if (ic.imageIndex == (int)ImageType.ParallelogramLong3 || ic.imageIndex == (int)ImageType.ParallelogramLong4
			|| ic.imageIndex == (int)ImageType.BigChangFangXing1
			|| ic.imageIndex == (int)ImageType.BigChangFangXing3
			|| ic.imageIndex == (int)ImageType.BigSangJiaoXingDao1
			|| ic.imageIndex == (int)ImageType.BigSangJiaoXingDao2
			)
		{
			if(ic.transform.localPosition.y > localPosition.y)
			{
				ic.transform.localPosition = new Vector3(localPosition.x, localPosition.y + CommonConfiguration.kuaiSize / 4, 0);
			}else
			{
				ic.transform.localPosition = new Vector3(localPosition.x, localPosition.y - CommonConfiguration.kuaiSize / 4, 0);
			}
			
		}
		else if (ic.imageIndex == (int)ImageType.ParallelogramLong1 || ic.imageIndex == (int)ImageType.ParallelogramLong2
			|| ic.imageIndex == (int)ImageType.BigChangFangXing2
			|| ic.imageIndex == (int)ImageType.BigChangFangXing4
			|| ic.imageIndex == (int)ImageType.BigSangJiaoXingDao3
			|| ic.imageIndex == (int)ImageType.BigSangJiaoXingDao4)
		{
			if (ic.transform.localPosition.x > localPosition.x)
			{
				ic.transform.localPosition = new Vector3(localPosition.x + CommonConfiguration.kuaiSize / 4, localPosition.y, 0);
			}
			else
			{
				ic.transform.localPosition = new Vector3(localPosition.x - CommonConfiguration.kuaiSize / 4, localPosition.y, 0);
			}
		}
		else if(ic.imageIndex == (int)ImageType.XiaoSangJiaoXingSS3
			|| ic.imageIndex == (int)ImageType.XiaoSangJiaoXingSS4
			|| ic.imageIndex == (int)ImageType.XiaoSangJiaoXingSS1
			|| ic.imageIndex == (int)ImageType.XiaoSangJiaoXingSS2)
		{
			float ic_local_x = ic.transform.localPosition.x;
			if (ic.transform.localPosition.y > localPosition.y)
			{
				ic.transform.localPosition = new Vector3(localPosition.x, localPosition.y + CommonConfiguration.kuaiSize / 4, 0);
			}
			else
			{
				ic.transform.localPosition = new Vector3(localPosition.x, localPosition.y - CommonConfiguration.kuaiSize / 4, 0);
			}
			if (ic_local_x > localPosition.x)
			{
				if (ic_local_x > localPosition.x + CommonConfiguration.kuaiSize / 4)
				{
					ic.transform.localPosition = new Vector3(localPosition.x - CommonConfiguration.kuaiSize / 4, ic.transform.localPosition.y, 0);
				}
				else
				{
					ic.transform.localPosition = new Vector3(localPosition.x + CommonConfiguration.kuaiSize / 4, ic.transform.localPosition.y, 0);
				}

			}
			else
			{
				if (ic_local_x > localPosition.x - CommonConfiguration.kuaiSize / 4)
				{
					ic.transform.localPosition = new Vector3(localPosition.x - CommonConfiguration.kuaiSize / 4, ic.transform.localPosition.y, 0);
				}
				else
				{
					ic.transform.localPosition = new Vector3(localPosition.x + CommonConfiguration.kuaiSize / 4, ic.transform.localPosition.y, 0);
				}
			}
		}
		else
		{
			ic.transform.localPosition = localPosition;
		}
		
		this.DoGame();
		if (!isShiLi && !this.DoGameEnd() && this.reDoDragEndCoroutine == null)
		{
			// ´ý¿¼ÂÇ
			this.reDoDragEndCoroutine = base.StartCoroutine(this.ReDoDragEnd(ic));
		}
	}

	public bool DoGameEnd()
	{
		int num = -2147483648;
		List<int> list = new List<int>();
		for (int i = 0; i < GameScene.Instance.Operational_Figure_Control.newColors.Length; i++)
		{
			if (GameScene.Instance.Operational_Figure_Control.newColors[i].a == 255f)
			{
				if (num == -2147483648)
				{
					num = i;
				}

				list.Add(i - num);
			}
		}

		int num2 = -2147483648;
		List<int> list2 = new List<int>();
		for (int j = 0; j < GameScene.Instance.Fixed_Figure_Control.newColors.Length; j++)
		{
			if (GameScene.Instance.Fixed_Figure_Control.newColors[j].a == 255f)
			{
				if (num2 == -2147483648)
				{
					num2 = j;
				}

				list2.Add(j - num2);
			}
		}

		if (list.Count != list2.Count)
		{
			return false;
		}

		for (int k = 0; k < list.Count; k++)
		{
			if (list[k] != list2[k])
			{
				return false;
			}
		}
		GameControl.Instance.DoGameOver();
		return true;
	}
}
