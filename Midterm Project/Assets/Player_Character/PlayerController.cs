using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float _JumpSpeed = 900f;
	[SerializeField] private float _runSpeed = 40f;
	[Range(0, .3f)] [SerializeField] private float _Smoothing = .05f;
	[SerializeField] private LayerMask _PlatformMask;
	[SerializeField] private Transform _PlatformCheck;
	private float _hDirection;
    bool _jump = false;
	const float _PlatformCheckRadius = .05f;
	private bool _OnPlatform;
	private Rigidbody2D _RigidBody;
	private string _facing = "Right";
	public Animator _animator;
	private Vector3 _CurrentSpeed = Vector3.zero;

	private void Awake()
	{
		_RigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update() 
	{
		// Get Inputs
        _hDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) _jump = true;
		if (Input.GetKeyDown("x"))
		{
			if (_facing == "Right") 
			{
				gameObject.transform.Find("HitRight").GetComponent<GameObject>().SetActive(true);
			}
			else 
			{
				gameObject.transform.Find("HitLeft").GetComponent<GameObject>().SetActive(true);
			}
		}

		// Change animation for running
		_animator.SetFloat("Speed", Mathf.Abs(_hDirection));

		// Check if falling to change animation
		if (_RigidBody.velocity.y < 0) _animator.SetBool("Falling", true);
		else _animator.SetBool("Falling", false);


	}

	private void FixedUpdate()
	{
		bool wasGrounded = _OnPlatform;
		_OnPlatform = false;

		// Check for platforms under Player
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_PlatformCheck.position, _PlatformCheckRadius, _PlatformMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_OnPlatform = true;
				if (!wasGrounded)
				{
					// End Jump animation
					_animator.SetBool("Jump", false);
				}
			}
		}

		// Call Move() and reset _jump
        Move(_hDirection * _runSpeed * Time.fixedDeltaTime);
        _jump = false;
	}

	public void Move(float move)
	{
		// Movement
		Vector3 targetVelocity = new Vector2(move * 10f, _RigidBody.velocity.y);
		_RigidBody.velocity = Vector3.SmoothDamp(_RigidBody.velocity, targetVelocity, ref _CurrentSpeed, _Smoothing);

		// Facing
		if (move > 0) 
		{
			GetComponent<SpriteRenderer>().flipX = false;
			_facing = "Right";
		}
		else if (move < 0) 
		{
			GetComponent<SpriteRenderer>().flipX = true;
			_facing = "Left";
		}

		// Jumping
		if (_OnPlatform && _jump)
		{
			_OnPlatform = false;
			_RigidBody.AddForce(new Vector2(0f, _JumpSpeed));
			_animator.SetBool("Jump", true);
		}
	}
}
