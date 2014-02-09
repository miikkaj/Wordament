using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheat
{
    class GridSearch
    {
        struct GridPoint
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        readonly string[] grid;

        public GridSearch( string[] letters )
        {
            if ( letters == null || letters.Length != 16 )
            {
                throw new ArgumentException( "GridSearch requires the 4x4 wordament grid" );
            }
            this.grid = letters;
        }

        /// <summary>
        /// Finds if the word can legitimatly be found on the grid (every letter adjacent to another)
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true if the word can be made on the grid, false otherwise</returns>
        public bool TryFindWord( string word )
        {
            // Get all possible start points
            foreach ( GridPoint point in GetPossibleStartPoints( word ) )
            {
                List<GridPoint> points = new List<GridPoint>();
                points.Add( point );

               if ( FindWord( word, point, points, GetLetterFromPoint( point ) ) )
               {
                    return true;
               }
            }

            return false;
        }

        /// <summary>
        /// Recursive function
        /// </summary>
        /// <param name="wholeWord">The entire word from the dictionary that we are trying to find</param>
        /// <param name="currentPoint">current position on the grid</param>
        /// <param name="visitedPoints">points we have passed through to get to this point (including current point)</param>
        /// <param name="currentWord">word that has been build up by travelling through the list so far
        /// I could recreate it from the points list instead but this seemed like it would work better after changing
        /// this to handle the special wordament rules later</param>
        /// <returns>true if it found the word, false otherwise</returns>
        bool FindWord( string wholeWord, GridPoint currentPoint, List<GridPoint> visitedPoints, string currentWord )
        {
            if ( wholeWord.Equals( currentWord ) )
            {
                return true;
            }

            if ( !wholeWord.StartsWith( currentWord ) )
            {
                return false;
            }

            // Get the list of leaves from this node, start with 8 surrounding nodes and reject the ones which are out of range
            var nextPoints = new List<GridPoint>();

            for ( int x = currentPoint.x - 1; x <= currentPoint.x + 1; x++ )
            {
                for ( int y = currentPoint.y - 1; y <= currentPoint.y + 1; y++ )
                {
                    if ( !( x == currentPoint.x && y == currentPoint.y ) &&
                        x >= 0 &&
                        x <= 3 &&
                        y >= 0 &&
                        y <= 3 )
                    {
                        nextPoints.Add(
                            new GridPoint
                            {
                                x = x,
                                y = y
                            } );
                    }
                }
            }

            // remove the nodes we have already visited
            nextPoints = nextPoints.Except( visitedPoints ).ToList<GridPoint>(); 

            // we now have a possible list, go through them. If one of them returns true return
            foreach ( var point in nextPoints )
            {
                string runnerWord = currentWord + GetLetterFromPoint( point );
                List<GridPoint> runnerList = new List<GridPoint>( visitedPoints );
                runnerList.Add( point );

                if ( FindWord( wholeWord, point, runnerList, runnerWord ) )
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Given the word that you are searching for, Get a list of all the possible 
        /// starting points for that word on the grid. For now I'm ignoring the special
        /// cases (pro-, a/n etc.)
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private List<GridPoint> GetPossibleStartPoints( string word )
        {
            List<GridPoint> startPoints = new List<GridPoint>();

            for ( int i = 0; i < grid.Length; i++ )
            {
                if ( word.StartsWith( grid[i] ) )
                {
                    startPoints.Add( GetGridPointFromIndex( i ) );
                }
            }

            return startPoints;
        }


        /// <summary>
        /// Get the string from the grid (translate from 2 dimensional grid to 1 dimensional array)
        /// </summary>
        /// <param name="point">point on the grid</param>
        /// <returns>string at that point</returns>
        private string GetLetterFromPoint( GridPoint point )
        {
            return grid[ point.y * 4 + point.x ];
        }

        /// <summary>
        /// Convert from the 1 dimensional array to the 2 dimensional grid
        /// </summary>
        /// <param name="index">point in the array</param>
        /// <returns>point on the grid</returns>
        private GridPoint GetGridPointFromIndex( int index )
        {
            return new GridPoint
            {
                x = index % 4,
                y = index / 4
            };
        }
    }
}
