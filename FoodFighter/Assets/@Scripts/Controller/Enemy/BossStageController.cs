using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class BossData
{
    public string name; // ���� �̸�
    public int StageNum; // �������� ��ȣ
    public float MaxHP; // ü��
    public float Damage; // ������
    public float RewardGold;// ��� ����
    public float RewardDiamond; // ���̾Ƹ�� ����
    public GameObject BossPrefab; // ���� ������ 
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

        Debug.Log($"[���� ����] {_bossData.name}, ü��: {_currentHP}, ���ݷ�: {_damage}");

        GetComponent<Collider2D>().enabled = true;
    }

   public void TakeDamage(float damage)
    {
        if (_isDead || _bossData == null) return;

        _currentHP -= damage;
       Debug.Log($"{_bossData.name} ���� {damage} ������ ����, ���� HP: {_currentHP}");

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

        Debug.Log($"{_bossData.name} ���� óġ");

        Invoke(nameof(OnDefeated), 1.5f); // �ִϸ��̼� �� ó��

        //Invoke(nameof(Despawn), 0.5f);
        //OnDefeated();
    }

    void OnDefeated()
    {
        // ���� ����
        GameManager.Instance.AddGold((int)_bossData.RewardGold);
        GameManager.Instance.AddDiamond((int)_bossData.RewardDiamond);

        // ���� ���� �������� �ر�
        int unlocked = PlayerPrefs.GetInt("UnlockedBossStage", 0);
        if (_bossData.StageNum + 1 > unlocked)
            PlayerPrefs.SetInt("UnlockedBossStage", _bossData.StageNum + 1);

        // ���� ������ ����
        SceneManager.LoadScene("Game");
    }

    void Despawn()
    {
        ObjectManager.Instance.Despawn(this);
    }
}
