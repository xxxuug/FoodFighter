using System.Collections;
using UnityEngine;

public class Menu_OpenAndClose : MonoBehaviour
{
    private Animator anim;
    private bool isOpen = false;
    private Coroutine closeRoutine = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenPanel()
    {
        if (anim == null) return;

        isOpen = true;
        gameObject.SetActive(true);
        anim.SetBool("isOpen", true);

        // ������ ���̶�� ��� �ߴ�
        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
            closeRoutine = null;
        }
    }

    public void ClosePanel()
    {
        //  if (anim == null) return;
        if (anim == null || !isOpen || !gameObject.activeInHierarchy) return;
        // if (!isOpen) return;

        isOpen = false;
        anim.SetBool("isOpen", false);

        // �ڷ�ƾ �ߺ� ����, ��� ���� �����ϵ���
        if (closeRoutine != null)
            StopCoroutine(closeRoutine);

        //  StartCoroutine(Delay());
        closeRoutine = StartCoroutine(DelayAndClose());
    }

    IEnumerator DelayAndClose()
    {
        // �ִϸ��̼� ���� ��ٸ�
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float delay = state.length;

        // yield return new WaitForSeconds(1.0f); // �������� �ִϸ��̼� ���̿� �°� ����
        yield return new WaitForSeconds(delay); // �������� �ִϸ��̼� ���̿� �°� ����
        gameObject.SetActive(false);
        closeRoutine = null;
    }
}
