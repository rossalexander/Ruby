using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private Vector2 _lookDirection = new Vector2(1, 0);

    private Rigidbody2D _rb;
    private float _horizontal, _vertical;

    public int maxHealth = 5;
    [SerializeField] private int currentHealth;

    private bool _isInvincible;
    [SerializeField] private float timeInvincible = 1f;
    private float _invincibleTimer;

    public int Health => currentHealth;

    private float _speed = 3f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip throwClip;
    [SerializeField] private AudioClip damageClip;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        var move = new Vector2(_horizontal, _vertical);

        // If we are moving
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            // Set the look direction vector according our input
            _lookDirection.Set(move.x, move.y);
            _lookDirection.Normalize();
        }

        _animator.SetFloat("Look X", _lookDirection.x);
        _animator.SetFloat("Look Y", _lookDirection.y);
        _animator.SetFloat("Speed", move.magnitude);

        // Control player damage  
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
                _isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            var hit = Physics2D.Raycast(_rb.position + Vector2.up * 0.2f, _lookDirection, 1.5f,
                LayerMask.GetMask("NPC"));

            if (hit.collider)
            {
                NonPlayableCharacter character = hit.collider.GetComponent<NonPlayableCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x += _speed * _horizontal * Time.deltaTime;
        position.y += _speed * _vertical * Time.deltaTime;

        _rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            _animator.SetTrigger("Hit");
            PlaySound(damageClip);
            if (_isInvincible) return;

            _isInvincible = true;
            _invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Debug.Log(_currentHealth + "/" + maxHealth);
        UIHealthBar.Instance.SetValue(currentHealth / (float) maxHealth);
    }

    private void Launch()
    {
        var projectileObject = Instantiate(projectilePrefab, _rb.position + Vector2.up * 0.5f, Quaternion.identity);
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
        _animator.SetTrigger("Launch");
        PlaySound(throwClip);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}