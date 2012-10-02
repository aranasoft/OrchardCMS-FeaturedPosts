using JetBrains.Annotations;
using Orchard.ContentManagement;
using Arana.FeaturedPosts.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace Arana.FeaturedPosts.Handlers {
    [UsedImplicitly]
	public class BlogPosttMenuItemPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;

		public BlogPosttMenuItemPartHandler( IContentManager contentManager, IRepository<BlogPostMenuItemPartRecord> repository ) {
            _contentManager = contentManager;
            Filters.Add(new ActivatingFilter<BlogPostMenuItemPart>("BlogPostMenuItem"));
            Filters.Add(StorageFilter.For(repository));

            OnLoading<BlogPostMenuItemPart>((context, part) => part._content.Loader(p => contentManager.Get(part.Record.BlogPostMenuItemRecord.Id)));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            base.GetItemMetadata(context);

            if (context.ContentItem.ContentType != "BlogPostMenuItem") {
                return;
            }

            var BlogPostMenuItemPart = context.ContentItem.As<BlogPostMenuItemPart>();
            // the display route for the menu item is the one for the referenced content item
            if(BlogPostMenuItemPart != null) {
                context.Metadata.DisplayRouteValues = _contentManager.GetItemMetadata(BlogPostMenuItemPart.Content).DisplayRouteValues;
            }
        }
    }
}