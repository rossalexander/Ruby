using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    private float _speed = 1f;
    private bool _vertical;

    private float changeTime = 3f;

    private float timer;
    private int direction = 1;

    private RubyController player;

    private bool broken = true;

    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private AudioClip fixedClip;

    private void Start()
    {
        if (!broken) return;
        
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();

        // Initialize timer
        timer = changeTime;
    }

    private void Update()
    {
        // Decrement timer
        timer -= Time.deltaTime;

        // Change direction once timer runs out and reset timer
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        if (!broken) return;

        var position = _rb.position;

        if (_vertical)
        {
            position.y += Time.deltaTime * _speed * direction;
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", direction);
        }
        else
        {
            position.x += Time.deltaTime * _speed * direction;
            _animator.SetFloat("moveX", direction);
            _animator.SetFloat("moveY", 0);
        }

        _rb.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        player = other.gameObject.GetComponent<RubyController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        _audioSource.PlayOneShot(fixedClip);
        broken = false;
        smokeEffect.Stop();
        _animator.SetBool("fixed", true);
        _rb.simulated = false;
    }
}