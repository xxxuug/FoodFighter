using System.Collections;
using UnityEngine;

public class Menu_OpenAndClose : MonoBehaviour
{
    private Animator anim;

    private bool isOpen = false;

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
    }

    public void ClosePanel()
    {
        if (anim == null) return;
        if (!isOpen) return;
        isOpen = false;
        anim.SetBool("isOpen", false);

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f); // 내려가는 애니메이션 길이에 맞게 조절
        gameObject.SetActive(false);
    }
}
