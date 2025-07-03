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
            bool UnLock = false; // ���� ����

            switch (Locks.lockType)
            {
                case LockType.AttackLevel: // ���ݷ�
                    UnLock = GameManager.Instance.AttackLevel >= Locks.Level; // ���� ���ݷ� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
                case LockType.Stage: // ��������
                    UnLock = GameManager.Instance.BossStageOpen[Locks.StageIndex];
                    //UnLock = StageLevel >= Locks.Level; // ���� �������� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
            }

            // Locks.UnlockObject.SetActive(UnLock);
            if (Locks.LockObject != null)
                Locks.LockObject.SetActive(!UnLock); // ��� ������ ���� true
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
