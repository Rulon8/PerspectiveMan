// Project: Perspective Man
/// \file IBlock.cs
/// \copyright (C) year, FG!P. All rights reserved.
/// \date 2016-12-31
/// \author Franco Solis
/// \author Giancarlo Longhi
/// \author Federico Ugarte
/// <summary> 
/// Interface used to describe block types.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock {

	#region Properties

	BlockType BlockType
	{
		get;
	}

//	Vector3 Position
//	{
//		get;
//		set;
//	}
//
//	string Name
//	{
//		get;
//		set;
//	}
//
//	Vector3 Size
//	{
//		get;
//		set;
//	}
//

	#endregion

}