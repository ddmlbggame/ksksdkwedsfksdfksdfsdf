using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonConfiguration
{
	public const bool isEditMode = false;

	public static int DotDistance = 25;

	public const int kuaiSize = 100;

	public const int kuaiBianSize = 0;

	public static int Operational_Figure_Length = 750;

	public static Dictionary<int, Dictionary<int, Vector3>> gameLevelPostions = new Dictionary<int, Dictionary<int, Vector3>>();

	public static Dictionary<int, BaseImage> baseImages = new Dictionary<int, BaseImage>();

	public static int maxLevel = 0;

	public const char splitChar = ',';

	public const string showTextureGoName = "showTextureGo";

	public static int currentLevel;

	public const string Current_Level_Name = "CurrentLeveL";
	public static void InitGameData()
	{
		if (PlayerPrefs.HasKey(Current_Level_Name))
		{
			CommonConfiguration.currentLevel = PlayerPrefs.GetInt(Current_Level_Name);
		}
		else
		{
			CommonConfiguration.currentLevel = 1;
		}
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_2, new BaseImage(ImageType.ZhengFangXing_2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize));
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_3, new BaseImage(ImageType.ZhengFangXing_3, 3*kuaiSize/2 + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize));
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_5, new BaseImage(ImageType.ZhengFangXing_5, 5 * kuaiSize / 2 + kuaiBianSize * 2, 5 * kuaiSize / 2 + kuaiBianSize));
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_6, new BaseImage(ImageType.ZhengFangXing_6, 6 * kuaiSize / 2 + kuaiBianSize * 2, 6 * kuaiSize / 2 + kuaiBianSize));
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_7, new BaseImage(ImageType.ZhengFangXing_7, 7 * kuaiSize / 2 + kuaiBianSize * 2, 7 * kuaiSize / 2 + kuaiBianSize));
		CommonConfiguration.baseImages.Add((int)ImageType.SanJiaoXing_U_2, new BaseImage(ImageType.SanJiaoXing_U_2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SanJiaoXing_L_2, new BaseImage(ImageType.SanJiaoXing_L_2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SanJiaoXing_D_2, new BaseImage(ImageType.SanJiaoXing_D_2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SanJiaoXing_R_2, new BaseImage(ImageType.SanJiaoXing_R_2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SangJiaoXing_U_4, new BaseImage(ImageType.SangJiaoXing_U_4, 2 * kuaiSize + kuaiBianSize * 2, 2 * kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SangJiaoXing_D_4, new BaseImage(ImageType.SangJiaoXing_D_4, 2 * kuaiSize + kuaiBianSize * 2, 2 * kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SangJiaoXing_L_4, new BaseImage(ImageType.SangJiaoXing_L_4, 2 * kuaiSize + kuaiBianSize * 2, 2 * kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.SangJiaoXing_R_4, new BaseImage(ImageType.SangJiaoXing_R_4, 2 * kuaiSize + kuaiBianSize * 2, 2 * kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.LingXing, new BaseImage(ImageType.LingXing, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigLingXing1, new BaseImage(ImageType.BigLingXing1, 3*kuaiSize + kuaiBianSize * 2, 3 * kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ChangFangXing1, new BaseImage(ImageType.ChangFangXing1, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ChangFangXing2, new BaseImage(ImageType.ChangFangXing2, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ChangFangXingLeft1, new BaseImage(ImageType.ChangFangXingLeft1, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ChangFangXingRight1, new BaseImage(ImageType.ChangFangXingRight1, kuaiSize + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ZhengFangXing_4, new BaseImage(ImageType.ZhengFangXing_4, kuaiSize * 2 + kuaiBianSize * 2, kuaiSize * 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.Parallelogram1, new BaseImage(ImageType.Parallelogram1, 4*kuaiSize/2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.Parallelogram2, new BaseImage(ImageType.Parallelogram2, 4 * kuaiSize / 2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.Parallelogram3, new BaseImage(ImageType.Parallelogram3, kuaiSize + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.Parallelogram4, new BaseImage(ImageType.Parallelogram4, kuaiSize + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.ParallelogramLong1, new BaseImage(ImageType.ParallelogramLong1, 5 * kuaiSize / 2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ParallelogramLong2, new BaseImage(ImageType.ParallelogramLong2, 5 * kuaiSize / 2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ParallelogramLong3, new BaseImage(ImageType.ParallelogramLong3, kuaiSize + kuaiBianSize * 2, 5*kuaiSize/2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.ParallelogramLong4, new BaseImage(ImageType.ParallelogramLong4, kuaiSize  + kuaiBianSize * 2, 5*kuaiSize/2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.TiXing1, new BaseImage(ImageType.TiXing1, 4 * kuaiSize / 2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.TiXing2, new BaseImage(ImageType.TiXing2, 4 * kuaiSize / 2 + kuaiBianSize * 2, kuaiSize + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.TiXing3, new BaseImage(ImageType.TiXing3, kuaiSize + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.TiXing4, new BaseImage(ImageType.TiXing4, kuaiSize + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.BigChangFangXing1, new BaseImage(ImageType.BigChangFangXing1, 4*kuaiSize/2 + kuaiBianSize * 2, 3*kuaiSize/2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigChangFangXing2, new BaseImage(ImageType.BigChangFangXing2, 3 * kuaiSize / 2 + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.BigChangFangXing3, new BaseImage(ImageType.BigChangFangXing3, kuaiSize + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigChangFangXing4, new BaseImage(ImageType.BigChangFangXing4, 3 * kuaiSize / 2 + kuaiBianSize * 2,  kuaiSize + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.BigSangJiaoXingDao1, new BaseImage(ImageType.BigSangJiaoXingDao1, 6 * kuaiSize / 2 + kuaiBianSize * 2, 3*kuaiSize/2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigSangJiaoXingDao2, new BaseImage(ImageType.BigSangJiaoXingDao2, 6 * kuaiSize / 2 + kuaiBianSize * 2, 3 * kuaiSize/2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigSangJiaoXingDao3, new BaseImage(ImageType.BigSangJiaoXingDao3, 3 * kuaiSize / 2 + kuaiBianSize * 2, 6 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.BigSangJiaoXingDao4, new BaseImage(ImageType.BigSangJiaoXingDao4, 3 * kuaiSize / 2 + kuaiBianSize * 2, 6 * kuaiSize / 2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingDao1, new BaseImage(ImageType.XiaoSangJiaoXingDao1, 4 * kuaiSize / 2 + kuaiBianSize * 2, 2 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingDao2, new BaseImage(ImageType.XiaoSangJiaoXingDao2, 4 * kuaiSize / 2 + kuaiBianSize * 2, 2 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingDao3, new BaseImage(ImageType.XiaoSangJiaoXingDao3, 2 * kuaiSize / 2 + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingDao4, new BaseImage(ImageType.XiaoSangJiaoXingDao4, 2 * kuaiSize / 2 + kuaiBianSize * 2, 4 * kuaiSize / 2 + kuaiBianSize * 2));

		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingSS1, new BaseImage(ImageType.XiaoSangJiaoXingSS1, 3*kuaiSize/2 + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingSS2, new BaseImage(ImageType.XiaoSangJiaoXingSS2, 3 * kuaiSize / 2 + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingSS3, new BaseImage(ImageType.XiaoSangJiaoXingSS3, 3 * kuaiSize / 2 + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize * 2));
		CommonConfiguration.baseImages.Add((int)ImageType.XiaoSangJiaoXingSS4, new BaseImage(ImageType.XiaoSangJiaoXingSS4, 3 * kuaiSize / 2 + kuaiBianSize * 2, 3 * kuaiSize / 2 + kuaiBianSize * 2));
	}

	public static Texture2D CreateTexture(BaseImage bi)
	{
		Texture2D texture2D = new Texture2D(bi.imageWidth, bi.imageHeight);
		texture2D.SetPixels(CommonConfiguration.CreateBaseImage(texture2D.width, texture2D.height, bi.baseImageType));
		texture2D.Apply();
		return texture2D;
	}


	public static Color[] CreateBaseImage(int imageWidth, int imageHeight, ImageType baseIMageType)
	{
		Color[] array = new Color[imageWidth * imageHeight];
		for (int i = 0; i < imageWidth; i++)
		{
			for (int j = 0; j < imageHeight; j++)
			{
				int num = j * imageWidth + i;
				if (CommonConfiguration.IsShowPoint(imageWidth, imageHeight, i, j, baseIMageType))
				{
					array[num] = new Color(0f, 0f, 0f, 1f);
				}
				else
				{
					array[num] = new Color(0f, 0f, 0f, 0f);
				}
			}
		}
		return array;
	}

	public static bool IsShowPoint(int imageWidth, int imageHeight, int x, int y, ImageType imageType)
	{
		int num = y * imageWidth + x;
		if (x >= imageWidth - kuaiBianSize || y >= imageHeight - kuaiBianSize || x < kuaiBianSize || y < kuaiBianSize)
		{
			return false;
		}
		switch (imageType)
		{
			case ImageType.ZhengFangXing_2:
			case ImageType.ZhengFangXing_3:
			case ImageType.ZhengFangXing_4:
			case ImageType.ZhengFangXing_5:
			case ImageType.ZhengFangXing_6:
			case ImageType.ZhengFangXing_7:
			case ImageType.BigChangFangXing1:
			case ImageType.BigChangFangXing2:
			case ImageType.BigChangFangXing3:
			case ImageType.BigChangFangXing4:
				return true;
			case ImageType.SanJiaoXing_U_2:
			case ImageType.SangJiaoXing_U_4:
			case ImageType.XiaoSangJiaoXingSS1:
				if (y >= x)
				{
					return true;
				}
				break;
			case ImageType.SanJiaoXing_D_2:
			case ImageType.SangJiaoXing_D_4:
			case ImageType.XiaoSangJiaoXingSS2:
				if (y < x)
				{
					return true;
				}
				break;
			case ImageType.SanJiaoXing_L_2:
			case ImageType.SangJiaoXing_L_4:
			case ImageType.XiaoSangJiaoXingSS3:
				if (y <= -x + imageWidth)
				{
					return true;
				}
				break;
			case ImageType.SanJiaoXing_R_2:
			case ImageType.SangJiaoXing_R_4:
			case ImageType.XiaoSangJiaoXingSS4:
				if (y > -x + imageWidth)
				{
					return true;
				}
				break;
			case ImageType.ChangFangXing1:
				if (y > imageHeight / 4 && y < 3 * imageHeight / 4 - 1)
				{
					return true;
				}
				break;
			case ImageType.ChangFangXing2:
				if (x > imageWidth / 4 && x < 3 * imageWidth / 4 - 1)
				{
					return true;
				}
				break;
			case ImageType.ChangFangXingLeft1:
				if (x <imageWidth/2)
				{
					return true;
				}
				break;
			case ImageType.ChangFangXingRight1:
				if (x > imageWidth / 2)
				{
					return true;
				}
				break;
			case ImageType.LingXing:
			case ImageType.BigLingXing1:
				if ((x <= imageWidth / 2 && y <= imageHeight / 2 && x > imageWidth / 2 - y + 2) || (x >= imageWidth / 2 && y <= imageHeight / 2 && x <= y + imageWidth / 2 - 2) || (x <= imageWidth / 2 && y >= imageHeight / 2 && x > y - imageWidth / 2 + 2) || (x >= imageWidth / 2 && y >= imageHeight / 2 && x <= 3 * imageWidth / 2 - y - 2))
				{
					return true;
				}
				break;
			case ImageType.Parallelogram1:
				if(x>imageWidth/4 && x < (3*imageWidth / 4)){
					return true;
				}
				if(x>y/2 && x<= imageWidth / 4)
				{
					return true;
				}
				if (x- (3 * imageWidth / 4) < y/2 && x >= (3 * imageWidth / 4))
				{
					return true;
				}
				break;
			case ImageType.Parallelogram2:
				if (x > imageWidth / 4 && x < (3 * imageWidth / 4))
				{
					return true;
				}
				if (x>-y/2+ imageWidth / 4 && x <= imageWidth / 4)
				{
					return true;
				}
				if (x < -y / 2 +imageWidth && x >= (3 * imageWidth / 4))
				{
					return true;
				}
				break;

			case ImageType.Parallelogram3:
				if(y>=imageHeight/4 && y <= 3 * imageHeight / 4)
				{
					return true;
				}
				if (y > x / 2 && y <= imageHeight / 4)
				{
					return true;
				}
				if (y - (3 * imageHeight / 4) < x / 2 && y >= (3 * imageHeight / 4))
				{
					return true;
				}
				break;

			case ImageType.Parallelogram4:
				if (y > imageHeight / 4 && y < (3 * imageHeight / 4))
				{
					return true;
				}
				if (y>-x/2 + imageHeight / 4 && y <= imageHeight / 4)
				{
					return true;
				}
				if(y<-x/2 +imageHeight && y >= (3 * imageHeight / 4))
				{
					return true;
				}
				break;

			case ImageType.ParallelogramLong1:
				if (x > 2*imageWidth / 5 && x < (3 * imageWidth / 5))
				{
					return true;
				}
				if (x > y && x <= 2 * imageWidth / 5)
				{
					return true;
				}
				if (x - (3 * imageWidth / 5) < y && x >= (3 * imageWidth / 5))
				{
					return true;
				}
				break;

			case ImageType.ParallelogramLong2:
				if (x > 2 * imageWidth / 5 && x < (3 * imageWidth / 5))
				{
					return true;
				}
				if (x > -y + 2 * imageWidth / 5 && x <= 2 * imageWidth / 5)
				{
					return true;
				}
				if (x < -y + imageWidth && x >= (3 * imageWidth / 5))
				{
					return true;
				}
				break;

			case ImageType.ParallelogramLong3:
				if (y >= 2 * imageHeight / 5 && y <= (3 * imageHeight / 5))
				{
					return true;
				}
				if (y > x && y <= 2 * imageHeight / 5)
				{
					return true;
				}
				if (y - (3 * imageHeight / 5) < x && y >= (3 * imageHeight / 5))
				{
					return true;
				}
				break;

			case ImageType.ParallelogramLong4:
				if (y > 2 * imageHeight / 5 && y < (3 * imageHeight / 5))
				{
					return true;
				}


				if (y > -x + 2 * imageHeight / 5 && y <= 2 * imageHeight / 5)
				{
					return true;
				}
				if (y < -x + imageHeight && y >= (3 * imageHeight / 5))
				{
					return true;
				}
				break;


			case ImageType.TiXing1:
				if (x > imageWidth / 4)
				{
					return true;
				}
				if (x > y / 2 && x <= imageWidth / 4)
				{
					return true;
				}
				break;
			case ImageType.TiXing2:
				if (x < (3 * imageWidth / 4))
				{
					return true;
				}
				if (x < -y / 2 + imageWidth && x >= (3 * imageWidth / 4))
				{
					return true;
				}
				break;

			case ImageType.TiXing3:
				if (y >= imageHeight / 4 )
				{
					return true;
				}
				if (y > x / 2 && y <= imageHeight / 4)
				{
					return true;
				}
				break;

			case ImageType.TiXing4:
				if (y < (3 * imageHeight / 4))
				{
					return true;
				}
				//if (y > -x / 2 + imageHeight / 4 && y <= imageHeight / 4)
				//{
				//	return true;
				//}
				if (y < -x / 2 + imageHeight && y >= (3 * imageHeight / 4))
				{
					return true;
				}
				break;

			case ImageType.BigSangJiaoXingDao1:
			case ImageType.XiaoSangJiaoXingDao1:
				if (x<imageWidth/2&& y-imageHeight >- x)
				{
					return true;
				}
				if (x >= imageWidth / 2 && y >x-imageWidth/2 )
				{
					return true;
				}
				break;
			case ImageType.BigSangJiaoXingDao2:
			case ImageType.XiaoSangJiaoXingDao2:
				if (x<imageWidth/2 && y < x)
				{
					return true;
				}
				if(x>=imageWidth/2 && y < -x + imageWidth)
				{
					return true;
				}
				break;
			case ImageType.BigSangJiaoXingDao3:
			case ImageType.XiaoSangJiaoXingDao3:
				if (y < imageHeight / 2 && x - imageWidth > -y)
				{
					return true;
				}
				if (y >= imageHeight / 2 && x > y - imageHeight / 2)
				{
					return true;
				}
				break;
			case ImageType.BigSangJiaoXingDao4:
			case ImageType.XiaoSangJiaoXingDao4:
				if (y < imageHeight / 2 && x < y)
				{
					return true;
				}
				if (y >= imageHeight / 2 && x < -y + imageHeight)
				{
					return true;
				}
				break;
		}
		return false;
	}

}
