// Project: Perspective Man
/// \file PlayerController.cs
/// \copyright (C) year, FG!P. All rights reserved.
/// \date 2016-12-31
/// \author Franco Solis
/// \author Giancarlo Longhi
/// \author Federico Ugarte
/// <summary> 
/// Basic player movement controller.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour {

	#region Variables

	public static PlayerControllerOld instance;

	public float distanceTraveled = 0;

	[SerializeField] private float _jumpSpeed = 20f;
	[SerializeField] private float _movementSpeed = 5f;

	private Rigidbody _rigidbody;

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
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Vector3 jumpForce = new Vector3(0, _jumpSpeed, 0);
			_rigidbody.AddForce(jumpForce, ForceMode.Impulse);
		}
		transform.Translate(_movementSpeed * Time.deltaTime, 0f, 0f);
		distanceTraveled = transform.localPosition.x;
	}

	public void ResetPosition(float positionDifference)
	{
		Vector3 temp = transform.position - positionDifference * Vector3.right;
		transform.position = temp;
	}

	#endregion 

}
