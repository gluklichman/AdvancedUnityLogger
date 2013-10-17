using System;
using System.Collections.Generic;
using UnityEngine;

public class TexturesGenerator
{
	public static Texture2D GenerateMonotonicTexture(Color textureColor)
	{
		Texture2D texture = new Texture2D(1,1);
		texture.SetPixel(0,0, textureColor);
		texture.Apply();
		return texture;
	}
}


