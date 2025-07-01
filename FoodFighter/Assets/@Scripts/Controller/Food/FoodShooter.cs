using UnityEngine;

public class FoodShooter : MonoBehaviour
{
    // ���� ���ð�
    public float Seconds = 1f;
    // �߻� ������ ����
    private bool _canShoot = true;

    public void Shooting()
    {
        if (StageManager.Instance.boss != null)
            if (StageManager.Instance.Player.battleState != BattleState.PlayerTurn)
                return;

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