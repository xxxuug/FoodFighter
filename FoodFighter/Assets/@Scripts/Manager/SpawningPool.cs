using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : Singleton<SpawningPool>
{
    private Coroutine _coRespawnPool;
    WaitForSeconds _spawnInterval = new WaitForSeconds(5f);

    private int _maxSpawnCount = 1; // �ִ� ���� ����
    private Vector3 _playerSpawn = new Vector2(-1.43f, 1.39f); // �÷��̾� ���� ��ġ
    private Vector3 _enemySpawn = new Vector2(3.34f, 1.39f);

    // �� ���� ����Ʈ
    private List<EnemyController> _spawnEnemy = new List<EnemyController>();
    // ���� ���� ����Ʈ
    private List<EnemyController> _deadEnemy = new List<EnemyController>();

    private void Awake()
    {
        ObjectManager.Instance.ResourceAllLoad();
        ObjectManager.Instance.Spawn<PlayerController>(_playerSpawn);
    }

    void Start()
    {
        for (int i = 0; i < _maxSpawnCount; i++)
        {
            // �� ����
            EnemyController enemy = PoolManager.Instance.GetObject<EnemyController>(_enemySpawn);

            _spawnEnemy.Add(enemy);
            StageManager.Instance.AddEnemy(enemy);
        }
    }

    public void EnemyDie(EnemyController enemy)
    {
        _deadEnemy.Add(enemy);

        if (_coRespawnPool == null)
        {
            _coRespawnPool = StartCoroutine(CoRespawnMonster());
        }
    }

    IEnumerator CoRespawnMonster()
    {
        yield return _spawnInterval;

        foreach (var enemy in _deadEnemy)
        {
            enemy.transform.position = _enemySpawn;
            enemy.gameObject.SetActive(true);
        }

        _deadEnemy.Clear();
        _coRespawnPool = null;
    }
}
