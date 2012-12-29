using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColliderManager))]
public class ColliderManagerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		ColliderManager colliderManager = (ColliderManager)target;
		
		colliderManager.LinePrefab = EditorGUILayout.ObjectField("Line Prefab", colliderManager.LinePrefab, typeof(GameObject), true) as GameObject;
		
		if (colliderManager.AddingColliders)
		{
			GUI.backgroundColor = Color.red;
			
			EditorGUILayout.HelpBox("Click sceneview to add new points. Hold down ALT to create FRONT/BACK colliders. Hold down SHIFT to align with grid. Hold down CTRL to snap to nearest point.", MessageType.Info);
		}
		else
		{
			GUI.backgroundColor = Color.white;
		}
		
		if (!colliderManager.AddingColliders && GUILayout.Button("Place New Collider Points"))
		{
			Vector3 camPos = colliderManager.transform.position + new Vector3(0, 10, 0);
	        SceneView.lastActiveSceneView.pivot = camPos;
			SceneView.lastActiveSceneView.rotation = Quaternion.LookRotation(-Vector3.up);
	        SceneView.lastActiveSceneView.Repaint();
			
			colliderManager.AddingColliders = true;
			colliderManager.Points.Clear();
		}
		else if (colliderManager.AddingColliders && GUILayout.Button("Cancel"))
		{
			colliderManager.AddingColliders = false;
			colliderManager.Points.Clear();
		}
		
		GUI.backgroundColor = Color.white;
		GUI.enabled = colliderManager.Points.Count > 1;
		
		if (colliderManager.AddingColliders && GUILayout.Button("Create New Colliders"))
		{
			if (colliderManager.LinePrefab != null)
			{
				for (int i = 0; i < colliderManager.Points.Count; i++)
				{
					if (i > 0)
					{
						Vector3 p1 = colliderManager.Points[i].Point;
						Vector3 p2 = colliderManager.Points[i - 1].Point;
						Vector3 midPoint = new Vector3((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, (p1.z + p2.z) / 2);
						GameObject go = GameObject.Instantiate(colliderManager.LinePrefab, midPoint, Quaternion.identity) as GameObject;
						go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, Vector3.Distance(p1, p2) + go.transform.localScale.x);
						go.transform.LookAt(p2);
						go.transform.parent = colliderManager.transform;
						if (colliderManager.Points[i].Front && colliderManager.Points[i - 1].Front)
							go.name = "Front";
						else if (colliderManager.Points[i].Back && colliderManager.Points[i - 1].Back)
							go.name = "Back";
						else
							go.name = "Side";
					}
				}
				colliderManager.AddingColliders = false;
				colliderManager.Points.Clear();
			}
			else
			{
				Debug.Log("Please set the LinePrefab!");
			}
		}
		
		GUI.enabled = true;
		
		if (GUI.changed)
			EditorUtility.SetDirty (target);
	}
	
	void OnSceneGUI()
	{
		if (!SceneView.currentDrawingSceneView.camera.pixelRect.Contains(Event.current.mousePosition))
		{
			ColliderManager colliderManager = (ColliderManager)target;
			
			colliderManager.HelperPoint = null;
			
			SceneView.currentDrawingSceneView.Repaint();
		}
		else if (SceneView.currentDrawingSceneView.camera != null)
		{
			ColliderManager colliderManager = (ColliderManager)target;
			
			Vector3 point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
			point.y = colliderManager.transform.position.y;
			
			// ctrl modifier for snapping to nearest point
			if (Event.current.control && colliderManager.Points.Count > 0)
			{
				int nearestIndex = 0;
				float nearestDistance = Mathf.Infinity;
				for (int i = 0; i < colliderManager.Points.Count; i++)
				{
					float distance = Vector3.Distance(colliderManager.Points[i].Point, point);
					if (distance < nearestDistance)
					{
						nearestDistance = distance;
						nearestIndex = i;
					}
				}
				
				point = colliderManager.Points[nearestIndex].Point;
			}
			// shift modifier for making straight lines (along x or z axis)
			else if (Event.current.shift && colliderManager.Points.Count > 0)
			{
				if (Mathf.Abs(point.x - colliderManager.Points[colliderManager.Points.Count - 1].Point.x + ((point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z) * 2.45f)) < Mathf.Abs(point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z))
				{
					point.x = colliderManager.Points[colliderManager.Points.Count - 1].Point.x - ((point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z) * 2.45f);
				}
				else
				{
					point.z = colliderManager.Points[colliderManager.Points.Count - 1].Point.z;
				}
			}
			
			colliderManager.HelperPoint = new ColliderManager.ColliderPoint(point);
			
			// alt modifier for setting Front / Back colliders
			if (Event.current.alt && colliderManager.Points.Count > 0)
			{
				bool frontSet = false;
				for (int i = 0; i < colliderManager.Points.Count; i++)
				{
					if (colliderManager.Points[i].Front)
						frontSet = true;
				}
				
				if (!frontSet)
				{
					colliderManager.HelperPoint.Front = true;
				}
				else
				{
					colliderManager.HelperPoint.Back = true;
				}
			}
			
			SceneView.currentDrawingSceneView.Repaint();
		}
		
		if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
		{
			if (SceneView.currentDrawingSceneView.camera != null)
			{
				ColliderManager colliderManager = (ColliderManager)target;
				
				Vector3 point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
				point.y = colliderManager.transform.position.y;
				
				// ctrl modifier for snapping to nearest point
				if (Event.current.control && colliderManager.Points.Count > 0)
				{
					int nearestIndex = 0;
					float nearestDistance = Mathf.Infinity;
					for (int i = 0; i < colliderManager.Points.Count; i++)
					{
						float distance = Vector3.Distance(colliderManager.Points[i].Point, point);
						if (distance < nearestDistance)
						{
							nearestDistance = distance;
							nearestIndex = i;
						}
					}
					
					point = colliderManager.Points[nearestIndex].Point;
				}
				// shift modifier for making straight lines (along x or z axis)
				else if (Event.current.shift && colliderManager.Points.Count > 0)
				{
					if (Mathf.Abs(point.x - colliderManager.Points[colliderManager.Points.Count - 1].Point.x + ((point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z) * 2.45f)) < Mathf.Abs(point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z))
					{
						point.x = colliderManager.Points[colliderManager.Points.Count - 1].Point.x - ((point.z - colliderManager.Points[colliderManager.Points.Count - 1].Point.z) * 2.45f);
					}
					else
					{
						point.z = colliderManager.Points[colliderManager.Points.Count - 1].Point.z;
					}
				}
				
				ColliderManager.ColliderPoint cp = new ColliderManager.ColliderPoint(point);
				colliderManager.Points.Add(cp);
				
				// alt modifier for setting Front / Back colliders
				if (Event.current.alt && colliderManager.Points.Count > 1)
				{
					bool frontSet = false;
					for (int i = 0; i < colliderManager.Points.Count; i++)
					{
						if (colliderManager.Points[i].Front)
							frontSet = true;
					}
					
					if (!frontSet)
					{
						cp.Front = true;
						colliderManager.Points[colliderManager.Points.Count - 2].Front = true;
					}
					else
					{
						cp.Back = true;
						colliderManager.Points[colliderManager.Points.Count - 2].Back = true;
					}
				}
				
				Repaint();
			}
			else
			{
				Debug.Log("camera is null");
			}
		}
	}
}