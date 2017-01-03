using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBlock : MonoBehaviour, IBlock {

	#region Variables

	[SerializeField] private BlockType _blockType;

	#endregion

	#region Properties

	public BlockType BlockType
	{
		get
		{
			return _blockType;
		}
	}

	#endregion
	
}
