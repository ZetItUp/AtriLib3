using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AtriLib3.Utility
{
    public class MapData
    {
        public Vector2 Position;
        public bool Walkable;

        public MapData()
        {

        }
    }

    public class SearchNode
    {
        public Point Position;
        public bool Walkable;
        public SearchNode[] Neighbours;
        public SearchNode Parent;
        public bool InOpenList;
        public bool InClosedList;
        public float DistanceToGoal;
        public float DistanceTraveled;
    }

    public class AStar
    {
        private SearchNode[,] searchNodes;
        private int levelWidth;
        private int levelHeight;
        private int _tileWidth;
        private int _tileHeight;

        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapData">AMapData[maxX, maxY]</param>
        /// <param name="LevelWidth">Level Width</param>
        /// <param name="LevelHeight">Level Height</param>
        public AStar(MapData[,] mapData, int LevelWidth, int LevelHeight, int tileWidth, int tileHeight)
        {
            levelWidth = LevelWidth;
            levelHeight = LevelHeight;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;

            InitializeSearchNodes(mapData);
        }

        /// <summary>
        /// Returns the distance between two points. (H value)
        /// </summary>
        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                Math.Abs(point1.Y - point2.Y);
        }

        /// <summary>
        /// Splits our level up into a grid of nodes.
        /// </summary>
        private void InitializeSearchNodes(MapData[,] mapData)
        {
            searchNodes = new SearchNode[levelWidth, levelHeight];

            // Create Search Nodes
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = new SearchNode();
                    node.Position = new Point(x, y);

                    node.Walkable = mapData[x, y].Walkable;

                    if (node.Walkable == true)
                    {
                        node.Neighbours = new SearchNode[8];
                        searchNodes[x, y] = node;
                    }
                }
            }

            // Connect each node to it's neighbours
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    // Only look for walkable nodes
                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    // An array of all possible neighbours this node could have
                    Point[] neighbours = new Point[]
                    {
                        new Point(x, y - 1), // Node above the current
                        new Point(x, y + 1), // Node below the current
                        new Point(x - 1, y), // Node to the left of current
                        new Point(x + 1, y), // Node to the right of current

                        // Diagonal
                        new Point(x - 1, y - 1), // Top Left Node
                        new Point(x + 1, y - 1), // Top Right Node
                        new Point(x - 1, y + 1), // Bottom Left Node
                        new Point(x + 1, y + 1), // Bottom Right Node
                    };

                    // Loop through each neighbour node
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        Point position = neighbours[i];

                        // Is the node part of the map?
                        if (position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        SearchNode neighbour = searchNodes[position.X, position.Y];

                        // We only bother keeping a ref to the nodes that can be walked on
                        if (neighbour == null || neighbour.Walkable == false)
                        {
                            continue;
                        }

                        // Store a ref to the neighbour
                        node.Neighbours[i] = neighbour;
                    }
                }
            }
        }

        /// <summary>
        /// Resets the state of the search nodes.
        /// </summary>
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Use the parent field of the search nodes to trace a path from the end
        /// node to the start node.
        /// </summary>
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Position.X * _tileWidth,
                                          closedList[i].Position.Y * _tileHeight));
            }

            return finalPath;
        }

        /// <summary>
        /// Returns the node with the smallest distance to goal.
        /// </summary>
        private SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }

            return currentTile;
        }

        /// <summary>
        /// Finds the optimal path from one point to another
        /// </summary>
        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            // Only try to find a path if the start and end points are different
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];

            if (startNode == null || endNode == null)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Step 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                SearchNode currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.Neighbours.Length; i++)
                {
                    SearchNode neighbor = currentNode.Neighbours[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + the estimated 
                        //     distance between the neighbouring node and goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found!
            return new List<Vector2>();
        }
    }
}
