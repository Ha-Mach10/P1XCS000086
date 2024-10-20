using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace P1XCS000086.Modules.CodeManagerView.Domains
{
    public class ProjectTypeItem
    {
		// ---------------------------------------------------------------
		// Properties
		// --------------------------------------------------------------- 

		public string ProjectText { get; private set; }
        public string ProjectHelpText { get; private set; }
        public List<string> Tags { get; private set; } = new List<string>();
        public BitmapSource BitmapSource { get; private set; }



		// ---------------------------------------------------------------
		// Constructor
		// ---------------------------------------------------------------

		public ProjectTypeItem(string projectText, string projectHelpText, List<string> tags)
		{
			ProjectText = projectText;
			ProjectHelpText = projectHelpText;
			Tags = tags;
		}
		public ProjectTypeItem(string projectText, string projectHelpText, List<string> tags, BitmapSource bitmapSource)
			:this(projectText, projectHelpText, tags)
		{
			BitmapSource = bitmapSource;
		}
	}
}
