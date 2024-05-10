using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class StartLoading : MonoBehaviour
{
    public Text txtLoading;
    public Image progressBar;
    private string sceneName;
    public bool wasRunLoad;
    private int prevenOpenAppAds;

    public void Init()
    {
        wasRunLoad = false;
        progressBar.fillAmount = 0f;

        StartCoroutine(LoadingText());
     
    }
    public void InitState()
    {

        if(!wasRunLoad)
        {
            wasRunLoad = true;
        
        }
   

    }
    public void LoadGamePlay()
    {
        StartCoroutine(ChangeScene());
    }

    // Use this for initialization
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        sceneName = "Game Scene";
       
        var _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!_asyncOperation.isDone)
        {
            progressBar.fillAmount = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            yield return null;
    }
    }

    IEnumerator LoadingText()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            txtLoading.text = "LOADING .";
            yield return wait;

            txtLoading.text = "LOADING ..";
            yield return wait;

            txtLoading.text = "LOADING ...";
            yield return wait;

        }
    }
    private IEnumerator OnChangeScene()
    {
        yield return new WaitForSeconds(1);
        prevenOpenAppAds += 1;
   
    }

}
