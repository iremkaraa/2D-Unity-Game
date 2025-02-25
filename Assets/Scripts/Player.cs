using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;

    private Vector2 _topLeftCorner;
    private Vector2 _bottomRightCorner;

    public static event Action OnAppleCollected;
    public static event Action OnTimeCollected;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _topLeftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        _bottomRightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
    }

    private void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(_movement.magnitude >= 0.1f)
            _animator.SetBool("IsRunning",true);
        else
        {
            _animator.SetBool("IsRunning", false);
        }
        
        if(_movement.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if(_movement.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        BorderChecker();
    }

    private void BorderChecker()
    {
        if (transform.position.x < _topLeftCorner.x)
        {
            transform.position = new Vector2(_bottomRightCorner.x, transform.position.y);
        }
        else if (transform.position.x > _bottomRightCorner.x)
        {
            transform.position = new Vector2(_topLeftCorner.x, transform.position.y);
        }
        
        
        if (transform.position.y > _topLeftCorner.y)
        {
           
            transform.position = new Vector2(transform.position.x, _bottomRightCorner.y);
        }
        else if (transform.position.y < _bottomRightCorner.y)
        {
            
            transform.position = new Vector2(transform.position.x, _topLeftCorner.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            OnAppleCollected?.Invoke();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("TimeCollectible"))
        {
            OnTimeCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movement.normalized * (_speed * Time.fixedDeltaTime);
    }
}
