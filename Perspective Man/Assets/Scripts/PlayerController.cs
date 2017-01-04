// Project: <Name Of Project>
/// \file PlayerController.cs
/// \copyright (C) 2017, FG!P. All rights reserved.
/// \date 2017-01-03
/// \author Rulon
/// <summary> 
/// Contiene todas las mecanicas o acciones que el jugador puede realizar dentro del juego
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	#region

	public const float FLOOR_WIDTH = 10f;

	#endregion

	#region Variables

	[SerializeField] private int _lane;
	[SerializeField] private float _forwardSpeed;
	[SerializeField] private bool _gravityInverted;
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

	#endregion

	#region Methods

	// Use this for initialization
	void Start() 
	{
		_lane = 2;
		_forwardSpeed = 2f;
		_gravityInverted = false;
	}

	// Update is called once per frame
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			Move("Left");
		}

		if (Input.GetKeyDown (KeyCode.D))
		{
			Move("Right");
		}

		if (Input.GetKeyDown (KeyCode.LeftShift))
		{
			ChangeGravity();
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			ChangeDimension();
		}

		//Move forward
		transform.Translate(2f * Time.deltaTime, 0, 0);
	}

	void FixedUpdate() 
	{
		
	}

	void Move(string direction) 
	{
		switch (direction)
		{
		case "Left":
			if(_lane != 1 && _lane != 4)
			{
				_lane--;
				transform.Translate(new Vector3(0, 0, FLOOR_WIDTH / 4));
			}
			break;

		case "Right":
			if(_lane != 3 && _lane != 6)
			{
				_lane++;
				transform.Translate(new Vector3(0, 0, -FLOOR_WIDTH / 4));
			}
			break;
		}
	}

	void Jump() 
	{
		
	}

	void ChangeGravity() 
	{
		Physics.gravity = new Vector3(0, -Physics.gravity.y, 0);
		_gravityInverted = !_gravityInverted;
	}

	void ChangeDimension() 
	{
		
	}

	#endregion
}
