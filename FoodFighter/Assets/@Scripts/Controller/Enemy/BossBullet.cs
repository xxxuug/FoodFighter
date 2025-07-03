using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject HitEffect;

    public float Speed = 10f;
    
    private float _attack;

    public void SetDamage(float _damage)
    {
        _attack = _damage;
    }

    void Update()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, 360 * Speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PlayerTag))
        {
            PoolManager.Instance.GetEffectObject(HitEffect, transform.position);

            PlayerController player = collision.GetComponent<PlayerController>();
            player.TakeDamage(_attack);

            /*
            ObjectManager.Instance.Despawn(this); // À½½Ä false
            */
            Destroy(gameObject);
        }
    }
}
