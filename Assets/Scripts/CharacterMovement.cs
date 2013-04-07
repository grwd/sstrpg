using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	public float Speed;
	public float MaxSpeed;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		
		move.x = Mathf.Min(move.x, MaxSpeed);
		move.z = Mathf.Min(move.z, MaxSpeed);
		
		rigidbody.AddForce(move, ForceMode.VelocityChange);
	}
}
