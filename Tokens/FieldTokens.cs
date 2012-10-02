using System;
using Orchard.ContentManagement;
using Orchard.Events;
using Arana.FeaturedPosts.Fields;
using Orchard.Localization;

namespace Arana.FeaturedPosts.Tokens {
    public interface ITokenProvider : IEventHandler {
        void Describe(dynamic context);
        void Evaluate(dynamic context);
    }

    public class BlogPostPickerFieldTokens : ITokenProvider {
        private readonly IContentManager _contentManager;

        public BlogPostPickerFieldTokens(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public Localizer T { get; set; }

        public void Describe(dynamic context) {
			context.For( "BlogPostPickerField", T( "Blog Post Picker Field" ), T( "Tokens for Blog Post Picker Fields" ) )
                .Token("Content", T("Content Item"), T("The content item."))
                ;
        }

        public void Evaluate(dynamic context) {
            context.For<BlogPostPickerField>("BlogPostPickerField")
                .Token("Content", (Func<BlogPostPickerField, object>)(field => field.Ids[0]))
                .Chain("Content", "Content", (Func<BlogPostPickerField, object>)(field => _contentManager.Get(field.Ids[0])))
                ;
        }
    }
}