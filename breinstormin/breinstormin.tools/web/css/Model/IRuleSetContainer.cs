using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace breinstormin.tools.web.css.Model
{
	/// <summary></summary>
	public interface IRuleSetContainer {
		/// <summary></summary>
		List<RuleSet> RuleSets { get; set; }
	}
}