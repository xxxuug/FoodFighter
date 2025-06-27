using System.Collections;
using UnityEngine;

public class FoodBullet : BaseController
{
    public GameObject HitEffect;

    public float Speed = 10f;
    public float Second = 1.5f;
    private float _atk; 

    private SpriteRenderer _spriteRederer;


    protected override void Initialize() 
    {
        _spriteRederer = GetComponent<SpriteRenderer>();
        SlotController.Instance.FindFoodBullet(this);
    }

    private void OnEnable()
    {
        // ���ݷ� ��������
        _atk = GameManager.Instance.TotalAttack();

        StartCoroutine(DisableTime());
    }

    private IEnumerator DisableTime()
    {
        float time = 0f;
        while (time < Second)
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, 360 * Speed * Time.deltaTime, Space.Self);
            time += Time.deltaTime;
            yield return null;
        }

        ObjectManager.Instance.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.EnemyTag))
        {
            PoolManager.Instance.GetEffectObject(HitEffect, transform.position);

            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage(_atk);

            ObjectManager.Instance.Despawn(this); // ���� false
        }
    }

    public void SetFoodSprite(Sprite maxSprite) // ���� �ִ� ���� ������ ���������� �������ִ� �Լ�
    {
        if (_spriteRederer == null)
            _spriteRederer = GetComponent<SpriteRenderer>();

        _spriteRederer.sprite = maxSprite;

        //Debug.Log("���� ��������Ʈ �����: " + maxSprite.name);
        // ���� ���ݷ� ���� �Լ� ����
    }
}
