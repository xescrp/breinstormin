using System;
using System.Globalization;
using System.IO;

namespace breinstormin.tools.visualstudio
{
    public class VSSolutionFileParser
    {
        public VSSolutionFileParser(TextReader reader)
        {
            this.reader = reader;
        }

        public int LineCount
        {
            get { return lineCount; }
        }

        public string NextLine()
        {
            string line = null;

            do
            {
                if (reader.Peek() == -1)
                    break;

                line = reader.ReadLine();
                IncrementLineCount();

                //if (log.IsDebugEnabled)
                //    log.DebugFormat ("Read line ({0}): {1}", parserContext.LineCount, line);
            }
            while (line.Trim().Length == 0 || line.StartsWith("#", StringComparison.OrdinalIgnoreCase));

            return line;
        }

        public void ThrowParserException(string reason)
        {
            throw new ArgumentException(
                String.Format(
                    CultureInfo.InvariantCulture,
                    "{0} (line {1})",
                    reason,
                    lineCount));
        }

        private void IncrementLineCount()
        {
            lineCount++;
        }

        private int lineCount;
        private readonly TextReader reader;
    }
}