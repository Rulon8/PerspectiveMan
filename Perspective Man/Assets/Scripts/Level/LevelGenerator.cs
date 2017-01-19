// Project: Perspective Man
/// \file LevelGenerator.cs
/// \copyright (C) year, FG!P. All rights reserved.
/// \date 2016-12-31
/// \author Franco Solis
/// \author Giancarlo Longhi
/// \author Federico Ugarte
/// <summary> 
/// Script used to generate infinite level.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	#region Variables

	[SerializeField] private int _numberOfBlocks = 40;
	[SerializeField] private int _initialBlocks = 40;
	[SerializeField] private float _startingPositionOffset = 10;
	[SerializeField] private float _worldXLimit = 100;
	[SerializeField] private float _screenDistanceOffset = 10;
	[SerializeField] private List<Transform> _objectPrefabList;
	[SerializeField] private Vector3 _startingPosition;

	private Vector3 blockSize;
	private Vector3 _nextPosition;
	private Queue<Transform> _objectQueue;

	#endregion

	#region Methods

	void Start () {
		_objectQueue = new Queue<Transform>(_numberOfBlocks);
		blockSize = _objectPrefabList [0].localScale; //At the moment every block is the same size, this should change when blocks vary in size
		InitialSpawn ();

	}

	void Update () { 
		
		if (_objectQueue.Peek().localPosition.x + _screenDistanceOffset < PlayerController.instance.DistanceTraveled) //Spawns a block every time the player reaches a certain distance from the last block spawned
		{
			SpawnBlock();
		}

		//When the maximum number of blocks is spawned, eliminates the last one
		if (_objectQueue.Count >= _numberOfBlocks)
		{
			DespawnLastBlock ();
		}
	}

	/// <summary>
	/// Spawns a defined number of blocks before the beginning of the game
	/// </summary>
	void InitialSpawn()
	{
		_nextPosition = _startingPosition;
		_nextPosition.x -= (blockSize.x * _startingPositionOffset);
		for (int i = 0; i < _numberOfBlocks; i++)
		{
			SpawnBlock ();
		}
		Debug.Log (_objectQueue.Count);
	}

	/// <summary>
	/// Spawns a single random block and places it on the queue of blocks
	/// </summary>
	void SpawnBlock()
	{
		int randomIndex = Random.Range (0, _objectPrefabList.Count);
		Transform o = (Transform)Instantiate (_objectPrefabList [randomIndex]);
		o.localPosition = _nextPosition;
		_nextPosition.x += o.localScale.x;
		_objectQueue.Enqueue (o);
	}

	/// <summary>
	/// Despawns the last block of the level
	/// </summary>
	void DespawnLastBlock()
	{
		if (_objectQueue.Count > 0)
		{
			Destroy (_objectQueue.Dequeue ().gameObject);
		}
	}

	#endregion 

}
