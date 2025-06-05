using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldController : BaseController
{
    public Image GoldIcon;

    private Rigidbody2D _rigidbody2D;
    private Vector3 _worldGoldPos;
    private Vector3 _startPos;

    private float _speed = 10f;
    private bool _isMoving = false;

    protected override void Initialize() { }

    private void OnEnable()
    {
        if (GoldIcon == null)
            GoldIcon = GameObject.Find("Gold Icon - Image").GetComponent<Image>();

        if (_rigidbody2D == null)
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        StartCoroutine(BounceAndMove());
    }

    void Update()
    {
        if (_isMoving)
            MoveToGoldIcon();
    }

    // 골드 ui 아이콘으로 이동시키기
    void MoveToGoldIcon()
    {
        transform.position = Vector3.MoveTowards(transform.position, _worldGoldPos, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _worldGoldPos) < 0.2f)
        {
            int randGold = Random.Range(300, 500);
            GameManager.Instance.AddGold(randGold);
            ObjectManager.Instance.Despawn(this);
            _isMoving = false;
        }
    }

    // 튀어오른 후 중력 없애고 골드 ui 아이콘 위치 찾기
    IEnumerator BounceAndMove()
    {
        _rigidbody2D.gravityScale = 1;
        _rigidbody2D.linearVelocity = Vector3.zero;

        float posX = Random.Range(-1f, 1f);
        float posY = Random.Range(1f, 3f);
        _rigidbody2D.AddForce(new Vector2(posX, posY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.8f);

        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.linearVelocity = Vector3.zero;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, GoldIcon.rectTransform.position);
        _worldGoldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        _isMoving = true;
    }
}
