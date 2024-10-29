using PricingLibrary.DataClasses;
using PricingLibrary.RebalancingOracleDescriptions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BacktestConsole.DataFilesHandlers
{
    public class JsonHandler
    {
        /*Méthode pour sauvegarder les résultats dans un fichier JSON*/
        public void SaveResultsToFile(List<OutputData> Results, string FilePath)
        {
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };
            var JsonString = JsonSerializer.Serialize(Results, Options);
            File.WriteAllText(FilePath, JsonString);
        }

        /*Méthode pour charger les paramètres de test à partir d'un fichier JSON*/
        public BasketTestParameters LoadTestParameters(string JsonFilePath)
        {
            var JsonFileContent = File.ReadAllText(JsonFilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };
            return JsonSerializer.Deserialize<BasketTestParameters>(JsonFileContent, Options);
        }

    }
}
