using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;

namespace BacktestConsole.Utils
{
    public class DeltaDictMaker
    {
        public BasketTestParameters Testparameters { get; set; }
        public DeltaDictMaker(BasketTestParameters testParameters)
        {
            Testparameters = testParameters;
        }

        public  Dictionary<string, double> MakeComposition(double[] Deltas)
        {
            string[] SharesId = Testparameters.BasketOption.UnderlyingShareIds;
            Dictionary<string, double> Composition = new Dictionary<string, double>();
            for (int i = 0; i < SharesId.Length; i++)
            {
                Composition[SharesId[i]] = Deltas[i];
            }
            return Composition;
        } 

        public double[] MakeSpots(DataFeed dataFeed)
        {
            string[] SharesId = Testparameters.BasketOption.UnderlyingShareIds;
            Dictionary<string, double> spotsDict = dataFeed.PriceList;
            double[] spots = new double[SharesId.Length];
            int i = 0;
            foreach (string ShareId in SharesId)
            {
                spots[i] = spotsDict[ShareId];
                i += 1;
            }
            return spots;

        }
    }
}
