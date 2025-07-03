using ClassDef;
using EnumDef;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LockManager : MonoBehaviour
{
    public LockInfo[] lockInfo;

    private bool _isBossState = true;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false; // 해제 여부

            switch (Locks.lockType)
            {
                case LockType.AttackLevel: // 공격력
                    UnLock = GameManager.Instance.AttackLevel >= Locks.Level; // 현재 공격력 >= 요구 공격력이라면 잠금 해제
                    break;
                case LockType.Stage: // 스테이지
                    UnLock = GameManager.Instance.BossStageOpen[Locks.StageIndex];
                    //UnLock = StageLevel >= Locks.Level; // 현재 스테이지 >= 요구 공격력이라면 잠금 해제
                    break;
            }

            // Locks.UnlockObject.SetActive(UnLock);
            if (Locks.LockObject != null)
                Locks.LockObject.SetActive(!UnLock); // 잠금 상태일 때만 true
        }
    }


    private void Update()
    {
        if (_isBossState)
        {
            RefreshUnlock();
            _isBossState = false;
        }
    }

    public void SetAttackLevel(int Level)
    {
        GameManager.Instance.AttackLevel = Level;
        RefreshUnlock();
    }

    public void OnBossStageEnterButtonClick()
    {
        StageManager.Instance.Player.SetBossStage();
        StageManager.Instance.EnemyRespawnStop();

        SpawningPool.Instance.EnemyClear();
        StageManager.Instance.RemoveAllEnemy();

        GameManager.Instance.CurBossStageIndex = lockInfo[0].StageIndex;
        //GameManager.Instance.StageUnlock = capturedIndex;
        SceneManager.LoadScene("StageBoss");
    }
}
