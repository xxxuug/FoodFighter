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
    public const string SlotIcon = "SlotIcon";
    public const string SlotBackground = "Slot Background - Image";
    public const string GoldText = "Gold Text - Text";
    public const string DiamondText = "Diamond Text - Text";
    #endregion

}