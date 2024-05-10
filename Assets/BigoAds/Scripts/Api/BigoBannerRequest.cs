using System;
using BigoAds.Scripts.Api.Constant;
using UnityEngine;

namespace BigoAds.Scripts.Api
{
    [Serializable]
    public class BigoBannerRequest : BigoRequest
    {
        [SerializeField()]
        private BigoBannerSize size; 
        public BigoBannerSize Size => size;

        public BigoPosition Position { get; }

        public BigoBannerRequest(BigoBannerSize size, BigoPosition position = BigoPosition.Bottom)
        {
            this.size = size;
            this.Position = position;
        }
    }
}