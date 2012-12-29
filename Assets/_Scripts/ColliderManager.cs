using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ColliderManager : MonoBehaviour
{
	public GameObject LinePrefab;
	
	public bool AddingColliders;
	
	public List<ColliderPoint> Points = new List<ColliderPoint>();
	
	public ColliderPoint HelperPoint;
	
	void OnRenderObject ()
	{
		if (AddingColliders)
		{
			Selection.activeTransform = transform;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if (AddingColliders)
		{
			Gizmos.color = Color.yellow;
			
			for (int i = 0; i < Points.Count; i++)
			{
				if (Points[i].Front || (i == Points.Count - 1 && HelperPoint != null && HelperPoint.Front))
					Gizmos.color = Color.red;
				else if (Points[i].Back || (i == Points.Count - 1 && HelperPoint != null && HelperPoint.Back))
					Gizmos.color = Color.blue;
				
				Gizmos.DrawSphere(Points[i].Point, 0.2f);
				
				if (i > 0)
				{
					if (!(Points[i - 1].Front && Gizmos.color == Color.red) && !(Points[i - 1].Back && Gizmos.color == Color.blue))
						Gizmos.color = Color.yellow;
					
					Gizmos.DrawLine(Points[i].Point, Points[i - 1].Point);
				}
				
				Gizmos.color = Color.yellow;
			}
			
			if (HelperPoint != null)
			{
				if (HelperPoint.Front)
					Gizmos.color = Color.red;
				else if (HelperPoint.Back)
					Gizmos.color = Color.blue;
				
				Gizmos.DrawSphere(HelperPoint.Point, 0.2f);
				
				if (Points.Count > 0)
				{
					//if (!(Points[Points.Count - 1].Front && Gizmos.color == Color.red) && !(Points[Points.Count - 1].Back && Gizmos.color == Color.blue))
					//	Gizmos.color = Color.yellow;
					
					Gizmos.DrawLine(HelperPoint.Point, Points[Points.Count - 1].Point);
				}
				
				Gizmos.color = Color.yellow;
			}
		}
	}
	
	public class ColliderPoint
	{
		public Vector3 Point { get; set; }
		public bool Front { get; set; }
		public bool Back { get; set; }
		
		public ColliderPoint(Vector3 point)
		{
			Point = point;
		}
	}
}