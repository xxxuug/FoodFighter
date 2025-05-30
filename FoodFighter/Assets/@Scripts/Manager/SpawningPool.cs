using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : Singleton<SpawningPool>
{
    private int _maxSpawnCount = 2; // 최대 스폰 개수
    private Vector3 _playerSpawn = new Vector2(-1.43f, 1.39f); // 플레이어 스폰 위치
    private Vector3 _initEnemySpawn = new Vector2(3.34f, 1.62f);
    private Vector3 _enemySpawn;

    // 총 몬스터 리스트
    private List<EnemyController> _spawnEnemy = new List<EnemyController>();

    private void Awake()
    {
        ObjectManager.Instance.ResourceAllLoad();
        ObjectManager.Instance.Spawn<PlayerController>(_playerSpawn);
        _enemySpawn = _initEnemySpawn;
    }

    void Start()
    {
        for (int i = 0; i < _maxSpawnCount; i++)
        {
            // 몹 생성
            EnemyController enemy = PoolManager.Instance.GetObject<EnemyController>(_enemySpawn);

            _spawnEnemy.Add(enemy);
            StageManager.Instance.AddEnemy(enemy);
            _enemySpawn += new Vector3(0.5f, 0);
        }
    }

    public void NextStageEnemyRespawn()
    {
        _enemySpawn = _initEnemySpawn;

        foreach (var enemy in _spawnEnemy)
        {
            enemy.transform.position = _enemySpawn;
            enemy.gameObject.SetActive(true);
            _enemySpawn += new Vector3(0.5f, 0);
        }
    }
}
