using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace breinstormin.tools.web.css.Model
{
	/// <summary></summary>
	public interface IDeclarationContainer {
		/// <summary></summary>
		List<Declaration> Declarations { get; set; }
	}
}