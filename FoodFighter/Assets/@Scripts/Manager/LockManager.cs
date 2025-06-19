using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    private int AttackLevel = 0; // 공격력 레벨
    private int StageLevel = 0;

    public LockInfo[] lockInfo;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false;

            switch (Locks.lockType)
            {
                case LockType.AttackLevel:
                    UnLock = AttackLevel >= Locks.Level;
                    break;
                case LockType.Stage:
                    UnLock = StageLevel >= Locks.Level;
                    break;
            }

           // Locks.UnlockObject.SetActive(UnLock);
            Locks.LockObject.SetActive(!UnLock);
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
