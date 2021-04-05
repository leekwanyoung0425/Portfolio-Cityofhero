using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator myAnim;
    public TargetSelect targetSelect;
    public float rotSpeed = 5.0f;
    bool attackStart = false;


    public enum STATE
    {
        Wait, NormalPunch, SkillKick, SkillMagicFire, SkillBomb
    }

    public STATE myState = STATE.Wait;
    float horizontal = 0.0f;
    float vertical = 0.0f;

    public PlayerControl playerControl;
    Coroutine changeweight = null;

    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch(myState)
        {
            case STATE.Wait:
                if (changeweight != null) StopCoroutine(changeweight);
                changeweight = StartCoroutine(ChangeLayerWeight(1, 0.0f, 0.5f));
                playerControl.ChangeState(PlayerControl.STATE.WALK);
                break;
            case STATE.NormalPunch:
                StartCoroutine(CharacterRotate(targetSelect.GetselectTarget));
                if (changeweight != null && attackStart) StopCoroutine(changeweight);
                changeweight = StartCoroutine(ChangeLayerWeight(1, 1.0f, 0.5f));
                attackStart = false;
                break;
            case STATE.SkillKick:
                break;
            case STATE.SkillMagicFire:
                break;
            case STATE.SkillBomb:
                break;
        }           
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Wait:
                break;
            case STATE.NormalPunch:
                myAnim.SetTrigger("Normal_Punch");
                if (horizontal != 0.0f || vertical != 0.0f)
                {
                    ChangeState(STATE.Wait);
                }                
                break;
            case STATE.SkillKick:
                break;
            case STATE.SkillMagicFire:
                break;
            case STATE.SkillBomb:
                break;
        }
    }

    IEnumerator ChangeLayerWeight(int layer, float target, float t)
    {
        float speed = t > Mathf.Epsilon ? 1.0f / t : 1f;
        float curweight = myAnim.GetLayerWeight(layer);
        float dir = target - curweight > 0f ? 1f : -1f;
        float value = Mathf.Abs(target - curweight);

        while (curweight < target - Mathf.Epsilon || curweight > target + Mathf.Epsilon)
        {
            float delta = Time.deltaTime * speed;
            if (value - delta <= Mathf.Epsilon)
            {
                delta = value;
            }
            value -= delta;

            curweight += dir * delta;
            myAnim.SetLayerWeight(layer, curweight);
            yield return null;
        }
    }

    IEnumerator CharacterRotate(Transform target)
    {
        float rotDirection = 1.0f;
        float delta = 0.0f;

        Vector3 direction = target.position - this.transform.position;
        direction.Normalize();

        float rot = Vector3.Dot(direction, this.transform.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;

        if(Vector3.Dot(this.transform.right, direction) < 0.0f)
        {
            rotDirection = -1.0f;
        }

        while(rot > Mathf.Epsilon && target != null)
        {
            delta = rotSpeed * Time.smoothDeltaTime;
            
            if(rot - delta > rot)
            {
                delta = rot;
                attackStart = true;
            }
            rot -= delta;

            this.transform.Rotate(this.transform.up * delta * rotDirection);

            yield return null;
        }
    }
}
