using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject[] FoodBullet; // 음식 프리팹
    public GameObject ShootEffect; // 발사 이펙트 프리팹

    private float spawnSeconds = 1f; // 생성 간격
    void Start()
    {
        StartCoroutine(CoStartFoodFly()); // 코루틴 시작
    }

    IEnumerator CoStartFoodFly()
    {
        while (true)
        {
            // 랜덤으로 음식이 나오도록
            int index = Random.Range(0, FoodBullet.Length);
            GameObject selected = FoodBullet[index];

            // 현재 위치에 음식 생성
            Instantiate(selected, transform.position, Quaternion.identity);

            // 발사 이펙트 생성
            if (ShootEffect != null)
            {
                GameObject effect = Instantiate(ShootEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f); // 1초 후 이펙트 제거
            }

            yield return new WaitForSeconds(spawnSeconds); // 2초 간격으로 음식이 나오도록
        }
    }
}