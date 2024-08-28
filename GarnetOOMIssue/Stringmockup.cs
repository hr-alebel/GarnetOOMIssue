using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarnetOOMIssue
{
	public static class Stringmockup
	{
		public static string CreateLargeString(int size, char fillCharacter)
		{
			StringBuilder sb = new StringBuilder(size);
			for (int i = 0; i < size; i++)
			{
				sb.Append(fillCharacter);
			}
			return sb.ToString();
		}
	}
}
