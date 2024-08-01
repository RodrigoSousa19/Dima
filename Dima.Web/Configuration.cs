using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.Web
{
    public static class Configuration
    {
        public const string HttpClientName = "dima";

        public static string BackendUrl { get; set; } = string.Empty;

        public static MudTheme Theme = new()
        {
            Typography = new Typography
            {
                Default = new Default
                {
                    FontFamily = ["Raleway", "sans-serif"]
                }
            },
            Palette = new PaletteLight
            {
                Primary = new MudColor("#f5f4f3"),
                PrimaryContrastText = new MudColor("#000000"),
                Secondary = Colors.LightGreen.Darken3,
                Background = "#ffffff",
                AppbarBackground = "#f5f4f3",
                AppbarText = "#3d3d3d",
                TextPrimary = "#727272",
                DrawerText = Colors.Shades.Black,
                DrawerBackground = "#f5f4f3"
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.LightGreen.Accent3,
                Secondary = Colors.LightGreen.Darken3,
                Background = "#1e1f21",
                TextPrimary = "#f5f4f3",
                AppbarBackground = "#1e1f21",
                AppbarText = "#f5f4f3",
                PrimaryContrastText = new MudColor("#000000"),
                DrawerText = "#f5f4f3",
            }
        };
    }
}
