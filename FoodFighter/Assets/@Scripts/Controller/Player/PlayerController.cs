using System.Collections;
using UnityEngine;
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

    // �ǰ� �ִϸ��̼� ���� �Լ�
    void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    // ���� �ִϸ��̼� ���� �Լ�
    void Die()
    {
        _animator.SetTrigger(Define.Die);

        _playerDiePanel.gameObject.SetActive(true);
        // ���̵� ��
        StartCoroutine(FadePlayerDiePanel(1f, 0f));
        // ���̵� �ƿ�
        StartCoroutine(FadePlayerDiePanel(0f, 1f));
    }

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();
        _playerDiePanel = GameObject.Find("PlayerDiePanel - Panel").GetComponent<Image>();

        // �޸��� ����
        Speed = 1;

        _playerDiePanel.gameObject.SetActive(false);
    }

    void Update()
    {
        // if (�� �������� ���� �ø���)
        FindDistance();
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

        if (isNearEnemy)
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