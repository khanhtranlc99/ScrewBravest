using System;

namespace BigoAds.Scripts.Api.Constant
{
    [Serializable]
    public enum BGAdLossReason
    {
        InternalError = 1,
        Timeout = 2,
        LowerThanFloorPrice = 100,
        LowerThanHighestPrice = 101
    }
}