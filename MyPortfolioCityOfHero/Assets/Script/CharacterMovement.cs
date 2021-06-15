using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public AnimationActionPlay _animationActionPlay;
    public GameObject springArm;

    Animator anim = null;
    public Animator animator
    {
        get
        {
            if(anim == null)
            {
                anim = GetComponentInChildren<Animator>();
            }

            return anim;
        }
    }

    Rigidbody rigid =null;
    public new Rigidbody rigidbody
    {
        get
        {
            if (rigid == null)
            {
                rigid = GetComponent<Rigidbody>();
            }

            return rigid;
        }
    }


    public float MaxSpeed = 5f;
    public float rotSpeed = 360.0f;
    public float JumpPower = 0.5f;


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
   

    public void JumpAnimationPlay()
    {
        _animationActionPlay.Jump += () => rigidbody.velocity = Vector3.up * JumpPower;
        animator.SetTrigger("Jump");
    }




    public void KeyboardMovePosition(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector3 dir = (this.transform.right * horizontal) + (this.transform.forward * vertical);
        dir.Normalize();


        rigidbody.MovePosition(this.transform.position + (dir * MaxSpeed * Time.deltaTime));

    }

    public void KeyboardFlyMovePosition(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector3 dir = (this.transform.right * horizontal) + (springArm.transform.forward * vertical);
        dir.Normalize();

     

        if (horizontal > 0 || vertical > 0)
        {
            this.transform.GetChild(0).localRotation = springArm.transform.localRotation;
        }

        rigidbody.MovePosition(this.transform.position + (dir * MaxSpeed * Time.deltaTime));
    }
}
