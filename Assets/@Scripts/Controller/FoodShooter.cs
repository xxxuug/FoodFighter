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
            Vector3 spawnPos = transform.position;

            // 총알 생성 (오브젝트 풀링을 이용하여 총알 생성)
            PoolManager.Instance.GetObject<FoodBullet>(spawnPos);

            yield return new WaitForSeconds(Seconds);
        }
    }
}