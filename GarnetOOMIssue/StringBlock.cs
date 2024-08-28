using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarnetOOMIssue
{
	public class StringBlock
	{
		public string data { get; set; } = string.Empty;
		public int length { get => data.Length; }
		public DateTime expire { get; set; } = DateTime.MinValue;
	}
}
