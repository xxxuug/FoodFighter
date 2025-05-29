using System;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int MainStage;
    public int SubStage;

    public string GetDisplayStage()
    {
        return $"{MainStage} - {SubStage}";
    }
}

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

    public void AddEnemy(EnemyController enemy)
    {
        if (!_aliveEnemy.Contains(enemy))
            _aliveEnemy.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _aliveEnemy.Remove(enemy);

        if (_aliveEnemy.Count == 0)
            NextStage();
    }

    void NextStage()
    {
        if (StageInfo.SubStage >= 6)
        {
            StageInfo.MainStage++;
            StageInfo.SubStage = 1;
        }
        else
            StageInfo.SubStage++;

        OnStageInfoChanged?.Invoke();

    }
}
