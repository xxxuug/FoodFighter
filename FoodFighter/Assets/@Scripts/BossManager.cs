using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    [Header("잠금 연동")]
    public LockManager[] lockManager;

    //public GameObject[] StageBoss;

    [Header("보스 클리어 상태")]
    private int StageLevel = 0; // 현재까지 클리어한 최고 스테이지 번호

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

    // 테스트
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
