using UnityEngine;

public class PlayerController : BaseController
{
    [Header("������Ʈ")]
    private Animator _animator;

    [Header("�÷��̾�� �� �Ÿ�")]
    private float _range = 3.5f;

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

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();
        // �޸��� ����
        Speed = 1;
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
            float distance = Distance.GetDistance(transform, enemy.transform);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(Define.EnemyTag))
        {
            GetHit();
        }
    }
}