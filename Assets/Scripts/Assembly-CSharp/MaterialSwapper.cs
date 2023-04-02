using System;
using UnityEngine;

[Serializable]
public class MaterialSwapper
{
	public Material Mat;

	public Shader Low;

	public Shader Med;

	public Shader High;

	public void SetShader(int level)
	{
		switch (level)
		{
		case 0:
			Mat.shader = Low;
			break;
		case 1:
			Mat.shader = Med;
			break;
		case 2:
			Mat.shader = High;
			break;
		default:
			Mat.shader = Low;
			break;
		}
	}
}
