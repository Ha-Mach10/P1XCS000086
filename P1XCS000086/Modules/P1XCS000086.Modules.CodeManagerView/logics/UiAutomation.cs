using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace P1XCS000086.Modules.CodeManagerView.logics
{
	internal class UiAutomation
	{
		UiAutomation(IntPtr hWnd)
		{
			AutomationElement element = AutomationElement.FromHandle(hWnd);

		}
	}
}
