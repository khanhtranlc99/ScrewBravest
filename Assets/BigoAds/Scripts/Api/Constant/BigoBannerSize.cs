using System;
using UnityEngine;

namespace BigoAds.Scripts.Api.Constant
{
    [Serializable]
    public struct BigoBannerSize
    {
        public static readonly BigoBannerSize BANNER_W_320_H_50 = new BigoBannerSize(320,50);
        public static readonly BigoBannerSize BANNER_W_300_H_250 = new BigoBannerSize(300,250);

        [SerializeField] private int width;
        [SerializeField] private int height;
        public int Width => width;
        public int Height => height;

        public BigoBannerSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}