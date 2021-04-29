using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInterface : MonoBehaviour
{
    public GameObject skillInterface;
    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(isActive) 
            {
                skillInterface.SetActive(false);
                isActive = false;
            }
            else
            {
                skillInterface.SetActive(true);
                isActive = true;
            }                             
        }
    }

    public void ExitButtonClick()
    {
        skillInterface.SetActive(false);
        isActive = false;
    }
}
