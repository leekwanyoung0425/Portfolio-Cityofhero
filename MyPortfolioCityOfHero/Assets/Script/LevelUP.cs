using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUP : MonoBehaviour
{
    Coroutine effect;
    // Start is called before the first frame update
    void Start()
    {
        if (effect != null) StopCoroutine(effect);
        effect = StartCoroutine(EffectOff());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator EffectOff()
    {
        yield return new WaitForSeconds(5.0f);
        this.gameObject.SetActive(false);
    }
}
