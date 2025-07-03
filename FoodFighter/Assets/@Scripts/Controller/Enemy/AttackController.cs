using UnityEngine;

public class AttackController : MonoBehaviour
{
    private bool _isTakeDamage = false; // �������� ������� ����

    public EnemyController enemyController { get; set; }
    public BossStageController bossStageController { get; set; }


    private void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTakeDamage) return; // �̹� �������� ����

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

    // �ִϸ��̼� �̺�Ʈ
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
