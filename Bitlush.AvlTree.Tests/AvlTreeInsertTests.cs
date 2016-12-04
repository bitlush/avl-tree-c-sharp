using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitlush.Tests
{
	public class AvlTreeInsertTests
	{
		[Fact]
		public void Empty()
		{
			var tree = new AvlTree<int, int>();

			Assert.Equal(0, tree.Count());
			AssertTreeValid("", tree);
		}

		[Fact]
		public void CountMultiple()
		{
			var tree = SetupTree(20, 8, 22);

			Assert.Equal(3, tree.Count());
		}

		[Fact]
		public void DuplicateFails()
		{
			var tree = new AvlTree<int, int>();

			bool success0a = tree.Insert(0);
			bool success0b = tree.Insert(0);

			Assert.True(success0a);
			Assert.False(success0b);
	
			AssertTreeValid("0", tree);
		}

		[Fact]
		public void LeftLeft()
		{
			var tree = SetupTree(20, 8);

			AssertTreeValid("20+:{8,~}", tree);

			tree.Insert(4);

			AssertTreeValid("8:{4,20}", tree);
		}

		[Fact]
		public void LeftRight()
		{
			var tree = SetupTree(20, 4);

			AssertTreeValid("20+:{4,~}", tree);

			tree.Insert(8);

			AssertTreeValid("8:{4,20}", tree);
		}

		[Fact]
		public void RightLeft()
		{
			var tree = SetupTree(4, 20);

			AssertTreeValid("4-:{~,20}", tree);

			tree.Insert(8);

			AssertTreeValid("8:{4,20}", tree);
		}

		[Fact]
		public void RightRight()
		{
			var tree = SetupTree(4, 8);

			AssertTreeValid("4-:{~,8}", tree);

			tree.Insert(20);

			AssertTreeValid("8:{4,20}", tree);
		}

		[Fact]
		public void RootLeftRightRight()
		{
			var tree = SetupTree(20, 4, 26, 3, 9);

			AssertTreeValid("20+:{4:{3,9},26}", tree);

			tree.Insert(15);

			AssertTreeValid("9:{4+:{3,~},20:{15,26}}", tree);
		}

		[Fact]
		public void RootLeftRightLeft()
		{
			var tree = SetupTree(20, 4, 26, 3, 9);

			AssertTreeValid("20+:{4:{3,9},26}", tree);

			tree.Insert(8);

			AssertTreeValid("9:{4:{3,8},20-:{~,26}}", tree);
		}

		[Fact]
		public void RootRightLeftLeft()
		{
			var tree = SetupTree(20, 4, 26, 24, 30);

			AssertTreeValid("20-:{4,26:{24,30}}", tree);

			tree.Insert(23);

			AssertTreeValid("24:{20:{4,23},26-:{~,30}}", tree);
		}

		[Fact]
		public void RootRightLeftRight()
		{
			var tree = SetupTree(20, 4, 26, 24, 30);

			AssertTreeValid("20-:{4,26:{24,30}}", tree);

			tree.Insert(25);

			AssertTreeValid("24:{20+:{4,~},26:{25,30}}", tree);
		}

		[Fact]
		public void RightParentedLeftLeft()
		{
			var tree = SetupTree(2, 1, 20, 8);

			AssertTreeValid("2-:{1,20+:{8,~}}", tree);

			tree.Insert(4);

			AssertTreeValid("2-:{1,8:{4,20}}", tree);
		}

		[Fact]
		public void LeftParentedLeftLeft()
		{
			var tree = SetupTree(24, 28, 20, 8);

			AssertTreeValid("24+:{20+:{8,~},28}", tree);

			tree.Insert(4);

			AssertTreeValid("24+:{8:{4,20},28}", tree);
		}

		[Fact]
		public void LeftParentedLeftRight()
		{
			var tree = SetupTree(24, 28, 20, 4);

			AssertTreeValid("24+:{20+:{4,~},28}", tree);

			tree.Insert(8);

			AssertTreeValid("24+:{8:{4,20},28}", tree);
		}

		[Fact]
		public void RightParentedLeftRight()
		{
			var tree = SetupTree(2, 1, 20, 4);

			AssertTreeValid("2-:{1,20+:{4,~}}", tree);

			tree.Insert(8);

			AssertTreeValid("2-:{1,8:{4,20}}", tree);
		}

		[Fact]
		public void LeftParentedRightLeft()
		{
			var tree = SetupTree(24, 28, 4, 20);

			AssertTreeValid("24+:{4-:{~,20},28}", tree);

			tree.Insert(8);

			AssertTreeValid("24+:{8:{4,20},28}", tree);
		}

		[Fact]
		public void RightParentedRightLeft()
		{
			var tree = SetupTree(2, 1, 4, 20);

			AssertTreeValid("2-:{1,4-:{~,20}}", tree);

			tree.Insert(8);

			AssertTreeValid("2-:{1,8:{4,20}}", tree);
		}

		[Fact]
		public void LeftParentedRightRight()
		{
			var tree = SetupTree(24, 28, 4, 8);

			AssertTreeValid("24+:{4-:{~,8},28}", tree);

			tree.Insert(20);

			AssertTreeValid("24+:{8:{4,20},28}", tree);
		}

		[Fact]
		public void RightParentedRightRight()
		{
			var tree = SetupTree(2, 1, 4, 8);

			AssertTreeValid("2-:{1,4-:{~,8}}", tree);

			tree.Insert(20);

			AssertTreeValid("2-:{1,8:{4,20}}", tree);
		}

		private void AssertTreeValid(string description, AvlTree<int, int> tree)
		{
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
