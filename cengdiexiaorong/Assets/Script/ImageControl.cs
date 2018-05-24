using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

[System.Serializable]
public class ImageControl : MonoBehaviour
{
	public Image image;

	private Vector2 deltaPosition = Vector2.zero;

	[HideInInspector]
	public int imageIndex;

	private Texture2D baseImagetexture;

	[HideInInspector]
	public Texture2D showImageTexture;

	[HideInInspector]
	public Color[] showImageColors;

	[HideInInspector]
	public int halfWidth;

	[HideInInspector]
	public int halfHeight;

	private DieJiaControl dieJiaControl;

	public ImageData image_data;

	public void SetImageData(ImageData data)
	{
		this.image_data = data;
	}

	public void SetImage(int imageIndex, DieJiaControl dieJiaControl)
	{
		this.imageIndex = imageIndex;
		this.dieJiaControl = dieJiaControl;
		this.baseImagetexture = CommonConfiguration.CreateTexture(CommonConfiguration.baseImages[imageIndex]);
		this.halfWidth = this.baseImagetexture.width / 2;
		this.halfHeight = this.baseImagetexture.height / 2;
		this.InitImage();
	}

	public void InitImage()
	{
		this.showImageTexture = new Texture2D(this.baseImagetexture.width, this.baseImagetexture.height);
		this.showImageTexture.SetPixels(this.baseImagetexture.GetPixels());
		this.showImageTexture.Apply();
		this.showImageColors = this.showImageTexture.GetPixels();
		base.GetComponent<RectTransform>().sizeDelta = new Vector2((float)this.showImageTexture.width, (float)this.showImageTexture.height);
		base.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
	}

	public void RefreshTexture()
	{
		this.showImageTexture.SetPixels(this.showImageColors);
		this.showImageTexture.Apply();
	}

	public void OnPointerDown()
	{
		this.StartDragPosition = base.transform.localPosition;
		base.transform.SetAsLastSibling();
		Vector2 one = Vector2.one;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GameScene.Instance.Operational_Figure, Input.mousePosition, GameScene.Instance.canvas.worldCamera, out one);
		this.deltaPosition = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y) - one;
	}

	private int frame_count;

	private Vector2 _last_input_position = Vector2.zero;

	private Vector2 _drag_delta = Vector2.zero;

	public Vector3 StartDragPosition;

	public void OnDragIng(Vector2 input_position)
	{
		//if (frame_count % 2 != 0)
		//{
		//	frame_count++;
		//	return;
		//}
		//frame_count++;
		Vector2 one = Vector2.one;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GameScene.Instance.Operational_Figure, input_position, GameScene.Instance.canvas.worldCamera, out one);
		if (_last_input_position == Vector2.zero)
		{
			_last_input_position = one;
			_drag_delta = Vector2.zero;
		}
		else
		{
			_drag_delta = one - _last_input_position;
			_last_input_position = one;
		}
		//Vector2 one = Vector2.one;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle(GameScene.gameSceneInsta.canvas.transform as RectTransform, Input.mousePosition, GameScene.gameSceneInsta.canvas.worldCamera, out one);
		//base.transform.localPosition = one + this.deltaPosition;
		base.transform.localPosition += new Vector3(_drag_delta.x, _drag_delta.y,0);
		Profiler.BeginSample("----------DoGame");
		this.dieJiaControl.DoGame();
		Profiler.EndSample();
	}

	public void DragToPos(Vector3 positon)
	{
		base.transform.localPosition = positon;
		this.dieJiaControl.DoGame();
	}

	public void OnDragEnd()
	{
		if(base.transform.localPosition.x <-CommonConfiguration.Operational_Figure_Length/2+ this.halfWidth || base.transform.localPosition.x>CommonConfiguration.Operational_Figure_Length/2- this.halfWidth
		|| base.transform.localPosition.y < -CommonConfiguration.Operational_Figure_Length/2+ halfHeight || base.transform.localPosition.y > CommonConfiguration.Operational_Figure_Length/2 - halfHeight)
		{
			base.transform.localPosition = this.StartDragPosition;
			this.dieJiaControl.DoGame();
			_last_input_position = Vector2.zero;
			_drag_delta = Vector2.zero;
			return;
		}
		_last_input_position = Vector2.zero;
		_drag_delta = Vector2.zero;
		this.dieJiaControl.DoDragEnd(this, false);
	}

}
