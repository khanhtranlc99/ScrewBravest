using UnityEngine;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Common;
using BigoAds.Scripts.Api.Constant;

public class AdHelper
{
    public static void ShowBannerAd(AndroidJavaObject bannerAd)
    {
        ShowBannerAd(bannerAd, BigoPosition.Bottom);
    }

    public static void ShowBannerAd(AndroidJavaObject bannerAd, BigoPosition position)
    {
        if (bannerAd == null) return;
        var adView = bannerAd.Call<AndroidJavaObject>("adView");
        SetViewPosition(adView, position);
    }

    public static void ShowNativeAd(AndroidJavaObject nativeAd)
    {
        ShowNativeAd(nativeAd, BigoPosition.Bottom);
    }

    public static void RemoveAdView()
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        new AndroidJavaClass("sg.bigo.ads.AdHelper").CallStatic("removeAdView", activity);
    }

    public static void ShowNativeAd(AndroidJavaObject nativeAd, BigoPosition position) 
    {
        if (nativeAd == null) return;
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var adView = new AndroidJavaClass("sg.bigo.ads.AdHelper").CallStatic<AndroidJavaObject>("renderNativeAdView", activity, nativeAd, "layout_bigo_native_ad");
        SetViewPosition(adView, position);
    }

    public static void SetViewPosition(AndroidJavaObject adView, BigoPosition position)
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        int positionInt;
        switch (position)
        {
            case BigoPosition.Top:
                positionInt = 48;
                break;
            case BigoPosition.Middle:
                positionInt = 16;
                break;
            case BigoPosition.Bottom:
            default:
                positionInt = 80;
                break;
        }
        new AndroidJavaClass("sg.bigo.ads.AdHelper").CallStatic("addAdView", activity, adView, positionInt);
    }
    

    public abstract class Task : AndroidJavaProxy
    {
        public Task() : base("java.lang.Runnable")
        {
        }

        public abstract void run();
    }

    public static void DestroyAd(AndroidJavaObject ad)
    {
        if (ad != null) {
            PostToAndroidMainThread(new DestryAdTask(ad));
        }
    }

    private class DestryAdTask : Task
    {
        public AndroidJavaObject Ad;
        
        public DestryAdTask(AndroidJavaObject ad)
        {
            this.Ad = ad;
        }

        public override void run()
        {
            Ad.Call("destroy");
            AdHelper.RemoveAdView();
        }
    }

    public static void PostToAndroidMainThread(Task task)
    {
        new AndroidJavaClass("sg.bigo.ads.AdHelper").CallStatic("postToAndroidMainThread", task);
    }
}