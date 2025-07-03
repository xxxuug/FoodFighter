using EnumDef;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossStageController : BaseController
{
    private Animator _animator;

   // [SerializeField] StageData _stageData;

    [SerializeField] Transform _player;
    public BattleState battleState = BattleState.None;
    [SerializeField] AttackController _attackController;
    [SerializeField] private GameObject rewardPopup; // 보상
    [SerializeField] private float attackDelay = 1.5f ; // 공격 텀

    BossStageInfo _stagInfo;

    private int rewardGold;
    private int rewardDiamond;

    private bool _isDead = false;
    [Header("Status")]
    private float _currentHP;
    private float _maxHP;    
    private float _speed = 0.3f;
    private float _damage;
    public float damage { get => _damage; }


    private bool _isAttacking = false;

    [Header("보스 이동 관련")]
    [SerializeField] private Vector3 _targetPosition = new Vector3(0.6f, 2f, 0f); // 중앙 목표 위치
    [SerializeField] private float _moveSpeed = 0.5f;

    private GameObject _hpUI;
    private Image _hpImage;
    private TMP_Text _hpText;

    [SerializeField] private BossStageInfo _stageInfo;

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    public bool IsSpellSkill = false;

    [Header("스킬 쿨타임 시간")]
    public float kSpellSkillDelay = 1f;
    private float mCurSkillDelay = 0f;

    [Header("스킬 투사체 프리팹")]
    public GameObject Bullet;

    protected override void Initialize()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
        _animator = GetComponent<Animator>();

        ObjectManager.Instance.ResourceAllLoad();

        _isDead = false;
        GetComponent<Collider2D>().enabled = true;
        
        _attackController.bossStageController = this;

        mCurSkillDelay = 0f;

        //_damage = _stageData.Damage;
        //_currentHP = _stageData.CurrentHp;

        // Init(1, bossType);
    }

    private void Start()
    {
        _hpUI = GameObject.Find("BossHpUI");
        _hpImage = GameObject.Find("BossHpBar - Image")?.GetComponent<Image>();
        _hpText = GameObject.Find("BossHpBar Text - Text")?.GetComponent<TMP_Text>();

        Debug.Log($"Hp UI 연결 _hpUI: {_hpUI != null}, _hpImage: {_hpImage != null}, _hpText: {_hpText != null}");

        if (_hpUI != null)
            _hpUI.SetActive(false);
    }

    public void SetData(BossStageInfo.Data _data)
    {
        _animator = GetComponent<Animator>();
        _isDead = false;
        GetComponent<Collider2D>().enabled = true;

        if (_stageInfo == null)
        {
            Debug.LogError("BossStageInfo가 설정되지 않았습니다.");
            return;
        }
/*
        var data = _stageInfo.list.Find(d => d.Stage == _data.Stage);
        if (data == null)
        {
            Debug.LogError($"BossStageInfo에 Stage {stage} 데이터가 없습니다.");
            return;
        }
*/
        // 초기화
        _currentHP = _data.CurrentHp;
        _maxHP = _data.MaxHp;
        _damage = _data.Damage;
        rewardGold = _data.RewardGold;
        rewardDiamond = _data.RewardDiamond;
        _targetPosition = _data.TargetPosition;

        transform.position = _data.SpawnPosition;

        Debug.Log($"보스 Init 완료! HP: {_currentHP}, 보상: {rewardGold}/{rewardDiamond}");
    }


    private void Update()
    {
        if (_isDead || _player == null || !gameObject.activeInHierarchy) return;

        if (_player == null) return;

        if (Utils.GetDistance(_player.transform, transform) >= 0.5f)
        {
            mCurSkillDelay += Time.deltaTime;

            if (mCurSkillDelay >= kSpellSkillDelay){
                mCurSkillDelay -= kSpellSkillDelay;
                IsSpellSkill = true;

                _animator.SetTrigger("UseSkill");
            }

            if(IsSpellSkill == false)
                Move();
        }
        else
        {
            IsAttacking = true;
        }
        /*
                switch (battleState)
                {
                    case BattleState.MoveToCenter:
                        MoveToBossBattlePosition();
                        break;

                    case BattleState.BossTurn:
                        if (!_isAttacking) StartCoroutine(HandleBossTurn());
                        break;
                }



        // 디버그용 바로 죽이기
        if (Input.GetKeyUp(KeyCode.P) == true)
        {
            TakeDamage(_currentHP);
            Debug.Log("디버그용 보스 사망");
        }

        if (battleState == BattleState.BossTurn && !_isAttacking)
        {
            StartCoroutine(HandleBossTurn());
        }
        */
    }
    void Move()
    {
        Vector3 targetPos = new Vector3(_targetPosition.x, transform.position.y, transform.position.z);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            battleState = BattleState.WaitTurn;
            //Debug.Log("보스 전투 위치 도착");

            _hpUI.SetActive(true);
            UpdateHP();
        }
    }
