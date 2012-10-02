using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Arana.FeaturedPosts.Fields;

namespace Arana.FeaturedPosts.Handlers {
    public class BlogPostPickerFieldHandler : ContentHandler {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public BlogPostPickerFieldHandler(
            IContentManager contentManager, 
            IContentDefinitionManager contentDefinitionManager) {
            
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
        }

        protected override void Loading(LoadContentContext context) {
            base.Loading(context);

            var fields = context.ContentItem.Parts.SelectMany(x => x.Fields.Where(f => f.FieldDefinition.Name == typeof (BlogPostPickerField).Name)).Cast<BlogPostPickerField>();
            
            // define lazy initializer for BlogPostPickerField.ContentItems
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(context.ContentType);
            if (contentTypeDefinition == null) {
                return;
            }

            foreach (var field in fields) {
                var localField = field;
                field._contentItems.Loader(x => _contentManager.GetMany<ContentItem>(localField.Ids, VersionOptions.Published, QueryHints.Empty));
            }
        }
    }
}