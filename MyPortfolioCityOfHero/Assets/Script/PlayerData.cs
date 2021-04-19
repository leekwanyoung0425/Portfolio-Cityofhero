using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerData : MonoBehaviour
{
    string playerName = "히어로";
    float maxHp = 1000.0f;
    float curHp = 0.0f;
    public TMP_Text playerNameTextPrefab;
    public Slider hpBarPrefab;
    public Transform uiParent;
    public Canvas canvas;
    public Transform uiPos;
    Vector2 canvasSize;
    TMP_Text text;
    Slider hpBar;
    TMP_Text damageTexObj;
    public TMP_Text damageTextPrefab;
    public GameObject damageTextParent;

    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
        canvasSize.x = canvas.pixelRect.width * 0.5f;
        canvasSize.y = canvas.pixelRect.height * 0.5f;
        text = Instantiate(playerNameTextPrefab);
        text.transform.SetParent(uiParent);
        hpBar = Instantiate(hpBarPrefab);
        hpBar.transform.SetParent(uiParent);
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
        text.text = playerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerControl>().IsDead)
        {
            HpBarSetting();
            NickNameSetting();
        }
    }

    public void HpBarSetting()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(uiPos.position);
        pos.x -= canvasSize.x;
        pos.y -= canvasSize.y;
        pos.y += 5.0f;
        hpBar.transform.localPosition = pos;
    }

    public void NickNameSetting()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(uiPos.position);
        pos.x -= canvasSize.x;
        pos.y -= canvasSize.y;
        pos.y += 25.0f;
        text.transform.localPosition = pos;
        text.outlineColor = new Color(0, 0, 255);
    }

    public void Damage(float damage)
    {
        if (curHp - damage <= Mathf.Epsilon)
        {
            curHp = 0.0f;
            hpBar.value = 0.0f;
            Destroy(text.gameObject);
            Destroy(hpBar.gameObject);
            GetComponent<PlayerControl>().ChangeState(PlayerControl.STATE.DEAD);
        }
        else
        {
            curHp -= damage;
            hpBar.value -= damage;
            StartCoroutine(InstantiateDamageText(damage, uiPos));
        }
    }

    IEnumerator InstantiateDamageText(float damage, Transform target)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 0.8f);
        pos.x -= canvasSize.x;
        pos.y -= canvasSize.y;
        float speed = 5.0f;
        damageTexObj = Instantiate(damageTextPrefab);
        damageTexObj.transform.SetParent(damageTextParent.transform);
        damageTexObj.transform.localPosition = pos;
        TMP_Text damagetext = damageTexObj.transform.GetComponent<TextMeshProUGUI>();
        damagetext.text = damage.ToString();
        damagetext.color = new Color(255, 0, 0);

        bool stop = false;

        while (!stop)
        {
            damageTexObj.transform.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            Color col = damagetext.color;
            col.a -= Time.deltaTime * 0.5f;
            damagetext.color = col;

            if (damagetext.color.a <= Mathf.Epsilon)
            {
                Destroy(damagetext.gameObject);
                stop = true;
            }
            yield return null;
        }
    }
}
