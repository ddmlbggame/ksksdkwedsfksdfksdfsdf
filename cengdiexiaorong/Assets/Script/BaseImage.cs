using System;
public class BaseImage
{
	public ImageType baseImageType;

	public int imageWidth;

	public int imageHeight;

	public BaseImage(ImageType baseImageType, int imageWidth, int imageHeight)
	{
		this.baseImageType = baseImageType;
		this.imageWidth = imageWidth;
		this.imageHeight = imageHeight;
	}
}
