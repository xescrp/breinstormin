using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace breinstormin.tools.visualstudio
{

    public class VSProjectWithFileInfo : VSProjectInfo
    {
        public VSProjectWithFileInfo(
            VSSolution ownerSolution,
            Guid projectGuid,
            string projectName,
            string projectFileName,
            Guid projectTypeGuid)
            : base(ownerSolution, projectGuid, projectName, projectTypeGuid)
        {
            this.projectFileName = projectFileName;
        }


        public VSProject Project { get; set; }


        public string ProjectDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(Path.Combine(OwnerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }


        public string ProjectFileName
        {
            get { return projectFileName; }
        }


        public string ProjectFileNameFull
        {
            get
            {
                return Path.GetFullPath(
                    Path.Combine(OwnerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        public IXPathNavigable OpenProjectFileAsXmlDocument()
        {
            //if (log.IsDebugEnabled)
            //    log.DebugFormat ("OpenProjectFileAsXmlDocument '{0}'", this.ProjectFileName);

            using (Stream stream = File.Open(Path.Combine(OwnerSolution.SolutionDirectoryPath, ProjectFileName), FileMode.Open, FileAccess.Read))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                return xmlDoc;
            }
        }

        public override void Parse(VSSolutionFileParser parser)
        {
            while (true)
            {
                string line = parser.NextLine();

                if (line == null)
                    parser.ThrowParserException("Unexpected end of solution file.");

                Match endProjectMatch = VSSolution.RegexEndProject.Match(line);

                if (endProjectMatch.Success)
                    break;
            }
        }

        private readonly string projectFileName;
        public const string MSBuildNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";
    }
}