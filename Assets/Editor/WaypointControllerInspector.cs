using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointController))]
public class WaypointControllerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		WaypointController waypointController = (WaypointController)target;
		
		if (waypointController.AddingWaypoints)
		{
			GUI.backgroundColor = Color.red;
			
			EditorGUILayout.HelpBox("Click sceneview to add a new waypoint.", MessageType.Info);
		}
		else
		{
			GUI.backgroundColor = Color.white;
		}
		
		if (!waypointController.AddingWaypoints && GUILayout.Button("Start Adding Waypoints"))
		{
			waypointController.AddingWaypoints = true;
			
			if (SceneView.lastActiveSceneView != null)
			{
				Vector3 camPos = waypointController.transform.position + new Vector3(0, 10, 0);
		        SceneView.lastActiveSceneView.pivot = camPos;
				SceneView.lastActiveSceneView.rotation = Quaternion.LookRotation(-Vector3.up);
		        SceneView.lastActiveSceneView.Repaint();
			}
		}
		else if (waypointController.AddingWaypoints && GUILayout.Button("Stop Adding Waypoints"))
		{
			waypointController.AddingWaypoints = false;
		}
		
		GUI.backgroundColor = Color.white;	
		
		WaypointController.Waypoint swapWP1 = null;
		WaypointController.Waypoint swapWP2 = null;
		WaypointController.Waypoint deleteWP = null;
		
		for (int i = 0; i < waypointController.Waypoints.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25));
			GUI.enabled = i > 0;
			if (GUILayout.Button("^", GUILayout.Width(25)))
			{
				swapWP1 = waypointController.Waypoints[i];
				swapWP2 = waypointController.Waypoints[i - 1];
			}
			GUI.enabled = i < waypointController.Waypoints.Count - 1;
			if (GUILayout.Button("v", GUILayout.Width(25)))
			{
				swapWP1 = waypointController.Waypoints[i];
				swapWP2 = waypointController.Waypoints[i + 1];
			}
			GUI.enabled = true;
			waypointController.Waypoints[i].Name = EditorGUILayout.TextField(waypointController.Waypoints[i].Name, GUILayout.Width(100));
			if (GUILayout.Button("X", GUILayout.Width(25)))
			{
				deleteWP = waypointController.Waypoints[i];
			}
			
			EditorGUILayout.EndHorizontal();
		}
		
		if (swapWP1 != null && swapWP2 != null)
		{
			int i1 = waypointController.Waypoints.IndexOf(swapWP1);
			int i2 = waypointController.Waypoints.IndexOf(swapWP2);
			waypointController.Waypoints[i1] = swapWP2;
			waypointController.Waypoints[i2] = swapWP1;
			SceneView.lastActiveSceneView.Repaint();
		}
		
		if (deleteWP != null)
		{
			waypointController.Waypoints.Remove(deleteWP);
			SceneView.lastActiveSceneView.Repaint();
		}
		
		if (GUI.changed)
			EditorUtility.SetDirty (target);
	}
	
	void OnSceneGUI()
	{
		WaypointController waypointController = (WaypointController)target;
		
		if (waypointController.AddingWaypoints)
		{
			if (!SceneView.currentDrawingSceneView.camera.pixelRect.Contains(Event.current.mousePosition))
			{
				waypointController.HelperPoint = null;
				
				SceneView.currentDrawingSceneView.Repaint();
			}
			else if (SceneView.currentDrawingSceneView.camera != null)
			{
				Vector3 point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
				point.y = waypointController.transform.position.y;
				
				// ctrl modifier for snapping to nearest point
				if (Event.current.control && waypointController.Waypoints.Count > 0)
				{
					int nearestIndex = 0;
					float nearestDistance = Mathf.Infinity;
					for (int i = 0; i < waypointController.Waypoints.Count; i++)
					{
						float distance = Vector3.Distance(waypointController.Waypoints[i].Position, point);
						if (distance < nearestDistance)
						{
							nearestDistance = distance;
							nearestIndex = i;
						}
					}
					
					point = waypointController.Waypoints[nearestIndex].Position;
				}
				// shift modifier for making straight lines (along x or z axis)
				else if (Event.current.shift && waypointController.Waypoints.Count > 0)
				{
					if (Mathf.Abs(point.x - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.x + ((point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z) * 2.45f)) < Mathf.Abs(point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z))
					{
						point.x = waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.x - ((point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z) * 2.45f);
					}
					else
					{
						point.z = waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z;
					}
				}
				
				waypointController.HelperPoint = new WaypointController.Waypoint(point, "Waypoint " + waypointController.Waypoints.Count);
				
				SceneView.currentDrawingSceneView.Repaint();
			}
			
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
			{
				if (SceneView.currentDrawingSceneView.camera != null)
				{
					Vector3 point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
					point.y = waypointController.transform.position.y;
					
					// ctrl modifier for snapping to nearest point
					if (Event.current.control && waypointController.Waypoints.Count > 0)
					{
						int nearestIndex = 0;
						float nearestDistance = Mathf.Infinity;
						for (int i = 0; i < waypointController.Waypoints.Count; i++)
						{
							float distance = Vector3.Distance(waypointController.Waypoints[i].Position, point);
							if (distance < nearestDistance)
							{
								nearestDistance = distance;
								nearestIndex = i;
							}
						}
						
						point = waypointController.Waypoints[nearestIndex].Position;
					}
					// shift modifier for making straight lines (along x or z axis)
					else if (Event.current.shift && waypointController.Waypoints.Count > 0)
					{
						if (Mathf.Abs(point.x - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.x + ((point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z) * 2.45f)) < Mathf.Abs(point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z))
						{
							point.x = waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.x - ((point.z - waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z) * 2.45f);
						}
						else
						{
							point.z = waypointController.Waypoints[waypointController.Waypoints.Count - 1].Position.z;
						}
					}
					
					WaypointController.Waypoint wp = new WaypointController.Waypoint(point, "Waypoint " + waypointController.Waypoints.Count);
					waypointController.Waypoints.Add(wp);
					
					Repaint();
				}
			}
			else
			{
				Debug.Log("camera is null");
			}
		}
	}
}