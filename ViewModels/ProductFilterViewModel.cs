namespace AgriEnergyConnect.ViewModels
{
    // ViewModel for filtering and displaying products based on a search term
    public class ProductFilterViewModel
    {
        // SearchTerm: The term used to filter the list of products (e.g., name, description, price)
        // It is nullable (indicated by the `?`) because a user might not always provide a search term.
        public string? SearchTerm { get; set; }

        // Products: A list of products that match the filter criteria (or all products if no filter is applied)
        // This is populated with a list of Product objects to be displayed in the view.
        public List<Product> Products { get; set; }
    }
}
