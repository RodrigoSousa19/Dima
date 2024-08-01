using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories
{
    public class CreateCategoryRequest : BaseRequest
    {
        [Required(ErrorMessage = "Titulo inválido")]
        [MaxLength(80, ErrorMessage = "O titulo ñão pode passar de 80 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição inválida")]
        public string Description { get; set; } = string.Empty;
    }
}
