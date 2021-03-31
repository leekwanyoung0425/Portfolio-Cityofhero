using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHeadPosition : MonoBehaviour
{
    public Vector3 pos;
    // Start is called before the first frame update
    private void Awake()
    {
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
