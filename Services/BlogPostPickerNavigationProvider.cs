using JetBrains.Annotations;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Arana.FeaturedPosts.Services {
	[UsedImplicitly]
	[OrchardFeature( "Arana.BlogPostPicker" )]
	public class BlogPostPickerNavigationProvider : INavigationProvider {
        public BlogPostPickerNavigationProvider() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public string MenuName {
            get { return "blog-post-picker"; }
        }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Blog Post Picker"),
                menu => menu
                    .Add(T("Recent Blog Post"), "5", item => item.Action("Index", "Admin", new {area = "Arana.FeaturedPosts"}).LocalNav()));
        }
    }
}