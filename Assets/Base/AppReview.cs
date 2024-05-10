using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using Google.Play.Review;
#endif

public class AppReview : MonoBehaviour
{
#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private Coroutine _coroutine;
#endif
    public void Init()
    {
    #if UNITY_ANDROID
        _coroutine = StartCoroutine(InitReview());
   #endif
    }


    // Ham nay Show AppReview
    public void RateAndReview()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#elif UNITY_ANDROID
        StartCoroutine(LaunchReview());
#endif
    }


    private IEnumerator InitReview(bool force = false)
    {
        if (_reviewManager == null) _reviewManager = new ReviewManager();

        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            if (force) DirectlyOpen();
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();
    }

    public IEnumerator LaunchReview()
    {
        if (_playReviewInfo == null)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            yield return StartCoroutine(InitReview(true));
        }
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        Debug.LogError(_playReviewInfo);
        yield return launchFlowOperation;

        _playReviewInfo = null;
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            DirectlyOpen();
            yield break;
        }
    }

    public List<GameObject> lsBolt;
    public List<GameObject> lsBroad;

    public Material tempMt;
    public GameObject Broad;
    public List<Sprite> lsSprite;
    [Button]
    private void Test()
    {

        var temp = GameObject.FindObjectsOfType<GameObject>();
        lsBolt = new List<GameObject>();
        lsBroad = new List<GameObject>();
        foreach (var item in temp)
        {
            if (item.name == "oc_tron")
            {
                lsBolt.Add(item);
            }
            if (item.name == "Board")
            {
                lsBroad.Add(item);
            }
          

        }
        //foreach (var item in lsBolt)
        //{

        //    item.GetComponent<MeshRenderer>().material = tempMt;
        //}
        foreach (var item in lsBroad)
        {

            item.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
            var temp1 = Instantiate(Broad);
            temp1.transform.position = item.transform.position;
            temp1.transform.parent = item.transform;
        }
        Debug.LogError("Test" + temp.Length);
    }
    public void DirectlyOpen()
    {
        Application.OpenURL($"https://play.google.com/store/apps/details?id=com.zgames.screw.puzzle.nuts.bolts");
    }


}