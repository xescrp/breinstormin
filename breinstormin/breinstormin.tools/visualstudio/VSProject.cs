using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;


namespace breinstormin.tools.visualstudio
{
   
    public class VSProject
    {
        public VSProject(string projectFileName)
        {
            this.projectFileName = projectFileName;
        }

        
        public IList<VSProjectConfiguration> Configurations
        {
            get { return configurations; }
        }

        
        public IList<VSProjectItem> Items
        {
            get { return items; }
        }

        public string ProjectFileName
        {
            get { return projectFileName; }
        }

       
        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        
        public VSProjectConfiguration FindConfiguration(string condition)
        {
            foreach (VSProjectConfiguration configuration in configurations)
            {
                if (0 == string.Compare(configuration.Condition, condition, StringComparison.Ordinal))
                    return configuration;
            }

            return null;
        }

        
        public static VSProject Load(string projectFileName)
        {
            //Log.DebugFormat("Load ('{0}')", projectFileName);

            using (Stream stream = File.OpenRead(projectFileName))
            {
                VSProject data = new VSProject(projectFileName) { propertiesDictionary = true };

                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

                using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                {
                    xmlReader.Read();
                    while (false == xmlReader.EOF)
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.XmlDeclaration:
                                xmlReader.Read();
                                break;

                            case XmlNodeType.Element:
                                if (xmlReader.Name == "Project")
                                {
                                    data.ReadProject(xmlReader);

                                    
                                }
                                if (xmlReader.Name == "VisualBasic" | xmlReader.Name.ToLower() == "csharp")
                                {
                                    data.ReadProject(xmlReader);

                                    
                                }
                                xmlReader.Read();
                                break;
                                
                            default:
                                xmlReader.Read();
                                continue;
                        }
                    }
                }

