using UnityEngine;
using System.Collections;

public class UVmover : MonoBehaviour
{
	public Vector2 OffSet;
	
	void Update ()
	{
		renderer.material.mainTextureOffset += OffSet;
	}
}
