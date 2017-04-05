using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Publishing.Pipelines.PublishItem
{
    public class ResetStandardValue
    {
        public virtual void Process(PublishItemContext context)
        {
            Item sourceItem = context.PublishHelper.GetSourceItem(context.ItemId);
            Item targetItem = context.PublishHelper.GetTargetItem(context.ItemId);
            if (sourceItem.SourceUri != null && targetItem != null)
            {
                Item item = Database.GetItem(sourceItem.SourceUri);
                if (item != null)
                {
                    FieldCollection fields = item.Fields;
                    fields.ReadAll();
                    foreach (Sitecore.Data.Fields.Field field in fields)
                    {
                        if (field.Key == "__renderings" && field.ContainsStandardValue)
                        {
                            if (sourceItem.Fields["__renderings"].Value == item.Fields["__renderings"].GetStandardValue())
                            {
                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                {
                                    using (new Sitecore.Data.Items.EditContext(targetItem, false, true))
                                    {
                                        targetItem.Fields["__renderings"].Reset();
                                        targetItem.Editing.EndEdit();
                                    }
                                }
                            }
                        }
                        if (field.Key == "__final renderings" && field.ContainsStandardValue)
                        {
                            if (sourceItem.Fields["__final renderings"].Value == item.Fields["__final renderings"].GetStandardValue())
                            {
                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                {
                                    using (new Sitecore.Data.Items.EditContext(targetItem, false, true))
                                    {
                                        targetItem.Fields["__final renderings"].Reset();
                                        targetItem.Editing.EndEdit();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}