using Mono.Cecil;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.SceneManagement;

public class DungeonButton : MonoBehaviour
{
    [SerializeField] private string dungeonName;
    [SerializeField] private int dungeonStage;
    [SerializeField] private int Diamond;
    [SerializeField] private int Gold;

    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text DiarewardText;
    [SerializeField] private TMP_Text GoldrewardText;

    private DungeonStageInfo.Data data;

    private void Start()
    {
        if (dungeonName == "∞ÒµÂ±§ªÍ")
        {
            dungeonStage = GameManager.Instance.GoldStageDungeon;
            Gold = GameManager.Instance.GoldDungeonMoney;
        }

        if (dungeonName == "¥Ÿ¿Ãæ∆±§ªÍ")
        {
            dungeonStage = GameManager.Instance.DiaStageDungeon;
            Diamond = GameManager.Instance.DiaDungeonMoney;
        }

        SetData(data);
    }

    public void SetData(DungeonStageInfo.Data Dungeondata)
    {
        data = Dungeondata;

        stageText.text = $"{dungeonStage} ¥‹∞Ë";

        if (GoldrewardText != null)
            GoldrewardText.text = $"{Gold}";

        if (DiarewardText != null)
            DiarewardText.text = $"{Diamond}";


    }


    public void OnBossStageEnterButtonClick()
    {
        GameManager.Instance.SelectDungeon = dungeonName;

        if (dungeonName == "∞ÒµÂ±§ªÍ")
        {
            GameManager.Instance.GoldStageDungeon = dungeonStage;
            GameManager.Instance.GoldDungeonMoney = Gold;
        }
        if (dungeonName == "¥Ÿ¿Ãæ∆±§ªÍ")
        {
            GameManager.Instance.DiaStageDungeon = dungeonStage;
            GameManager.Instance.DiaDungeonMoney = Diamond;
        }

        StageManager.Instance.EnemyRespawnStop();

        SpawningPool.Instance.EnemyClear();
        StageManager.Instance.RemoveAllEnemy();

        SceneManager.LoadScene(Define.Dungeon);
    }
}
