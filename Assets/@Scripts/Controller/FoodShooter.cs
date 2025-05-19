using System.Collections;
using UnityEngine;

public class FoodShooter : BaseController
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
            Vector3 spawnPos = transform.position;
            PoolManager.Instance.GetObject<FoodBullet>(spawnPos);
            yield return new WaitForSeconds(Seconds);
        }
    }

    protected override void Initialize()
    {
        
    }
}