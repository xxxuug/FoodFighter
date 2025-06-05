using System;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int MainStage;
    public int SubStage;

    public string GetDisplayStage()
    {
        return $"STAGE {MainStage}-{SubStage}";
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
    private EnemyController _enemyController;

    private float _subMultipleHpAmount = 1.1f;
    private float _mainMultipleHpAmount = 1.3f;

    private void Start()
    {
        _enemyController = FindAnyObjectByType<EnemyController>();
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (!_aliveEnemy.Contains(enemy))
        {
            _aliveEnemy.Add(enemy);
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _aliveEnemy.Remove(enemy);

        if (_aliveEnemy.Count == 0)
            NextStage();
    }

    void NextStage()
    {
        if (StageInfo.SubStage >= 5)
        {
            StageInfo.MainStage++;
            StageInfo.SubStage = 1;
            _enemyController.MultipleHp(_mainMultipleHpAmount);
        }
        else
        {
            StageInfo.SubStage++;
            _enemyController.MultipleHp(_subMultipleHpAmount);
        }

        OnStageInfoChanged?.Invoke();
        //Debug.Log($"다음 스테이지 : {StageInfo.GetDisplayStage()}");
        Invoke(nameof(EnemyRespawn), 2f);
    }

    void EnemyRespawn()
    {
        SpawningPool.Instance.NextStageEnemyRespawn();
    }
}
