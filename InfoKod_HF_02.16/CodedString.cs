using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoKod_HF_02._16
{
	public class CodedString
	{
		private static readonly Random random = new Random();

		int charNum;
		int stringLength;
		string generatedString;
		Dictionary<char, int> charCount;
		Dictionary<char, double> charChance;
		double entrophy;
		int lovestBorder;
		int highestBorder;
		double efficiency;
		Dictionary<char, string> bynaryTree;
		class Node
		{
			public Node(char _ch, int _freq, Node _left = null, Node _right = null)
			{
				ch = _ch;
				freq = _freq;
				left = _left;
				right = _right;
			}
			public char ch;
			public int freq;
			public Node left, right;
		}
		string compressedString;

		public void GetCharNum()
		{
			string cread = string.Empty;
			bool parsedOnce = false;

			Console.Write("A program egy random szöveget generál\nKérem adja meg a rendelkezésre álló karakterek\nszámát: ");

			while (!int.TryParse(cread, out charNum) || charNum <= 0)
			{
				if (parsedOnce)
				{
					Console.Write("A megadott érték nem szám, vagy kisebb mint 1! kérlek add meg újra!\nKarakterek száma: ");
				}
				else
				{
					parsedOnce = true;
				}
				cread = Console.ReadLine();
			}
		}

		public void GetStringLenght()
		{
			string cread = string.Empty;
			bool parsedOnce = false;

			Console.Write("Kérem adja meg a szöveg hosszát: ");

			while (!int.TryParse(cread, out stringLength) || stringLength < 60)
			{
				if (parsedOnce)
				{
					Console.Write("A megadott érték nem szám, vagy kisebb mint 60! kérlek add meg újra!\nKarakterek száma: ");
				}
				else
				{
					parsedOnce = true;
				}
				cread = Console.ReadLine();
			}
		}

		public void GenerateString()
		{
			List<char> letters = new List<char>();
			List<char> characterList = new List<char>();
			for (int i = 97; i < 123; i++)
			{
				letters.Add((char)i);
			}

			if (charNum <= 0)
			{
				charNum = 12;
			}
			else if (charNum > letters.Count)
			{
				charNum = letters.Count;
			}
			if (stringLength < 60)
			{
				stringLength = 120;
			}
			else if (stringLength > 9999)
			{
				stringLength = 9999;
			}

			Console.WriteLine("\n\nA gereált karakterek a következő:");
			for (int i = 0; i < charNum; i++)
			{
				// ASCII a, z
				int newCharPos = (char)random.Next(0, letters.Count);
				char newChar = letters[newCharPos];
				letters.RemoveAt(newCharPos);
				characterList.Add(newChar);
				Console.Write(newChar);
				if (i != charNum - 1) Console.Write(", ");
			}


			generatedString = string.Empty;
			for (int i = 0; i < stringLength; i++)
			{
				generatedString += characterList[random.Next(0, characterList.Count)];
			}

			Console.WriteLine("\n\nA gereált szöveg a következő:\n" + generatedString);
			Console.WriteLine();
		}

		public void CountNumbers()
		{
			charCount = new Dictionary<char, int>();

			for (int i = 0; i < generatedString.Length; i++)
			{
				if (charCount.ContainsKey(generatedString[i]))
				{
					charCount[generatedString[i]]++;
				}
				else
				{
					charCount.Add(generatedString[i], 1);
				}
			}

			charCount = new Dictionary<char, int>(charCount.OrderByDescending(x => x.Value));

			Console.Write("\nA begyűjtött rendezve:\n  { ");
			foreach (KeyValuePair<char, int> character in charCount)
			{
				Console.Write("{0} db '{1}'", character.Value, character.Key);
				if (!character.Equals(charCount.Last())) Console.Write("; ");
			}
			Console.WriteLine(" }");
		}

		public void CalculateChances()
		{
			charChance = new Dictionary<char, double>();

			Console.Write("\nA begyűjtött karakterek valószínűségei:\n  { ");
			foreach(KeyValuePair<char, int> ci in charCount)
			{
				double chance = (double)ci.Value / (double)generatedString.Length;
				charChance.Add(ci.Key, chance);
				Console.Write("p({0}): {1:0.00}", ci.Key, chance);
				if (!ci.Equals(charCount.Last())) Console.Write("; ");
			}
			Console.WriteLine(" }");
		}

		public void CcalculateEntropy()
		{
			entrophy = 0.0f;
			foreach(var cc in charChance)
			{
				entrophy += cc.Value * Math.Log2(cc.Value);
			}
			entrophy = -entrophy;
			Console.WriteLine("\n\nAz entrópia:\n  H(x) = {0:0.00}", entrophy);
			Console.WriteLine("  H(x) < log2({0})", charNum);
			double log2charNum = Math.Log2(charNum);
			Console.Write("  {0:0.00} < {1:0.00}", entrophy, log2charNum);
			Console.WriteLine(" ({0})\n", entrophy <= log2charNum ? "igaz" : "hamis");
		}

		public void CalculateLowestBorder()
		{
			lovestBorder = (int)Math.Floor(entrophy / Math.Log2(charNum)) + 1;
			Console.WriteLine("A kódsor  alsó határa: {0:0.00}", lovestBorder);
		}

		public void CalculateHighestBorder()
		{
			highestBorder = (int)Math.Ceiling(entrophy / Math.Log2(charNum) + 1) + 1;
			Console.WriteLine("A kódsor felső határa: {0:0.00}", highestBorder);
		}

		//public void CalculateEfficiency()
		//{
		//	efficiency = entrophy / 
		//}

		public void GeneratePrefixCodeTree()
		{
			bynaryTree = new Dictionary<char, string>();
			List<Node> nodes = new List<Node>();

			foreach (var item in charCount)
			{
				nodes.Insert(0, new Node(item.Key, item.Value));
			}

			while (nodes.Count != 1)
			{
				Node left = nodes.First(); nodes.Remove(left);
				Node right = nodes.First(); nodes.Remove(right);

				nodes.Add(new Node('\0', left.freq + right.freq, left, right));
			}

			Node root = nodes.Last();
			Encode(root, string.Empty);
			//bynaryTree = new Dictionary<string, char>(bynaryTree.Reverse());

			Console.WriteLine("\n\nA begyűjtött karakterek számított kódjai:\n  {");
			foreach (var item in bynaryTree)
			{
				Console.Write("    {0}: \t'{1}'", item.Value, item.Key);
				if (!item.Equals(bynaryTree.Last())) Console.WriteLine(",");
			}
			Console.WriteLine("\n  }");
		}

		private void Encode(Node root, string str)
		{
			if (root == null)
			{
				return;
			}

			Console.Write("\n  ");
			string lineSpaces = string.Empty;
			for (int i = 0; i < str.Length; i++)
			{
				lineSpaces += " ";
			}
			Console.Write(lineSpaces);
			if (str.Length != 0)
			{
				Console.Write("∟{0}", str.Last());
			}

			if (root.left == null && root.right == null)
			{
				bynaryTree.Add(root.ch, str != string.Empty ? str : "1");
				if (str != string.Empty) Console.Write(": '{0}'", root.ch);
				else Console.Write("{0}  ∟1: '{1}'", lineSpaces, root.ch);
			}

			Encode(root.left, str + "0");
			Encode(root.right, str + "1");
		}

		public void CompressString()
		{
			compressedString = string.Empty;
			string originalBinary = string.Empty;

			for (int i = 0; i < generatedString.Length; i++)
			{
				compressedString += bynaryTree[generatedString[i]];
				originalBinary += Convert.ToString(generatedString[i], 2).PadLeft(8, '0');
				//Console.Write(bynaryTree[generatedString[i]]);
				//if (i != generatedString.Length - 1) Console.Write(", ");
			}

			Console.WriteLine("\nEredeti szöveg binárisan:\n { ");
			Console.WriteLine(originalBinary);
			Console.WriteLine(" } " + originalBinary.Length + " bit\n");

			Console.Write("\nSzöveg kódolása a követhezőképpen:\n  { ");
			Console.WriteLine(compressedString);
			Console.WriteLine(" } " + compressedString.Length + " bit\n");
			//Console.WriteLine("Kódolt szöveg:\n  { {0} }\n", compressedString);

			Console.WriteLine("\nNyert bitek: {0} bit", originalBinary.Length - compressedString.Length);
		}

		public void DecompressString()
		{
			string decompressedString = string.Empty;
			string buffer = string.Empty;

			Console.Write("\nSzöveg kikódolása...");
			for (int i = 0; i < compressedString.Length; i++)
			{
				buffer += compressedString[i];
				if (bynaryTree.ContainsValue(buffer))
				{
					decompressedString += bynaryTree.FirstOrDefault(x => x.Value == buffer).Key;
					buffer = string.Empty;
				}
			}
			Console.WriteLine("  KÉSZ.\n");

			Console.WriteLine("A kikódolt szöveg:\n  { " + decompressedString + " }\n");

			if (decompressedString == generatedString)
			{
				Console.WriteLine("A két szöveg megegyezik.");
			}
			else
			{
				Console.WriteLine("A két szöveg különbözik.");
			}
		}

	}
}
