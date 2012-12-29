using UnityEngine;
using System.Collections;

public class DepthSortObject : MonoBehaviour
{
	public bool IsEnvironment;
	
	private Vector3 _startingPos;
	
	void Start ()
	{
		_startingPos = transform.localPosition;
	}
	
	public Vector3 GetLocalStartingPos()
	{
		return _startingPos;
	}
}
