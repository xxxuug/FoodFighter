using UnityEngine;

public class AttackController : MonoBehaviour
{
    private bool _isTakeDamage = false; // �������� ������� ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTakeDamage) return; // �̹� �������� ����

        if (other.CompareTag(Define.PlayerTag) && gameObject.activeSelf)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            EnemyController enemyController = GetComponentInParent<EnemyController>();
            playerController.TakeDamage(enemyController._damage);
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
