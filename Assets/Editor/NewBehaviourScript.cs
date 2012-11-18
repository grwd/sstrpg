using UnityEngine;
using UnityEditor;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	[MenuItem ("SST-RPG/Remove Mesh Colliders")]
    static void RemoveMeshColliders () {
		MeshCollider[] meshColliders = FindObjectsOfType(typeof(MeshCollider)) as MeshCollider[];
		
		foreach(MeshCollider mc in meshColliders)
		{
			if (mc.gameObject.name != "GroundCollider")
				DestroyImmediate(mc);
		}
	}
}
