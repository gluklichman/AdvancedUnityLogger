using System;
using System.Collections.Generic;
using UnityEngine;

public class TexturesGenerator
{
	//private static Dictionary<Color, Texture2D> _monotonicTextures = new Dictionary<Color, Texture2D>();
	//private static TexturesGenerator _instance;
	
	/*public static TexturesGenerator Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TexturesGenerator();
			}
			return _instance;
		}
	}
	
	private TexturesGenerator()
	{}*/
	
	public static Texture2D GenerateMonotonicTexture(Color textureColor)
	{
		//if (_monotonicTextures.ContainsKey(textureColor))
		//	return _monotonicTextures[textureColor];
		//else
		//{
			//generate texture
			Texture2D texture = new Texture2D(1,1);
			texture.SetPixel(0,0, textureColor);
			//texture.SetPixel(0,1, textureColor);
			//texture.SetPixel(1,0, textureColor);
			//texture.SetPixel(1,1, textureColor);
			texture.Apply();
		//	_monotonicTextures.Add(textureColor, texture);
			return texture;
		//}
	}
}


