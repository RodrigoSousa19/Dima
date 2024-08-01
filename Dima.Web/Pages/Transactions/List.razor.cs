using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions
{
    public partial class List : ComponentBase
    {
        #region Propriedades
        public bool IsBusy { get; set; } = false;
        public List<Transaction> Transactions { get; set; } = [];
        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public int CurrentMonth { get; set; } = DateTime.Now.Month;
        public int[] Years { get; set; } =
        {
            DateTime.Now.Year,
            DateTime.Now.AddYears(-1).Year,
            DateTime.Now.AddYears(-2).Year,
            DateTime.Now.AddYears(-3).Year,
        };
        #endregion

        #region Serviços
        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;
        [Inject]
        public IDialogService DialogService { get; set; } = null!;
        [Inject]
        public ITransactionHandler Handler { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync() => await GetTransactions();
        #endregion

        #region Métodos Privados
        private async Task GetTransactions()
        {
            IsBusy = true;
            try
            {
                var request = new GetTransactionsByPeriodRequest
                {
                    EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                    StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                    PageNumber = 1,
                    PageSize = 50
                };

                var result = await Handler.GetByPeriodAsync(request);

                if (result.IsSuccess)
                {
                    Transactions = result.Data ?? [];
                }
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Métodos Publicos
        public async Task OnSearchAsync()
        {
            await GetTransactions();
            StateHasChanged();
        }

        public async void OnDeleteButtonClickAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("Atenção!",
                                                            $"Ao prosseguir, a transação: {title} será excluída.\n\tEsta é uma ação irreversível, deseja continuar?",
                                                            yesText: "Excluir",
                                                            cancelText: "Cancelar");

            if (result is true)
            {
                await OnDeleteAsync(id, title);
                StateHasChanged();
            }
        }

        private async Task OnDeleteAsync(long id, string title)
        {
            try
            {
                var request = new DeleteTransactionRequest() { Id = id };
                await Handler.DeleteAsync(request);
                Transactions?.RemoveAll(x => x.Id == id);
                SnackBar.Add($"Transação: {title} removida com sucesso!", Severity.Success);
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }

        public Func<Transaction, bool> Filter => transaction =>
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return true;

            return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                   transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        };
        #endregion
    }
}
