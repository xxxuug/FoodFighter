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

        // 닫히는 중이라면 즉시 중단
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

        // 코루틴 중복 방지, 즉시 새로 시작하도록
        if (closeRoutine != null)
            StopCoroutine(closeRoutine);

        //  StartCoroutine(Delay());
        closeRoutine = StartCoroutine(DelayAndClose());
    }

    IEnumerator DelayAndClose()
    {
        // 애니메이션 길이 기다림
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float delay = state.length;

        // yield return new WaitForSeconds(1.0f); // 내려가는 애니메이션 길이에 맞게 조절
        yield return new WaitForSeconds(delay); // 내려가는 애니메이션 길이에 맞게 조절
        gameObject.SetActive(false);
        closeRoutine = null;
    }
}
