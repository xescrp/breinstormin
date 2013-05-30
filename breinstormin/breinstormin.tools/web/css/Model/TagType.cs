using System;
using System.Collections.Generic;

namespace breinstormin.tools.web.css.Model
{
	/// <summary></summary>
	[Flags]
	public enum TagType {
		/// <summary></summary>
		Named = 1,
		/// <summary></summary>
		Classed = 2,
		/// <summary></summary>
		IDed = 4,
		/// <summary></summary>
		Pseudoed = 8,
		/// <summary></summary>
		Directive = 16
	}
}