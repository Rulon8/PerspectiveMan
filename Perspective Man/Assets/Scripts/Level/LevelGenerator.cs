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

	public int numberOfBlocks;
	public float screenDistanceOffset;

	public Transform objPrefab;
	public List<Transform> objList;
	public Vector3 startingPosition;

	private Vector3 _nextPosition;
	private Queue<Transform> _objectQueue;

	#endregion

	#region Methods

	void Start () {
		_objectQueue = new Queue<Transform>(numberOfBlocks);
		int randomIndex = Random.Range (0, objList.Count);
		for(int i = 0; i <= numberOfBlocks; i++)
		{
			_objectQueue.Enqueue((Transform)Instantiate(objList[randomIndex]));
		}

		_nextPosition = startingPosition;
		for(int i = 0; i <= numberOfBlocks; i++)
		{
			SpawnBlock();
		}
	}

	// Update is called once per frame
	void Update () {
		if (_objectQueue.Peek().localPosition.x + screenDistanceOffset < PlayerController.distanceTraveled)
		{
			SpawnBlock();
		}
	}

	void SpawnBlock()
	{
		Transform o = _objectQueue.Dequeue();
		o.localPosition = _nextPosition;
		_nextPosition.x += o.localScale.x;
		int randomIndex = Random.Range (0, objList.Count);
		_objectQueue.Enqueue((Transform)Instantiate(objList[randomIndex]));
	}

	#endregion 

}
