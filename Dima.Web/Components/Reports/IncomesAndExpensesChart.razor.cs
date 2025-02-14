﻿using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports
{
    public partial class IncomesAndExpensesChart : ComponentBase
    {
        #region Propriedades
        public ChartOptions Options { get; set; } = new();
        public List<ChartSeries>? Series { get; set; }
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
            var request = new GetIncomesAndExpensesRequest();
            var result = await Handler.GetIncomesAndExpensesReportAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                SnackBar.Add("Falha ao obter os dados do relatório", Severity.Error);
                return;
            }

            var incomes = new List<double>();
            var expenses = new List<double>();

            foreach (var item in result.Data)
            {
                incomes.Add((double)item.Incomes);
                expenses.Add(-(double)item.Expenses);
                Labels.Add(GetMonthName(item.Month));
            }

            Options.YAxisTicks = 400;
            Options.LineStrokeWidth = 5;
            Options.ChartPalette = ["#f5f4f3", Colors.Red.Default];
            Series =
                [
                    new ChartSeries {Name = "Receitas", Data = incomes.ToArray()},
                    new ChartSeries {Name = "Saidas", Data = expenses.ToArray()}
                ];

            StateHasChanged();
        }
        #endregion

        private static string GetMonthName(int month)
        {
            return new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
        }
    }
}
