using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace Arana.FeaturedPosts {
    public class Migrations : DataMigrationImpl {
        public int Create() {
			SchemaBuilder.CreateTable( "BlogPostMenuItemPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("BlogPostMenuItemRecord_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("BlogPostMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("BlogPostMenuItemPart")
                .DisplayedAs("Blog Post Link")
                .WithSetting("Description", "Adds a Blog Post to the menu.")
                .WithSetting("Stereotype", "MenuItem")
                );

            return 1;
        }

		public void Uninstall( ) {
			ContentDefinitionManager.DeleteTypeDefinition( "BlogPostMenuItem" );
			ContentDefinitionManager.DeletePartDefinition( "BlogPostMenuItemPart" );
			SchemaBuilder.DropTable( "BlogPostMenuItemPartRecord" );
        }
    }
}