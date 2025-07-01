using UnityEngine;
using System;

public class BossStageSpawningPool : MonoBehaviour
{
    //[SerializeField] private BossData[] _bossDataList;

    private void Awake()
    {
        // ObjectManager.Instance.Spawn<PlayerController>(_playerSpawn);         
    }

    private void Start()
    {
        int currentStage = GameManager.Instance.CurBossStageIndex;

        BossStageInfo.Data selectBoss = null;
        foreach (var data in GameManager.Instance.bossStageInfo.list)
        {
            if (data.Stage == currentStage)
            {
                selectBoss = data;
                break;
            }
        }

        if (selectBoss == null)
        {
            Debug.Log("�ش��ϴ� ���� ����");
            return;
        }

        // Ǯ�� ���� ����
        var baseObj = PoolManager.Instance.GetObject(selectBoss.BossPrefab, selectBoss.SpawnPosition);
        BossStageController boss = baseObj as BossStageController;

        if (boss == null)
        {
            Debug.Log("���� �����տ� BossStageController�� �����ϴ�.");
            return;
        }

        boss.gameObject.SetActive(true);
        boss.battleState = BattleState.MoveToCenter;

        StageManager.Instance.boss = boss;

        boss.AllInit(currentStage);

        // ������ �̵��� �߾� ��ġ ����
        //boss.SendMessage("SetTArgetPosition", selectBoss.TargetPosition);
        //boss.Init(currentStage, selectBoss.BossType);
    }
}
