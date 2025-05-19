using UnityEngine;

public class FoodBullet : MonoBehaviour
{
    public float force = 800.0f;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.right * force);
    }
}