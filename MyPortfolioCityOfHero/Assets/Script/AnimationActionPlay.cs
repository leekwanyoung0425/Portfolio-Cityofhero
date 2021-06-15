using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void AnimationAction();

public class AnimationActionPlay : MonoBehaviour
{
    public event AnimationAction Jump;
    public event AnimationAction Teleport;
    public float prevPos;
    public Transform tr;
    public Animator anim;
    public GameObject player;

    public void CharacterJumpAction()
    {
        Jump?.Invoke();
        Jump = null;
    }

    public void AddForce()
    {
        transform.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * 2500f);
        StartCoroutine("JumpPos");
    }

    IEnumerator JumpPos()
    {
        prevPos = tr.position.y;
        while (prevPos <= tr.position.y)
        {
            prevPos = tr.position.y;
            yield return null;
        }

        anim.SetTrigger("LandingMode");

        while (!player.GetComponent<PlayerControl>().isGround)
        {
            yield return null;
        }

        anim.SetTrigger("JumpEndMode");
        player.GetComponent<PlayerControl>().ChangeState(PlayerControl.STATE.IDLE);
    }

    public void ChracterTeleportAction()
    {
        Teleport?.Invoke();
        Teleport = null;
    }
}
