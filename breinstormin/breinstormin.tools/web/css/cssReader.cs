using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.web.css
{
    public class cssReader
    {
        private Model.CSSDocument _readedcss;
        public Model.CSSDocument cssDocument { get { return _readedcss; } }

        public cssReader(string filename) 
        {
            CSSParser css = new CSSParser();
            _readedcss = css.ParseFile(filename);
        }
    }
}
