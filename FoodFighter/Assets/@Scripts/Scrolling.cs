using UnityEngine;

public class Scrolling : MonoBehaviour
{
    private PlayerController _player;
    private SpriteRenderer _spriteRenderer;
    private float _offset;
    private float _speed = 0.5f;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }

    void Update()
    {
        if (_player != null && _player.IsAttacking == false)
        {
            _offset += Time.deltaTime * _speed;
            _spriteRenderer.material.mainTextureOffset = new Vector2(_offset, 0);
        }
    }

}

