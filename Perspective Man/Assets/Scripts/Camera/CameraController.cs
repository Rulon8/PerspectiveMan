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
	public const float X_OFFSET_2D = 6f;
	public const float X_OFFSET_3D = -8f;
	public const float Y_POSITION = 0f;
	public const float Z_POSITION_2D = -14f;
	public const float Z_POSITION_3D = 0f;

	#endregion

	#region Variables

	[SerializeField] private bool _changingPerspective;
	[SerializeField] private float _movementChangeSpeed;
	[SerializeField] private float _perspectiveChangeSpeed;
	[SerializeField] private float _forwardSpeed;
	private bool _2DMode;
	private Camera _camera;
	private GameObject _player;
	private Quaternion _rotation;
	private Matrix4x4 _orthographic;
	private Matrix4x4 _perspective;
	private static CameraController _instance;

	#endregion

	#region Properties

	public bool Is2DMode
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

	public Quaternion Rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
		}
	}

	public float ForwardSpeed
	{
		get
		{
			return _forwardSpeed;
		}
		set
		{
			_forwardSpeed = value;
		}
	}

	public static CameraController Instance
	{
		get
		{
			return _instance;
		}
	}

	#endregion

	#region Methods

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
		_camera.farClipPlane = 100f;
		_orthographic = Matrix4x4.Ortho(-_camera.orthographicSize * _camera.aspect, _camera.orthographicSize * _camera.aspect, -_camera.orthographicSize, _camera.orthographicSize, _camera.nearClipPlane, _camera.farClipPlane);
		_perspective = Matrix4x4.Perspective(_camera.fieldOfView, _camera.aspect, _camera.nearClipPlane, _camera.farClipPlane);
		_player = GameObject.FindWithTag("Player");
		_changingPerspective = false;
		_movementChangeSpeed = 30f;
		_perspectiveChangeSpeed = 0.05f;
		_forwardSpeed = 2f;

		if (_2DMode)
		{
			transform.position = new Vector3(_player.transform.position.x + X_OFFSET_2D, Y_POSITION, Z_POSITION_2D);
			_camera.orthographic = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_changingPerspective)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (_2DMode)
				{
					StartCoroutine(ChangeDimension(-X_TRANSLATION, Y_TRANSLATION, Z_TRANSLATION, X_ROTATION, Y_ROTATION, Z_ROTATION, _camera.projectionMatrix, _perspective, _perspectiveChangeSpeed));
				}
				else
				{
					StartCoroutine(ChangeDimension(X_TRANSLATION, Y_TRANSLATION, -Z_TRANSLATION, X_ROTATION, -Y_ROTATION, Z_ROTATION, _camera.projectionMatrix, _orthographic, _perspectiveChangeSpeed));
				}
			}

			if (!_changingPerspective)
			{
				transform.Translate(_forwardSpeed * Time.deltaTime, 0, 0, Space.World); 
			}
		}
	}

	/// <summary>
	/// Coroutine for dimensional change. Pauses time, moves the camera and changes the camera's perspective according to the parameters.
	/// </summary>
	IEnumerator ChangeDimension(float xTrans, float yTrans, float zTrans, float xRot, float yRot, float zRot, Matrix4x4 src, Matrix4x4 dest, float duration)
	{
		_changingPerspective = true;
		Time.timeScale = 0; //Pauses time while perspective changes

		if (_2DMode)
		{
			for (float i = 0f; i < 1f; i += _perspectiveChangeSpeed)
			{
				_camera.projectionMatrix = MatrixLerp(src, dest, i);
				yield return 1;
			}
			_camera.projectionMatrix = dest;

			for (float i = 0f; i < _movementChangeSpeed; i += 1f)
			{
				transform.Translate(xTrans / _movementChangeSpeed, 0, zTrans / _movementChangeSpeed, Space.World);
				transform.Rotate(0, yRot / _movementChangeSpeed, 0);
				yield return null;
			}
		}
		else
		{
			for (float i = 0f; i < _movementChangeSpeed; i += 1f)
			{
				transform.Translate(xTrans / _movementChangeSpeed, 0, zTrans / _movementChangeSpeed, Space.World);
				transform.Rotate(0, yRot / _movementChangeSpeed, 0);
				yield return null;
			}

			for (float i = 0f; i < 1f; i += _perspectiveChangeSpeed)
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

	/// <summary>
	/// Brain-fucking math. DO NOT TOUCH!
	/// </summary>
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
