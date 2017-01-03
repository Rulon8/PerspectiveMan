// Project: Perspective Man
/// \file FollowPlayer.cs
/// \copyright (C) year, FG!P. All rights reserved.
/// \date 2016-12-31
/// \author Franco Solis
/// \author Giancarlo Longhi
/// \author Federico Ugarte
/// <summary> 
/// Script for camera to follow player.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	#region Variables

	[SerializeField]
	private Transform _player;
	
	[SerializeField] 
	private Vector3 _offset = new Vector3 (6, 0, -100);

	#endregion

	#region Methods

	void Update () {
		transform.position = new Vector3(_player.position.x + _offset.x, _offset.y, _offset.z);
	}

	#endregion 

	
}
