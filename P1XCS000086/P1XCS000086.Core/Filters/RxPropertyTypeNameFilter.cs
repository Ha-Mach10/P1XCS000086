using P1XCS000086.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Core.Filters
{
	public static class RxPropertyTypeNameFilter<T>
		where T : RegionViewModelBase
	{
		public static IEnumerable<dynamic> GenerateReactiveProperties(T target)
		{
			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in props)
			{

			}
			return props.Select(p => p);
		}
	}
}
