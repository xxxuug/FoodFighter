using System.Collections;
using UnityEngine;

public class FoodBullet : BaseController
{
    public GameObject HitEffect;

    public float speed = 5f;
    public float Second = 1.5f;

    private SpriteRenderer _spriteRederer;


    protected override void Initialize() 
    {
        _spriteRederer = GetComponent<SpriteRenderer>();
        SlotController.Instance.FindFoodBullet(this);
    }

    private void OnEnable()
    {
        StartCoroutine(DisableTime());
    }

    private IEnumerator DisableTime()
    {
        float time = 0f;
        while (time < Second)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, 360 * speed * Time.deltaTime, Space.Self);
            time += Time.deltaTime;
            yield return null;
        }

        ObjectManager.Instance.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.EnemyTag))
        {
            StartCoroutine(PlayHitEffect(HitEffect));

            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage(2);
        }
    }

    public void SetFoodSprite(Sprite maxSprite) // 현재 최대 레벨 음식의 아이콘으로 변경해주는 함수
    {
        if (_spriteRederer == null)
            _spriteRederer = GetComponent<SpriteRenderer>();

        _spriteRederer.sprite = maxSprite;

        Debug.Log("무기 스프라이트 변경됨: " + maxSprite.name);
        // 추후 공격력 증가 함수 참조
    }

    IEnumerator PlayHitEffect(GameObject effect)
    {
        GameObject hitEffect = PoolManager.Instance.GetEffectObject(effect, transform.position);
        Debug.Log("이펙트 생성");
        yield return new WaitForSeconds(0.3f);

        Debug.Log($"이펙트 사라짐 처리: {hitEffect.name}");
        ObjectManager.Instance.Despawn(hitEffect); // 이펙트 false
        yield return new WaitForSeconds(0.3f);
        ObjectManager.Instance.Despawn(this); // 음식 false
    }
}
