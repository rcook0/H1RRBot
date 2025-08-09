using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class H1RRTraderBot : Robot
    {
        [Parameter("Symbol", DefaultValue = "XAUUSD")]
        public string SymbolName { get; set; }

        [Parameter("Stop Loss (pips)", DefaultValue = 200)]
        public int StopLoss { get; set; }

        [Parameter("Risk/Reward Ratio", DefaultValue = 2.0)]
        public double RRR { get; set; }

        [Parameter("Auto Size Trade", DefaultValue = true)]
        public bool AutoSize { get; set; }

        [Parameter("Lot Size (if not auto)", DefaultValue = 1.0)]
        public double ManualLots { get; set; }

        [Parameter("London Session Only", DefaultValue = true)]
        public bool LondonOnly { get; set; }

        [Parameter("NY Session Only", DefaultValue = true)]
        public bool NYOnly { get; set; }

        [Parameter("Use H4 Trend Filter", DefaultValue = true)]
        public bool UseH4Trend { get; set; }

        private ExponentialMovingAverage _emaH1;
        private ExponentialMovingAverage _emaH4;

        protected override void OnStart()
        {
            _emaH1 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 50);
            _emaH4 = Indicators.ExponentialMovingAverage(MarketData.GetBars(TimeFrame.Hour4).ClosePrices, 50);
        }

        protected override void OnBar()
        {
            if (SymbolName != Symbol.Name)
                return;

            var time = Server.Time.ToUniversalTime();
            bool inLondon = !LondonOnly || (time.Hour >= 8 && time.Hour < 12);
            bool inNY = !NYOnly || (time.Hour >= 13 && time.Hour < 17);
            if (!(inLondon || inNY))
                return;

            bool h1Up = Bars.ClosePrices.Last(1) > _emaH1.Result.Last(1);
            bool h4Up = MarketData.GetBars(TimeFrame.Hour4).ClosePrices.Last(1) > _emaH4.Result.Last(1);
            bool trendUp = h1Up && (!UseH4Trend || h4Up);

            bool bullishPattern = Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1) && Bars.ClosePrices.Last(2) < Bars.OpenPrices.Last(2);
            bool bearishPattern = Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1) && Bars.ClosePrices.Last(2) > Bars.OpenPrices.Last(2);

            double volume = AutoSize ? Symbol.QuantityToVolume(Symbol.Bid * 0.01) : Symbol.QuantityToVolume(ManualLots);
            double slPips = StopLoss;
            double tpPips = StopLoss * RRR;

            if (trendUp && bullishPattern)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, volume, "Long", slPips, tpPips);
            }
            else if (!trendUp && bearishPattern)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, volume, "Short", slPips, tpPips);
            }
        }
    }
}

