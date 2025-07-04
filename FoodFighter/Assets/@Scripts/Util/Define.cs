using UnityEngine;

public class Define
{
    #region Path
    public const string BulletPath = "@Prefabs/FoodBullet";
    public const string PlayerPath = "@Prefabs/Player";
    public const string HitEffectPath = "@Prefabs/Enemy/HitEffect.prefab";
    public const string AllEnemyPath = "@Prefabs/Enemy";
    public const string GoldPath = "@Prefabs/Gold";
    public const string BossPath = "@Prefabs/Enemy/TestBoss";
    public const string HitDamagePath = "@Prefabs/Enemy/Hit Damage - Text.prefab";
    public const string PopUpPath = "@Prefabs/General Pop-up.prefab";
    #endregion

    #region Tag
    public const string EnemyTag = "Enemy";
    public const string BossTag = "Boss";
    public const string PlayerTag = "Player";
    #endregion

    #region Animation
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int isAttacking = Animator.StringToHash("isAttacking");
    public readonly static int GetHit = Animator.StringToHash("GetHit");
    public readonly static int Die = Animator.StringToHash("Die");
    #endregion

    #region Other String
    public const string GameScene = "Game";
    public const string BossStageScene = "StageBoss";
    public const string SlotIcon = "SlotIcon";
    public const string SlotBackground = "Slot Background - Image";
    public const string GoldText = "Gold Text - Text";
    public const string DiamondText = "Diamond Text - Text";
    public const string TotalAtkText = "Attack Text - Text";
    public const string PlayerDeath = "Player Death";
    public const string UI = "UI";
    #endregion

}