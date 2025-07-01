using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : BaseController
{
    [Header("������Ʈ")]
    private Animator _animator;

    [Header("�÷��̾�� �� �Ÿ�")]
    private float _range = 4.5f;

    [Header("�÷��̾� �׾��� ���")]
    private Image _playerDiePanel;

    public float Speed
    {
        get { return _animator.GetFloat(Define.Speed); }
        set { _animator.SetFloat(Define.Speed, value); }
    }

    // ���� �ִϸ��̼� ���� �Լ�
    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    //public GameObject boss { get; set; }
    private Vector3 _stagePlayerSpawn = new Vector2(-1.43f, 1.39f); // �÷��̾� ���� ��ġ
    private Vector3 _bossStagePlayerSpawn = new Vector3(-4f, 1.8f, 0f); // �÷��̾� ���� ��ġ

    public bool isBossStage { get; set; }

    public BattleState battleState = BattleState.None;
    public Vector3 BossBattleTargetPos = new Vector3(-0.9f, 1.8f, 0f); // �߾� ��ǥ ��ġ    

    private float moveSpeed = 1.3f;


    // �ǰ� �ִϸ��̼� ���� �Լ�
    void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    // ���� �ִϸ��̼� ���� �Լ�
    void Die()
    {

        Debug.Log("���ΰ��� �׾����ϴ�. ó�����ּ���!");
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
            Destroy(this.gameObject); // �ߺ� ������ �Ÿ� ����
        }


        _animator = GetComponent<Animator>();
        /*
                _playerDiePanel = GameObject.Find("PlayerDiePanel - Panel").GetComponent<Image>();
        */
        // �� �̸��� "Boss"�� ���ԵǾ� ������ ���� ���������� ����
        isBossStage = SceneManager.GetActiveScene().name.Contains(Define.BossStageScene);

        // �޸��� ����
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
            // ó�� ������ ��츸 ����
            StageManager.Instance.Player = ObjectManager.Instance.Spawn<PlayerController>(_bossStagePlayerSpawn);
            DontDestroyOnLoad(StageManager.Instance.Player.gameObject);
        }
        else // �ߺ��̶��
        {
            // ������ ��ġ�� �̵�
            StageManager.Instance.Player.transform.position = _bossStagePlayerSpawn;
        }

        battleState = BattleState.MoveToCenter;
        _animator.StopPlayback();
        isBossStage = true;

        ResetForBossSTage(); // ���� �� �� �ʱ�ȭ
    }

    void Update()
    {
        if (isBossStage == true)
        {
            HandleBossStage();
        }
        else
        {
            // if (�� �������� ���� �� ����)
            FindDistance();
        }
    }

    // Enemy �±� ���� ������Ʈ ã�� �Ÿ� ���ϴ� �Լ�
    void FindDistance()
    {
        // ���� ��ó�� �ִ��� Ȯ�� ����
        bool isNearEnemy = false;

        // Enemy �±װ� ���� ������Ʈ�� ��� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Define.EnemyTag);

        // enemies �迭�� ��������� foreach ���� ��ü�� �ƿ� ������ �� ��
        foreach (GameObject enemy in enemies)
        {
            // �÷��̾�� �� ���� �Ÿ� ���ϱ�
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            //Debug.Log($"�÷��̾�� �� ���� �Ÿ� : {distance}");
            if (distance <= _range)
            {
                isNearEnemy = true;
                break;
            }
        }

        if (isNearEnemy == true)
        {
            // idle ����
            IsAttacking = true;
            Speed = 0;
        }
        else
        {
            IsAttacking = false;
            Speed = 1; // �޸��� ������
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

    // �÷��̾ ���� �ϴ� ����� �̵��ϴ� �Լ�
    void MoveToBossBattlePosition()
    {
        // y, z �����ϰ� x�� �̵�
        Vector3 targetPos = new Vector3(BossBattleTargetPos.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        Speed = 1;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Speed = 0f;
            battleState = BattleState.PlayerTurn;
            Debug.Log("�÷��̾� ���� ��ġ ����");
        }
    }

    private void HandlePlayerTurn()
    {
        if (StageManager.Instance.boss == null)
            return;

        bool isNearEnemy = StageManager.Instance.boss.battleState == BattleState.WaitTurn;

        if (isNearEnemy == true)
        {
            // idle ����
            IsAttacking = true;
            Speed = 0;
        }
        else
        {
            IsAttacking = false;
            Speed = 1; // �޸��� ������
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
        Debug.Log($"���� �÷��̾� HP : {GameManager.Instance[PlayerStat.CurrentHp]}");

        if (GameManager.Instance[PlayerStat.CurrentHp] <= 0)
            Die();
        else
        {
            GetHit();
        }
    }

    // �ʱ�ȭ
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