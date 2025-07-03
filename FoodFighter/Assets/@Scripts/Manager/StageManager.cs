using ClassDef;
using EnumDef;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    #region Stage Info
    public event Action OnStageInfoChanged;

    private StageInfo _stageInfo = new StageInfo()
    {
        MainStage = 1,
        SubStage = 1,
    };

    public StageInfo StageInfo
    {
        get { return _stageInfo; }
        set
        {
            _stageInfo = value;
            OnStageInfoChanged?.Invoke();
        }
    }
    #endregion

    private List<EnemyController> _aliveEnemy = new();
    public BossStageController boss { get; set; }
    public PlayerController Player { get; set; }

    public void AddEnemy(EnemyController enemy)
    {
        if (!_aliveEnemy.Contains(enemy))
        {
            _aliveEnemy.Add(enemy);
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        ObjectManager.Instance.Despawn(enemy);

        _aliveEnemy.Remove(enemy);
    }

    public void HuntEnemy(EnemyController enemy)
    {
        RemoveEnemy(enemy);
        
        if (_aliveEnemy.Count == 0)
        {
            NextStage();
        }
    }

    public void RemoveAllEnemy()
    {
        foreach (var enemy in _aliveEnemy)
        {
            ObjectManager.Instance.Despawn(enemy);
        }
        _aliveEnemy.Clear();
    }

    void NextStage()
    {
        if (StageInfo.SubStage >= 5)
        {
            StageInfo.MainStage++;
            StageInfo.SubStage = 1;
        }
        else
        {
            StageInfo.SubStage++;
        }

        ResetPlayerHP();
        OnStageInfoChanged?.Invoke();
        Invoke(nameof(EnemyRespawn), 2f);
    }

    void EnemyRespawn()
    {
        SpawningPool.Instance.NextStageEnemyRespawn();
    }

    public void EnemyRespawnStop()
    {
        CancelInvoke(nameof(EnemyRespawn));
    }

    void ResetPlayerHP()
    {
        GameManager.Instance[PlayerStat.CurrentHp] = GameManager.Instance[PlayerStat.MaxHp];
    }

    public void SetPrevStage()
    {
        if (StageInfo.SubStage == 1)
        {
            StageInfo.MainStage--;
            StageInfo.SubStage = 5;
        }
        else
        {
            StageInfo.SubStage--;
        }

        ResetPlayerHP();
        OnStageInfoChanged?.Invoke();
        Invoke(nameof(EnemyRespawn), 2f);
    }
}
