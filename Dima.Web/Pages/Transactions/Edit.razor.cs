using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions
{
    public partial class Edit : ComponentBase
    {
        #region Propriedades
        [Parameter]
        public string Id { get; set; } = string.Empty;
        public bool IsBusy { get; set; }
        public UpdateTransactionRequest InputModel { get; set; } = new();
        public List<Category> Categories { get; set; } = [];

        #endregion

        #region Services
        [Inject]
        public ITransactionHandler TransactionHandler { get; set; } = null!;
        [Inject]
        public ICategoryHandler CategoryHandler { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;
        #endregion

        #region overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            await GetCategoriesAsync();

            await GetTransactionByIdAsync();

            IsBusy = false;
        }
        #endregion

        #region Metodos Publicos
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await TransactionHandler.UpdateAsync(InputModel);
                if (result.IsSuccess)
                {
                    SnackBar.Add(result.Message, Severity.Success);
                    NavigationManager.NavigateTo("/lancamentos/historico");
                }
                else
                    SnackBar.Add(result.Message, Severity.Error);
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

        #region Métodos Privados
        private async Task GetTransactionByIdAsync()
        {
            IsBusy = true;
            GetTransactionByIdRequest? request = null;

            try
            {
                request = new GetTransactionByIdRequest() { Id = long.Parse(Id) };
            }
            catch
            {
                SnackBar.Add("Parâmetro inválido", Severity.Error);
            }

            if (request is null)
                return;

            try
            {
                var result = await TransactionHandler.GetByIdAsync(request);

                if (result is { IsSuccess: true, Data: not null })
                {
                    InputModel = new UpdateTransactionRequest
                    {
                        CategoryId = result.Data.CategoryId,
                        PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                        Title = result.Data.Title,
                        Type = result.Data.Type,
                        Amount = result.Data.Amount,
                        Id = result.Data.Id
                    };
                    InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
                }
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }

        private async Task GetCategoriesAsync()
        {
            IsBusy = true;

            try
            {
                var request = new GetAllCategoriesRequest();
                var result = await CategoryHandler.GetAllAsync(request);

                if (result.IsSuccess)
                {
                    Categories = result.Data ?? [];
                    InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
                }
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }
        #endregion
    }
}
