using UnityEngine;

public class BossStageSpawningPool : MonoBehaviour
{
    private int StageSelect;

  //  [Header("���� ������")]
    private BossData[] bossData;

    private Vector3 _playerSpawn = new Vector3(-1.43f, 1.39f, 0); // �÷��̾� ���� ��ġ
    private Vector3 _initBossSpawn = new Vector2(3.34f, 1.62f); // ���� ���� ��ġ

    private BossStageController _stageController;

    private void Awake()
    {
        ObjectManager.Instance.ResourceAllLoad();
        ObjectManager.Instance.Spawn<PlayerController>(_playerSpawn);

        StageSelect = PlayerPrefs.GetInt("SelectedBossStage", 0);
        SpawnBoss(StageSelect);
    }

    void SpawnBoss(int StageSelect)
    {
        if (StageSelect < 0 || StageSelect >= bossData.Length)
        {
            return;
        }

        BossData data = bossData[StageSelect];

        // Ǯ�� ���� ���� ����
        var bossObj = PoolManager.Instance.GetObject(data.BossPrefab, _initBossSpawn);
        _stageController = bossObj as BossStageController;

        if (_stageController != null)
        {
            _stageController.Init(data);
        }
    }
}
