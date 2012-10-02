using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;

namespace Arana.FeaturedPosts.Controllers {
	[OrchardFeature( "Arana.BlogPostPicker" )]
	public class AdminController : Controller {
        private readonly ISiteService _siteService;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public AdminController(
            IOrchardServices orchardServices,
            ISiteService siteService,
            IContentDefinitionManager contentDefinitionManager) {
            _siteService = siteService;
            _contentDefinitionManager = contentDefinitionManager;
            Services = orchardServices;
        }

        public IOrchardServices Services { get; set; }

        [Themed(false)]
        public ActionResult Index(ListContentsViewModel model, PagerParameters pagerParameters) {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

			var query = Services.ContentManager.Query( VersionOptions.Latest, GetBlogPostTypes().Select( ctd => ctd.Name ).ToArray( ) );

            switch (model.Options.OrderBy) {
                case ContentsOrder.Modified:
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.ModifiedUtc);
                    break;
                case ContentsOrder.Published:
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.PublishedUtc);
                    break;
                case ContentsOrder.Created:
                    query = query.OrderByDescending<CommonPartRecord, DateTime?>(cr => cr.CreatedUtc);
                    break;
            }

            var pagerShape = Services.New.Pager(pager).TotalItemCount(query.Count());
            var pageOfContentItems = query.Slice(pager.GetStartIndex(), pager.PageSize).ToList();

            var list = Services.New.List();
            list.AddRange(pageOfContentItems.Select(ci => Services.ContentManager.BuildDisplay(ci, "SummaryAdmin")));

            foreach(IShape item in list.Items) {
                item.Metadata.Type = "BlogPostPicker";
            }

            dynamic tab = Services.New.RecentBlogPostTab()
                .ContentItems(list)
                .Pager(pagerShape)
                .Options(model.Options)
                .TypeDisplayName(model.TypeDisplayName ?? "");

            return new ShapeResult(this, Services.New.BlogPostPicker().Tab(tab));
        }

        private IEnumerable<ContentTypeDefinition> GetBlogPostTypes() {
        	return new List<ContentTypeDefinition> {_contentDefinitionManager.GetTypeDefinition("BlogPost")};
        }
    }
}