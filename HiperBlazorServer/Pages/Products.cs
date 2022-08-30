using HiperBlazorServer.Data;
using HiperShared.Model;
using Microsoft.AspNetCore.Components;

namespace HiperBlazorServer.Pages
{
    public partial class Products
    {
        public IEnumerable<HiperShared.Model.Product> products { get; set; }
        public HiperShared.Model.Product product { get; set; }
        [Inject]
        public IProductService _productService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            products = (await _productService.GetProducts()).ToList();
        }
        protected async Task DeleteItem(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                await _productService.DeleteProduct(id);
                await OnInitializedAsync();
            }

        }
    }
}
