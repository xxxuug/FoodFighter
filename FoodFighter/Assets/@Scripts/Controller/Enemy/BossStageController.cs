using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//public class StageData
//{
//    public string Name;
//    public int CurrentHp;
//    public int MaxHP;
//    public int Damage;
//}

public class BossStageController : BaseController
{
    private Animator _anim;

   // [SerializeField] StageData _stageData;

    [SerializeField] Transform _player;
    public BattleState battleState = BattleState.None;

    [SerializeField] private GameObject rewardPopup; // 보상
    [SerializeField] private float attackDelay = 1.5f ; // 공격 텀

    BossStageInfo _stagInfo;

    private int rewardGold;
    private int rewardDiamond;

    private bool _isDead = false;
    [Header("Status")]
    private float _currentHP;
    private float _maxHP;
    private float _damage;
    private float _speed = 0.3f;

    private bool _isAttacking = false;

    [Header("보스 이동 관련")]
    [SerializeField] private Vector3 _targetPosition = new Vector3(0.6f, 2f, 0f); // 중앙 목표 위치
    [SerializeField] private float _moveSpeed = 0.5f;

    private GameObject _hpUI;
    private Image _hpImage;
    private TMP_Text _hpText;

    [SerializeField] private BossStageInfo _stageInfo;

    protected override void Initialize()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
        _anim = GetComponent<Animator>();

        ObjectManager.Instance.ResourceAllLoad();

        _isDead = false;
        GetComponent<Collider2D>().enabled = true;

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

    public void AllInit(int stage)
    {
        _anim = GetComponent<Animator>();
        _isDead = false;
        GetComponent<Collider2D>().enabled = true;

        if (_stageInfo == null)
        {
            Debug.LogError("BossStageInfo가 설정되지 않았습니다.");
            return;
        }

        var data = _stageInfo.list.Find(d => d.Stage == stage);
        if (data == null)
        {
            Debug.LogError($"BossStageInfo에 Stage {stage} 데이터가 없습니다.");
            return;
        }

        // 초기화
        _currentHP = data.CurrentHp;
        _maxHP = data.MaxHp;
        _damage = data.Damage;
        rewardGold = data.RewardGold;
        rewardDiamond = data.RewardDiamond;
        _targetPosition = data.TargetPosition;

        transform.position = data.SpawnPosition;

        Debug.Log($"보스 Init 완료! HP: {_currentHP}, 보상: {rewardGold}/{rewardDiamond}");
    }


    private void Update()
    {
        if (_isDead || _player == null || !gameObject.activeInHierarchy) return;

        switch(battleState)
        {
            case BattleState.MoveToCenter:
                MoveToBossBattlePosition();
                break;

            case BattleState.BossTurn:
                if (!_isAttacking) StartCoroutine(HandleBossTurn());
                break;
        }

        if(Input.GetKeyUp(KeyCode.P) == true)
        {
            TakeDamage(_currentHP);
        }

        if (battleState == BattleState.BossTurn && !_isAttacking)
        {
            StartCoroutine(HandleBossTurn());
        }
    }

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

    IEnumerator HandleBossTurn()
    {
        _isAttacking = true;

        _anim.SetTrigger("Attack");

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
            _anim?.SetTrigger("GetHit");
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
}