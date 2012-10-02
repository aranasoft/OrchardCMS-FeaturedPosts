using Orchard.ContentManagement.Records;

namespace Arana.FeaturedPosts.Models {
    public class BlogPostMenuItemPartRecord : ContentPartRecord {
        public virtual ContentItemRecord BlogPostMenuItemRecord { get; set; }
    }
}