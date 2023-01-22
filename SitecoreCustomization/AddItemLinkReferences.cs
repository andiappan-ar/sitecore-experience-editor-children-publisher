// Sitecore.Foundation.ScExtensions.SitecoreCustomization
// Sitecore.Publishing.Pipelines.GetItemReferences.AddItemLinkReferences

/*-----------------------------------------------------------------------------------------------------------------
 * -----------------------------------------------------------------------------------------------------------------
 * -----------------------------------------------------------------------------------------------------------------
 * -----------------------THIS DLL IS OVERWRITTEN FROM SITECORE KERNEL----------------------------------------------
 * -PLEASE CONSIDER THIS DLL DURING SITECORE UPGRADE. WE HAVE TO TAKE SURCE FROM RESPECTING SITECORE KERNEL DLL-----
 * -----------------------------------------------------------------------------------------------------------------
 * -----------------------------------------------------------------------------------------------------------------
 * -----------------------------------------------------------------------------------------------------------------
 * -----------------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.GetItemReferences;
using Sitecore.Publishing.Pipelines.Publish;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Sitecore.Foundation.ScExtensions.SitecoreCustomization
{
    public class AddItemLinkReferences : GetItemReferencesProcessor
    {
        public bool DeepScan { get; set; }
       

        public AddItemLinkReferences()
        {
            DeepScan = Settings.Publishing.PublishingDeepScanRelatedItems;
        }

        protected override List<Item> GetItemReferences(PublishItemContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            List<Item> list = new List<Item>();
            if (context.PublishOptions.Mode != PublishMode.SingleItem)
            {
                return list;
            }
            switch (context.Action)
            {
                case PublishAction.PublishVersion:
                    {
                        Item versionToPublish = context.VersionToPublish;
                        if (versionToPublish == null)
                        {
                            return list;
                        }
                        list.AddRange(GetReferences(versionToPublish, sharedOnly: false, new HashSet<ID>()));
                        break;
                    }
                case PublishAction.PublishSharedFields:
                    {
                        Item sourceItem = context.PublishHelper.GetSourceItem(context.ItemId);
                        if (sourceItem == null)
                        {
                            return list;
                        }
                        list.AddRange(GetReferences(sourceItem, sharedOnly: true, new HashSet<ID>()));
                        break;
                    }
                default:
                    return list;
            }
            return list;
        }

        private IEnumerable<Item> GetReferences(Item item, bool sharedOnly, HashSet<ID> processedItems)
        {
            Assert.ArgumentNotNull(item, "item");
            processedItems.Add(item.ID);
            List<Item> list = new List<Item>();
            ItemLink[] validLinks = item.Links.GetValidLinks();
            validLinks = validLinks.Where((ItemLink link) => item.Database.Name.Equals(link.TargetDatabaseName, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (sharedOnly)
            {
                validLinks = validLinks.Where(delegate (ItemLink link)
                {
                    Item sourceItem = link.GetSourceItem();
                    return sourceItem != null && (ID.IsNullOrEmpty(link.SourceFieldID) || sourceItem.Fields[link.SourceFieldID].Shared);
                }).ToArray();
            }
            List<Item> list2 = (from link in validLinks
                                select link.GetTargetItem() into relatedItem
                                where relatedItem != null
                                select relatedItem).ToList();
            foreach (Item item2 in list2)
            {
                if (DeepScan && !processedItems.Contains(item2.ID))
                {
                    list.AddRange(GetReferences(item2, sharedOnly, processedItems));
                }
                list.AddRange(PublishQueue.GetParents(item2));
                list.Add(item2);

                // Custom code --------------------->
                try
                {                   
                    if (AllowedDataSources.AllowedDataSourcesList != null)
                    {                       
                        if (AllowedDataSources.AllowedDataSourcesList.Any(x => x.Equals(item2.ID)))
                        {                            
                            list.AddRange(item2.Axes.GetDescendants());
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Error occured in Sitecore.Foundation.ScExtensions.SitecoreCustomization.AddItemLinkReferences: ", ex, typeof(AddItemLinkReferences));
                }
                // Custom code --------------------->
            }



            return list.Distinct(new ItemIdComparer());
        }
    }

}
