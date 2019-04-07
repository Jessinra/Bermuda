using System;
using System.Collections.Generic;
using System.IO;

public class Maze {

    public static String wallSymbol = "@";

    private int height;
    private int width;
    public List<List<String>> playArea = new List<List<String>>();

    public Maze(int height, int width) {
        this.height = height;
        this.width = width;
        this.createPlayArea();
    }

    /* =================================================
                        Getter Setter
    ================================================= */
    
    public String getTile(int row, int col) {
        try {
            return this.playArea[row][col];
        } catch (Exception) {
            return null;
        }
    }

    public String getTileType(int row, int col) {
        if (isTileWall(row, col)) {
            return "Wall";
        }

        /*
            Grid for checking wall relative to current tile (x)
                | 1 | 2 | 3 |
                | 4 | x | 6 |
                | 7 | 8 | 9 |
        */

        // Unity top and bottom is reversed ,, normal cartesian
        bool isWall01 = this.isTileWall(row + 1, col - 1);
        bool isWall02 = this.isTileWall(row + 1, col + 0);
        bool isWall03 = this.isTileWall(row + 1, col + 1);
        bool isWall04 = this.isTileWall(row + 0, col - 1);
        bool isWall06 = this.isTileWall(row + 0, col + 1);
        bool isWall07 = this.isTileWall(row - 1, col - 1);
        bool isWall08 = this.isTileWall(row - 1, col + 0);
        bool isWall09 = this.isTileWall(row - 1, col + 1);

        /* =====   Check for corner tile   ===== */
        if (isWall08 && isWall06) {
            return "CornerTopLeft";
        }
        if (isWall08 && isWall04) {
            return "CornerTopRight";
        }
        if (isWall02 && isWall06) {
            return "CornerBottomLeft";
        }
        if (isWall02 && isWall04) {
            return "CornerBottomRight";
        }

        /* =====   Check for edge tile   ===== */
        if (isWall09 && !isWall08 && !isWall06) {
            return "EdgeTopLeft";
        }
        if (isWall07 && !isWall08 && !isWall04) {
            return "EdgeTopRight";
        }
        if (isWall03 && !isWall02 && !isWall06) {
            return "EdgeBottomLeft";
        }
        if (isWall01 && !isWall02 && !isWall04) {
            return "EdgeBottomRight";
        }

        /* =====    Check for side tile   ===== */
        if (isWall08) {
            return "SideTop";
        }
        if (isWall02) {
            return "SideBottom";
        }
        if (isWall06) {
            return "SideLeft";
        }
        if (isWall04) {
            return "SideRight";
        }

        return "Empty";
    }

    public int getHeight() {
        return this.height;
    }

    public int getWidth() {
        return this.width;
    }

    public List<List<String>> getPlayArea(){
        return this.playArea;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    public void setPlayArea(List<List<String>> playArea) {
        this.playArea = playArea;
    }

    /* =================================================
                        Member Method
    ================================================= */

    private bool isTileEmpty(int row, int col) {
        return !(this.isTileWall(row, col));
    }

    private bool isTileWall(int row, int col) {
        return this.getTile(row, col) == Maze.wallSymbol;
    }

    private void createPlayArea() {
        List<String> row;

        for (int i = 0; i < this.height; i++) {
            row = new List<String>();
            for (int j = 0; j < this.width; j++) {
                row.Add(Maze.wallSymbol);
            }
            this.playArea.Add(row);
        }
    }

    /* =================================================
                        Debug Area
    ================================================= */

    public void printMaze(String filename) {
        System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
        file.WriteLine("\n\n\n");

        for (int i = this.getHeight() - 1; i >= 0; i--) {

            List<String> row = this.playArea[i];

            file.Write("\t\t");
            foreach (String col in row) {
                file.Write(col);
            }
            file.Write("\n");
        }

        file.WriteLine("\n\n\n");
        file.Close();
    }

    public void printStatistic(String filename) {
        int wall = 0;
        foreach (List<String> row in this.playArea) {
            foreach (String col in row) {
                if (col == Maze.wallSymbol) {
                    wall++;
                }
            }
        }
        int space = width * height - wall;

        System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
        file.WriteLine("Statistic : ");
        file.WriteLine("height: " + height);
        file.WriteLine("width: " + width);
        file.WriteLine("wall: " + wall);
        file.WriteLine("space: " + space);
        file.WriteLine("percentage space: " + (space / (wall + space)));
        file.Close();

    }
}