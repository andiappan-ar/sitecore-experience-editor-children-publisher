using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;

namespace Sitecore.Foundation.ScExtensions.SitecoreCustomization
{

    public class AllowedDataSources
    {
        public static string GlobalSettingAllowedRendering = "{C76F84F8-8644-4ED2-A0CF-5ECD290B87A4}";
        public static string FieldNameAllowedRendering = "AllowedRenderingForPublishChildrens";
        public static List<ID> AllowedDataSourcesList { get; set; }

        static AllowedDataSources()
        {
            InitAllowedDataSourceList();
        }

        public static void InitAllowedDataSourceList()
        {
            List<ID> result = new List<ID>();
            
            try
            {
                // Check global settings
                Item globalSettingAllowedRendering = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(GlobalSettingAllowedRendering);
                if (globalSettingAllowedRendering != null)
                {
                    // Collect all allowed renderings
                    Sitecore.Data.Fields.MultilistField allowedRederingField = globalSettingAllowedRendering.Fields[FieldNameAllowedRendering];

                    Sitecore.Data.Items.Item[] allowedRederingItems = allowedRederingField.GetItems();
                   
                    //Iterate through each item
                    if (allowedRederingItems != null && allowedRederingItems.Length > 0)
                    {
                        foreach (Item list in allowedRederingItems)
                        {
                            result.Add(list.ID);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error occured in Sitecore.Foundation.ScExtensions.SitecoreCustomization.AddItemLinkReferences: ", ex, typeof(AddItemLinkReferences));
            }

            AllowedDataSourcesList = result;
        }

    }
}