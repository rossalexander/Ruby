using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private RubyController _controller;

    private void OnTriggerStay2D(Collider2D other)
    {
        _controller = other.GetComponent<RubyController>();
        if (!_controller) return;

        _controller.ChangeHealth(-1);
    }
}