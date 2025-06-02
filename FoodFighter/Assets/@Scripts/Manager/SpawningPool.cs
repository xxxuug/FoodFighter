using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : Singleton<SpawningPool>
{
    private Vector3 _playerSpawn = new Vector2(-1.43f, 1.39f); // 플레이어 스폰 위치
    private Vector3 _initEnemySpawn = new Vector2(3.34f, 1.62f);
    private Vector3 _enemySpawn;

    // 총 몬스터 리스트
    private List<EnemyController> _spawnEnemy = new List<EnemyController>();

    // 서브 스테이지별 마릿수 지정
    private Dictionary<int, int[]> _spawnTable = new()
    {
        {1, new[] {2,2,0,0,0} },
        {2, new[] {1,2,2,0,0} },
        {3, new[] {1,2,3,0,0} },
        {4, new[] {1,1,2,3,0} },
        {5, new[] {1,1,2,3,1} },
    };

    private void Awake()
    {
        ObjectManager.Instance.ResourceAllLoad();
        ObjectManager.Instance.Spawn<PlayerController>(_playerSpawn);
    }

    void Start()
    {
        NextStageEnemyRespawn();
    }

    void SpawnStage(int main, int sub)
    {
        _spawnEnemy.Clear();

        int stageIndex = Mathf.Clamp(sub, 1, 5);
        if (!_spawnTable.TryGetValue(stageIndex, out var spawnCount)) return;

        var prefabs = ObjectManager.Instance.EnemyPrefabs;

        for (int i = 0; i < spawnCount.Length && i < prefabs.Count; i++) 
        {
            int count = spawnCount[i];
            GameObject prefab = prefabs[i];

            for (int j = 0; j < count; j++) 
            {
                // 몹 생성
                EnemyController enemy = PoolManager.Instance.GetObject(prefab, _enemySpawn) as EnemyController;

                _spawnEnemy.Add(enemy);
                StageManager.Instance.AddEnemy(enemy);
                _enemySpawn += new Vector3(0.5f, 0);
            }
        }
    }

    public void NextStageEnemyRespawn()
    {
        _enemySpawn = _initEnemySpawn;

        var main = StageManager.Instance.StageInfo.MainStage;
        var sub = StageManager.Instance.StageInfo.SubStage;
        SpawnStage(main, sub);
    }
}
