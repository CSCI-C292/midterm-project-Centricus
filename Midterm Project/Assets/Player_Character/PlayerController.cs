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
	private float _horizontalInputDirection;
    bool _jump = false;
	const float _PlatformCheckRadius = .05f;
	private bool _OnPlatform;
	private Rigidbody2D _RigidBody;
	private string _facing = "Right";
	public Animator _animator;
	private Vector3 _CurrentVelocity = Vector3.zero;
	private Vector3 _CurrentAcceleration = Vector3.zero;
	

	private void Awake()
	{
		_RigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update() 
	{
		// Get Inputs
        _horizontalInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) _jump = true;
		if (Input.GetKeyDown("x"))
		{
			Attack();
		}

		// Change animation for running
		_animator.SetFloat("Speed", Mathf.Abs(_horizontalInputDirection));

		// Check if falling to change animation
		if (_CurrentVelocity.y < 0) _animator.SetBool("Falling", true);
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
					_animator.SetBool("Jump", false);	// End Jump animation
				}
			}
		}

		// Call Move() and reset _jump
        Move(_horizontalInputDirection * _runSpeed * Time.fixedDeltaTime);
        _jump = false;
	}

	private void Move(float move)
	{
		// Movement
		Vector3 targetVelocity = new Vector2(move * 10f, _RigidBody.velocity.y);
		_RigidBody.velocity = Vector3.SmoothDamp(_RigidBody.velocity, targetVelocity, ref _CurrentVelocity, _Smoothing);

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

	private void Attack() 
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
}
