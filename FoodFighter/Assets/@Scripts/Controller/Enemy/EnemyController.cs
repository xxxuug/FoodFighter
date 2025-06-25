using UnityEngine;

public enum EnemyKind
{
    Minion1, Minion2, Minion3, Minion4, Minion5,
}

public class EnemyController : BaseController
{
    [SerializeField] EnemyKind enemyKind;
    [SerializeField] Transform _player;
    [SerializeField] AttackController _attackController;
    private Animator _animator;
    private float _speed = 0.5f;

    [Header("Status")]
    [SerializeField] private float _initHp;
    private float _currentHp;
    public float _damage;

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
        _currentHp = (int)_initHp;
        _damage = (int)_damage;
        Debug.Log($"{gameObject.name} 리스폰 잡몹 현재 HP : " + _currentHp);
        Debug.Log($"{gameObject.name} 리스폰 잡몹 현재 공격력 : " + _damage);
    }

    void Update()
    {
        if (_player == null) return;

        if (Utils.GetDistance(_player.transform, transform) >= 0.5f)
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

        if (_currentHp <= 0)
        {
            Die();

            switch (enemyKind)
            {
                case EnemyKind.Minion1:
                    _initHp *= 1.1f;
                    _damage *= 1.1f;
                    break;
                case EnemyKind.Minion2:
                    _initHp *= 1.1f;
                    _damage *= 1.1f;
                    break;
                case EnemyKind.Minion3:
                    _initHp *= 1.2f;
                    _damage *= 1.2f;
                    break;
                case EnemyKind.Minion4:
                    _initHp *= 1.4f;
                    _damage *= 1.4f;
                    break;
                case EnemyKind.Minion5:
                    _initHp *= 2f;
                    _damage *= 2f;
                    break;
            }

            Debug.Log($"{gameObject.name}의 증가한 체력 : {_initHp}");
        }
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