                return data;
            }
        }

        
        public IList<VSProjectItem> GetSingleTypeItems(string getItemType)
        {
            List<VSProjectItem> returnList = new List<VSProjectItem>();
            foreach (VSProjectItem item in Items)
            {
                if (item.ItemType == getItemType)
                    returnList.Add(item);
            }

            return returnList;
        }

        private void ReadProject(XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement && false == xmlReader.EOF)
            {
                switch (xmlReader.Name)
                {
                    case "PropertyGroup":
                        if (propertiesDictionary)
                        {
                            ReadPropertyGroup(xmlReader);
                            propertiesDictionary = false;
                        }
                        else
                        {
                            configurations.Add(ReadPropertyGroup(xmlReader));
                        }

                        xmlReader.Read();
                        break;
                    case "ItemGroup":
                        ReadItemGroup(xmlReader);
                        xmlReader.Read();
                        break;
                    case "Build":
                        ReadBuildConfigSettings(xmlReader);
                        xmlReader.Read();
                        ReadVS2003Items(xmlReader);
                        break;
                    default:
                        xmlReader.Read();
                        
                        continue;
                }
            }
        }

        private void ReadBuildConfigSettings(XmlReader xmlReader) 
        {
            string assemblyname = "";
            string outputtype = "";
            xmlReader.Read();

            if (xmlReader.Name == "Settings") 
            {
                assemblyname = xmlReader["AssemblyName"];
                outputtype = xmlReader["OutputType"];
                properties.Add("AssemblyName", assemblyname);
                properties.Add("OutputType", outputtype);
            }
            //Leer las dos configuraciones, si existen... 
            xmlReader.Read();
            //Supuestamente Debug...
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.Name == "Config")
                {
                    string name = xmlReader["Name"];
                    string outputpath = xmlReader["OutputPath"];
                    VSProjectConfiguration dbg_conf = new VSProjectConfiguration();
                    dbg_conf.Condition = "";
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("AssemblyName", assemblyname));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("OutputType", outputtype));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("Name", name));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("OutputPath", outputpath));
                    configurations.Add(dbg_conf);
                }
                xmlReader.Read();
                //Supuestamente Release...
                if (xmlReader.Name == "Config")
                {
                    string name = xmlReader["Name"];
                    string outputpath = xmlReader["OutputPath"];
                    VSProjectConfiguration dbg_conf = new VSProjectConfiguration();
                    dbg_conf.Condition = "";
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("AssemblyName", assemblyname));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("OutputType", outputtype));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("Name", name));
                    dbg_conf.Properties.Add(new KeyValuePair<string, string>("OutputPath", outputpath));
                    configurations.Add(dbg_conf);
                }
                xmlReader.Read();
            }
            xmlReader.Read();
            if (xmlReader.Name == "References") 
            {
                xmlReader.Read();
                while (xmlReader.Name == "Reference")
                {
                    try
                    {
                        string refname = xmlReader["Name"];
                        VSProjectItem item = new VSProjectItem(VSProjectItem.Reference);
                        item.Item = refname;
                        item.ItemProperties.Add(new KeyValuePair<string, string>("AssemblyName", xmlReader["AssemblyName"]));
                        if (xmlReader["HintPath"] != null && xmlReader["HintPath"] != "")
                        { item.ItemProperties.Add(new KeyValuePair<string, string>("HintPath", xmlReader["HintPath"])); }
                        items.Add(item);

                    }
                    catch (Exception ex) { }
                    finally { xmlReader.Read(); }

                }

            }
            xmlReader.Read();
            if (xmlReader.Name == "Imports") 
            { 
                xmlReader.Read();
                while (xmlReader.Name == "Import")
                { xmlReader.Read(); }
                xmlReader.Read();
            }

            
        }

        private void ReadVS2003Items(XmlReader xmlReader) 
        {
            if (xmlReader.Name == "Files") 
            {
                xmlReader.Read();
                if (xmlReader.Name == "Include") 
                {
                    xmlReader.Read();
                    while (xmlReader.NodeType != XmlNodeType.EndElement) 
                    { 
                        try
                        {
                            if (xmlReader.Name == "File")
                            {
                                string relPath = xmlReader["RelPath"];
                                string type = xmlReader["BuildAction"];
                                VSProjectItem contentitem = new VSProjectItem(type);
                                contentitem.Item = relPath;
                                items.Add(contentitem);
                                xmlReader.Read();
                            }
                        }
                        catch(Exception ex)
                        {}
                        finally
                        { xmlReader.Read(); }
                        
                    }
                }
            }
        }
       
        private VSProjectConfiguration ReadPropertyGroup(XmlReader xmlReader)
        {
            VSProjectConfiguration configuration = new VSProjectConfiguration();

            if (xmlReader["Condition"] != null && propertiesDictionary == false)
            {
                configuration.Condition = xmlReader["Condition"];
            }

            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (propertiesDictionary)
                {
                    if (properties.ContainsKey(xmlReader.Name))
                        throw new ArgumentException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Property '{0}' Ya se ha añadido al grupo. VS project '{1}'",
                                xmlReader.Name,
                                ProjectFileName));

                    properties.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
                else
                {
                    if (configuration.Properties.ContainsKey(xmlReader.Name))
                        throw new ArgumentException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Property '{0}' Ya se ha añadido al grupo. VS project '{1}'",
                                xmlReader.Name,
                                ProjectFileName));

                    configuration.Properties.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
            }

            return configuration;
        }

        private void ReadItemGroup(XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement && false == xmlReader.EOF)
            {
                switch (xmlReader.Name)
                {
                    case "Content":
                        VSProjectItem contentItem = ReadItem(xmlReader, VSProjectItem.Content);
                        items.Add(contentItem);
                        break;

                    case "Compile":
                        VSProjectItem compileItems = ReadItem(xmlReader, VSProjectItem.CompileItem);
                        items.Add(compileItems);
                        break;

                    case "None":
                        VSProjectItem noneItem = ReadItem(xmlReader, VSProjectItem.NoneItem);
                        items.Add(noneItem);
                        break;

                    case "ProjectReference":
                        VSProjectItem projectReference = ReadItem(xmlReader, VSProjectItem.ProjectReference);
                        items.Add(projectReference);
                        break;

                    case "Reference":
                        VSProjectItem reference = ReadItem(xmlReader, VSProjectItem.Reference);
                        items.Add(reference);
                        break;

                    case "EmbeddedResource":
                        VSProjectItem embeddedresource = ReadItem(xmlReader, VSProjectItem.EmbeddedResource);
                        items.Add(embeddedresource);
                        break;

                    default:
                        xmlReader.Skip();
                        continue;
                }
            }
        }

        private static VSProjectItem ReadItem(XmlReader xmlReader, string itemType)
        {
            VSProjectItem item = new VSProjectItem(itemType) { Item = xmlReader["Include"] };

            if (false == xmlReader.IsEmptyElement)
            {
                xmlReader.Read();

                while (true)
                {
                    if (xmlReader.NodeType == XmlNodeType.EndElement)
                        break;

                    ReadItemProperty(item, xmlReader);
                }
            }

            xmlReader.Read();

            return item;
        }

        private static void ReadItemProperty(VSProjectItem item, XmlReader xmlReader)
        {
            string propertyName = xmlReader.Name;
            string propertyValue = xmlReader.ReadElementContentAsString();
            item.ItemProperties.Add(propertyName, propertyValue);
        }

        private readonly List<VSProjectConfiguration> configurations = new List<VSProjectConfiguration>();
        private readonly List<VSProjectItem> items = new List<VSProjectItem>();
        //private static readonly ILog Log = LogManager.GetLogger(typeof(VSProject));
        private readonly string projectFileName;
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private bool propertiesDictionary;
    }
}
