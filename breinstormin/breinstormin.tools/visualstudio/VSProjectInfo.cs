using System;

namespace breinstormin.tools.visualstudio
{
    public abstract class VSProjectInfo
    {
        public VSSolution OwnerSolution
        {
            get { return ownerSolution; }
        }

        public Guid ProjectGuid
        {
            get { return projectGuid; }
        }

        public string ProjectName
        {
            get { return projectName; }
        }

        public Guid ProjectTypeGuid
        {
            get { return projectTypeGuid; }
        }



        public abstract void Parse(VSSolutionFileParser parser);

        protected VSProjectInfo(
            VSSolution ownerSolution,
            Guid projectGuid,
            string projectName,
            Guid projectTypeGuid)
        {
            this.ownerSolution = ownerSolution;
            this.projectTypeGuid = projectTypeGuid;
            this.projectName = projectName;
            this.projectGuid = projectGuid;
        }

        private readonly VSSolution ownerSolution;
        private readonly Guid projectGuid;
        private readonly string projectName;
        private readonly Guid projectTypeGuid;
    }
}