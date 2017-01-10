using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	#region Variables

	[SerializeField] private bool _2DMode;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _player;

	#endregion

	#region Properties

	public bool TwoDMode
	{
		get
		{
			return _2DMode;
		}
		set
		{
			_2DMode = value;
		}
	}

	#endregion

	#region Methods

	// Use this for initialization
	void Start () {
		_2DMode = false;
		_camera = GetComponent<Camera>();
		_player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.E))
		{
			ChangeDimension();
		}

		if (_2DMode)
		{
			transform.position = new Vector3(_player.transform.position.x + 6, 0, -10);
		}
		else
		{
			transform.position = new Vector3(_player.transform.position.x - 8, 0, 0);
		}
	}

	void ChangeDimension() 
	{
		if (_2DMode)
		{
			transform.position = new Vector3(_player.transform.position.x - 8, 0, 0);
			transform.Rotate(0, 90, 0);
			_camera.orthographic = false;
			_2DMode = false;
		}
		else
		{
			transform.position = new Vector3(_player.transform.position.x + 6, 0, -10);
			transform.Rotate(0, -90, 0);
			_camera.orthographic = true;
			_2DMode = true;
		}
	}

	#endregion
}
