using Grpc.Core;
using GrpcBacktestServer.Protos;
using PricingLibrary.DataClasses;
using BacktestConsoleLibrary.HedgingStrategyBacktest;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;

namespace GrpcBacktestServer.Services
{
    public class BacktestService : BacktestRunner.BacktestRunnerBase
    {
        public override Task<BacktestOutput> RunBacktest(BacktestRequest request, ServerCallContext context)
        {
            var testParams = request.TstParams;
            var dataParams = request.Data;
            BasketTestParameters basketTestParameters = ConvertToBasketTestParameters(testParams);
            List<DataFeed> marketDataFeed = ConvertToMarketDataFeed(dataParams);
            RebalancingStrategy rebalancingStrategy = new RebalancingStrategy(basketTestParameters, marketDataFeed);

            if (testParams.RebParams?.Regular != null)
            {
                // regular rebalancing
                var period = testParams.RebParams.Regular.Period;
                List<OutputData> results = rebalancingStrategy.RegularRebalancingStrategy(period);
                return Task.FromResult(ConvertToBacktestOutput(results));
            }
            var output = new BacktestOutput
            {
                BacktestInfo = { }
            };

            return Task.FromResult(output);
        }
        public BasketTestParameters ConvertToBasketTestParameters(TestParams testParams)
        {
            // Map the fields from TestParams to BasketTestParameters
            return new BasketTestParameters
            {
                PricingParams = new BasketPricingParameters
                {
                    Volatilities = testParams.PriceParams.Vols.ToArray(),
                    Correlations = testParams.PriceParams.Corrs.Select(cl => cl.Value.ToArray()).ToArray()
                },
                BasketOption = new Basket
                {
                    Strike = testParams.BasketParams.Strike,
                    Maturity = testParams.BasketParams.Maturity.ToDateTime(),
                    UnderlyingShareIds = testParams.BasketParams.ShareIds.ToArray(),
                    Weights = testParams.BasketParams.Weights.ToArray()
                },
                RebalancingOracleDescription = ConvertToRebalancingOracleDescription(testParams.RebParams)
            };
        }

        public List<DataFeed> ConvertToMarketDataFeed(DataParams dataParams)
        {
            return dataParams.DataValues
                .GroupBy(dv => dv.Date.ToDateTime())
                .Select(group => new DataFeed(
                    group.Key,  // date du groupe
                    group.ToDictionary(dv => dv.Id, dv => dv.Value)  // Dictionnaire des prix
                )).ToList();
        }

        public IRebalancingOracleDescription ConvertToRebalancingOracleDescription(RebalancingParams rebParams)
        {
            if (rebParams.Regular != null)
            {
                return new RegularOracleDescription
                {
                    Period = rebParams.Regular.Period
                };
            }
            throw new ArgumentException("Invalid rebalancing parameters provided.");
        }

        public BacktestOutput ConvertToBacktestOutput(List<OutputData> results)
        {
            var output = new BacktestOutput();
            foreach (var result in results)
            {
                output.BacktestInfo.Add(new BacktestInfo
                {
                    Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.Date.ToUniversalTime()),
                    PortfolioValue = result.Value,
                    Delta = { result.Deltas.ToArray() },
                    DeltaStddev = { result.DeltasStdDev.ToArray() },
                    Price = result.Price,
                    PriceStddev = result.PriceStdDev
                });
            }
            return output;
        }
    }

}
