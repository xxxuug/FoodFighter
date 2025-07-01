using UnityEngine;

public class FoodShooter : MonoBehaviour
{
    // 재사용 대기시간
    public float Seconds = 1f;
    // 발사 가능한 여부
    private bool _canShoot = true;

    public void Shooting()
    {
        if (StageManager.Instance.boss != null)
            if (StageManager.Instance.Player.battleState != BattleState.PlayerTurn)
                return;

        // 재사용 대기시간 중일 땐 return
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