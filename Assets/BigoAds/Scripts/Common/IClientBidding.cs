using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Common
{
    public interface IClientBidding
    {
        /// get price
        double getPrice();

        ///notify win
        void notifyWin(double secPrice, string secBidder);

        ///notify loss
        void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason);
    }
}