using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour
{
	public Transform FollowObject;
	
	private Vector3 _startPos;
	
	void Start ()
	{
		_startPos = transform.position - FollowObject.position;
	}
	
	void Update ()
	{
		transform.position = FollowObject.position + _startPos;
	}
}
