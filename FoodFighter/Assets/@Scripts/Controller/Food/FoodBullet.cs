using EnumDef;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        StopAllCoroutines();
        SceneManager.sceneLoaded += OnSceneLoade;
        
        // ���ݷ� ��������
        _atk = GameManager.Instance[PlayerStat.TotalAtk];

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

        if (collision.CompareTag(Define.BossTag))
        {
            PoolManager.Instance.GetEffectObject(HitEffect, transform.position);

            BossStageController boss = collision.GetComponent<BossStageController>();
            boss.TakeDamage(_atk);
            // Debug.Log($"���Ϳ��� {baseAtk} ������ ����");

            // �� �ѱ��
            //if (StageManager.Instance.Player is PlayerController player)
            //{
            //    player.battleState = BattleState.BossTurn;
            //}
            ObjectManager.Instance.Despawn(this);
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoade;
    }

    private void OnSceneLoade(Scene scne, LoadSceneMode mode)
    {
        ObjectManager.Instance?.Despawn(this);
    }
}
