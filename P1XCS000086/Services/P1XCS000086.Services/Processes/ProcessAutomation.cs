using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace P1XCS000086.Services.Processes
{
    public class ProcessAutomation
    {
        public ProcessAutomation()
        {

        }



        
        public IEnumerable<Process> UpdateTargetProcess(string title)
        {
            return Process.GetProcesses().Where(x => x.MainWindowTitle == title);
            // Process.GetProcesses().
        }
        /*
        public IEnumerable<Process> UpdateTargetProcess(string title) =>
            Process.GetProcesses();
        */
	}
}
