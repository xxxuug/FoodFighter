using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LockType
{
    AttackLevel, // ���ݷ� ����
    Stage, // �������� ���� 
}

[Serializable]
public class LockInfo
{
    public LockType lockType; // � �������� ������ (���ݷ� or ��������)    
    public int Level; // �䱸 ����
    public int StageIndex;
    public GameObject LockObject; // ������� �� ������ ������Ʈ
}

public class LockManager : MonoBehaviour
{
    private int AttackLevel = 0; // ���ݷ� ����
    public int StageLevel = 0; // �������� ����

    public LockInfo[] lockInfo;

    bool mIsLock = false;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false; // ���� ����

            switch (Locks.lockType)
            {
                case LockType.AttackLevel: // ���ݷ�
                    mIsLock = AttackLevel >= Locks.Level; // ���� ���ݷ� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
                case LockType.Stage: // ��������
                    UnLock = GameManager.Instance.BossStageOpen[Locks.StageIndex];
                    //UnLock = StageLevel >= Locks.Level; // ���� �������� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
            }

            // Locks.UnlockObject.SetActive(UnLock);
            if(Locks.LockObject != null)
                Locks.LockObject.SetActive(!UnLock); // ��� ������ ���� true
        }
    }


    private void Update()
    {
        RefreshUnlock();
    }


    public void SetStage(int Level)
    {
        StageLevel = Level;

        RefreshUnlock();
    }

    public void SetAttackLevel(int Level)
    {
        AttackLevel = Level;
        RefreshUnlock();
    }

    public void OnBossStageEnterButtonClick()
    {
        StageManager.Instance.Player.SetBossStage();
        StageManager.Instance.EnemyRespawnStop();

        SpawningPool.Instance.EnemyClear();

        GameManager.Instance.CurBossStageIndex = lockInfo[0].StageIndex;
        //GameManager.Instance.StageUnlock = capturedIndex;
        SceneManager.LoadScene("StageBoss");
    }
}