/*
    // 보스가 전투하는 가운데로 이동하는 함수
    void MoveToBossBattlePosition()
    {
        Vector3 targetPos = new Vector3(_targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            battleState = BattleState.WaitTurn;
            Debug.Log("보스 전투 위치 도착");

            _hpUI.SetActive(true);
            UpdateHP();
        }
    }
*/
/*
    IEnumerator HandleBossTurn()
    {
        _isAttacking = true;

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(attackDelay);

        PlayerController player = StageManager.Instance.Player;
        if (player != null)
        {
            battleState = BattleState.None;
            player.battleState = BattleState.PlayerTurn;
        }

        _isAttacking = false;
    }
*/
    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        Debug.Log($"보스 현재 HP: {_currentHP}");
        UpdateHP();

        if (_currentHP <= 0)
        {
            Die();
        }
        else
        {
            _animator?.SetTrigger("GetHit");
        }
    }

    public void Die()
    {
        Debug.Log("보스 사망");
        //  _anim.SetTrigger("Die");

        _hpUI.SetActive(false);

        int nextStageIndex = GameManager.Instance.CurBossStageIndex + 1;
        GameManager.Instance.BossStageOpen[nextStageIndex] = true;

        Invoke(nameof(Despawn), 0.5f);
        OnDefeted();
    }

    void OnDefeted()
    {
        // 보상 지급
        GameManager.Instance.AddGold(rewardGold);
        GameManager.Instance.AddDiamond(rewardDiamond);

        if (rewardPopup != null)
            rewardPopup.SetActive(true);

        Debug.Log($"보상 지급 완료! 골드: {rewardGold}, 다이아몬드: {rewardDiamond}");
        
        StageManager.Instance.Player.SetStage();

        // 게임 씬으로 복귀
        SceneManager.LoadScene(Define.GameScene);
    }

    void UpdateHP()
    {
        if (_hpImage != null && _maxHP > 0)
        {
            _hpImage.fillAmount = _currentHP / _maxHP;
            _hpText.text = $"{_currentHP}";
        }
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(this);
    }

    // AttackRange 오브젝트 애니메이션 이벤트 호출
    public void EnableAttack() => _attackController.EnableAttack();

    public void DisableAttack() => _attackController.DisableAttack();

    public void StartSpellSkill()
    {
        var boss = GetComponentInParent<BossStageController>();
        boss.IsSpellSkill = true;
        Debug.Log("Skill~~");
    }

    public void UseSkill()
    {
        Debug.Log($"Fire!!! {_damage} Damage");

        UnityEngine.Object obj = Resources.Load("@Prefabs/BossBullet");
        var bullet = Instantiate(obj).GetComponent<BossBullet>();
        bullet.transform.position = transform.position;
        bullet.SetDamage(_damage + 15);

        /*
        var bullet = Instantiate(Bullet).GetComponent<BossBullet>();
        bullet.transform.position = transform.position;
        bullet.SetDamage(_damage);
        */
    }

    public void EndSpellSkill()
    {
        var boss = GetComponentInParent<BossStageController>();
        boss.IsSpellSkill = false;
        Debug.Log("Skill~~");
    }
}