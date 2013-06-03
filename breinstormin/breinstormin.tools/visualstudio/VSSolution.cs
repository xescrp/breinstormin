using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;


namespace breinstormin.tools.visualstudio
{

    public class VSSolution
    {
 
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
        public ReadOnlyCollection<VSProjectInfo> Projects
        {
            get { return projects.AsReadOnly(); }
        }


        public VSProjectTypesDictionary ProjectTypesDictionary
        {
            get { return projectTypesDictionary; }
            set { projectTypesDictionary = value; }
        }

        public string SolutionDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(solutionFileName);
            }
        }

        public string SolutionFileName
        {
            get { return solutionFileName; }
        }

        public decimal SolutionVersion
        {
            get { return solutionVersion; }
        }


        public VSProjectInfo FindProjectById(Guid projectGuid)
        {
            foreach (VSProjectInfo projectData in projects)
                if (projectData.ProjectGuid == projectGuid)
                    return projectData;

            throw new ArgumentException(string.Format(
                CultureInfo.InvariantCulture,
                "Project {0} not found.",
                projectGuid));
        }

        public VSProjectInfo FindProjectByName(string projectName)
        {
            foreach (VSProjectInfo projectData in projects)
                if (projectData.ProjectName == projectName)
                    return projectData;

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Project {0} not found.", projectName));
        }


        public void ForEachProject(Action<VSProjectInfo> action)
        {
            projects.ForEach(action);
        }


        public static VSSolution Load(string fileName)
        {
            //Log.DebugFormat("Load ('{0}')", fileName);

            VSSolution solution = new VSSolution(fileName);

            using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    VSSolutionFileParser parser = new VSSolutionFileParser(reader);

                    string line = parser.NextLine();

                    Match solutionMatch = RegexSolutionVersion.Match(line);

                    if (solutionMatch.Success == false)
                        parser.ThrowParserException("Not a solution file.");

                    solution.solutionVersion = decimal.Parse(
                        solutionMatch.Groups["version"].Value,
                        CultureInfo.InvariantCulture);

                    while (true)
                    {
                        try
                        {
                            line = parser.NextLine();

                            if (line == null)
                                break;

                            //salimos del loop cuando 'Global' aparece
                            Match globalMatch = RegexGlobal.Match(line);
                            if (globalMatch.Success)
                                break;

                            Match projectMatch = RegexProject.Match(line);

                            if (projectMatch.Success == false)
                                parser.ThrowParserException(
                                    String.Format(
                                        CultureInfo.InvariantCulture,
                                        "Could not parse solution file (line {0}).",
                                        parser.LineCount));

                            Guid projectGuid = new Guid(projectMatch.Groups["projectGuid"].Value);
                            string projectName = projectMatch.Groups["name"].Value;
                            string projectFileName = projectMatch.Groups["path"].Value;
                            Guid projectTypeGuid = new Guid(projectMatch.Groups["projectTypeGuid"].Value);

                            if (projectFileName.ToLower().Contains("http://"))
                            {
                                string path = System.IO.Path.GetDirectoryName(fileName);
                                Uri r = new Uri(projectFileName);
                                string f = "";
                                f = r.Segments[r.Segments.Length - 1];
                                path = System.IO.Directory.GetParent(path).FullName;
                                string[] files = System.IO.Directory.GetFiles(path, f, SearchOption.AllDirectories);
                                if (files != null && files.Length > 0)
                                {
                                    projectFileName = files[files.Length - 1];
                                }
                            }
                            try
                            {
                                VSProjectInfo project;
                                if (projectTypeGuid == VSProjectType.SolutionFolderProjectType.ProjectTypeGuid)
                                {
                                    project = new VSSolutionFilesInfo(
                                        solution,
                                        projectGuid,
                                        projectName,
                                        projectTypeGuid);
                                }
                                else
                                {
                                    project = new VSProjectWithFileInfo(
                                        solution,
                                        projectGuid,
                                        projectName,
                                        projectFileName,
                                        projectTypeGuid);
                                }
                                project.Parse(parser);
                                solution.projects.Add(project);

                            }
                            catch (Exception ex)
                            {
                                //Asumimos el error. Si falla posiblemente se trate de un proyecto de setup o no regular
                            }
                        }
                        catch (Exception ex) 
                        {
                            //Asumimos el error. Si falla posiblemente se trate de un proyecto de setup o no regular
                        }
                    }
                }
            }

            return solution;
        }

       
        public void LoadProjects()
        {
            ForEachProject(
                delegate(VSProjectInfo projectInfo)
                {
                    if (projectInfo.ProjectTypeGuid == VSProjectType.CSharpProjectType.ProjectTypeGuid)
                        ((VSProjectWithFileInfo)projectInfo).Project = VSProject.Load(((VSProjectWithFileInfo)projectInfo).ProjectFileNameFull);
                });
        }

        public static readonly Regex RegexEndProject = new Regex(@"^EndProject$");
        public static readonly Regex RegexGlobal = new Regex(@"^Global$");
        public static readonly Regex RegexProject = new Regex(@"^Project\(""(?<projectTypeGuid>.*)""\) = ""(?<name>.*)"", ""(?<path>.*)"", ""(?<projectGuid>.*)""$");
        public static readonly Regex RegexSolutionVersion = new Regex(@"^Microsoft Visual Studio Solution File, Format Version (?<version>.+)$");

        protected VSSolution(string fileName)
        {
            solutionFileName = fileName;
        }

        private readonly List<VSProjectInfo> projects = new List<VSProjectInfo>();
        private VSProjectTypesDictionary projectTypesDictionary = new VSProjectTypesDictionary();
        private readonly string solutionFileName;
        private decimal solutionVersion;

        //private static readonly ILog Log = LogManager.GetLogger(typeof(VSSolution));
    }
}