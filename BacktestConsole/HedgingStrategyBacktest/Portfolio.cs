using PricingLibrary.MarketDataFeed;

namespace BacktestConsole.HedgingStrategyBacktest
{
    internal class Portfolio
    {
        public Dictionary<string, double> Composition { get; private set; }
        public double Cash { get; private set; }
        public DateTime PreviousRebalancingDate { get; private set; }

        public Portfolio(Dictionary<string, double> composition, DataFeed dataFeed, double initialValue)
        {
            Composition = composition;
            Cash = initialValue - DictionaryUtils.ScalarProduct(composition, dataFeed.PriceList);
            PreviousRebalancingDate = dataFeed.Date;
        }

        public double ComputeValue(DataFeed dataFeed)
        {
            Dictionary<string, double> spots = dataFeed.PriceList;
            double capitalisation = RiskFreeRateProvider.GetRiskFreeRateAccruedValue( PreviousRebalancingDate, dataFeed.Date);
            double Value = Cash * capitalisation;
            Value += DictionaryUtils.ScalarProduct(Composition, spots);
            return Value;
        }

        public void UpdatePortfolio(Dictionary<string, double> NewComposition, DataFeed dataFeed)
        {
            Cash = ComputeValue(dataFeed) - DictionaryUtils.ScalarProduct(NewComposition, dataFeed.PriceList);
            Composition = NewComposition;
            PreviousRebalancingDate = dataFeed.Date;
        } 
    }
}
