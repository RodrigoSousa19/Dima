using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports
{
    public partial class IncomesByCategoryChart : ComponentBase
    {
        #region Propriedades
        public List<double> Data { get; set; } = [];
        public List<string> Labels { get; set; } = [];
        #endregion

        #region Serviços
        [Inject]
        public IReportHandler Handler { get; set; } = null!;
        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;
        #endregion

        #region Overrides
        protected override Task OnInitializedAsync() => GetExpensesByCategoryAsync();
        #endregion

        #region Métodos Privados
        private async Task GetExpensesByCategoryAsync()
        {
            var request = new GetIncomesByCategoryRequest();
            var result = await Handler.GetIncomesByCategoryReportAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                SnackBar.Add("Falha ao obter os dados do relatório", Severity.Error);
                return;
            }

            foreach (var item in result.Data)
            {
                Labels.Add($"{item.Category} ({item.Incomes:C})");
                Data.Add((double)item.Incomes);
            }
        }
        #endregion
    }
}
