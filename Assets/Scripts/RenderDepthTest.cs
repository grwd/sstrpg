using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderDepthTest : MonoBehaviour
{
	public int Depth;
	
	public Transform MovingTransform;
	
	private int _lastDepth;
	private float _startingZ;
	
	void Start ()
	{
		if (MovingTransform != null)
			_startingZ = MovingTransform.position.z;
		
		RecursiveDepth(transform);
		_lastDepth = Depth;
	}
	
	void Update ()
	{
		if (_lastDepth != Depth || MovingTransform != null)
		{
			RecursiveDepth(transform);
			_lastDepth = Depth;
		}
	}
	
	void RecursiveDepth(Transform transObj)
	{
		int zAdd = 0;
		
		if (MovingTransform != null)
		{
			zAdd = (int)(_startingZ - MovingTransform.position.z);
		}
		
		if (transObj.renderer != null)
				transObj.renderer.sharedMaterial.renderQueue = Depth + 3000 + zAdd;
		
		for (int i = 0; i < transObj.childCount; i++)
		{
			RecursiveDepth(transObj.GetChild(i));
		}
	}
}
