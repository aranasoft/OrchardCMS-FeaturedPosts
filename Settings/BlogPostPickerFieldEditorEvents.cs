using System.Collections.Generic;
using System.Globalization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Arana.FeaturedPosts.Settings {
    public class BlogPostPickerFieldEditorEvents : ContentDefinitionEditorEventsBase {

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "BlogPostPickerField") {
                var model = definition.Settings.GetModel<BlogPostPickerFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.FieldType != "BlogPostPickerField") {
                yield break;
            }

            var model = new BlogPostPickerFieldSettings();
            if (updateModel.TryUpdateModel(model, "BlogPostPickerFieldSettings", null, null)) {
                builder.WithSetting("BlogPostPickerFieldSettings.Hint", model.Hint);
                builder.WithSetting("BlogPostPickerFieldSettings.Required", model.Required.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("BlogPostPickerFieldSettings.Multiple", model.Multiple.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(model);
        }
    }
}