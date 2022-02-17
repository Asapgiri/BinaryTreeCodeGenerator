using System;

namespace InfoKod_HF_02._16
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Információ és Kódelmélet házi feladat!");
			Console.WriteLine("Készítette: Korcsák Gergely\n");

			CodedString coder = new();

			coder.GetCharNum();
			coder.GetStringLenght();
			coder.GenerateString();

			coder.CountNumbers();
			coder.CalculateChances();
			coder.CcalculateEntropy();

			Console.WriteLine();
			coder.CalculateLowestBorder();
			coder.CalculateHighestBorder();

			coder.GeneratePrefixCodeTree();
			coder.CompressString();
			coder.DecompressString();

			Console.WriteLine("\n\n\n\n\n\n\n\n\n");

		}

		
	}
}
