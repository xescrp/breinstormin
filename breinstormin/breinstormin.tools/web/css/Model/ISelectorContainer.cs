using System;
using System.Collections.Generic;

namespace breinstormin.tools.web.css.Model
{
	/// <summary></summary>
	public interface ISelectorContainer {
		/// <summary></summary>
		List<Selector> Selectors { get; set; }
	}
}