using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_EDII.Algoritmos
{
    public class ArbolHuffman
    {

        private List<NodoHuffman> nodes = new List<NodoHuffman>();
        public NodoHuffman Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new NodoHuffman() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<NodoHuffman> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<NodoHuffman>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<NodoHuffman> taken = orderedNodes.Take(2).ToList<NodoHuffman>();

                    // Create a parent node by combining the frequencies
                    NodoHuffman parent = new NodoHuffman()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();

            }

        }

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            NodoHuffman current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(NodoHuffman node)
        {
            return (node.Left == null && node.Right == null);
        }

    }
}
