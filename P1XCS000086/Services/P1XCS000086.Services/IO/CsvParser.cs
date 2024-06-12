using Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace P1XCS000086.Services.IO
{
	internal static class CsvParser
	{
		public static string FilePath { get; private set; }



		private static bool CheckResourceOrFile(string filePath)
		{
			// ディレクトリ区切り記号「\」が存在しているか判定
			if (filePath.Contains("\\"))
			{
				// リソースファイルではない
				return false;
			}

			return true;
		}
		public static DataTable ReadCsv(string filePath, Type targetType)
		{
			FilePath = filePath;

			DataTable dt = new DataTable();

			if (CheckResourceOrFile(FilePath) == true)
			{
				var assembly = targetType.Assembly;

				try
				{
					// アセンブリからストリームを生成
					using (var stream = assembly.GetManifestResourceStream(FilePath))
					{
						bool isFinished = true;

						List<List<string>> dnuB = CsvReader.ReadFromStream(stream).Select(x =>
						{
							if (isFinished == true)
							{
								List<string> dnuA = x.Headers.Select(x =>
								{
									dt.Columns.Add(new DataColumn(x));
									return x.ToString();

								}).ToList();

								isFinished = false;
							}

							DataRow dr = dt.NewRow();

							List<string> dnuC = x.Headers.Select(y =>
							{
								dr[y] = x[y];
								return y;

							}).ToList();

							dt.Rows.Add(dr);
							return dnuC;

						}).ToList();
					}
				}
				catch (Exception ex)
				{
					return null;
				}
			}
			return dt;
		}
	}
}
