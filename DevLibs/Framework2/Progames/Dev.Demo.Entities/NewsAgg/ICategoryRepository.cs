namespace Dev.Demo.Entities.NewsAgg
{
    using System.Collections.Generic;

    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();

        Category GetCategoryById(int id);
    }
}