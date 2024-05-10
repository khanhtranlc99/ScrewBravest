#if UNITY_IOS
using System.Collections.Generic;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{
    public class BigoUnityTools
    {
        private static int mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

        internal static bool isMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadID;
        }

        internal static void LOGWithMessage(string className,string methodName,string msg)
        {
            Debug.Log($"BigoAds-iOS-[{className}]-[{methodName}]-{msg}");
        }
    }
}
#endif