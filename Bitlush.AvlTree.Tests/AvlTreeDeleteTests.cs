using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bitlush.Tests
{
	public class AvlTreeDeleteTests
	{
		[Fact]
		public void ParentNullAndReplaceWithRight()
		{
			var tree = SetupTree(1, 2);
			
			AssertTreeValid("1-:{~,2}", tree);

			tree.Delete(1);

			AssertTreeValid("2", tree);
		}
		
		[Fact]
		public void ParentNullAndReplaceWithLeft()
		{
			var tree = SetupTree(2, 1);
			
			AssertTreeValid("2+:{1,~}", tree);

			tree.Delete(2);

			AssertTreeValid("1", tree);
		}

		[Fact]
		public void Empty()
		{
			var tree = SetupTree(1);
			
			AssertTreeValid("1", tree);

			tree.Delete(1);

			AssertTreeValid("", tree);
		}

		[Fact]
		public void NotFound()
		{
			var tree = SetupTree(1);

			AssertTreeValid("1", tree);

			tree.Delete(2);

			AssertTreeValid("1", tree);
		}

		[Fact]
		public void LeftRight()
		{
			var tree = SetupTree(4, 2, 5, 3);

			AssertTreeValid("4+:{2-:{~,3},5}", tree);

			tree.Delete(5);

			AssertTreeValid("3:{2,4}", tree);
		}

		[Fact]
		public void RightRight()
		{
			var tree = SetupTree(4, 2, 5, 6);

			AssertTreeValid("4-:{2,5-:{~,6}}", tree);

			tree.Delete(2);

			AssertTreeValid("5:{4,6}", tree);
		}

		[Fact]
		public void RightLeft()
		{
			var tree = SetupTree(4, 2, 6, 5);

			AssertTreeValid("4-:{2,6+:{5,~}}", tree);

			tree.Delete(2);

			AssertTreeValid("5:{4,6}", tree);
		}

		[Fact]
		public void LeftLeft()
		{
			var tree = SetupTree(4, 2, 5, 1);

			AssertTreeValid("4+:{2+:{1,~},5}", tree);

			tree.Delete(1);

			AssertTreeValid("4:{2,5}", tree);
		}

		[Fact]
		public void RightNull_NonNullParentToLeft()
		{
			var tree = SetupTree(9, 4, 20, 3, 19, 21, 27);

			AssertTreeValid("9-:{4+:{3,~},20-:{19,21-:{~,27}}}", tree);

			tree.Delete(4);

			AssertTreeValid("20:{9:{3,19},21-:{~,27}}", tree);
		}
		
		[Fact]
		public void RightNull_NonNullParentToRight()
		{
			var tree = SetupTree(9, 4, 20, 3, 5, 19, 2);

			AssertTreeValid("9+:{4+:{3+:{2,~},5},20+:{19,~}}", tree);

			tree.Delete(20);

			AssertTreeValid("4:{3+:{2,~},9:{5,19}}", tree);
		}

		[Fact]
		public void LeftNull_NonNullParentToRight()
		{
			var tree = SetupTree(9, 4, 20, 3, 5, 26, 2);

			AssertTreeValid("9+:{4+:{3+:{2,~},5},20-:{~,26}}", tree);

			tree.Delete(20);

			AssertTreeValid("4:{3+:{2,~},9:{5,26}}", tree);
		}
		
		[Fact]
		public void LeftNull_NonNullParentToLeft()
		{
			var tree = SetupTree(9, 4, 20, 5, 19, 21, 27);

			AssertTreeValid("9-:{4-:{~,5},20-:{19,21-:{~,27}}}", tree);

			tree.Delete(4);

			AssertTreeValid("20:{9:{5,19},21-:{~,27}}", tree);
		}

		[Fact]
		public void SuccessorLeftNull()
		{
			var tree = SetupTree(20, 4, 26);

			AssertTreeValid("20:{4,26}", tree);

			tree.Delete(20);

			AssertTreeValid("26+:{4,~}", tree);
		}

		[Fact]
		public void LeftParentSuccessorLeftNull()
		{
			var tree = SetupTree(20, 10, 40, 30, 50);

			AssertTreeValid("20-:{10,40:{30,50}}", tree);

			tree.Delete(40);

			AssertTreeValid("20-:{10,50+:{30,~}}", tree);
		}

		[Fact]
		public void RightParentSuccessorLeftNull()
		{
			var tree = SetupTree(20, 10, 40, 5, 15);

			AssertTreeValid("20+:{10:{5,15},40}", tree);

			tree.Delete(10);

			AssertTreeValid("20+:{15+:{5,~},40}", tree);
		}

		[Fact]
		public void SuccessorRight()
		{
			var tree = SetupTree(20, 4, 26, 25);

			AssertTreeValid("20-:{4,26+:{25,~}}", tree);

			tree.Delete(20);

			AssertTreeValid("25:{4,26}", tree);
		}

		[Fact]
		public void RightParentSuccessorRight()
		{
			var tree = SetupTree(20, 10, 40, 15, 30, 50, 45);

			AssertTreeValid("20-:{10-:{~,15},40-:{30,50+:{45,~}}}", tree);

			tree.Delete(40);

			AssertTreeValid("20:{10-:{~,15},45:{30,50}}", tree);
		}

		[Fact]
		public void LeftParentSuccessorRight()
		{
			var tree = SetupTree(20, 10, 40, 5, 15, 30, 14);

			AssertTreeValid("20+:{10-:{5,15+:{14,~}},40+:{30,~}}", tree);

			tree.Delete(10);

			AssertTreeValid("20:{14:{5,15},40+:{30,~}}", tree);
		}

		
		[Fact]
		public void ExitRebalanceRightEarly()
		{
			var tree = SetupTree(20, 10, 30, 5, 15, 35, 4, 16);

			AssertTreeValid("20+:{10:{5+:{4,~},15-:{~,16}},30-:{~,35}}", tree);

			tree.Delete(35);

			AssertTreeValid("10-:{5+:{4,~},20+:{15-:{~,16},30}}", tree);
		}

		[Fact]
		public void ExitRebalanceLeftEarly()
		{
			var tree = SetupTree(20, 10, 30, 5, 25, 35, 24, 36);

			AssertTreeValid("20-:{10+:{5,~},30:{25+:{24,~},35-:{~,36}}}", tree);

			tree.Delete(5);

			AssertTreeValid("30+:{20-:{10,25+:{24,~}},35-:{~,36}}", tree);
		}

		private void AssertTreeValid(string description, AvlTree<int, int> tree)
		{
			Console.WriteLine(tree.Description());

			Assert.Equal(description, tree.Description());
			
			if (tree.Root != null)
			{
				Assert.Null(tree.Root.Parent);
			}
			else if (description == "")
			{
				Assert.Null(tree.Root);
			}
		}

		private AvlTree<int, int> SetupTree(params int[] values)
		{
			var tree = new AvlTree<int, int>();

			foreach (int value in values)
			{
				tree.Insert(value);
			}

			return tree;
		}
	}
}
