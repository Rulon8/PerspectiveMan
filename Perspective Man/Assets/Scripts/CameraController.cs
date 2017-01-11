using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	#region Variables

	[SerializeField] private bool _2DMode;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _player;
	[SerializeField] private bool _changingPerspective;
	[SerializeField] private float _changeSpeed;
	[SerializeField] private Rigidbody _playerRigidbody;

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

	public bool ChangingPerspective
	{
		get
		{
			return _changingPerspective;
		}
		set
		{
			_changingPerspective = value;
		}
	}

	#endregion

	#region Methods

	// Use this for initialization
	void Start () {
		_2DMode = false;
		_camera = GetComponent<Camera>();
		_camera.farClipPlane = 60f;
		_player = GameObject.FindWithTag("Player");
		_changingPerspective = false;
		_changeSpeed = 45f;
		_playerRigidbody = _player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_changingPerspective)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				StartCoroutine(ChangeDimension());
			}

			if (_2DMode)
			{
				transform.position = new Vector3(_player.transform.position.x + 6, 0, -14);
			}
			else
			{
				transform.position = new Vector3(_player.transform.position.x - 8, 0, 0);
			}
		}
	}

	IEnumerator ChangeDimension() 
	{
		_changingPerspective = true;
		_playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		if (_2DMode)
		{
			_camera.orthographic = false;
			_camera.farClipPlane = 10f;
			for (float i = 0f; i < _changeSpeed; i++)
			{
				_camera.farClipPlane += 50f / _changeSpeed;
				transform.Translate(-14f / _changeSpeed, 0, 14f / _changeSpeed, Space.World);
				transform.Rotate(0, 90f / _changeSpeed, 0);
				yield return null;
			}
			_2DMode = false;
		}
		else
		{
			for (float i = 0f; i < _changeSpeed; i++)
			{
				_camera.farClipPlane -= 50f / _changeSpeed;
				transform.Translate(14f / _changeSpeed, 0, -14f / _changeSpeed, Space.World);
				transform.Rotate(0, -90f / _changeSpeed, 0);
				yield return null;
			}
			_camera.farClipPlane = 20f;
			_camera.orthographic = true;
			_2DMode = true;
		}
		_playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		_changingPerspective = false;
	}

	#endregion
}
