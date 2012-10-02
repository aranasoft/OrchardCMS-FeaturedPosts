using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Arana.FeaturedPosts.Models;
using Arana.FeaturedPosts.ViewModels;
using Orchard.Core.Navigation;
using Orchard.Localization;
using Orchard.Security;

namespace Arana.FeaturedPosts.Drivers {
	[UsedImplicitly]
    public class BlogPostMenuItemPartDriver : ContentPartDriver<BlogPostMenuItemPart> {
        private readonly IContentManager _contentManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public BlogPostMenuItemPartDriver(
            IContentManager contentManager,
            IAuthorizationService authorizationService, 
            IWorkContextAccessor workContextAccessor) {
            _contentManager = contentManager;
            _authorizationService = authorizationService;
            _workContextAccessor = workContextAccessor;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Editor(BlogPostMenuItemPart part, dynamic shapeHelper) {
            return ContentShape("Parts_BlogPostMenuItem_Edit",
                                () => {
                                    var model = new BlogPostMenuItemEditViewModel {
                                        ContentItemId = part.Content == null ? -1 : part.Content.Id,
                                        Part = part
                                    };
                                    return shapeHelper.EditorTemplate(TemplateName: "Parts.BlogPostMenuItem.Edit", Model: model, Prefix: Prefix);
                                });
        }

        protected override DriverResult Editor(BlogPostMenuItemPart part, IUpdateModel updater, dynamic shapeHelper) {
            var currentUser = _workContextAccessor.GetContext().CurrentUser;
            if (!_authorizationService.TryCheckAccess(Permissions.ManageMainMenu, currentUser, part))
                return null;

            var model = new BlogPostMenuItemEditViewModel();

            if(updater.TryUpdateModel(model, Prefix, null, null)) {
                var contentItem = _contentManager.Get(model.ContentItemId);
                if(contentItem == null) {
                    updater.AddModelError("ContentItemId", T("You must select a Content Item"));
                }
                else {
                    part.Content = contentItem;
                }
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(BlogPostMenuItemPart part, ImportContentContext context) {
            var contentItemId = context.Attribute(part.PartDefinition.Name, "ContentItem");
            if (contentItemId != null) {
                var contentItem = context.GetItemFromSession(contentItemId);
                part.Content = contentItem;
            }
            else {
                part.Content = null;
            }
        }

        protected override void Exporting(BlogPostMenuItemPart part, ExportContentContext context) {
            if (part.Content != null) {
                var contentItem = _contentManager.Get(part.Content.Id);
                if (contentItem != null) {
                    var containerIdentity = _contentManager.GetItemMetadata(contentItem).Identity;
                    context.Element(part.PartDefinition.Name).SetAttributeValue("ContentItem", containerIdentity.ToString());
                }
            }
        }
    }
}