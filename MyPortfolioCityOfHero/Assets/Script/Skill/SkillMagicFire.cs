using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillMagicFire : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    RectTransform size;
    public Animator myAnim;
    public GameObject magicFireEffectBallPrefab;
    public Transform magicFireLeftPos;
    public Transform magicFireRightPos;
    GameObject magicFireLeftEffect;
    GameObject magicFireRightEffect;
    GameObject magicFireLeftEffect1;
    GameObject magicFireRightEffect1;
    public GameObject magicFireEffectExplodePrefab;
    public GameObject magicFireEffectArrowPrefab;
    public GameObject coolDownEffect;

    private void Start()
    {
        skillNumber = 2;
        isRotateSkill = true;
        skillName = "Skill_MagicFire";
        needPrecedingSkillName = "Skill_NormalPunch";
        iconImage = gameObject.GetComponent<Image>();
        //iconImage.sprite = Resources.Load<Sprite>("Icon/S_Blue_firework");
        coolDownTime = 4.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
        damage = 30.0f;
        dist = 15.0f;
        skillStep = 2;
    }

    private void Update()
    {
        
    }



    public override void SkillAnim()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Skill_MagicFire"))
        {
            magicFireLeftEffect = Instantiate(magicFireEffectBallPrefab, magicFireLeftPos.position, magicFireLeftPos.rotation);
            magicFireRightEffect = Instantiate(magicFireEffectBallPrefab, magicFireRightPos.position, magicFireRightPos.rotation);
            magicFireLeftEffect.transform.SetParent(magicFireLeftPos);
            magicFireRightEffect.transform.SetParent(magicFireLeftPos);

            StartCoroutine(EffectUp(magicFireLeftPos, magicFireRightPos, target));

            myAnim.SetTrigger("Skill_MagicFire");
        }
    }
    public override void CoolDown()
    {
        StartCoroutine(CoolDownInit(coolDownTime));
    }

    IEnumerator CoolDownInit(float coolDownTime)
    {
        size = this.transform.parent.GetComponent<RectTransform>();
        isCoolDown = true;
        size.localScale = new Vector3(0.3f, 0.3f, 0.0f);
        float delta = 0.0f;
        float startTime = 0.0f;
        Color col = iconImage.color;
        col.a = 0.15f;
        iconImage.color = col;
        col.a = 0f;
        float setEffectTime = coolDownTime * 0.6f;
        while (startTime < coolDownTime)
        {
            if (setEffectTime < startTime)
            {
                GameObject obj = Instantiate(coolDownEffect, this.transform.parent);
                obj.GetComponent<CoolDownEffect>().startime = setEffectTime;
                obj.GetComponent<CoolDownEffect>().endtime = coolDownTime;
                obj.GetComponent<CoolDownEffect>().midletime = Mathf.Lerp(setEffectTime, coolDownTime, 0.5f);
                obj.GetComponent<CoolDownEffect>().start = true;
                setEffectTime = coolDownTime;
            }
            startTime += Time.deltaTime;
            delta = 0.7f * Time.deltaTime / coolDownTime;
            col.a = 0.85f * Time.deltaTime / coolDownTime;
            iconImage.color += col;
            size.localScale += new Vector3(delta, delta, 0.0f);
            yield return null;
        }
        size.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        col.a = 1.0f;
        iconImage.color = col;
        isCoolDown = false;
    }

    IEnumerator EffectUp(Transform leftEffect, Transform rightEffect, Transform target)
    {
        float heightStep1 = 1.0f;
        float heightStep2 = 2.0f;
        float speed = 1.0f;
        while (leftEffect.position.y < heightStep1)
        {
            leftEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            rightEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);

            yield return null;
        }
        Destroy(magicFireLeftEffect);
        Destroy(magicFireRightEffect);

        magicFireLeftEffect = Instantiate(magicFireEffectExplodePrefab, leftEffect.position, leftEffect.rotation);
        magicFireRightEffect = Instantiate(magicFireEffectExplodePrefab, rightEffect.position, rightEffect.rotation);
        magicFireLeftEffect.transform.SetParent(leftEffect);
        magicFireRightEffect.transform.SetParent(rightEffect);
        while (leftEffect.position.y < heightStep2)
        {
            leftEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            rightEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);

            if (magicFireLeftEffect.transform.Find("Ring2") != null && magicFireLeftEffect.transform.Find("Ring2").GetComponent<ParticleSystem>().time > 0.1f && magicFireLeftEffect1 == null)
            {
                magicFireLeftEffect1 = Instantiate(magicFireEffectArrowPrefab, leftEffect.position, leftEffect.rotation);
                magicFireRightEffect1 = Instantiate(magicFireEffectArrowPrefab, rightEffect.position, rightEffect.rotation);
                magicFireLeftEffect1.transform.SetParent(leftEffect);
                magicFireRightEffect1.transform.SetParent(rightEffect);
                magicFireLeftEffect1.transform.LookAt(target);
                magicFireRightEffect1.transform.LookAt(target);
            }
            yield return null;
        }

        Destroy(magicFireLeftEffect);
        Destroy(magicFireRightEffect);

        StartCoroutine(MovigMagicFire(magicFireLeftEffect1.transform, magicFireRightEffect1.transform, target, leftEffect, rightEffect));
    }
    IEnumerator MovigMagicFire(Transform fire1, Transform fire2, Transform target, Transform leftEffect, Transform rightEffect)
    {

        float speed = 10.0f;
        float delta1 = speed * Time.deltaTime;
        float delta2 = speed * Time.deltaTime;

        while (fire1 != null && fire2 != null && target != null)
        {
            Vector3 dir1 = (target.position + Vector3.up * 1.3f) - fire1.position;
            float dist1 = dir1.magnitude;
            dir1.Normalize();
            Vector3 dir2 = (target.position + Vector3.up * 1.3f) - fire2.position;
            float dist2 = dir2.magnitude;
            dir2.Normalize();

            if (dist1 < delta1 || dist2 < delta2)
            {
                if (dist1 < delta1)
                {
                    delta1 = dist1;
                }
                else if (dist2 < delta2)
                {
                    delta2 = dist2;
                }
                playerAttack.TargetDamage();
                Destroy(fire1.gameObject);
                Destroy(fire2.gameObject);
                leftEffect.localPosition = new Vector3(-1, 0, 0);
                rightEffect.localPosition = new Vector3(1, 0, 0);
            }

            if (fire1 != null) fire1.Translate(dir1 * delta1, Space.World);
            if (fire2 != null) fire2.Translate(dir2 * delta2, Space.World);

            yield return null;
        }
    }
}
