using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;

namespace Arana.FeaturedPosts.Models {
	public class BlogPostMenuItemPart : ContentPart<BlogPostMenuItemPartRecord> {

        public readonly LazyField<ContentItem> _content = new LazyField<ContentItem>();

        public ContentItem Content {
            get { return _content.Value;  }
            set {
                _content.Value = value; 
                Record.BlogPostMenuItemRecord = value == null ? null : value.Record; 
            }
        }
    }
}
