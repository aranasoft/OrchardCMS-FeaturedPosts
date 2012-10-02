using System.Collections.Generic;
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
	[OrchardFeature( "Arana.BlogPostPicker" )]
	public class BlogPostPickerShapes : IShapeTableProvider {
        private readonly Work<INavigationManager> _navigationManager;
        private readonly Work<WorkContext> _workContext;
        private readonly Work<IShapeFactory> _shapeFactory;

        public BlogPostPickerShapes(
            Work<INavigationManager> navigationManager,
            Work<WorkContext> workContext,
            Work<IShapeFactory> shapeFactory) {
            _navigationManager = navigationManager;
            _workContext = workContext;
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

            IEnumerable<MenuItem> menuItems = _navigationManager.Value.BuildMenu("blog-post-picker");

            // Set the currently selected path
            Stack<MenuItem> selectedPath = NavigationHelper.SetSelectedPath(menuItems, _workContext.Value.HttpContext.Request.RequestContext.RouteData);

            dynamic shapeFactory = _shapeFactory.Value;

            // Populate local nav
            dynamic localMenuShape = shapeFactory.LocalMenu().MenuName("blog-post-picker");
            NavigationHelper.PopulateLocalMenu(shapeFactory, localMenuShape, localMenuShape, selectedPath);
            return Display(localMenuShape);
        }
    }
}