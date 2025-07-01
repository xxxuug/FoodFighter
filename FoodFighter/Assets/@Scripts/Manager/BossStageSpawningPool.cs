using UnityEngine;
using System;
using System.Net.NetworkInformation;

[Serializable]
public class BossData
{
    public int Stage;
    public GameObject BossPrefab;
    public Vector3 SpawnPosition; // 보스 시작 위치
    public Vector3 TargetPosition; // 보스가 이동해야 할 중앙 위치
}

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
            Debug.Log("해당하는 보스 없음");
            return;
        }

        // 풀로 보스 생성
        var baseObj = PoolManager.Instance.GetObject(selectBoss.BossPrefab, selectBoss.SpawnPosition);
        BossStageController boss = baseObj as BossStageController;

        if (boss == null)
        {
            Debug.Log("보스 프리팹에 BossStageController가 없습니다.");
            return;
        }

        boss.gameObject.SetActive(true);
        boss.battleState = BattleState.MoveToCenter;

        StageManager.Instance.boss = boss;

        boss.AllInit(currentStage);

        // 보스가 이동할 중앙 위치 설정
        //boss.SendMessage("SetTArgetPosition", selectBoss.TargetPosition);
        //boss.Init(currentStage, selectBoss.BossType);
    }
}
