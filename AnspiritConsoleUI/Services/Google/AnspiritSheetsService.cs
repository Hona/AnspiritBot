using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using System.IO;
using AnspiritConsoleUI.Constants;

namespace AnspiritConsoleUI.Services.Google
{
    public class AnspiritSheetsService
    {
        private string[] _scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private string _applicationName = "Anzac Spirit War Placements";
        private SheetsService _service;
        public AnspiritSheetsService(LogService logger)
        {
            // Create Google Sheets API service.
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = _applicationName,
                ApiKey = File.ReadAllText(DiscordConstants.GoogleAPIKeyFilePath)
            });
        }
        internal IList<IList<object>> GetValues(string range)
        {

            var spreadsheetId = File.ReadAllText(DiscordConstants.AnzacSpiritSheetsID);
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(spreadsheetId, range);
            request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
            ValueRange response = request.Execute();
            return response.Values;
        }
        internal IList<IList<object>> GetWarPlacementValues()
        {
            if (!File.Exists(DiscordConstants.AnzacSpiritSheetsID))
            {
                throw new FileNotFoundException("Could not find the AnspiritSheetsID file", DiscordConstants.AnzacSpiritSheetsID);
            }
            if (!File.Exists(DiscordConstants.AnzacSpiritSheetsRange))
            {
                throw new FileNotFoundException("Could not find the AnspiritSheetsRange file", DiscordConstants.AnzacSpiritSheetsRange);
            }

            var spreadsheetId = File.ReadAllText(DiscordConstants.AnzacSpiritSheetsID);
            var range = File.ReadAllText(DiscordConstants.AnzacSpiritSheetsRange);
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(spreadsheetId, range);
            request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
            ValueRange response = request.Execute();
            return response.Values;
        }
    }
}
