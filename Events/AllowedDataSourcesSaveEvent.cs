using Sitecore.Foundation.ScExtensions.SitecoreCustomization;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.ScExtensions.Events
{
    public class AllowedDataSourcesSaveEvent
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;

            if (savedItem != null)
            {
                if (savedItem.ID.ToString() == AllowedDataSources.GlobalSettingAllowedRendering)
                {
                    AllowedDataSources.InitAllowedDataSourceList();
                }
            }

            return;

        }
    }
}