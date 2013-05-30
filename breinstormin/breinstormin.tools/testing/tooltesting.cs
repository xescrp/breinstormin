using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.testing
{
    class tooltesting
    {
        void testcss() 
        {
            web.css.cssReader reader = 
                new web.css.cssReader(@"C:\Users\paco\Downloads\HostalDog_PAGINA_TEXTO_03-05-2013 - copia\css\estilos.css");
            web.css.Model.CSSDocument doc = reader.cssDocument;

            if (doc != null) 
            {
                foreach (web.css.Model.RuleSet rule in doc.RuleSets) 
                {
                    Console.Write(rule.ToString());
                }
            }

        }
    }
}
