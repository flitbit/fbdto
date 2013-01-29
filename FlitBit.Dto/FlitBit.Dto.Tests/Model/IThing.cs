using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlitBit.Dto.Tests.Model
{
	[DTO]
	public interface IThing
	{
		int Identity { get; set; }
		string Name { get; set; }
	}
}
