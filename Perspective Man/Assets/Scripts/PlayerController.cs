using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static float distanceTraveled = 0;

    [SerializeField] private float _jumpSpeed = 20f;
    [SerializeField] private float _movementSpeed = 5f;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jumpForce = new Vector3(0, _jumpSpeed, 0);
            _rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        }
        transform.Translate(_movementSpeed * Time.deltaTime, 0f, 0f);
        distanceTraveled = transform.localPosition.x;
    }
}
