using System;

namespace breinstormin.tools.visualstudio
{

    public class VSProjectType : IEquatable<VSProjectType>
    {

        public VSProjectType(Guid projectTypeGuid, string projectTypeName)
        {
            this.projectTypeGuid = projectTypeGuid;
            this.projectTypeName = projectTypeName;
        }

        public static VSProjectType CSharpProjectType
        {
            get { return cSharpProjectType; }
        }


        public Guid ProjectTypeGuid
        {
            get { return projectTypeGuid; }
        }


        public string ProjectTypeName
        {
            get { return projectTypeName; }
        }


        public static VSProjectType SolutionFolderProjectType
        {
            get { return solutionFolderProjectType; }
        }

        public bool Equals(VSProjectType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.projectTypeGuid.Equals(projectTypeGuid);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(VSProjectType))
                return false;
            return Equals((VSProjectType)obj);
        }


        public override int GetHashCode()
        {
            return projectTypeGuid.GetHashCode();
        }


        public static bool operator ==(VSProjectType left, VSProjectType right)
        {
            return Equals(left, right);
        }


        public static bool operator !=(VSProjectType left, VSProjectType right)
        {
            return !Equals(left, right);
        }

        private static readonly VSProjectType cSharpProjectType = new VSProjectType(
            new Guid("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"),
            "C# Project");

        private static readonly VSProjectType solutionFolderProjectType = new VSProjectType(
            new Guid("{2150E333-8FDC-42A3-9474-1A3956D46DE8}"),
            "Solution Folder");

        private readonly Guid projectTypeGuid;
        private readonly string projectTypeName;
    }
}