using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using AtriLib3.Interfaces;

namespace AtriLib3.Utility
{
    public class QuadTree
    {
        private int _level;
        private List<IWorldObject> _objects;
        private Rectangle _bounds;
        private QuadTree[] nodes;

        public QuadTree(int level, Rectangle bounds)
        {
            _level = level;
            _bounds = bounds;
            _objects = new List<IWorldObject>();
            nodes = new QuadTree[4];
        }

        /// <summary>
        /// Clear the QuadTree
        /// </summary>
        public void Clear()
        {
            _objects.Clear();

            for(int i = 0; i < nodes.Length; i++)
            {
                if(nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Split the node into 4 subnodes
        /// </summary>
        private void Split()
        {
            int subWidth = (int)(_bounds.Width / 2);
            int subHeight = (int)(_bounds.Height / 2);
            int x = _bounds.X;
            int y = _bounds.Y;

            nodes[0] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(_level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(_level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        //private List<int> GetIndexes(Rectangle rect)
        //{
        //    List<int> indexes = new List<int>();

        //    double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
        //    double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);

        //    bool topQuadrant = rect.Y >= horizontalMidpoint;
        //    bool bottomQuadrant = (rect.Y - rect.Height) <= horizontalMidpoint;
        //    bool topAndBottomQuadrant = rect.Y + rect.Height + 1 >= horizontalMidpoint && rect.Y + 1 <= horizontalMidpoint;

        //    if(topAndBottomQuadrant)
        //    {
        //        topQuadrant = false;
        //        bottomQuadrant = false;
        //    }

        //    if(rect.X + rect.Width + 1 >= verticalMidpoint && rect.X - 1 <= verticalMidpoint)
        //    {
        //        if(topQuadrant)
        //        {
        //            indexes.Add(2);
        //            indexes.Add(3);
        //        }
        //        else if(bottomQuadrant)
        //        {
        //            indexes.Add(0);
        //            indexes.Add(1);
        //        }
        //        else if(topAndBottomQuadrant)
        //        {
        //            indexes.Add(0);
        //            indexes.Add(1);
        //            indexes.Add(2);
        //            indexes.Add(3);
        //        }
        //    }
        //    else if(rect.X + 1 >= verticalMidpoint)
        //    {
        //        if(topQuadrant)
        //        {
        //            indexes.Add(3);
        //        }
        //        else if(bottomQuadrant)
        //        {
        //            indexes.Add(0);
        //        }
        //        else if(topAndBottomQuadrant)
        //        {
        //            indexes.Add(3);
        //            indexes.Add(0);
        //        }
        //    }
        //    else if(rect.X  - rect.Width <= verticalMidpoint)
        //    {
        //        if(topQuadrant)
        //        {
        //            indexes.Add(2);
        //        }
        //        else if(bottomQuadrant)
        //        {
        //            indexes.Add(1);
        //        }
        //        else if(topAndBottomQuadrant)
        //        {
        //            indexes.Add(2);
        //            indexes.Add(1);
        //        }
        //    }
        //    else
        //    {
        //        indexes.Add(-1);
        //    }

        //    return indexes;
        //}

        private List<int> GetIndexes(Rectangle rect)
        {
            List<int> retList = new List<int>();

            int index = -1;
            double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
            double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);

            bool topQuadrant = (rect.Y < horizontalMidpoint && rect.Y + rect.Height < horizontalMidpoint);
            bool bottomQuadrant = (rect.Y > horizontalMidpoint);

            if(rect.X < verticalMidpoint && rect.X + rect.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if(bottomQuadrant)
                {
                    index = 2;
                }
            }
            else if(rect.X > verticalMidpoint)
            {
                if(topQuadrant)
                {
                    index = 0;
                }
                else if(bottomQuadrant)
                {
                    index = 3;
                }
            }

            retList.Add(index);

            return retList;
        }

        public int Size()
        {
            return _objects.Count();
        }

        public List<IWorldObject> Retrieve(List<IWorldObject> objList, Rectangle rect)
        {
            List<int> indexes = GetIndexes(rect);

            for (int i = 0; i < indexes.Count; i++)
            {
                int idx = indexes[i];

                if(idx != -1 && nodes[0] != null)
                {
                    nodes[idx].Retrieve(objList, rect);
                }

                objList.AddRange(_objects);
            }

            return objList;
        }

        /// <summary>
        /// Add an object to the QuadTree
        /// </summary>
        /// <param name="rect">Object Rectangle</param>
        public void Insert(IWorldObject iWorldObj)
        {
            IWorldObject wo = iWorldObj;

            if(nodes[0] != null)
            {
                List<int> indexes = GetIndexes(wo.ObjectRectangle);

                for (int i = 0; i < indexes.Count; i++)
                {
                    int index = indexes[i];

                    if (index != -1)
                    {
                        nodes[index].Insert(wo);

                        return;
                    }
                }
            }

            _objects.Add(wo);

            if(_objects.Count > Constants.QUADTREE_MAX_OBJECTS && _level < Constants.QUADTREE_MAX_LEVELS)
            {
                if(nodes[0] == null)
                {
                    Split();
                }

                int i = 0;

                while(i < _objects.Count)
                {
                    var obj = _objects[i];
                    Rectangle rect = obj.ObjectRectangle;
                    List<int> indexes = GetIndexes(rect);

                    for(int j = 0; j < indexes.Count; j++)
                    {
                        int index = indexes[j];

                        if(index != -1)
                        {
                            nodes[index].Insert(obj);
                            _objects.Remove(obj);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
        }
    }
}
