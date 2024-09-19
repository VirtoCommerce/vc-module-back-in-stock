namespace VirtoCommerce.BackInStock.Web.Model
{
    public class ProductCatalogRatingRequest
    {
        public string CatalogId { get; set; }
        public string[] ProductIds { get; set; }
    }
}