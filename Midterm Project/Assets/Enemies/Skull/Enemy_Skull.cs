using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skull : MonoBehaviour
{
    [SerializeField] public float _hp;
	[SerializeField] private float _runSpeed = 10f;
	[Range(0, .3f)] [SerializeField] private float _Smoothing = .05f;
	[SerializeField] private LayerMask _PlatformMask;
	[SerializeField] private Transform _PlatformCheckLeft;
	[SerializeField] private Transform _PlatformCheckRight;
	const float _PlatformCheckRadius = .05f;
	private Rigidbody2D _RigidBody;
	public Animator _animator;
    private bool _active;
	private Vector3 _CurrentSpeed = Vector3.zero;
	private string _facing = "Right";

    private void Awake()
	{
		_RigidBody = GetComponent<Rigidbody2D>();
	}
    
    void Start()
    {
        _active = true;
    }

    void Update()
    {
        
    }

    private void FixedUpdate() {
        
        // Check if there is platform to left
		Collider2D[] leftColliders = Physics2D.OverlapCircleAll(_PlatformCheckLeft.position, _PlatformCheckRadius, _PlatformMask);
		if (leftColliders.Length == 0) _facing = "Right";
		Collider2D[] rightColliders = Physics2D.OverlapCircleAll(_PlatformCheckRight.position, _PlatformCheckRadius, _PlatformMask);
		if (rightColliders.Length == 0) _facing = "Left";

        if (_active)
        {
            _animator.SetBool("Active", true);
            int direction = 0;
            if (_facing == "Right") direction = 1;    
            else direction = -1;  

            // Call Move() and reset _jump
            Move(direction * _runSpeed * Time.fixedDeltaTime);
        }
        else{
            _animator.SetBool("Active", true);
        }
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
	}
}
