using System.Collections;
using UnityEngine;

public class FoodShooter : MonoBehaviour
{
    public float Seconds = 1f;

    private void Start()
    {
        StartCoroutine(CoShoot());
    }

    IEnumerator CoShoot()
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0f);
            PoolManager.Instance.GetObject<FoodBullet>(spawnPos);
            yield return new WaitForSeconds(Seconds);
        }
    }
}