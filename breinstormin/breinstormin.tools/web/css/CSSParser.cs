using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace breinstormin.tools.web.css {
	public class CSSParser {
		private List<string> errors = new List<string>();
        private Model.CSSDocument doc;

        public Model.CSSDocument ParseFile(string file)
        {
			StringBuilder sb = new StringBuilder();
			TextWriter errorCatch = new StringWriter(sb);
			Scanner scanner = new Scanner(file);
			Parser parser = new Parser(scanner);
			parser.errors.errorStream = errorCatch;
			parser.Parse();
			doc = parser.CSSDoc;
			SpitErrors(sb);
			return doc;
		}
		public List<Token> GetTokens(string file) {
			StringBuilder sb = new StringBuilder();
			TextWriter errorCatch = new StringWriter(sb);
			Scanner scanner = new Scanner(file);

			List<Token> ts = new List<Token>();
			Token t = scanner.Scan();
			if (t.val != "\0") { ts.Add(t); }
			while (t.val != "\0") {
				t = scanner.Scan();
				ts.Add(t);
			}
			return ts;
		}

        public Model.CSSDocument ParseText(string content)
        {
			MemoryStream mem = new MemoryStream();
			byte[] bytes = ASCIIEncoding.ASCII.GetBytes(content);
			mem.Write(bytes, 0, bytes.Length);
			return ParseStream(mem);
		}

        public Model.CSSDocument ParseStream(Stream stream)
        {
			StringBuilder sb = new StringBuilder();
			TextWriter errorCatch = new StringWriter(sb);
			Scanner scanner = new Scanner(stream);
			Parser parser = new Parser(scanner);
			parser.errors.errorStream = errorCatch;
			parser.Parse();
			doc = parser.CSSDoc;
			SpitErrors(sb);
			return doc;
		}

        public Model.CSSDocument CSSDocument
        {
			get { return doc; }
		}

		public List<string> Errors {
			get { return errors; }
		}

		private void SpitErrors(StringBuilder sb) {
			errors = new List<string>();
			string text = sb.ToString().Replace("\r", "");
			if (text.Length == 0) { return; }
			string[] lines = text.Split('\n');
			foreach (string line in lines) {
				errors.Add(line);
			}
		}

		/*
				System.Text.StringBuilder sb = new StringBuilder();
				System.IO.TextWriter errorCatch = new System.IO.StringWriter(sb);
				Scanner scanner = new Scanner(ofd.FileName);
				Parser parser = new Parser(scanner);
				parser.errors.errorStream = errorCatch;
				parser.Parse();
				doc = parser.CSSDocument;
		*/
	}
}