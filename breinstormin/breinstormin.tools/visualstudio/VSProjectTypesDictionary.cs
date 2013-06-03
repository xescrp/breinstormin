using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace breinstormin.tools.visualstudio
{

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class VSProjectTypesDictionary
    {
        public VSProjectTypesDictionary()
        {

            RegisterProjectType(VSProjectType.SolutionFolderProjectType);
            RegisterProjectType(VSProjectType.CSharpProjectType);
        }

        public void RegisterProjectType(VSProjectType projectType)
        {
            projectTypes.Add(projectType.ProjectTypeGuid, projectType);
        }


        public VSProjectType FindProjectType(Guid projectTypeGuid)
        {
            if (projectTypes.ContainsKey(projectTypeGuid))
                return projectTypes[projectTypeGuid];

            return null;
        }

        private readonly Dictionary<Guid, VSProjectType> projectTypes = new Dictionary<Guid, VSProjectType>();
    }
}