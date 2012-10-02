using System.Collections.Generic;
using Orchard.ContentManagement;
using Arana.FeaturedPosts.Fields;

namespace Arana.FeaturedPosts.ViewModels {

    public class BlogPostPickerFieldViewModel {

        public ICollection<ContentItem> ContentItems { get; set; }
        public string SelectedIds { get; set; }
        public BlogPostPickerField Field { get; set; }
    }
}