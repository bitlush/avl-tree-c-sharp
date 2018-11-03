using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitlush
{
	public class AvlTree<TKey, TValue> : IEnumerable<AvlNode<TKey, TValue>>
	{
		private IComparer<TKey> _comparer;
		private AvlNode<TKey, TValue> _root;

		public AvlTree(IComparer<TKey> comparer)
		{
			_comparer = comparer;
		}

		public AvlTree()
		   : this(Comparer<TKey>.Default)
		{

		}

		public AvlNode<TKey, TValue> Root
		{
			get
			{
				return _root;
			}
		}

		public IEnumerator<AvlNode<TKey, TValue>> GetEnumerator()
		{
			return new AvlNodeEnumerator<TKey, TValue>(_root);
		}

		public bool Search(TKey key, out TValue value)
		{
			AvlNode<TKey, TValue> node = _root;

			while (node != null)
			{
				if (_comparer.Compare(key, node.Key) < 0)
				{
					node = node.Left;
				}
				else if (_comparer.Compare(key, node.Key) > 0)
				{
					node = node.Right;
				}
				else
				{
					value = node.Value;

					return true;
				}
			}

			value = default(TValue);

			return false;
		}

		public bool Insert(TKey key, TValue value)
		{
			if(_root == null) //moved up case
			{
				_root = new AvlNode<TKey, TValue> { Key = key, Value = value };
				return true;
			}
			
			AvlNode<TKey, TValue> node = _root;
			
			while(true) //was: while(node != null) --- no need check each loop
			{
				int compare = _comparer.Compare(key, node.Key);

				if (compare < 0)
				{
					AvlNode<TKey, TValue> left = node.Left;

					if (left == null)
					{
						node.Left = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node };

						InsertBalance(node, 1);

						return true;
					}
					else
					{
						node = left;
					}
				}
				else if (compare > 0)
				{
					AvlNode<TKey, TValue> right = node.Right;

					if (right == null)
					{
						node.Right = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node };

						InsertBalance(node, -1);

						return true;
					}
					else
					{
						node = right;
					}
				}
				else
				{
					node.Value = value;

					return false;
				}
			}
		}

		private void InsertBalance(AvlNode<TKey, TValue> node, int balance)
		{
			while (node != null)
			{
				balance = (node.Balance += balance);

				if (balance == 0)
				{
					return;
				}
				else if (balance == 2)
				{
					if (node.Left.Balance == 1)
					{
						RotateRight(node);
					}
					else
					{
						RotateLeftRight(node);
					}

					return;
				}
				else if (balance == -2)
				{
					if (node.Right.Balance == -1)
					{
						RotateLeft(node);
					}
					else
					{
						RotateRightLeft(node);
					}

					return;
				}

				AvlNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? 1 : -1;
				}

				node = parent;
			}
		}

		private AvlNode<TKey, TValue> RotateLeft(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> right = node.Right;
			AvlNode<TKey, TValue> rightLeft = right.Left;
			AvlNode<TKey, TValue> parent = node.Parent;

			right.Parent = parent;
			right.Left = node;
			node.Right = rightLeft;
			node.Parent = right;

			if (rightLeft != null)
			{
				rightLeft.Parent = node;
			}

			if (node == _root)
			{
				_root = right;
			}
			else if (parent.Right == node)
			{
				parent.Right = right;
			}
			else
			{
				parent.Left = right;
			}

			right.Balance++;
			node.Balance = -right.Balance;

			return right;
		}

		private AvlNode<TKey, TValue> RotateRight(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> left = node.Left;
			AvlNode<TKey, TValue> leftRight = left.Right;
			AvlNode<TKey, TValue> parent = node.Parent;

			left.Parent = parent;
			left.Right = node;
			node.Left = leftRight;
			node.Parent = left;

			if (leftRight != null)
			{
				leftRight.Parent = node;
			}

			if (node == _root)
			{
				_root = left;
			}
			else if (parent.Left == node)
			{
				parent.Left = left;
			}
			else
			{
				parent.Right = left;
			}

			left.Balance--;
			node.Balance = -left.Balance;

			return left;
		}

		private AvlNode<TKey, TValue> RotateLeftRight(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> left = node.Left;
			AvlNode<TKey, TValue> leftRight = left.Right;
			AvlNode<TKey, TValue> parent = node.Parent;
			AvlNode<TKey, TValue> leftRightRight = leftRight.Right;
			AvlNode<TKey, TValue> leftRightLeft = leftRight.Left;

			leftRight.Parent = parent;
			node.Left = leftRightRight;
			left.Right = leftRightLeft;
			leftRight.Left = left;
			leftRight.Right = node;
			left.Parent = leftRight;
			node.Parent = leftRight;

			if (leftRightRight != null)
			{
				leftRightRight.Parent = node;
			}

			if (leftRightLeft != null)
			{
				leftRightLeft.Parent = left;
			}

			if (node == _root)
			{
				_root = leftRight;
			}
			else if (parent.Left == node)
			{
				parent.Left = leftRight;
			}
			else
			{
				parent.Right = leftRight;
			}

			if (leftRight.Balance == -1)
			{
				node.Balance = 0;
				left.Balance = 1;
			}
			else if (leftRight.Balance == 0)
			{
				node.Balance = 0;
				left.Balance = 0;
			}
			else
			{
				node.Balance = -1;
				left.Balance = 0;
			}

			leftRight.Balance = 0;

			return leftRight;
		}

		private AvlNode<TKey, TValue> RotateRightLeft(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> right = node.Right;
			AvlNode<TKey, TValue> rightLeft = right.Left;
			AvlNode<TKey, TValue> parent = node.Parent;
			AvlNode<TKey, TValue> rightLeftLeft = rightLeft.Left;
			AvlNode<TKey, TValue> rightLeftRight = rightLeft.Right;

			rightLeft.Parent = parent;
			node.Right = rightLeftLeft;
			right.Left = rightLeftRight;
			rightLeft.Right = right;
			rightLeft.Left = node;
			right.Parent = rightLeft;
			node.Parent = rightLeft;

			if (rightLeftLeft != null)
			{
				rightLeftLeft.Parent = node;
			}

			if (rightLeftRight != null)
			{
				rightLeftRight.Parent = right;
			}

			if (node == _root)
			{
				_root = rightLeft;
			}
			else if (parent.Right == node)
			{
				parent.Right = rightLeft;
			}
			else
			{
				parent.Left = rightLeft;
			}

			if (rightLeft.Balance == 1)
			{
				node.Balance = 0;
				right.Balance = -1;
			}
			else if (rightLeft.Balance == 0)
			{
				node.Balance = 0;
				right.Balance = 0;
			}
			else
			{
				node.Balance = 1;
				right.Balance = 0;
			}

			rightLeft.Balance = 0;

			return rightLeft;
		}

		public bool Delete(TKey key)
		{
			AvlNode<TKey, TValue> node = _root;

			while (node != null)
			{
				if (_comparer.Compare(key, node.Key) < 0)
				{
					node = node.Left;
				}
				else if (_comparer.Compare(key, node.Key) > 0)
				{
					node = node.Right;
				}
				else
				{
					AvlNode<TKey, TValue> left = node.Left;
					AvlNode<TKey, TValue> right = node.Right;

					if (left == null)
					{
						if (right == null)
						{
							AvlNode<TKey, TValue> parent = node.Parent; //moved
							if(parent==null) //was: if(node == _root) --- slower
							{
								_root = null;
							}
							else
							{
								//(moved up) AvlNode<TKey, TValue> parent = node.Parent;
								if (parent.Left == node)
								{
									parent.Left = null;
									DeleteBalance(parent, -1);
								}
								else
								{
									parent.Right = null;
									DeleteBalance(parent, 1);
								}
							}
						}
						else
						{
							//(was) Replace(node, right);
							//(was) DeleteBalance(node, 0);
							Replace_(node, right);
							DeleteBalance(right, 0);
						}
					}
					else if (right == null)
					{
						//(was) Replace(node, left);
						//(was) DeleteBalance(node, 0);
						Replace_(node, left);
						DeleteBalance(left, 0);
					}
					else
					{
						AvlNode<TKey, TValue> successor = right;

						if (successor.Left == null)
						{
							//(unused) AvlNode<TKey, TValue> parent = node.Parent;
							//(updated in Replace_()) successor.Parent = parent;
							successor.Left = left;
							successor.Balance = node.Balance;
							left.Parent = successor;

							//(was) if(node == _root)
							//(was) {
							//(was) 	_root = successor;
							//(was) }
							//(was) else
							//(was) {
							//(was) 	if (parent.Left == node)
							//(was) 	{
							//(was) 		parent.Left = successor;
							//(was) 	}
							//(was) 	else
							//(was) 	{
							//(was) 		parent.Right = successor;
							//(was) 	}
							//(was) }
							Replace_(node, successor); //(new)
							
							DeleteBalance(successor, 1);
						}
						else
						{ //here: successor.Left != null
							while (successor.Left != null) //as min 1 loop done
							{
								successor = successor.Left;
							}
							//here: successor is always Left child

							//(unused) AvlNode<TKey, TValue> parent = node.Parent;
							AvlNode<TKey, TValue> successorParent = successor.Parent;
							AvlNode<TKey, TValue> successorRight = successor.Right;

							//(always true) if (successorParent.Left == successor)
							//{
								successorParent.Left = successorRight;
							//}
							//else
							//{
							//	successorParent.Right = successorRight;
							//}

							if (successorRight != null)
							{
								successorRight.Parent = successorParent;
							}

							//(slow) successor.Parent = parent;
							//(slow) successor.Left = left;
							//(slow) successor.Balance = node.Balance;
							//(slow) successor.Right = right;
							//(slow) right.Parent = successor;
							//(slow) left.Parent = successor;
							//(slow) if (node == _root)
							//(slow) {
							//(slow)		_root = successor;
							//(slow) }
							//(slow) else
							//(slow) {
							//(slow) 	if (parent.Left == node)
							//(slow)		{
							//(slow)			parent.Left = successor;
							//(slow)		}
							//(slow)		else
							//(slow)		{
							//(slow)			parent.Right = successor;
							//(slow)		}
							//(slow) }
							node.Key = successor.Key;    //(fast)
							node.Value = succesor.Value; //(fast)

							DeleteBalance(successorParent, -1);
						}
					}

					return true;
				}
			}

			return false;
		}

		private void DeleteBalance(AvlNode<TKey, TValue> node, int balance)
		{
			while (node != null)
			{
				balance = (node.Balance += balance);

				if (balance == 2)
				{
					if (node.Left.Balance >= 0)
					{
						node = RotateRight(node);

						if (node.Balance == -1)
						{
							return;
						}
					}
					else
					{
						node = RotateLeftRight(node);
					}
				}
				else if (balance == -2)
				{
					if (node.Right.Balance <= 0)
					{
						node = RotateLeft(node);

						if (node.Balance == 1)
						{
							return;
						}
					}
					else
					{
						node = RotateRightLeft(node);
					}
				}
				else if (balance != 0)
				{
					return;
				}

				AvlNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? -1 : 1;
				}

				node = parent;
			}
		}

		/*(replaced by faster & usable) 
		private static void Replace(AvlNode<TKey, TValue> target, AvlNode<TKey, TValue> source)
		{ 
		 	AvlNode<TKey, TValue> left = source.Left;
			AvlNode<TKey, TValue> right = source.Right;
			target.Balance = source.Balance;
			target.Key = source.Key;
			target.Value = source.Value;
			target.Left = left;
			target.Right = right;
			if (left != null)
			{
				left.Parent = target;
			}
			if (right != null)
			{
				right.Parent = target;
			}
		}*/
		private void Replace_(AvlNode<TKey, TValue> target, AvlNode<TKey, TValue> source)
		{//!! changed surviving node
			Node parent = target.Parent;
			if(parent == null)
			{
				_root = source;
			}
			else
			{
				if(parent.Left == target) 
				{
					parent.Left = source;
				}
				else
				{
					parent.Right = source;
				}
			}
			source.Parent = parent;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
