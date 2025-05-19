using UnityEngine;
using UnityEngine.UIElements;

public class FoodBullet : BaseController
{
    public float speed = 5f;
    public float Seconds = 3f;

    private float timer = 0f;
    
    // ÃÊ±âÈ­
    protected override void Initialize() { }

    private void OnEnable()
    {
        timer = 0f;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= Seconds)
        {
            timer = 0f;
            ObjectManager.Instance.Despawn(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectManager.Instance.Despawn(this);
    }
}