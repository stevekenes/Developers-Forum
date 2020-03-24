using System.Collections.Generic;

namespace DevelopersForum.ViewModels.Home
{
    public class HomeIndexModel
    {
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
        public string SearchQuery { get; set; }
    }
}
