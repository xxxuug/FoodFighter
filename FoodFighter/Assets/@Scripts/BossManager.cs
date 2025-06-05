using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    [Header("��� ����")]
    public LockManager[] lockManager;

    //public GameObject[] StageBoss;

    [Header("���� Ŭ���� ����")]
    private int StageLevel = 0; // ������� Ŭ������ �ְ� �������� ��ȣ

    void Stage(int Stage)
    {
        if (Stage > StageLevel)
        {
            StageLevel = Stage;

            foreach(var lockManagers in lockManager)
            {
                if (lockManagers != null)
                {
                    lockManagers.SetStage(Stage);
                }
            }
        }
    }

    // �׽�Ʈ
    // public Button buttons;
    //private void Start()
    //{
    //    buttons.onClick.AddListener(OnClickBossButton);
    //} 

    //void OnClickBossButton()
    //{
    //    StageLevel = 1;
    //    if (lockManager != null && lockManager.Length > 0)
    //    {
    //        foreach (var lockManagers in lockManager)
    //        {
    //            if (lockManagers != null)
    //                lockManagers.SetStage(StageLevel);
    //        }
    //    }
    //}

}
