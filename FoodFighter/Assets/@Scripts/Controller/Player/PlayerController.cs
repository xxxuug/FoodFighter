using System.Collections;
using UnityEngine;
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

    // 피격 애니메이션 실행 함수
    void GetHit()
    {
        _animator.SetTrigger(Define.GetHit);
    }

    // 죽음 애니메이션 실행 함수
    void Die()
    {
        _animator.SetTrigger(Define.Die);

        _playerDiePanel.gameObject.SetActive(true);
        // 페이드 인
        StartCoroutine(FadePlayerDiePanel(1f, 0f));
        // 페이드 아웃
        StartCoroutine(FadePlayerDiePanel(0f, 1f));
    }

    protected override void Initialize()
    {
        _animator = GetComponent<Animator>();
        _playerDiePanel = GameObject.Find("PlayerDiePanel - Panel").GetComponent<Image>();

        // 달리기 유지
        Speed = 1;

        _playerDiePanel.gameObject.SetActive(false);
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
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
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