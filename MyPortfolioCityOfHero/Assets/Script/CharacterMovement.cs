using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Transform myModel;
    Animator _anim = null;
    //프로퍼티 사용방식.
    Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = this.GetComponentInChildren<Animator>();
            }
            return _anim;
        }
    }

    public float RunSpeed = 2.0f;
    public float WalkMaxSpeed = 1.0f;
    float CurSpeed = 0.0f;
    public float RotSpeed = 360.0f;
    Coroutine move = null;
    //Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePosition(Vector3 pos)
    {
        if (move != null) StopCoroutine(move);
        move = StartCoroutine(Moving(pos));
    }

    IEnumerator Moving(Vector3 point)
    {        
        Vector3 dir = point - this.transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        float rot = Vector3.Dot(dir, myModel.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;
        float rdir = 1.0f;
        if(Vector3.Dot(myModel.right,dir) < 0.0f)
        {
            rdir = -1f;
        }

        //float 근사값. 0.0f = -Mathf.Epsilon ~ Mathf.Epsilon
        while (dist > Mathf.Epsilon || rot > Mathf.Epsilon)
        {
           // CurSpeed = CurSpeed < WalkMaxSpeed ? CurSpeed + Time.deltaTime * WalkMaxSpeed : WalkMaxSpeed;
            CurSpeed = Mathf.Clamp(CurSpeed + Time.deltaTime * WalkMaxSpeed, 0.0f, WalkMaxSpeed);
            myAnim.SetFloat("Speed", CurSpeed/ WalkMaxSpeed);
            float delta = 0.0f;
            if (dist > Mathf.Epsilon)
            {
                #region Move
                delta = CurSpeed * Time.deltaTime;

                if (dist - delta <= Mathf.Epsilon)
                {
                    delta = dist;
                }

                dist -= delta;
                this.transform.Translate(dir * delta, Space.World);
                #endregion
            }

            if (rot > Mathf.Epsilon)
            {
                #region Rotate
                delta = RotSpeed * Time.smoothDeltaTime;

                if (rot - delta > rot)
                {
                    delta = rot;
                }

                rot -= delta;
                //this.transform.Rotate(this.transform.up * delta * rdir);
                myModel.Rotate(this.transform.up * delta * rdir);
                #endregion
            }            
            yield return null;
        }
        
        while(CurSpeed > Mathf.Epsilon)
        {
            CurSpeed = Mathf.Clamp(CurSpeed - Time.deltaTime * 2.0f, 0.0f, MaxSpeed);
            myAnim.SetFloat("Speed", CurSpeed / MaxSpeed);
            yield return null;
        }

       myAnim.SetFloat("Speed", 0.0f);
    }
}
