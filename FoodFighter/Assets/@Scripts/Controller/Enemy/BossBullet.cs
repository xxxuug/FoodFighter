using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject HitEffect;

    public float Speed = 10f;
    
    private float mAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    public void SetDamage(float _damage)
    {
        mAttack = _damage;
    }

    // Update is called once per frame
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
            player.TakeDamage(mAttack);

            /*
            ObjectManager.Instance.Despawn(this); // À½½Ä false
            */
            Destroy(gameObject);
        }
    }
}
