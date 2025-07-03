using UnityEngine;

public class AttackController : MonoBehaviour
{
    private bool _isTakeDamage = false; // 데미지를 맞췄는지 여부

    public EnemyController enemyController { get; set; }
    public BossStageController bossStageController { get; set; }


    private void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTakeDamage) return; // 이미 맞췄으면 무시

        if (other.CompareTag(Define.PlayerTag) && gameObject.activeSelf)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(enemyController != null){
                playerController.TakeDamage(enemyController._damage);
            }

            if (bossStageController != null){
                playerController.TakeDamage(bossStageController.damage);
            }
        }                    
    }

    // 애니메이션 이벤트
    public void EnableAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        _isTakeDamage = false;
    }

    public void DisableAttack()
    {
        GetComponent<Collider2D>().enabled = false;
        _isTakeDamage = true;
    }
}
