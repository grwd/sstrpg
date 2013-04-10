using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour
{
	public Transform FollowObject;
	public bool OnlyFollowZ;
	
	private Vector3 _startPos;
	
	void Start ()
	{
		_startPos = transform.position - FollowObject.position;
	}
	
	void Update ()
	{
		if (OnlyFollowZ)
			transform.position = new Vector3(_startPos.x, _startPos.y, FollowObject.position.z + _startPos.z);
		else
			transform.position = FollowObject.position + _startPos;
	}
}
