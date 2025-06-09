using UnityEngine;

public class Define
{
    #region Path
    public const string BulletPath = "@Prefabs/FoodBullet";
    public const string PlayerPath = "@Prefabs/Player";
    public const string EffectPath = "@Prefabs/Effect";
    public const string AllEnemyPath = "@Prefabs/Enemy";
    public const string GoldPath = "@Prefabs/Gold";
    #endregion

    #region Tag
    public const string EnemyTag = "Enemy";
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
    #endregion

}