using System.Web;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;

// ReSharper disable InconsistentNaming

namespace Arana.FeaturedPosts.Services {
    [UsedImplicitly]
    [OrchardFeature("Arana.BlogPostPicker")]
    public class BlogPostPickerShapes : IShapeTableProvider {
        private readonly Work<INavigationManager> _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;

        public BlogPostPickerShapes(
            Work<INavigationManager> navigationManager,
            IWorkContextAccessor workContextAccessor,
            Work<IShapeFactory> shapeFactory) {
            _navigationManager = navigationManager;
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("BlogPostPicker")
                .OnDisplaying(displaying => {
                    ContentItem contentItem = displaying.Shape.ContentItem;
                    if (contentItem != null) {
                        displaying.ShapeMetadata.Alternates.Add("BlogPostPicker_" + displaying.ShapeMetadata.DisplayType);
                    }
                });
        }

        [Shape]
        public IHtmlString BlogPostPickerNavigation(dynamic Display) {
            var menuItems = _navigationManager.Value.BuildMenu("blog-post-picker");

            // Set the currently selected path
            var request = _workContextAccessor.GetContext().HttpContext.Request;
            var routeData = request.RequestContext.RouteData;
            var selectedPath = NavigationHelper.SetSelectedPath(menuItems, request, routeData);

            dynamic shapeFactory = _shapeFactory.Value;

            // Populate local nav
            var localMenuShape = shapeFactory.LocalMenu().MenuName("blog-post-picker");
            NavigationHelper.PopulateLocalMenu(shapeFactory, localMenuShape, localMenuShape, selectedPath);
            return Display(localMenuShape);
        }
    }
}