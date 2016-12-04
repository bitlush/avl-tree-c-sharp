using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitlush.Tests
{
	public static class AvlTreeExtensions
	{
		public static string Description<TKey>(this AvlTree<TKey, TKey> tree)
		{
			StringBuilder builder = new StringBuilder();

			Description(builder, tree.Root);

			return builder.ToString();
		}

		public static bool Insert<TKey>(this AvlTree<TKey, TKey> source, TKey key)
		{
			return source.Insert(key, key);
		}

		public static int Count<TKey>(this AvlTree<TKey, TKey> source)
		{
			AvlNode<TKey, TKey> node = source.Root;

			if (node == null)
			{
				return 0;
			}
			else
			{
				return node.Count();
			}
		}

		private static void Description<TKey>(StringBuilder builder, AvlNode<TKey, TKey> node)
		{
			if (node != null)
			{
				builder.Append(node.Key);

				for (int i = 0; i < node.Balance; i++)
				{
					builder.Append("+");
				}

				for (int i = node.Balance; i < 0; i++)
				{
					builder.Append("-");
				}

				if (node.Left != null || node.Right != null)
				{
					builder.Append(":{");

					if (node.Left == null)
					{
						builder.Append("~");
					}
					else
					{
						Description(builder, node.Left);
					}

					builder.Append(",");

					if (node.Right == null)
					{
						builder.Append("~");
					}
					else
					{
						Description(builder, node.Right);
					}

					builder.Append("}");
				}
			}
		}
	}
}
