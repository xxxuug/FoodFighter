using UnityEngine;

public class EnemyController : BaseController
{
    [SerializeField] Transform _player;
    private Animator _animator;
    private float _speed = 0.3f;

    [Header("Status")]
    private float _hp = 10;

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    public void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    public void Die()
    {
        _animator.SetTrigger(Define.Die);
        Invoke(nameof(Despawn), 0.5f);
    }

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindWithTag("Player")?.transform;

    }

    void Update()
    {
        if (_player == null) return;

        if (Distance.GetDistance(_player.transform, transform) >= 0.5f)
        {
            Move();
        }
        else
        {
            IsAttacking = true;
        }
    }

    void Move()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        Debug.Log("Àâ¸÷ HP : " + _hp);

        if (_hp <= 0)
            Die();
        else
            GetHit();
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(this);
    }
}
