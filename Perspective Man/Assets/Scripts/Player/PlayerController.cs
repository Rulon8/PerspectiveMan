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
	public const float HORIZONTAL_MOVEMENT_SPEED = 10f;
	public const float GROUNDED_ROTATION_SPEED = 15f;
	public const float AIRBORNE_ROTATION_SPEED = 7f;

	#endregion

	#region Variables

	public static PlayerController instance;

	[SerializeField] private int _lane;
	[SerializeField] private float _forwardSpeed;
	[SerializeField] private bool _gravityInverted;
	[SerializeField] private bool _isGrounded;
	[SerializeField] float _gravityMultiplier;
	[SerializeField] private float _jumpPower;
	[SerializeField] private bool _isGravityChanging;
	[SerializeField] private bool _isMoving;
	private float _distanceTraveled = 0;
	private float _origGroundCheckDistance;
	private Camera _camera;
	private CameraController _cameraController;
	private Rigidbody _rigidbody;
	private float _groundCheckDistance;

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

	public float DistanceTraveled
	{
		get
		{
			return _distanceTraveled;
		}
	}

	#endregion

	#region Methods

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		instance = this; 
	}


	void Start() 
	{
		_lane = 2;
		_forwardSpeed = 2f;
		_gravityInverted = false;
		_groundCheckDistance = 0.8f;
		_rigidbody = GetComponent<Rigidbody>();
		_isGrounded = true;
		_jumpPower = 12f;
		_origGroundCheckDistance = _groundCheckDistance;
		_gravityMultiplier = 2f;
		_camera = Camera.main;
		_cameraController = (CameraController) _camera.GetComponent("CameraController");
		_isGravityChanging = false;
		_isMoving = false;
	}

	// Update is called once per frame
	void Update() 
	{
		if (!_cameraController.Is2DMode && !_isMoving && !_isGravityChanging)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				StartCoroutine(Move("Left"));
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				StartCoroutine(Move("Right"));
			}
		}

		//Move forward
		if (!_cameraController.ChangingPerspective)
		{
			transform.Translate(_forwardSpeed * Time.deltaTime, 0, 0);
			_distanceTraveled = transform.localPosition.x; 
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
					StartCoroutine(Rotate());
				}
			}
			else
			{
				HandleAirborneMovement();
			}
		}
	}

	IEnumerator Move(string direction) 
	{
		_isMoving = true;
		switch (direction)
		{
		case "Left":
			if (_lane != 1 && _lane != 4)
			{
				for (float f = 0f; f <= HORIZONTAL_MOVEMENT_SPEED; f += 1f)
				{
					transform.Translate(new Vector3(0, 0, (FLOOR_WIDTH / 4) / HORIZONTAL_MOVEMENT_SPEED), Space.World);
					yield return null;
				}
				_lane--;

			}
			break;

		case "Right":
			if (_lane != 3 && _lane != 6)
			{
				for (float f = 0f; f <= HORIZONTAL_MOVEMENT_SPEED; f += 1f)
				{
					transform.Translate(new Vector3(0, 0, (-FLOOR_WIDTH / 4) / HORIZONTAL_MOVEMENT_SPEED), Space.World);
					yield return null;
				}
				_lane++;
			}
			break;
		}
		_isMoving = false;
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

	IEnumerator Rotate()
	{
		if (_isGrounded)
		{
			for (float f = 0f; f < GROUNDED_ROTATION_SPEED; f += 1f)
			{
				transform.Rotate(180f / GROUNDED_ROTATION_SPEED, 0, 0);
				_cameraController.ResetRotation();
				yield return null;
			}
		}
		else
		{
			for (float f = 0f; f < AIRBORNE_ROTATION_SPEED; f += 1f)
			{
				transform.Rotate(180f / AIRBORNE_ROTATION_SPEED, 0, 0);
				_cameraController.ResetRotation();
				yield return null;
			}
		}
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
