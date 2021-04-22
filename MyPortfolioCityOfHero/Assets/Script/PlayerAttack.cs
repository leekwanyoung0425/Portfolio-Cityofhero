using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void SkillInit();

public class PlayerAttack : MonoBehaviour
{
    public event SkillInit skillInit;

    public Animator myAnim;
    public TargetSelect targetSelect;
    public float rotSpeed = 5.0f;
    public Transform myModel;
    public CameraMove cameramove;


    public PlayerControl playerControl;
    Coroutine characterRotate = null;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    IEnumerator CharacterRotate(Transform target)
    {
        float rotDirection = 1.0f;
        float delta = 0.0f;

        Vector3 direction = target.position - myModel.position;
        direction.Normalize();
        float rot = Vector3.Dot(direction, myModel.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;

        if(Vector3.Dot(myModel.right, direction) < 0.0f)
        {
            rotDirection = -1.0f;
        }
     
        while (rot > Mathf.Epsilon && target.gameObject != null)
        {
            delta = rotSpeed * Time.smoothDeltaTime;
            
            if(rot - delta <= Mathf.Epsilon)
            {

                delta = rot;
         
            }
           rot -= delta;
           myModel.Rotate(myModel.transform.up * delta * rotDirection);

            yield return null;
        }
        cameramove.TurnRight = myModel.localRotation.eulerAngles;
        SkillInit();
    }

 

    public void AttackReady()
    {
        if (characterRotate != null) StopCoroutine(characterRotate);
        characterRotate = StartCoroutine(CharacterRotate(targetSelect.GetselectTarget));
    }
    
   
    public void SkillInit()
    {
        skillInit?.Invoke();
        skillInit = null;
    }
}
