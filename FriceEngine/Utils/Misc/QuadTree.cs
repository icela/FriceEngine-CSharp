using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FriceEngine.Object;

namespace FriceEngine.Utils.Misc
{
	public class QuadTree
	{
		public int MaxObjects { get; set; } = 10;
		public int MaxLevels { get; set; } = 10;
		internal List<PhysicalObject> Objects = new List<PhysicalObject>();
		internal QuadTree[] Nodes = new QuadTree[4];
		internal int Level;
		internal Rectangle Bounds;

		public QuadTree(Rectangle bounds, int level = 0)
		{
			Level = level;
			Bounds = bounds;
		}

		public void Clear()
		{
			Objects.Clear();
			Nodes.Length.ForEach(i =>
			{
				if (Nodes[i] != null)
				{
					Nodes[i].Clear();
					Nodes[i] = null;
				}
			});
		}

		public void Split()
		{
			int subWidth = Bounds.Width/2;
			int subHeight = Bounds.Height/2;
			int x = Bounds.X;
			int y = Bounds.Y;
			Nodes[0] = new QuadTree(new Rectangle(x + subWidth, y, subWidth, subHeight), Level + 1);
			Nodes[1] = new QuadTree(new Rectangle(x, y, subWidth, subHeight), Level + 1);
			Nodes[2] = new QuadTree(new Rectangle(x, y + subHeight, subWidth, subHeight), Level + 1);
			Nodes[3] = new QuadTree(new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight), Level + 1);
		}

		internal int GetIndex(PhysicalObject rectF)
		{
			int index = -1;
			int verticalMidpoint = Bounds.X + Bounds.Width/2;
			int horizontalMidpoint = Bounds.Y + Bounds.Height/2;
			bool topQuadrant = rectF.Y < horizontalMidpoint && rectF.Height < horizontalMidpoint;
			bool bottomQuadrant = rectF.Y > horizontalMidpoint;
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
						i++;
					}
				}
			}
		}

		public void Insert(IEnumerable<PhysicalObject> physicalObjects)
		{
			physicalObjects.ForEach(Insert);
		}

		public List<PhysicalObject> Retrieve(List<PhysicalObject> returnObjects, PhysicalObject rectF)
		{
			int index = GetIndex(rectF);
			if (index != -1 && Nodes[0] != null)
			{
				Nodes[index]?.Retrieve(returnObjects, rectF);
			}
			Objects.ForEach(returnObjects.Add);
			return returnObjects;
		}
	}

}
