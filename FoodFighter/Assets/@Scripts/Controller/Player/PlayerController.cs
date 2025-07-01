using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : BaseController
{
    [Header("컴포넌트")]
    private Animator _animator;

    [Header("플레이어와 적 거리")]
    private float _range = 4.5f;

    [Header("플레이어 죽었을 경우")]
    private Image _playerDiePanel;

    public float Speed
    {
        get { return _animator.GetFloat(Define.Speed); }
        set { _animator.SetFloat(Define.Speed, value); }
    }

    // 공격 애니메이션 실행 함수
    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    //public GameObject boss { get; set; }
    private Vector3 _stagePlayerSpawn = new Vector2(-1.43f, 1.39f); // 플레이어 스폰 위치
    private Vector3 _bossStagePlayerSpawn = new Vector3(-4f, 1.8f, 0f); // 플레이어 시작 위치

    public bool isBossStage { get; set; }

    public BattleState battleState = BattleState.None;
    public Vector3 BossBattleTargetPos = new Vector3(-0.9f, 1.8f, 0f); // 중앙 목표 위치    

    private float moveSpeed = 1.3f;


    // 피격 애니메이션 실행 함수
    void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    // 죽음 애니메이션 실행 함수
    void Die()
    {

        Debug.Log("주인공이 죽었습니다. 처리해주세요!");
        return;
    }

    protected override void Initialize()
    {
        if (StageManager.Instance.Player == null)
        {
            StageManager.Instance.Player = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject); // 중복 생성된 거면 삭제
        }


        _animator = GetComponent<Animator>();
        /*
                _playerDiePanel = GameObject.Find("PlayerDiePanel - Panel").GetComponent<Image>();
        */
        // 씬 이름에 "Boss"가 포함되어 있으면 보스 스테이지로 간주
        isBossStage = SceneManager.GetActiveScene().name.Contains(Define.BossStageScene);

        // 달리기 유지
        Speed = 1;

        /*
                _playerDiePanel.gameObject.SetActive(false);
        */
    }
    
    public void SetStage()
    {
        transform.position = _stagePlayerSpawn;
        isBossStage = false;
    }

    public void SetBossStage()
    {
        if (StageManager.Instance.Player == null)
        {
            // 처음 시작한 경우만 생성
            StageManager.Instance.Player = ObjectManager.Instance.Spawn<PlayerController>(_bossStagePlayerSpawn);
            DontDestroyOnLoad(StageManager.Instance.Player.gameObject);
        }
        else // 중복이라면
        {
            // 있으면 위치만 이동
            StageManager.Instance.Player.transform.position = _bossStagePlayerSpawn;
        }

        battleState = BattleState.MoveToCenter;
        _animator.StopPlayback();
        isBossStage = true;

        ResetForBossSTage(); // 상태 한 번 초기화
    }

    void Update()
    {
        if (isBossStage == true)
        {
            HandleBossStage();
        }
        else
        {
            // if (새 스테이지 진입 시 마다)
            FindDistance();
        }
    }

    // Enemy 태그 붙은 오브젝트 찾고 거리 구하는 함수
    void FindDistance()
    {
        // 적이 근처에 있는지 확인 여부
        bool isNearEnemy = false;

        // Enemy 태그가 붙은 오브젝트들 모두 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Define.EnemyTag);

        // enemies 배열이 비어있으면 foreach 루프 자체가 아예 실행이 안 됨
        foreach (GameObject enemy in enemies)
        {
            // 플레이어와 적 사이 거리 구하기
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            //Debug.Log($"플레이어와 적 사이 거리 : {distance}");
            if (distance <= _range)
            {
                isNearEnemy = true;
                break;
            }
        }

        if (isNearEnemy == true)
        {
            // idle 진입
            IsAttacking = true;
            Speed = 0;
        }
        else
        {
            IsAttacking = false;
            Speed = 1; // 달리기 재진입
        }
    }

    void HandleBossStage()
    {
        switch (battleState)
        {
            case BattleState.MoveToCenter:
                MoveToBossBattlePosition();
                break;

            case BattleState.PlayerTurn:
                HandlePlayerTurn();
                break;

            case BattleState.BossTurn:
                break;

            case BattleState.End:
                Speed = 0;
                break;
        }
    }

    // 플레이어가 전투 하는 가운데로 이동하는 함수
    void MoveToBossBattlePosition()
    {
        // y, z 고정하고 x만 이동
        Vector3 targetPos = new Vector3(BossBattleTargetPos.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        Speed = 1;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Speed = 0f;
            battleState = BattleState.PlayerTurn;
            Debug.Log("플레이어 전투 위치 도착");
        }
    }

    private void HandlePlayerTurn()
    {
        if (StageManager.Instance.boss == null)
            return;

        bool isNearEnemy = StageManager.Instance.boss.battleState == BattleState.WaitTurn;

        if (isNearEnemy == true)
        {
            // idle 진입
            IsAttacking = true;
            Speed = 0;
        }
        else
        {
            IsAttacking = false;
            Speed = 1; // 달리기 재진입
        }

        //StageManager.Instance.boss
        
        //battleState = BattleState.BossTurn;
        //boss.GetComponent<BossStageController>().battleState = BattleState.BossTurn;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Define.EnemyTag))
        {
            GetHit();
        }
    }

    public void TakeDamage(float damage)
    {
        GameManager.Instance[PlayerStat.CurrentHp] -= damage;
        Debug.Log($"현재 플레이어 HP : {GameManager.Instance[PlayerStat.CurrentHp]}");

        if (GameManager.Instance[PlayerStat.CurrentHp] <= 0)
            Die();
        else
        {
            GetHit();
        }
    }

    // 초기화
    public void ResetForBossSTage()
    {
        battleState = BattleState.MoveToCenter;
        IsAttacking = false;
        Speed = 1f;

        _animator.ResetTrigger("Attak");
        _animator.ResetTrigger(Define.GetHit);
        _animator.ResetTrigger(Define.Die);
    }

    IEnumerator FadePlayerDiePanel(float from, float to)
    {
        float time = 0f;
        float duration = 1f;
        Color c = _playerDiePanel.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / duration);
            _playerDiePanel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        _playerDiePanel.color = new Color(c.r, c.g, c.b, to);
    }
}