using UnityEngine;

public class FoodShooter : MonoBehaviour
{
    // ���� ���ð�
    public float Seconds = 1f;
    // �߻� ������ ����
    private bool _canShoot = true;

    public void Shooting()
    {
        // ���� ���ð� ���� �� return
        if (!_canShoot)
            return;

        Vector3 spawnPos = transform.position;
        FoodBullet bullet = PoolManager.Instance.GetObject<FoodBullet>(spawnPos);

        if (bullet != null)
        {
            _canShoot = false;
            Invoke(nameof(ResetShooting), Seconds);
        }
    }

    void ResetShooting()
    {
        _canShoot = true;
    }
}