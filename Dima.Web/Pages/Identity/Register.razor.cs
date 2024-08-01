using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity
{
    public partial class Register : ComponentBase
    {
        #region Dependencias
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IAccountHandler handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        #endregion

        #region Propriedades
        public bool IsBusy { get; set; } = false;
        public RegisterRequest InputModel { get; set; } = new();
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
                NavigationManager.NavigateTo("/");
        }
        #endregion

        #region Metodos
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await handler.RegisterAsync(InputModel);

                if (result.IsSuccess)
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    NavigationManager.NavigateTo("/login");
                }
                else
                    Snackbar.Add(result.Message, Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

    }
}
