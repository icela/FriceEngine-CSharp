using System.Collections.Generic;
using System.Drawing;
using FriceEngine.Object;

namespace FriceEngine.Utils.Misc
{
	public class QuadTree
	{
		public int MaxObjects { get; set; } = 3;
		public int MaxLevels { get; set; } = 5;
		internal List<PhysicalObject> Objects = new List<PhysicalObject>();
		internal QuadTree[] Nodes = new QuadTree[4];
		internal int Level;
		internal Rectangle Bounds;

		public QuadTree(int level, Rectangle bounds)
		{
			Level = level;
			Bounds = bounds;
		}

		public void Clear()
		{
			Objects.Clear();
			Nodes.Length.ForEach(i => Nodes[i] = null);
		}

		public void Split()
		{
			// width & height
			int subWidth = Bounds.Width/2;
			int subHeight = Bounds.Height/2;
			int x = Bounds.X;
			int y = Bounds.Y;
			// split to four nodes
			Nodes[0] = new QuadTree(Level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
			Nodes[1] = new QuadTree(Level, new Rectangle(x, y, subWidth, subHeight));
			Nodes[2] = new QuadTree(Level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
			Nodes[3] = new QuadTree(Level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
		}

		/**
		 * 获取rect 所在的 index
		 *
		 * @param rectF 传入对象所在的矩形
		 * @return index 使用类别区分所在象限
		 */

		internal int GetIndex(PhysicalObject rectF)
		{
			int index = -1;
			int verticalMidpoint = Bounds.X + Bounds.Width/2;
			int horizontalMidpoint = Bounds.Y + Bounds.Height/2;
			// contain top
			bool topQuadrant = rectF.Y < horizontalMidpoint && rectF.Height < horizontalMidpoint;
			// contain bottom
			bool bottomQuadrant = rectF.Y > horizontalMidpoint;
			// contain left
			if (rectF.X < verticalMidpoint && rectF.X + rectF.Width < verticalMidpoint)
			{
				if (topQuadrant)
				{
					index = 1;
				}
				else if (bottomQuadrant)
				{
					index = 2;
				}
			}
			// contain right
			else if (rectF.X > verticalMidpoint)
			{
				if (topQuadrant)
				{
					index = 0;
				}
				else if (bottomQuadrant)
				{
					index = 3;
				}
			}
			return index;
		}

		/**
		* insert object to tree
		*
		* @param rectF object
		*/

		public void Insert(PhysicalObject rectF)
		{
			if (Nodes[0] != null)
			{
				int index = GetIndex(rectF);
				if (index != -1)
				{
					Nodes[index]?.Insert(rectF);
					return;
				}
			}
			Objects.Add(rectF);
			if (Objects.Count > MaxObjects && Level < MaxLevels)
			{
				// don't have subNodes
				// split node
				if (Nodes[0] == null)
				{
					Split();
				}
				int i = 0;
				while (i < Objects.Count)
				{
					int index = GetIndex(Objects[i]);
					if (index != -1)
					{
						Nodes[index]?.Insert(Objects[i]);
						Objects.RemoveAt(i);
					}
					else
					{
						// don't in subNode save to parent node.
						// eq: object on line
						i++;
					}
				}
			}
		}

		/**
		* return all the object collision with the object
		 * @param returnObjects return list
		 * @param rectF         object
		 * @return list of collision
		 */

		public List<List<PhysicalObject>> Retrieve(List<List<PhysicalObject>> returnObjects, PhysicalObject rectF)
		{
			int index = GetIndex(rectF);
			if (index != -1 && Nodes[0] != null)
			{
				Nodes[index]?.Retrieve(returnObjects, rectF);
			}
			returnObjects.Add(Objects);
			return returnObjects;
		}
	}

}
