using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories
{
    public partial class List : ComponentBase
    {
        #region Propriedades
        public bool IsBusy { get; set; }
        public List<Category>? Categories { get; set; } = [];

        public string searchTerm { get; set; } = string.Empty;

        #endregion

        #region Services
        [Inject]
        public ICategoryHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;

        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            try
            {
                var request = new GetAllCategoriesRequest();
                var result = await Handler.GetAllAsync(request);
                if (result.IsSuccess)
                {
                    Categories = result.Data ?? [];
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

        public async void OnDeleteButtonClickAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("Atenção!",
                                                            $"Ao prosseguir, a categoria {title} será excluída.\nEsta é uma ação irreversível, deseja continuar?",
                                                            yesText: "Excluir",
                                                            cancelText: "Cancelar");

            if (result is true)
            {
                await OnDeleteAsync(id, title);
                StateHasChanged();
            }
        }

        public async Task OnDeleteAsync(long id, string title)
        {
            try
            {
                var request = new DeleteCategoryRequest() { Id = id };
                await Handler.DeleteAsync(request);
                Categories?.RemoveAll(x => x.Id == id);
                SnackBar.Add($"Categoria: {title} removida com sucesso!", Severity.Success);
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }

        public Func<Category, bool> Filter => category =>
        {
            if (string.IsNullOrEmpty(searchTerm))
                return true;

            if (category.Id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (category.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (category.Description is not null && category.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        #endregion
    }
}
