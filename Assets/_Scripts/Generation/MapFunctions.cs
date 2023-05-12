using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctions
{
    /// <summary>
    /// Generates an int array of the supplied width and height
    /// </summary>
    /// <param name="width">How wide you want the array</param>
    /// <param name="height">How high you want the array</param>
    /// <returns>The map array initialised</returns>
    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];



        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    /// <summary>
    /// Draws the map to the screen
    /// </summary>
    /// <param name="map">Map that we want to draw</param>
    /// <param name="tilemap">Tilemap we will draw onto</param>
    /// <param name="tile">Tile we will draw with</para
    /// <param name="tileSpawner">Tile that will be used as a spawnPoint</param>

    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile, TileBase tileSpawner)
    {
        //Adding padding to the map 
        for (int x = (int)(map.GetUpperBound(0) * (-0.39)); x < map.GetUpperBound(0) * 1.39; x++)
        {
            for (int y = (int)(map.GetUpperBound(1) * (-0.39)); y < map.GetUpperBound(1) * 1.39; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        //Calc spots for spawners???




        for (int x = 0; x < map.GetUpperBound(0); x++) //Loop through the width of the map
        {
            for (int y = 0; y < map.GetUpperBound(1); y++) //Loop through the height of the map
            {
                if (map[x, y] == 0) // 1 = tile, 0 = no tile,
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }

               else if (map[x, y] == 2 )
               {
                   tilemap.SetTile(new Vector3Int(x, y, 0), tileSpawner);
             }
            }
        }
    }

    /// <summary>
    /// Renders a map using an offset provided, Useful for having multiple maps on one tilemap
    /// </summary>
    /// <param name="map">The map to draw</param>
    /// <param name="tilemap">The tilemap to draw on</param>
    /// <param name="tile">The tile to draw with</param>
    /// <param name="offset">The offset to apply</param>
    public static void RenderMapWithOffset(int[,] map, Tilemap tilemap, TileBase tile, Vector2Int offset)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), tile);
                }
            }
        }
    }


    /// <summary>
    /// Renders the map but with a delay, this allows us to see it being generated before our eyes
    /// </summary>
    /// <param name="map">The map to draw</param>
    /// <param name="tilemap">The tilemap to draw on</param>
    /// <param name="tile">The tile to draw with</param>
    public static IEnumerator RenderMapWithDelay(int[,] map, Tilemap tilemap, TileBase tile)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Same as the Render function but only removes tiles
    /// </summary>
    /// <param name="map">Map that we want to draw</param>
    /// <param name="tilemap">Tilemap we want to draw onto</param>
    public static void UpdateMap(int[,] map, Tilemap tilemap)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                //We are only going to update the map, rather than rendering again
                //This is because it uses less resources to update tiles to null
                //As opposed to re-drawing every single tile (and collision data)
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);

                }
            }
        }
    }
    /// <summary>
    /// Used to create a new cave using the Random Walk Algorithm. Doesn't exit out of bounds.
    /// </summary>
    /// <param name="map">The array that holds the map information</param>
    /// <param name="seed">The seed for the random</param>
    /// <param name="requiredFloorPercent">The amount of floor we want</param>
    /// <returns>The modified map array</returns>
    public static int[,] RandomWalkCave(int[,] map, float seed, int requiredFloorPercent)
    {
        //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

        //Define our start x position
        int floorX = rand.Next(1, map.GetUpperBound(0) - 1);
        //Define our start y position
        int floorY = rand.Next(1, map.GetUpperBound(1) - 1);
        //Determine our required floorAmount
        int reqFloorAmount = ((map.GetUpperBound(1) * map.GetUpperBound(0)) * requiredFloorPercent) / 100;
        //Used for our while loop, when this reaches our reqFloorAmount we will stop tunneling
        int floorCount = 0;

        //Set our start position to not be a tile (0 = no tile, 1 = tile)
        map[floorX, floorY] = 0;
        //Increase our floor count
        floorCount++;

        while (floorCount < reqFloorAmount)
        {
            //Determine our next direction
            int randDir = rand.Next(4);

            switch (randDir)
            {
                case 0: //Up
                    //Ensure that the edges are still tiles
                    if ((floorY + 1) < map.GetUpperBound(1) - 1)
                    {
                        //Move the y up one
                        floorY++;

                        //Check if that piece is currently still a tile
                        if (map[floorX, floorY] == 1)
                        {
                            //Change it to not a tile
                            map[floorX, floorY] = 0;
                            //Increase floor count
                            floorCount++;
                        }
                    }
                    break;
                case 1: //Down
                    //Ensure that the edges are still tiles
                    if ((floorY - 1) > 1)
                    {
                        //Move the y down one
                        floorY--;
                        //Check if that piece is currently still a tile
                        if (map[floorX, floorY] == 1)
                        {
                            //Change it to not a tile
                            map[floorX, floorY] = 0;
                            //Increase the floor count
                            floorCount++;
                        }
                    }
                    break;
                case 2: //Right
                    //Ensure that the edges are still tiles
                    if ((floorX + 1) < map.GetUpperBound(0) - 1)
                    {
                        //Move the x to the right
                        floorX++;
                        //Check if that piece is currently still a tile
                        if (map[floorX, floorY] == 1)
                        {
                            //Change it to not a tile
                            map[floorX, floorY] = 0;
                            //Increase the floor count
                            floorCount++;
                        }
                    }
                    break;
                case 3: //Left
                    //Ensure that the edges are still tiles
                    if ((floorX - 1) > 1)
                    {
                        //Move the x to the left
                        floorX--;
                        //Check if that piece is currently still a tile
                        if (map[floorX, floorY] == 1)
                        {
                            //Change it to not a tile
                            map[floorX, floorY] = 0;
                            //Increase the floor count
                            floorCount++;
                        }
                    }
                    break;
            }
        }
        //Return the updated map
        //return FindSpawnerPoints(map);
        return map;    
    }

    /// <summary>
    /// Same as the Render function but finds spawners and add them in correct spots
    /// </summary>
    /// <param name="map">Map that we want to draw</param>
    
    public static int[,] FindSpawnerPoints(int[,] map)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                bool temp = CheckSurroundedByZeroes(x,y,map);
                if (temp)
                {
                    map[x, y] = 2;
                }
            }
        }

        return map;
    }
   static private bool CheckSurroundedByZeroes(int x, int y,int[,] map)
    {
        int numRows = map.GetUpperBound(0);
        int numCols = map.GetUpperBound(1);

        // Check if the element is at a border position
        if (x == 0 || y == 0 || x == numRows - 1 || y == numCols - 1)
        {
            return false;
        }

        // Check if the element is surrounded by zeroes
        return map[x - 1, y] == 0 && map[x + 1, y] == 0 && map[x, y - 1] == 0 && map[x, y + 1] == 0;
    }
}