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

	[SerializeField] private int _numberOfBlocks;
	[SerializeField] private int _initialBlocks;
	[SerializeField] private float _screenDistanceOffset;
	[SerializeField] private List<Transform> _objectPrefabList;
	[SerializeField] private Vector3 _startingPosition;
	[SerializeField] private float _worldXLimit;

	private Vector3 blockSize;
	private Vector3 _nextPosition;
	private Queue<Transform> _objectQueue;

	#endregion

	#region Methods

	void Start () {
		_objectQueue = new Queue<Transform>(_numberOfBlocks);
		blockSize = _objectPrefabList [0].localScale;

		InitialSpawn ();
	}

	// Update is called once per frame
	void Update () {
		if (_objectQueue.Peek().localPosition.x + _screenDistanceOffset < PlayerController.instance.distanceTraveled)
		{
			SpawnBlock();
		}
		if (_objectQueue.Count == _numberOfBlocks)
		{
			DespawnLastBlock ();
		}

	}

	void InitialSpawn()
	{
		_nextPosition = _startingPosition;
		_nextPosition.x -= (blockSize.x * 10);
		for (int i = 0; i < _numberOfBlocks; i++)
		{
			SpawnBlock ();
			Debug.Log ("Initial Spawn");
		}
		Debug.Log (_objectQueue.Count);
	}

	void SpawnBlock()
	{
		//Transform o = _objectQueue.Dequeue();
		int randomIndex = Random.Range (0, _objectPrefabList.Count);
		Transform o = (Transform)Instantiate (_objectPrefabList [randomIndex]);
		o.localPosition = _nextPosition;
		_nextPosition.x += o.localScale.x;
		_objectQueue.Enqueue (o);

//		_objectQueue.Enqueue((Transform)Instantiate(objList[randomIndex]));
	}
	void DespawnLastBlock()
	{
		if (_objectQueue.Count > 0)
		{
			Destroy (_objectQueue.Dequeue ().gameObject);
		}
	}

	#endregion 

}
