using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class BossData
{
    public string name; // 보스 이름
    public int StageNum; // 스테이지 번호
    public float MaxHP; // 체력
    public float Damage; // 데미지
    public float RewardGold;// 골드 보상
    public float RewardDiamond; // 다이아몬드 보상
    public GameObject BossPrefab; // 보스 프리팹 
}

public class BossStageController : BaseController
{
    private float _currentHP;
    private float _damage;

    public BossData _bossData;
    private Animator _anim;

    private float _speed = 0.3f;
    [SerializeField] Transform _player;

    private bool _isDead = false;
    protected override void Initialize()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

            transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    public void Init(BossData data)
    {
        _bossData = data;
        _currentHP = data.MaxHP;
        _damage = data.Damage;

        _anim = GetComponent<Animator>();
        _isDead = false;

        Debug.Log($"[보스 생성] {_bossData.name}, 체력: {_currentHP}, 공격력: {_damage}");

        GetComponent<Collider2D>().enabled = true;
    }

   public void TakeDamage(float damage)
    {
        if (_isDead || _bossData == null) return;

        _currentHP -= damage;
       Debug.Log($"{_bossData.name} 보스 {damage} 데미지 입음, 남은 HP: {_currentHP}");

        if (_currentHP <= 0)
        {
            Die();
        }
        else
        {
            _anim?.SetTrigger("GetHit");
        }
    }

    void Die()
    {
        _isDead = true;
        _anim?.SetTrigger("Die");

        Debug.Log($"{_bossData.name} 보스 처치");

        Invoke(nameof(OnDefeated), 1.5f); // 애니메이션 후 처리

        //Invoke(nameof(Despawn), 0.5f);
        //OnDefeated();
    }

    void OnDefeated()
    {
        // 보상 지급
        GameManager.Instance.AddGold((int)_bossData.RewardGold);
        GameManager.Instance.AddDiamond((int)_bossData.RewardDiamond);

        // 다음 보스 스테이지 해금
        int unlocked = PlayerPrefs.GetInt("UnlockedBossStage", 0);
        if (_bossData.StageNum + 1 > unlocked)
            PlayerPrefs.SetInt("UnlockedBossStage", _bossData.StageNum + 1);

        // 게임 씬으로 복귀
        SceneManager.LoadScene("Game");
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(this);
    }
}
