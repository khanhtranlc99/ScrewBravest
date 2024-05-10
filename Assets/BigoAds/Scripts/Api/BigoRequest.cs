using System;
using UnityEngine;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Api
{
    [Serializable]
    public class BigoRequest
    {
        [SerializeField] private string extraInfo;
        [SerializeField] private int age;
        [SerializeField] private BGAdGender gender;
        [SerializeField] private long activatedTime;

        public string ExtraInfoJson
        {
            get => extraInfo;
            set => extraInfo = value;
        }

        /// Only works on Android
        public int Age
        {
            get => age;
            set => age = value;
        }

        /// Only works on Android
        public BGAdGender Gender
        {
            get => gender;
            set => gender = value;
        }

        /// Only works on Android
        public long ActivatedTime
        {
            get => activatedTime;
            set => activatedTime = value;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}