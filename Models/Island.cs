using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace IslandGenerator.Models
{
    public class Island
    {
        public List<List<Cell>> island = new List<List<Cell>>();
        private List<String> alphabet = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        int islandSize = 20;

        public List<List<Cell>> Generate(int size)
        {
            islandSize = size;

            for (int r = 0; r < size; r++)
            {
                List<Cell> row = new List<Cell>();

                for (int c = 0; c < size; c++)
                {
                    Cell cell = new Cell("🌊", Cell.Type.SaltWater, alphabet[c] + r.ToString());
                    cell.row = r;
                    cell.column = c;
                    row.Add(cell);
                }

                island.Add(row);
            }

            FindNeighbours();
            return generateIsland();
        }

        public void FindNeighbours()
        {
            List<Cell> allCells = [];
            foreach (List<Cell> column in island)
            {
                foreach (Cell cell in column)
                {
                    allCells.Add(cell);
                }
            }

            foreach (Cell cell in allCells)
            {
                List<Cell> foundNeighbors = [];

                //Find left
                if (cell.row != 0)
                {
                    foreach (Cell cells in allCells)
                    {
                        if ((cells.row == (cell.row - 1)) && (cells.column == cell.column))
                        {
                            Debug.WriteLine("Left: " + cell.id + "  cell row:" + cell.row + "  cell neigbor row:" + cells.row + "  cell column:" + (cell.column - 1) + "  cell neigbor column:" + cells.column);
                            foundNeighbors.Add(cells);
                        }
                    }
                }

                //Find right
                if (cell.row < (islandSize - 1))
                {
                    foreach (Cell cells in allCells)
                    {
                        if ((cells.row == (cell.row + 1)) && (cells.column == cell.column))
                        {
                            Debug.WriteLine("Right: " + cell.id + "  cell row:" + cell.row + "  cell neigbor row:" + cells.row + "  cell column:" + (cell.column - 1) + "  cell neigbor column:" + cells.column);
                            foundNeighbors.Add(cells);
                        }

                    }
                }

                //Find up
                if (cell.column != 0)
                {
                    foreach (List<Cell> columns in island)
                    {
                        foreach (Cell cells in columns)
                        {
                            if ((cells.column == (cell.column - 1)) && (cells.row == cell.row))
                            {
                                Debug.WriteLine("Up: " + cell.id + "  cell row:" + cell.row + "  cell neigbor row:" + cells.row + "  cell column:" + (cell.column - 1) + "  cell neigbor column:" + cells.column);
                                foundNeighbors.Add(cells);
                            }
                        }
                    }
                }

                //Find down
                if (cell.column != (islandSize - 1))
                {
                    foreach (List<Cell> columns in island)
                    {
                        foreach (Cell cells in columns)
                        {
                            if ((cells.column == (cell.column + 1)) && (cells.row == cell.row))
                            {
                                Debug.WriteLine("Down: " + cell.id + "  cell row:" + cell.row + "  cell neigbor row:" + cells.row + "  cell column:" + (cell.column - 1) + "  cell neigbor column:" + cells.column);
                                foundNeighbors.Add(cells);
                            }
                        }
                    }
                }

                cell.neighbours = foundNeighbors;
                Debug.WriteLine(cell.id + " had the following neighbours " + foundNeighbors.Count);
            }
        }

        public List<List<Cell>> generateIsland()
        {
            Random random = new Random();

            foreach (List<Cell> column in island)
            {
                foreach (Cell cell in column)
                {
                    int range = 5;

                    //Get the center tiles of the map and give them a 25% chance to spawn lakes
                    if (cell.column >= (islandSize / 2 - range) && cell.column <= (islandSize / 2 + range) && cell.row >= (islandSize / 2 - range) && cell.row <= (islandSize / 2 + range))
                    {
                        if (random.Next(0, 100) > 75)
                        {
                            cell.icon = "💧";
                            cell.type = Cell.Type.FreshWater;
                        }
                    }
                }

                //Set Silt and Clay
                foreach (List<Cell> cells in island)
                {
                    foreach (Cell cell in cells)
                    {
                        if (cell.type == Cell.Type.FreshWater)
                        {
                            foreach (Cell neighbor in cell.neighbours)
                            {
                                if (neighbor.type != Cell.Type.FreshWater)
                                {
                                    //75% chance Silt
                                    if (random.Next(0, 100) > 25)
                                    {
                                        neighbor.type = Cell.Type.Silt;
                                        neighbor.icon = "🌱";
                                    }
                                    //25% chance Clay
                                    else
                                    {
                                        neighbor.type = Cell.Type.Clay;
                                        neighbor.icon = "🌸";
                                    }
                                }
                            }
                        }
                    }
                }

                //Set Loam
                foreach (List<Cell> cells in island)
                {
                    foreach (Cell cell in cells)
                    {
                        if (cell.type == Cell.Type.Silt || cell.type == Cell.Type.Clay)
                        {
                            foreach (Cell neighbors in cell.neighbours)
                            {
                                if (neighbors.type == Cell.Type.SaltWater)
                                {
                                    neighbors.type = Cell.Type.Loam;
                                    neighbors.icon = "🍄‍";
                                }
                            }
                        }
                    }
                }

                //Set Sand 
                foreach (List<Cell> cells in island)
                {
                    foreach (Cell cell in cells)
                    {
                        if (cell.type == Cell.Type.Loam)
                        {
                            foreach (Cell neighbors in cell.neighbours)
                            {
                                if (neighbors.type == Cell.Type.SaltWater)
                                {
                                    neighbors.type = Cell.Type.Sand;
                                    neighbors.icon = "🌴";
                                }
                            }
                        }

                    }
                }
            }

            return island;
        }
    }
}
