using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace P1XCS000086.Services.IO.Data
{
	[XmlRoot("TemplateData")]
	internal class TemplateData
	{
		[XmlElement("Name")]
		public string Name;
		
		[XmlElement("Description")]
		public string Description;
		
		[XmlElement("Icon")]
		public string Icon;
		
		[XmlElement("ProjectType")]
		public string ProjectType;
		
		[XmlElement("TemplateID")]
		public string TemplateID;
		
		[XmlElement("SortOrder")]
		public string SortOrder;

		[XmlElement("NumberOfParentCategoriesToRollUp")]
		public string NumberOfParentCategoriesToRollUp;

		[XmlElement("CreateNewFolder")]
		public string CreateNewFolder;

		[XmlElement("DefaultName")]
		public string DefaultName;

		[XmlElement("ProvideDefaultName")]
		public string ProvideDefaultName;

		[XmlElement("PreviewImage")]
		public string PreviewImage;
	}
}
