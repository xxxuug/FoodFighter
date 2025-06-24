using System;
using UnityEngine;
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
    public GameObject LockObject; // ������� �� ������ ������Ʈ
}

public class LockManager : MonoBehaviour
{
    private int AttackLevel = 0; // ���ݷ� ����
    public int StageLevel = 0; // �������� ����

    public LockInfo[] lockInfo;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false; // ���� ����

            switch (Locks.lockType)
            {
                case LockType.AttackLevel: // ���ݷ�
                    UnLock = AttackLevel >= Locks.Level; // ���� ���ݷ� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
                case LockType.Stage: // ��������
                    UnLock = StageLevel >= Locks.Level; // ���� �������� >= �䱸 ���ݷ��̶�� ��� ����
                    break;
            }

            // Locks.UnlockObject.SetActive(UnLock);
            Locks.LockObject.SetActive(!UnLock); // ��� ������ ���� true
        }
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
}
