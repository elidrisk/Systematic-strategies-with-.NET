using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using BacktestConsole.Utils;
using PricingLibrary.RebalancingOracleDescriptions;
namespace BacktestConsole.HedgingStrategyBacktest

{
    /*Méthode pour effectuer le backtest de la stratégie de Delta-Hedging*/
    public class RebalancingStrategy
    {
        public Pricer Pricer { get; private set; }
        public BasketTestParameters Testparameters { get; set; }
        public DeltaDictMaker DeltaDictMaker { get; private set; }
        public List<DataFeed> LstDF { get; set; }

        public RebalancingStrategy(BasketTestParameters testparameters, List<DataFeed> lstDF)
        {
            Testparameters = testparameters;
            Pricer = new Pricer(Testparameters);
            DeltaDictMaker = new DeltaDictMaker(Testparameters);
            LstDF = lstDF;
        }
        public OutputData CreateOutputData(DateTime date, PricingResults result, double portfolioValue)
        {
            return new OutputData
            {
                Date = date,
                Value = portfolioValue,
                Price = result.Price,
                Deltas = result.Deltas,
                DeltasStdDev = result.DeltaStdDev,
                PriceStdDev = result.PriceStdDev
            };

        }
        public List<OutputData> RegularRebalancingStrategy()
        {
            DataFeed dataFeed = LstDF[0];
            DateTime date = dataFeed.Date;
            List<OutputData> results = new List<OutputData>();
            double[] spots = DeltaDictMaker.MakeSpots(dataFeed);
            PricingResults result = Pricer.Price(date, spots);
            Dictionary<string, double> composition = DeltaDictMaker.MakeComposition(result.Deltas);
            Portfolio portfolio = new Portfolio(composition, dataFeed, result.Price);

            OutputData output = CreateOutputData(date, result, result.Price);
            results.Add(output);

            RegularOracleDescription RegularOracle = (RegularOracleDescription)Testparameters.RebalancingOracleDescription;
            int period = RegularOracle.Period;

            for (int i = period; i < LstDF.Count; i += period)
            {
                dataFeed = LstDF[i];
                date = dataFeed.Date;
                spots = DeltaDictMaker.MakeSpots(dataFeed);
                result = Pricer.Price(date, spots);
                Dictionary<string, double> newComposition = DeltaDictMaker.MakeComposition (result.Deltas);
                portfolio.UpdatePortfolio(newComposition, dataFeed);
                double value = portfolio.ComputeValue(dataFeed);

                output = CreateOutputData(date, result, value);
                results.Add(output);
            }
            return results;
        }
    }
}
