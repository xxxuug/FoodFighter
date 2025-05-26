using UnityEngine;

public class PlayerController : BaseController
{
    [Header("컴포넌트")]
    private Animator _animator;

    [Header("플레이어와 적 거리")]
    private float _range = 3.5f;

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

    // 피격 애니메이션 실행 함수
    void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();
        // 달리기 유지
        Speed = 1;
    }

    void Update()
    {
        // if (새 스테이지 진입 시마다)
        FindDistance();
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
            float distance = Distance.GetDistance(transform, enemy.transform);
            //Debug.Log($"플레이어와 적 사이 거리 : {distance}");
            if (distance <= _range)
            {
                isNearEnemy = true;
                break;
            }
        }

        if (isNearEnemy)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(Define.EnemyTag))
        {
            GetHit();
        }
    }
}