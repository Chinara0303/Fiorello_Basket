using Practice.Models;

namespace Practice.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Slider { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ExpertExpertPosition> ExpertExpertPositions { get; set; }
        public IEnumerable<Expert> Experts { get; set; }
        public IEnumerable<ExpertPosition> ExpertPositions { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Instagram> Instagrams { get; set; }
        public About About { get; set; }
        public SliderInfo SliderInfo { get; set; }
        public ExpertHeader ExpertHeader { get; set; }
        public Subscribe Subscribe { get; set; }
        public BlogHeader BlogHeader { get; set; }

    }
}
