using System;
using System.Collections.Generic;
using System.IO;

public class MazeBlueprint {

    private int mazeHeight;
    private int mazeWidth;
    public List<List<String>> playAreaBlueprint = new List<List<String>>();

    public MazeBlueprint(int mazeHeight, int mazeWidth) {
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;
    }

    public int getMazeHeight() {
        return this.mazeHeight;
    }

    public int getMazeWidth() {
        return this.mazeWidth;
    }

    public String getTileType(int row, int col) {
        return this.playAreaBlueprint[row][col];
    }

    public void setPlayAreaBlueprint(List<List<String>> playAreaBlueprint) {
        this.playAreaBlueprint = playAreaBlueprint;
    }

    public void printBlueprint(String filename) {
        System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
        file.WriteLine("\n\n\n");

        for (int i = this.getMazeHeight() - 1; i >= 0; i--) {
            List<String> row = this.playAreaBlueprint[i];
            file.Write("\t\t");
            foreach (String col in row) {
                file.Write(" " + col + " ");
            }
            file.Write("\n");
        }
        file.WriteLine("\n\n\n");
        file.Close();
    }
}