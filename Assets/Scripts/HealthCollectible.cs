using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    private RubyController _controller;
    [SerializeField] private AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _controller = other.GetComponent<RubyController>();
        if (!_controller) return;

        if (_controller.Health >= _controller.maxHealth) return;
        _controller.ChangeHealth(1);
        _controller.PlaySound(collectedClip);
        Destroy(gameObject);
    }
}