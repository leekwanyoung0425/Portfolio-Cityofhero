using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private float _loadingProgress;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SceneChange(int i)
    {       
        StartCoroutine(SceneLoading(i));
    }

    IEnumerator SceneLoading(int i)
    {
        yield return SceneManager.LoadSceneAsync("2.LodingScene");
        yield return StartCoroutine(LoadScene(i));
    }

    IEnumerator LoadScene(int i)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(i);
        op.allowSceneActivation = false;
        Slider lodingBar = FindObjectOfType<Slider>();
        lodingBar.maxValue = 1f;
        float timer = 0.0f;
        //씬로딩이 끝나기 전까진 씬을 활성화 하지 않는다.
        while(!op.isDone)
        {
            timer += Time.deltaTime;
            if(op.progress >= 0.9f)
            {
                lodingBar.value = Mathf.Lerp(lodingBar.value, 1f, timer);
                if (lodingBar.value == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                lodingBar.value = Mathf.Lerp(lodingBar.value, op.progress, timer);
                if(lodingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            yield return null;
        }        
    }
}
