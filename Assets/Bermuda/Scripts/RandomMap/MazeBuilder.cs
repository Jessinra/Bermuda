using System;
using System.Collections.Generic;

public class MazeBuilder {

    private Maze maze;
    private MazeBlueprint mazeBlueprint;

    private int mazeHeight = 0;
    private int mazeWidth = 0;

    private List<PathDigger> pathDiggers = new List<PathDigger>();
    private List<HallDigger> hallDiggers = new List<HallDigger>();
    private List<EdgeDigger> edgeDiggers = new List<EdgeDigger>();

    public MazeBuilder() { }

    public void generateMaze(int height, int width) {
        this.maze = new Maze(height, width);
        this.mazeHeight = height;
        this.mazeWidth = width;
    }

    public void addPathDigger(int numberOfDigger) {
        for (int i = 0; i < numberOfDigger; i++) {
            PathDigger digger = new PathDigger(this.maze);
            this.pathDiggers.Add(digger);
        }
    }

    public void addHallDigger(int numberOfDigger) {
        for (int i = 0; i < numberOfDigger; i++) {
            HallDigger digger = new HallDigger(this.maze);
            this.hallDiggers.Add(digger);
        }
    }

    public void addEdgeDigger(int numberOfDigger) {
        for (int i = 0; i < numberOfDigger; i++) {
            EdgeDigger digger = new EdgeDigger(this.maze);
            this.edgeDiggers.Add(digger);
        }
    }

    public void runPathDigger(float percentage) {
        int stepRequired = (int) (this.mazeWidth * this.mazeHeight * percentage);
        int step = 0;

        while (step < stepRequired) {
            foreach (PathDigger digger in this.pathDiggers) {
                digger.move();
                step++;
            }
        }
    }

    public void runHallDigger(float percentage) {
        int stepRequired = (int) (this.mazeWidth * this.mazeHeight * percentage);
        int step = 0;

        while (step < stepRequired) {
            foreach (HallDigger digger in this.hallDiggers) {
                digger.move();
                step++;
            }
        }
    }

    public void runEdgeDigger(float percentage) {
        int stepRequired = (int) (this.mazeWidth * this.mazeHeight * percentage);
        int step = 0;

        while (step < stepRequired) {
            foreach (EdgeDigger digger in this.edgeDiggers) {
                digger.move();
                step++;
            }
        }
    }

    public void expandMaze(int expandMultiplier = 2) {

        List<List<String>> expandedPlayArea = new List<List<String>>();

        // for each original row
        for (int i = 0; i < this.mazeHeight; i++) {

            List<String> newRow = new List<String>();

            // for each original column
            for (int j = 0; j < this.mazeWidth; j++) {
                String tile = this.maze.getTile(i, j);

                // Add tile col(repeat n many times)
                for (int k = 0; k < expandMultiplier; k++) {
                    newRow.Add(tile);
                }
            }
            // Add tile row(repeat n many times)
            for (int l = 0; l < expandMultiplier; l++) {
                expandedPlayArea.Add(newRow);
            }
        }
        this.maze.setPlayArea(expandedPlayArea);
        this.mazeHeight *= expandMultiplier;
        this.mazeWidth *= expandMultiplier;
        this.maze.setHeight(this.mazeHeight);
        this.maze.setWidth(this.mazeWidth);
    }

    public void generateBluePrint() {

        mazeBlueprint = new MazeBlueprint(this.maze.getHeight(), this.maze.getWidth());
        List<List<String>> playAreaBlueprint = new List<List<String>>();

        for (int i = 0; i < this.mazeHeight; i++) {

            List<String> newRow = new List<String>();
            for (int j = 0; j < this.mazeWidth; j++) {

                newRow.Add(this.maze.getTileType(i, j));
            }
            playAreaBlueprint.Add(newRow);
        }
        this.mazeBlueprint.setPlayAreaBlueprint(playAreaBlueprint);
    }

    public Maze checkoutMaze() {
        return this.maze;
    }

    public MazeBlueprint checkoutBlueprint() {
        return this.mazeBlueprint;
    }
}