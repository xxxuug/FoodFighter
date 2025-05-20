using UnityEngine;
using System.Collections;

public class FoodBullet : BaseController
{
    public float speed = 5f;
    public float Second = 1.5f;

    private Coroutine coroutine;

    protected override void Initialize() { }

    private void OnEnable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(DisableTime());
    }

    private IEnumerator DisableTime()
    {
        float _Time = 0f;
        while (_Time < Second)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            _Time += Time.deltaTime;
            yield return null;
        }

        coroutine = null;

        ObjectManager.Instance.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Define.EnemyTag)) return;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        ObjectManager.Instance.Despawn(this);
    }
}
