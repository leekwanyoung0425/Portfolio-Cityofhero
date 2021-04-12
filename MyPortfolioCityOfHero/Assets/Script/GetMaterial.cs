using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMaterial : MonoBehaviour
{
    Material[] _myMaterial;

    public Material[] GetmyMaterial
    {
        get
        {
            if(_myMaterial == null)
            {
                _myMaterial = transform.GetComponent<Renderer>().materials;
            }

            return _myMaterial;
        }
    }
}
