# Sitecore experience editor child publisher
SITECORE experience editor publishing ITEMS/DATA sources not publishing the childrens

## Problem 
When publishing page items from the Experience Editor and desiring the publication of all data sources along with their children, it's important to note that, in this scenario, child items of data sources will not be published. This holds true regardless of the options selected, including the 'related items' publishing.

In the following example, we have a homepage with a component called 'Main Menu.' The data source root for this component is set as a data source. When publishing this page from the Experience Editor, only the MainMenu data source root item will be published, and not its children.

![image](https://github.com/andiappan-ar/sitecore-experience-editor-children-publisher/assets/11770345/807c7823-a1be-4d37-9f79-9cc12b84598f)

## Solution
Overwrite the default pipeline (AddItemLinkReferences)

* Maintain the Sitecore setting as a list of data sources you want to get their children to be published
* During publish event this pipeline will trigger. In the execution get the list of maintained list Sitecore settings and add their references  in the process queue

Note : In this module i referred direct datasource items as a settings. This can be change into template or other logics, as per your convenience.

## Implementation

1. Create sitecore item settings to hold list of datsources to be consider(This list will determine the list of childrens to be published or not). 
Note : In this module i referred direct datasource items as a settings. This can be change into template or other logics, as per your convenience.

2. Place the [/App_Config/Include/Foundation/Sitecore.Foundation.ScExtensions.SitecoreCustomization.config](https://github.com/andiappan-ar/sitecore-experience-editor-children-publisher/blob/master/App_Config/Include/Foundation/Sitecore.Foundation.ScExtensions.SitecoreCustomization.config)
3. Place the dll(Sitecore.Foundation.ScExtensions.dll).
4. Now publish the page from experience editor. CHild items will get publish , as you mentioned in settings.
