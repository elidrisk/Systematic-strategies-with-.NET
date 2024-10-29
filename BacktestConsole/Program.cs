using PricingLibrary.DataClasses;
using BacktestConsole.DataFilesHandlers;
using BacktestConsole.HedgingStrategyBacktest;
namespace BacktestConsole
{
    static class  Program
    {
        /*Méthode Main*/
        static void Main(string[] args)
        {
            string TestParamsFile = args[0];
            string MktDataFile = args[1];
            string OutputFile = args[2];

            JsonHandler JsonHandler = new JsonHandler();
            CsvParser CsvParser = new CsvParser();
            
            var BasketTestParameters = JsonHandler.LoadTestParameters(TestParamsFile);
            var LstDF = CsvParser.ConstructDataFeedFromCsv(MktDataFile);
            RebalancingStrategy RebalancingStrategy = new RebalancingStrategy(BasketTestParameters, LstDF);
            List<OutputData> Results = RebalancingStrategy.RegularRebalancingStrategy();

            /*Sauvegarde des résultats dans un fichier JSON*/
            JsonHandler.SaveResultsToFile(Results, OutputFile);
        }
    }
}
