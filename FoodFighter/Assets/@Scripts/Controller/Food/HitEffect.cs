using System.Collections;
using UnityEngine;

public class HitEffect : BaseController
{

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.3f);
        ObjectManager.Instance.Despawn(this);
    }

    protected override void Initialize() { }
}
