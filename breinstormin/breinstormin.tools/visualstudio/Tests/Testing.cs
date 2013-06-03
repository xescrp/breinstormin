using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.visualstudio.Tests
{
    class Testing
    {
        void test1() 
        {
            string sol_file = @"\\svigi-11\e$\Webs\DotNetDes\ECI.Config\ECI.Applications\MergeSystem.Indexus.Service\Logs\NAnt\NAntBuild00002\MergeSystem.Indexus.Service.sln";
            string sol_file2 = @"C:\Development\Instalador\2.0\DotNet\dotNET.Deploy\dotNET.Deploy.sln";
            VSSolution solution = VSSolution.Load(sol_file);

            solution.LoadProjects();
            

            if (solution != null) 
            {
                if (solution.Projects != null) 
                {
                    foreach (VSProjectWithFileInfo info in solution.Projects) 
                    {
                        info.Project = VSProject.Load(info.ProjectFileNameFull);   
                            foreach (VSProjectItem item in info.Project.Items) 
                            {
                                Console.WriteLine(item.ItemType.ToString());
                                Console.WriteLine(item.Item);
                                foreach (string key in item.ItemProperties.Keys) 
                                {
                                    Console.WriteLine(item.ItemProperties[key]);
                                }
                            }
                        
                    }
                }
            }
            
        }

        
    }
}
