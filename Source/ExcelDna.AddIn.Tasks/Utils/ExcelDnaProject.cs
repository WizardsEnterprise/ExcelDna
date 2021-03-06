﻿using System;
using System.IO;

namespace ExcelDna.AddIn.Tasks.Utils
{
    public class ExcelDnaProject : IExcelDnaProject
    {
        public bool TrySetDebuggerOptions(string projectName, string excelExePath, string excelAddInToDebug, Action<string> logDebugMessage)
        {
            using (var dte = new DevToolsEnvironment())
            {
                var project = dte.GetProjectByName(projectName, logDebugMessage);
                if (project != null)
                {
                    logDebugMessage($"Found project: {project.Name}");
                    var configuration = project
                        .ConfigurationManager
                        .ActiveConfiguration;

                    var startAction = configuration.Properties.Item("StartAction");
                    var startProgram = configuration.Properties.Item("StartProgram");
                    var startArguments = configuration.Properties.Item("StartArguments");

                    startAction.Value = 1; // Start external program
                    startProgram.Value = excelExePath;
                    startArguments.Value = string.Format(@"""{0}""", Path.GetFileName(excelAddInToDebug));

                    project.Save(string.Empty);

                    return true;
                }
            }

            return false;
        }
    }
}