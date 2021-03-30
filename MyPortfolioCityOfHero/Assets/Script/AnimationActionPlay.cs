using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void AnimationAction();

public class AnimationActionPlay : MonoBehaviour
{
    public event AnimationAction Jump;

    public void CharacterJumpAction()
    {
        Jump?.Invoke();
        Jump = null;
    }
}
