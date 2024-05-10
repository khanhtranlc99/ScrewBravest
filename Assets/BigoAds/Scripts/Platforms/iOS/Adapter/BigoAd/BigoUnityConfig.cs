#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BigoAds.Scripts.Api;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{
    class BigoUnityConfig
    {
        private IntPtr _config;
        internal void Init(BigoAdConfig config)
        {
            _config = BigoIOS_config(config.AppId);
            BigoIOS_configSetInfo(_config, config.DebugLog, config.Age, config.Gender, config.ActivatedTime);
            // set extra
            foreach (KeyValuePair<string, string> item in config.ExtraDictionary)
            {
                BigoIOS_configSetExtra(_config, item.Key, item.Value);
            }

        }

        public IntPtr getInternalConfig()
        {
            return _config;
        }

        //c# - > oc
        [DllImport("__Internal")]
        private static extern IntPtr BigoIOS_config(string appID);
            
        [DllImport(dllName: "__Internal")]
        private static extern void BigoIOS_configSetInfo(IntPtr configPt, bool debugLog, int age, int gender, long activatedTime);

        [DllImport(dllName: "__Internal")]
        private static extern void BigoIOS_configSetExtra(IntPtr configPt, string key, string extra);
        
    }
}
#endif