/*using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{
	public float Speed = 10.0f;
	public float MaxVelocityChange = 10.0f;
	public float Gravity = 10.0f;
	public float JumpHeight = 2.0f;
	
	private bool _grounded = false;
	
	void Start ()
	{
		Application.targetFrameRate = 30;
		
		_startingY = transform.position.y;
		_ground = Ground.GetComponent<GroundStatus>();
	}
	
	void Update()
	{
		_frames++;
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 100), (_frames / Time.time).ToString());
	}
	
	void FixedUpdate()
	{
		if (_changingLanesUp || _changingLanesDown)
		{
			if ((_changingLanesUp && _changeLaneTimer < 0.1f) ||
				(_changingLanesDown && _changeLaneTimer < 0.1f))
			{
				_changeLaneTimer += Time.deltaTime;
				transform.position = Vector3.Lerp(transform.position, _newLanePos, _changeLaneTimer * 10);
			}
			else
			{
				_changeLaneTimer = 0f;
				_changingLanesUp = false;
				_changingLanesDown = false;
				rigidbody.isKinematic = false;
				rigidbody.velocity = Vector3.zero;
			}
		}
		else
		{
			if (_isOn && _grounded && !Input.GetMouseButton(0) &&
				(((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && _laneNumber > 0) || //transform.position.y < _startingY + 1) ||
				((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && _laneNumber < 8))) // && transform.position.y > _startingY - 1.5f)))
			{
				int oldLaneNumber = _laneNumber;
				if (Input.GetAxis("Vertical") > 0)
				{
					_changingLanesUp = true;
					_laneNumber--;
					_newLanePos = transform.position + Ground.forward * Vector3.Distance(_ground.Lanes[_laneNumber].position, _ground.Lanes[oldLaneNumber].position);
					//_newLanePos = transform.position + Ground.forward * 0.75f;
				}
				else if (Input.GetAxis("Vertical") < 0)
				{
					_changingLanesDown = true;
					_laneNumber++;
					_newLanePos = transform.position - Ground.forward * Vector3.Distance(_ground.Lanes[_laneNumber].position, _ground.Lanes[oldLaneNumber].position);
					//_newLanePos = transform.position + Ground.forward * -0.75f;
				}
				rigidbody.isKinematic = true;
			}
			else
			{
				// Calculate how fast we should be moving
				Vector3 targetVelocity = Vector3.zero;
				
				if (_isOn && !Input.GetMouseButton(0))
					targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
				
				targetVelocity *= Speed;
		 
			 	// Apply a force that attempts to reach our target velocity
				Vector3 velocity = rigidbody.velocity;
				targetVelocity.y = velocity.y;
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
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
}*/