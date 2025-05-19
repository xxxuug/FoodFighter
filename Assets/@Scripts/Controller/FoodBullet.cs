using UnityEngine;

public class FoodBullet : BaseController
{
    public float speed = 5f;
    
    // √ ±‚»≠
    protected override void Initialize() { }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectManager.Instance.Despawn(this);
    }
}