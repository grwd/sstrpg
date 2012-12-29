using UnityEngine;
using System.Collections;
using System;

public class DepthSortingManager : MonoBehaviour
{
	private DepthSortObject[] _dsObjects;
	
	// Use this for initialization
	void Start ()
	{
		DepthSortObject[] dsObjects = GameObject.FindObjectsOfType(typeof(DepthSortObject)) as DepthSortObject[];
		
		_dsObjects = dsObjects;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Array.Sort(_dsObjects, delegate(DepthSortObject dso1, DepthSortObject dso2) {
					return dso1.transform.parent.position.z.CompareTo(dso2.transform.parent.position.z);
				});
		
		for (int i = 0; i < _dsObjects.Length; i++)
		{
			if (_dsObjects[i].IsEnvironment)
			{
				_dsObjects[i].transform.localPosition = _dsObjects[i].GetLocalStartingPos() + new Vector3(0, -i * 0.001f, 0);
			}
			else
			{
				_dsObjects[i].transform.localPosition = _dsObjects[i].GetLocalStartingPos() + new Vector3(0, -i * 0.001f, 0);
			}
		}
	}
}
