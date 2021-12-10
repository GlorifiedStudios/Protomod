﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoslynCSharp.Example
{
    class TestCrawler : MazeCrawler
    {
        private Stack<MazeDirection> searchPath = new Stack<MazeDirection>();
        private List<MazeDirection> available = new List<MazeDirection>();
        private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        public override MazeDirection DecideDirection(Vector2Int position, bool canMoveLeft, bool canMoveRight, bool canMoveUp, bool canMoveDown)
        {
            available.Clear();

            // Check left
            if (canMoveLeft == true)
                if (visited.Contains(position + new Vector2Int(-1, 0)) == false)
                    available.Add(MazeDirection.Left);

            // Check right
            if (canMoveRight == true)
                if (visited.Contains(position + new Vector2Int(1, 0)) == false)
                    available.Add(MazeDirection.Right);

            // Check up
            if (canMoveUp == true)
                if (visited.Contains(position + new Vector2Int(0, 1)) == false)
                    available.Add(MazeDirection.Up);

            // Check down
            if (canMoveDown == true)
                if (visited.Contains(position + new Vector2Int(0, -1)) == false)
                    available.Add(MazeDirection.Down);

            MazeDirection moveDirection = 0;
            bool isRetracing = false;

            // Check for dead end
            if(available.Count == 0)
            {
                if(searchPath.Count > 0)
                {
                    // Move backwards along this branch
                    moveDirection = searchPath.Pop();

                    // Invert the direction
                    if (moveDirection == MazeDirection.Left) moveDirection = MazeDirection.Right;
                    else if (moveDirection == MazeDirection.Right) moveDirection = MazeDirection.Left;
                    else if (moveDirection == MazeDirection.Up) moveDirection = MazeDirection.Down;
                    else if (moveDirection == MazeDirection.Down) moveDirection = MazeDirection.Up;

                    isRetracing = true;
                }
                else
                {
                    // Give up - we have not found a solution
                    enabled = false;
                    throw new Exception("Mouse crawler could not find a solution to the maze. Giving up...");
                }
            }
            else
            {
                // Select random available direction
                moveDirection = available[UnityEngine.Random.Range(0, available.Count)];
            }

            // Trace previous movements
            if(isRetracing == false)
                searchPath.Push(moveDirection);

            // Mark as visited
            if (visited.Contains(position) == false)
            {
                visited.Add(position);
            }

            return moveDirection;
        }
    }
}
