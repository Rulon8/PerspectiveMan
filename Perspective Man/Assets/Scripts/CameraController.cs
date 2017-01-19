// Project: <Name Of Project>
/// \file CameraController.cs
/// \copyright (C) 2017, FG!P. All rights reserved.
/// \date 2017-01-10
/// \author Rulon
/// <summary> 
/// Sets main camera's initial values and controls changes in dimension.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	#region Constants

	public const float X_TRANSLATION = 14f;
	public const float Y_TRANSLATION = 0f;
	public const float Z_TRANSLATION = 14f;
	public const float X_ROTATION = 0f;
	public const float Y_ROTATION = 90f;
	public const float Z_ROTATION = 0f;

	#endregion

	#region Variables

	[SerializeField] private bool _2DMode;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _player;
	[SerializeField] private bool _changingPerspective;
	[SerializeField] private float _changeSpeed;
	[SerializeField] private Rigidbody _playerRigidbody;
	[SerializeField] private Matrix4x4 _orthographic;
	[SerializeField] private Matrix4x4 _perspective;
	[SerializeField] private float _aspect;
	[SerializeField] private float _fieldOfView;
	[SerializeField] private float _nearClipPlane;
	[SerializeField] private float _farClipPlane;
	[SerializeField] private float _orthographicViewSize;

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
		_camera.farClipPlane = 100f;
		_orthographic = Matrix4x4.Ortho(-_camera.orthographicSize * _camera.aspect, _camera.orthographicSize * _camera.aspect, -_camera.orthographicSize, _camera.orthographicSize, _camera.nearClipPlane, _camera.farClipPlane);
		_perspective = Matrix4x4.Perspective(_camera.fieldOfView, _camera.aspect, _camera.nearClipPlane, _camera.farClipPlane);
		_player = GameObject.FindWithTag("Player");
		_changingPerspective = false;
		_changeSpeed = 30f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_changingPerspective)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (_2DMode)
				{
					StartCoroutine(ChangeDimension(-X_TRANSLATION, Y_TRANSLATION, Z_TRANSLATION, X_ROTATION, Y_ROTATION, Z_ROTATION, _camera.projectionMatrix, _perspective, 2f));
				}
				else
				{
					StartCoroutine(ChangeDimension(X_TRANSLATION, Y_TRANSLATION, -Z_TRANSLATION, X_ROTATION, -Y_ROTATION, Z_ROTATION, _camera.projectionMatrix, _orthographic, 2f));
				}
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

	IEnumerator ChangeDimension(float xTrans, float yTrans, float zTrans, float xRot, float yRot, float zRot, Matrix4x4 src, Matrix4x4 dest, float duration)
	{
		_changingPerspective = true;
		Time.timeScale = 0; //Pauses time while perspective changes

		if (_2DMode)
		{
			for (float i = 0f; i < 1f; i += 0.05f)
			{
				_camera.projectionMatrix = MatrixLerp(src, dest, i);
				yield return 1;
			}
			_camera.projectionMatrix = dest;

			for (float i = 0f; i < _changeSpeed; i += 1f)
			{
				transform.Translate(xTrans / _changeSpeed, 0, zTrans / _changeSpeed, Space.World);
				transform.Rotate(0, yRot / _changeSpeed, 0);
				yield return null;
			}
		}
		else
		{
			for (float i = 0f; i < _changeSpeed; i += 1f)
			{
				transform.Translate(xTrans / _changeSpeed, 0, zTrans / _changeSpeed, Space.World);
				transform.Rotate(0, yRot / _changeSpeed, 0);
				yield return null;
			}

			for (float i = 0f; i < 1f; i += 0.05f)
			{
				_camera.projectionMatrix = MatrixLerp(src, dest, i);
				yield return 1;
			}

			_camera.projectionMatrix = dest;
		}

		Time.timeScale = 1;  //Unpause time
		_changingPerspective = false;
		_2DMode = !_2DMode;
	}

	static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
	{
		Matrix4x4 ret = new Matrix4x4();
		for (int i = 0; i < 16; i++)
		{
			ret[i] = Mathf.Lerp(from[i], to[i], time);
		}
		return ret;
	}

	#endregion
}
