// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Runtime.InteropServices;

namespace Azure.Bicep.MSBuild
{
    public class Bicep : ToolTask
    {
        [Required]
        public ITaskItem? SourceFile { get; set; }

        [Required]
        public ITaskItem? OutputFile { get; set; }

        protected override string ToolName => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bicep.exe" : "bicep";

        protected override string GenerateFullPathToTool()
        {
            // fall back to searching in the path if ToolExe is not set
            return this.ToolName;
        }

        protected override string GenerateCommandLineCommands()
        {
            var builder = new CommandLineBuilder(quoteHyphensOnCommandLine: false, useNewLineSeparator: false);

            builder.AppendSwitch("build");
            builder.AppendFileNameIfNotNull(this.SourceFile);

            builder.AppendSwitch("--outfile");
            builder.AppendFileNameIfNotNull(this.OutputFile);

            return builder.ToString();
        }
    }
}
