using UnityEngine;
using System.Collections;

public class FoodBullet : BaseController
{
    public float speed = 5f;
    public float Second = 1.5f;


    protected override void Initialize() { }

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
            ObjectManager.Instance.Despawn(this);
            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage(2);
        }
    }
}
