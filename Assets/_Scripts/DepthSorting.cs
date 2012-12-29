using UnityEngine;
using System.Collections;

public class DepthSorting : MonoBehaviour
{
	private Transform _player;
	private Transform _front;
	private Transform _back;
	
	private Vector3 _infrontPos;
	private Vector3 _behindPos;
	
	// Use this for initialization
	void Start ()
	{
		_infrontPos = new Vector3(transform.position.x, 15, transform.position.z);
		_behindPos = new Vector3(transform.position.x, 13, transform.position.z);
			
		TestController p = FindObjectOfType(typeof(TestController)) as TestController;
		_player = p.transform;
		
		for (int i = 0; i < transform.GetChildCount(); i++)
		{
			Transform wall = transform.GetChild(i);
			if (wall.name.Equals("Front"))
				_front = wall;
			else if (wall.name.Equals("Back"))
				_back = wall;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log("player z: " + _player.position.z + " | front z: " + _front.position.z);
		
		if (_player.position.z < _front.position.z)
		{
			transform.position = _behindPos;
		}
		else if (_player.position.z > _back.position.z)
		{
			transform.position = _infrontPos;
		}
		else if (_player.position.x < transform.position.x)
		{
			transform.position = _behindPos;
		}
		else
		{
			transform.position = _infrontPos;
		}
	}
}
