using UnityEngine;

public class FoodBullet : BaseController
{
    //public float speed = 5f;
    //public float lifetime = 3f;

    //private float timer = 0f;

    //void OnEnable()
    //{
    //    timer = 0f;
    //}

    //void Update()
    //{
    //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //    timer += Time.deltaTime;

    //    if (timer >= lifetime)
    //    {
    //        timer = 0f;
    //        ObjectManager.Instance.Despawn(this);
    //    }
    //}

   // Rigidbody2D rb;
    public float speed = 5f;
    
    // √ ±‚»≠
    protected override void Initialize()
    {
       //rb = GetComponent<Rigidbody2D>();
       // rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectManager.Instance.Despawn(this);
    }
}