using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class WaypointController : MonoBehaviour
{
	public bool AddingWaypoints;
	
	[SerializeField]
	public List<Waypoint> Waypoints;
	
	public Waypoint HelperPoint;
	
	public float Speed = 1f;
	
	private int _currentWpIndex;
	
	void Update ()
	{
		if (Application.isPlaying)
		{
			if (_currentWpIndex < Waypoints.Count)
			{
				transform.position = Vector3.MoveTowards(transform.position, Waypoints[_currentWpIndex].Position, Speed * Time.deltaTime);
				
				if (transform.position == Waypoints[_currentWpIndex].Position)
				{
					_currentWpIndex++;
				}
			}
		}
	}
	
	void OnRenderObject ()
	{
		if (AddingWaypoints)
		{
			Selection.activeTransform = transform;
		}
	}
	
	void OnDrawGizmosSelected ()
	{
		if (Waypoints != null)
		{
			Gizmos.color = Color.yellow;
			
			for (int i = 0; i < Waypoints.Count; i++)
			{
				Gizmos.DrawSphere(Waypoints[i].Position, 0.2f);
				TextGizmo.Instance.DrawText(SceneView.lastActiveSceneView.camera, Waypoints[i].Position, i);
				
				if (i > 0)
				{
					Gizmos.DrawLine(Waypoints[i].Position, Waypoints[i - 1].Position);
				}
				else if (!Application.isPlaying || _currentWpIndex == 0)
				{
					Gizmos.DrawLine(Waypoints[i].Position, transform.position);
				}
			}
			
			if (HelperPoint != null && AddingWaypoints)
			{
				Gizmos.DrawSphere(HelperPoint.Position, 0.2f);
				TextGizmo.Instance.DrawText(SceneView.lastActiveSceneView.camera, HelperPoint.Position, Waypoints.Count);
				
				if (Waypoints.Count > 0)
				{
					Gizmos.DrawLine(HelperPoint.Position, Waypoints[Waypoints.Count - 1].Position);
				}
				else
				{
					Gizmos.DrawLine(HelperPoint.Position, transform.position);
				}
			}
		}
	}
	
	[System.Serializable]
	public class Waypoint
	{
		public Vector3 Position;
		public string Name;
		public int ActionType;
		
		public Waypoint (Vector3 position, string name)
		{
			Position = position;
			Name = name;
		}
	}
}
