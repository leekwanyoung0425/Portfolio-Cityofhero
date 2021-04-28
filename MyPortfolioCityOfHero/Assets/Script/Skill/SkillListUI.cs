using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListUI : MonoBehaviour
{
    public Image[] leftLineEffect;
    public Image[] rightLineEffect;
    public int LeftRightSelect = 0;

 
    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
    }

 

    public void LineDrawEffect(int skillStep)
    {
        switch(LeftRightSelect)
        {
            case 1:
                StartCoroutine(LeftLineDraw(skillStep));
                break;
            case 2:
                StartCoroutine(RightLineDraw(skillStep));
                break;
        }
    }

    IEnumerator LeftLineDraw(int skillStep)
    {
        Image img = leftLineEffect[skillStep-2];
        float delta = 0.0f;
        float speed = 1.0f;
        float targetamount = 1.0f;
        while (img.fillAmount < 1.0f)
        {
            delta = speed * Time.smoothDeltaTime;
            
            if(targetamount - delta <= Mathf.Epsilon)
            {
                delta = targetamount;
            }
            targetamount -= delta;

            img.fillAmount += delta;
            yield return null;
        } 
    }

    IEnumerator RightLineDraw(int skillStep)
    {
        Image img = rightLineEffect[skillStep - 2];
        float delta = 0.0f;
        float speed = 1.0f;
        float targetamount = 1.0f;
        while (img.fillAmount < 1.0f)
        {
            delta = speed * Time.smoothDeltaTime;
            if (targetamount - delta <= Mathf.Epsilon)
            {
                delta = targetamount;
            }
            targetamount -= delta;

            img.fillAmount += delta;
            yield return null;
        }
    }

}
