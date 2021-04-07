using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHp : MonoBehaviour
{
    [SerializeField]
    private MonsterData _monsterData;
    Coroutine _followHpBar = null;

    Canvas _canvas;
    Vector2 halfsize;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        halfsize.x = _canvas.pixelRect.width / 2.0f;
        halfsize.y = _canvas.pixelRect.height / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFollow(Transform target)
    {
        if (_followHpBar !=null) StopCoroutine(_followHpBar);
        _followHpBar = StartCoroutine(FollowTarget(target));

    }

    IEnumerator FollowTarget(Transform target)
    {
        while(target!= null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(target.position);
            pos.x -= halfsize.x;
            pos.y -= halfsize.y;

            this.transform.localPosition = pos;

            yield return null;
        }
    }
}
