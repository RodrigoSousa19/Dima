using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions
{
    public partial class Create : ComponentBase
    {
        #region Propriedades
        public bool IsBusy { get; set; }
        public CreateTransactionRequest InputModel { get; set; } = new();
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
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Metodos
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await TransactionHandler.CreateAsync(InputModel);
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
    }
}
