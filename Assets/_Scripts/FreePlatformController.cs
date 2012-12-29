using UnityEngine;
using System.Collections;

public class FreePlatformController : MonoBehaviour
{
	public float Speed = 10.0f;
	public float MaxVelocityChange = 10.0f;
	public float Gravity = 10.0f;
	public float JumpHeight = 2.0f;
	
	private bool _grounded = false;
	
	private bool _isOn = true;
	
	void FixedUpdate()
	{
		// Calculate how fast we should be moving
		Vector3 targetVelocity = Vector3.zero;
		
		if (_isOn && !Input.GetMouseButton(0))
		{
			targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			targetVelocity += Input.GetAxis("Vertical") * Vector3.forward;
		}
		
		targetVelocity *= Speed;
 
	 	// Apply a force that attempts to reach our target velocity
		Vector3 velocity = rigidbody.velocity;
		targetVelocity.y = velocity.y;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxVelocityChange, MaxVelocityChange);
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
		// Jump
		if (_isOn && _grounded && Input.GetButton("Jump") && !Input.GetMouseButton(0))
		{
			rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			_grounded = false;
		}
		
		// We apply gravity manually for more tuning control
		if (!_grounded)
		{
			rigidbody.AddForce(new Vector3 (0, -Gravity * rigidbody.mass, 0));
		}
		
		if (_isOn && !Input.GetMouseButton(0) &&
			((Input.GetAxis("Horizontal") < 0 && transform.localScale.x > 0) ||
			 (Input.GetAxis("Horizontal") > 0 && transform.localScale.x < 0)))
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
		else if (_isOn && Input.GetMouseButton(0) &&
			((Input.mousePosition.x < Screen.width / 2 && transform.localScale.x > 0) ||
			 (Input.mousePosition.x > Screen.width / 2 && transform.localScale.x < 0)))
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}
	
	void OnCollisionStay ()
	{
		_grounded = true;    
	}
	
	float CalculateJumpVerticalSpeed ()
	{
		return Mathf.Sqrt(2 * JumpHeight);
	}
	
	public void ToggleOnOff()
	{
		_isOn = !_isOn;
	}
}