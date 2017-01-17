// Project: <Name Of Project>
/// \file PlayerController.cs
/// \copyright (C) 2017, FG!P. All rights reserved.
/// \date 2017-01-03
/// \author Rulon
/// <summary> 
/// Contains every player mechanic in the game: forward movment, movement between lanes, gravity change, and jump.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	#region Constants

	public const float FLOOR_WIDTH = 10f;

	#endregion

	#region Variables

	[SerializeField] private int _lane;
	[SerializeField] private float _forwardSpeed;
	[SerializeField] private bool _gravityInverted;
	[SerializeField] private bool _isGrounded;
	[SerializeField] private float _groundCheckDistance;
	[SerializeField] float _gravityMultiplier;
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private float _jumpPower;
	[SerializeField] private float _origGroundCheckDistance;
	[SerializeField] private Camera _camera;
	[SerializeField] private CameraController _cameraController;
	[SerializeField] private bool _isGravityChanging;
	#endregion

	#region Properties

	public int Lane
	{
		get 
		{
			return _lane;
		}
		set
		{
			_lane = value;
		}
	}

	public float ForwardSpeed
	{
		get
		{
			return _forwardSpeed;
		}
		set
		{
			_forwardSpeed = value;
		}
	}

	public float GroundCheckDistance
	{
		get
		{
			return _groundCheckDistance;
		}
		set
		{
			_groundCheckDistance = value;
		}
	}

	public float JumpPower
	{
		get
		{
			return _jumpPower;
		}
		set
		{
			_jumpPower = value;
		}
	}

	#endregion

	#region Methods

	// Use this for initialization
	void Start() 
	{
		_lane = 2;
		_forwardSpeed = 2f;
		_gravityInverted = false;
		_groundCheckDistance = 0.6f;
		_rigidbody = GetComponent<Rigidbody>();
		_isGrounded = true;
		_jumpPower = 12f;
		_origGroundCheckDistance = _groundCheckDistance;
		_gravityMultiplier = 2f;
		_camera = Camera.main;
		_cameraController = (CameraController) _camera.GetComponent("CameraController");
		_isGravityChanging = false;
	}

	// Update is called once per frame
	void Update() 
	{
		if (!_cameraController.TwoDMode)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				Move("Left");
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				Move("Right");
			}
		}

		//Move forward
		if (!_cameraController.ChangingPerspective)
		{
			transform.Translate(_forwardSpeed * Time.deltaTime, 0, 0);
		}
	}

	void FixedUpdate() 
	{
		if (!_cameraController.ChangingPerspective)
		{
			CheckGroundStatus();

			if (!_isGravityChanging)
			{
				if (_isGrounded)
				{
					if (Input.GetKeyDown(KeyCode.Space))
					{
						Jump();
					}
				}
				else
				{
					HandleAirborneMovement();
				}

				if (Input.GetKeyDown(KeyCode.Q))
				{
					ChangeGravity();
				}
			}
			else
			{
				HandleAirborneMovement();
			}
		}
	}

	void Move(string direction) 
	{
		switch (direction)
		{
		case "Left":
			if (_lane != 1 && _lane != 4)
			{
				_lane--;
				transform.Translate(new Vector3(0, 0, FLOOR_WIDTH / 4));
			}
			break;

		case "Right":
			if (_lane != 3 && _lane != 6)
			{
				_lane++;
				transform.Translate(new Vector3(0, 0, -FLOOR_WIDTH / 4));
			}
			break;
		}
	}

	void Jump() 
	{
		if (_gravityInverted)
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -_jumpPower, _rigidbody.velocity.z);
		}
		else
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpPower, _rigidbody.velocity.z);
		}
	}

	void ChangeGravity() 
	{
		_isGravityChanging = true;
		Physics.gravity = new Vector3(0, -Physics.gravity.y, 0);
		_gravityInverted = !_gravityInverted;
	}

	void CheckGroundStatus()
	{
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * _groundCheckDistance));
		#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (_gravityInverted)
		{
			if (Physics.Raycast(transform.position + (Vector3.down * 0.1f), Vector3.up, _groundCheckDistance))
			{
				_isGrounded = true;
				_isGravityChanging = false;
			}
			else
			{
				_isGrounded = false;
			}
		}
		else
		{
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, _groundCheckDistance))
			{
				_isGrounded = true;
				_isGravityChanging = false;
			}
			else
			{
				_isGrounded = false;
			}
		}
	}

	void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * _gravityMultiplier) - Physics.gravity;
		_rigidbody.AddForce(extraGravityForce);

		if (_gravityInverted)
		{
			_groundCheckDistance = _rigidbody.velocity.y > 0 ? _origGroundCheckDistance : 0.01f;
		}
		else
		{
			_groundCheckDistance = _rigidbody.velocity.y < 0 ? _origGroundCheckDistance : 0.01f;
		}
	}
		
	#endregion
}
