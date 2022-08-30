using HiperBlazorServer.Data;
using Microsoft.AspNetCore.Components;

namespace HiperBlazorServer.Pages
{
    public partial class Product
    {
        protected string Message = string.Empty;
        [Inject]
        public IProductService _productService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        public HiperShared.Model.Product Item { get; set; } = new HiperShared.Model.Product();
       
        protected async override Task OnInitializedAsync()
        {

            if (!String.IsNullOrEmpty(Id))
            {
                Item = await _productService.GetProduct(Id);
            }
        }

        protected async Task HandleValidRequest()
        {
            if (String.IsNullOrEmpty(Id)) 
            {
                // mongo não esta aceitando id null.. 
                //Item.Id = String.Empty;
                var res = await _productService.CreateProduct(Item);

                if (res != null)
                {
                    Message = "O produto foi criado!";
                }
                else
                {
                    Message = "Algo deu errado.";
                }
            }
            else
            {
                await _productService.UpdateProduct(Item);
                Message = "O produto foi atualizado";
            }
        }

        protected void HandleInvalidRequest()
        {
            Message = "Failed to submit form";
        }

        protected void goToOverview()
        {
            _navigationManager.NavigateTo("/products");
        }

        protected async Task DeleteItem()
        {
            if (!String.IsNullOrEmpty(Id))
            {
                await _productService.DeleteProduct(Id);

                _navigationManager.NavigateTo("/products");
            }

            Message = "Something went wrong, unable to delete";
        }

    }
}
