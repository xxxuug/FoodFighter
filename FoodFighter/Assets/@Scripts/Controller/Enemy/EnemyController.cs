using UnityEngine;

public class EnemyController : BaseController
{
    [SerializeField] Transform _player;
    [SerializeField] AttackController _attackController;
    private Animator _animator;
    private float _speed = 0.3f;

    [Header("Status")]
    private float _initHp = 10;
    private float _currentHp;
    public float _damage = 5;

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
        DropGold();

        Invoke(nameof(Despawn), 0.5f);
    }

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindWithTag("Player")?.transform;

    }

    private void OnEnable()
    {
        _currentHp = _initHp;
        //Debug.Log("리스폰 잡몹 현재 HP : " + _hp);
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
        _currentHp -= damage;
        //Debug.Log("잡몹 HP : " + _hp);

        if (_currentHp <= 0)
            Die();
        else
            GetHit();
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(this);
    }

    void DropGold()
    {
        int rndGoldCount = Random.Range(4, 8);
        for (int i = 0; i < rndGoldCount; i++)
        {
            PoolManager.Instance.GetObject<GoldController>(transform.position);
        }
    }

    public void MultipleHp(float amount)
    {
        _initHp *= amount;
    }

    // AttackRange 오브젝트 애니메이션 이벤트 호출
    public void EnableAttack() => _attackController.EnableAttack();

    public void DisableAttack() => _attackController.DisableAttack();
}
