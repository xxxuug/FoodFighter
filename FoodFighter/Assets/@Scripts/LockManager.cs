using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    public int PlayerLevel = 0; // 플레이어의 레벨
    public int AttackLevel = 0; // 공격력 레벨

    public LockInfo[] lockInfo;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false;

            switch (Locks.lockType)
            {
                case LockType.PlayerLevel:
                    UnLock = PlayerLevel >= Locks.Level;
                    break;
                case LockType.AttackLevel:
                    UnLock = AttackLevel >= Locks.Level;
                    break;
            }

            Locks.UnlockObject.SetActive(UnLock);
            Locks.LockObject.SetActive(!UnLock);
        }
    }

    public void SetPlayerLevel(int Level)
    {
        PlayerLevel = Level;
        RefreshUnlock();
    }

    public void SetAttackLevel(int Level)
    {
        AttackLevel = Level;
        RefreshUnlock();
    }
}
