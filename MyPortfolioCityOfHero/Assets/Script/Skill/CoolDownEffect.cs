using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownEffect : MonoBehaviour
{
    public float startime = 0.0f;
    float secondStartime = 0.0f;
    public float endtime = 0.0f;
    public float midletime = 0.0f;
    RectTransform size;
    Image image;
    float delta = 0.0f;
    public bool start = false;
    public bool firststart = false;
    public bool secondstart = false;
    float time = 0.0f;

    void Start()
    {
        size = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        Color col = image.color;
        col.a = 0.3f;
        image.color = col;
        secondStartime = endtime;
        time = midletime - startime;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            firststart = true;
            secondstart = true;
            if (firststart)
            {
                if (startime < midletime)
                {
                    Color col = image.color;
                    startime += Time.deltaTime;
                    delta = 1.0f * Time.deltaTime / time;
                    col.a += 0.7f * Time.deltaTime / time;
                    image.color = col;
                    size.localScale += new Vector3(delta, delta, 0.0f);

                    if (startime >= midletime)
                    {
                        firststart = false;
                        secondStartime = midletime;
                        time = endtime - secondStartime;
                        delta = 2.0f;
                        col.a = 1.0f;
                        image.color = col;
                        size.localScale = new Vector3(delta, delta, 0.0f);
                    }
                }
            }

            if (secondstart)
            {
                if (secondStartime < endtime)
                {
                    Color col = image.color;
                    secondStartime += Time.deltaTime;
                    col.a -= 0.7f * Time.deltaTime / time;
                    image.color = col;

                    if (secondStartime >= endtime)
                    {
                        //col.a = 0.3f;
                        //image.color = col;
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
