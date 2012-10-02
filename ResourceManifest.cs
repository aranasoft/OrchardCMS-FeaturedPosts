using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace Arana.FeaturedPosts {
	[OrchardFeature("Arana.BlogPostPicker")]
	public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("BlogPostPicker").SetUrl("BlogPostPicker.js", "BlogPostPicker.js").SetDependencies("jQuery");
        }
    }
}
