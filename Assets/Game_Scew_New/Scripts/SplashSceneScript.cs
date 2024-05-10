using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class SplashSceneScript : MonoBehaviour
{
    public Image Bar;
    void Start()
    {

        Advertisements.Instance.Initialize();
        Invoke("LoadMenu", 3f);
    }
    private void Update()
    {

        Bar.fillAmount = Mathf.Lerp(Bar.fillAmount, 1, .5f * Time.deltaTime);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
