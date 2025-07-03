using System.Collections;
using TMPro;
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
    public float _speed = 1f; // 몬스터마다 다르게

    [Header("Status")]
    [SerializeField] private float _initHp;
    private float _currentHp;
    public float _damage;

    [Header("Hit Damage")]
    public GameObject HitDamagePrefab;
    private GameObject _hitDamage;
    private bool _isDead = false;

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
        if (StageManager.Instance.boss != null)
            StageManager.Instance.RemoveEnemy(this);

        _animator = GetComponent<Animator>();

        _player = GameObject.FindWithTag(Define.PlayerTag)?.transform;

        _attackController.enemyController = this;
    }

    private void OnEnable()
    { 
        _currentHp = (int)_initHp;
        _damage = (int)_damage;
        _isDead = false;

        ObjectManager.Instance.Despawn(_hitDamage);
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

    IEnumerator HitDamageUp()
    {
        Vector3 startPos = _hitDamage.GetComponent<RectTransform>().localPosition;
        Vector3 endPos = startPos + Vector3.up * 0.5f;

        float t = 0;
        float duration = 0.3f;

        while (t < duration)
        {
            _hitDamage.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPos, endPos, t / duration);
            t += Time.deltaTime;

            yield return null;
        }
        ObjectManager.Instance.Despawn(_hitDamage);
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _currentHp -= damage;

        _hitDamage = PoolManager.Instance.GetEffectObject(HitDamagePrefab, Vector3.zero);
        _hitDamage.GetComponent<RectTransform>().transform.SetParent(this.transform.Find(Define.UI));

        _hitDamage.GetComponent<RectTransform>().localPosition = new Vector3(0, 0.15f, 0);
        _hitDamage.GetComponent<TMP_Text>().text = $"{damage}";
        StartCoroutine(HitDamageUp());
        //Debug.Log($"[TakeDamage] {this.gameObject.name} 피격 데미지 텍스트 생성");
        // 적이 사라지는 것보다 텍스트를 더 먼저 사라지게

        if (_currentHp <= 0)
        {
            _isDead = true;
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
        }
        else
            GetHit();
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(_hitDamage);
        StageManager.Instance.HuntEnemy(this);        
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
