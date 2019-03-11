using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeDigger {
    private static String visitedMark = " ";
    protected static Maze maze = null;
    protected static System.Random randomGenerator = new System.Random();

    private int row;
    private int col;
    private Action lastMove;
    private int changeDirectionChance;

    public MazeDigger(Maze maze) {

        if (MazeDigger.maze == null) {
            MazeDigger.maze = maze;
        }
    }

    public void setStartPosition(int row, int col) {
        this.row = row;
        this.col = col;
    }

    public void setChangeDirectionChance(int chance) {
        this.changeDirectionChance = chance;
    }

    protected void mark() {
        MazeDigger.maze.playArea[this.row][this.col] = MazeDigger.visitedMark;
    }

    protected bool hasVisited(int row, int col) {
        return MazeDigger.maze.playArea[row][col] == MazeDigger.visitedMark;
    }

    protected void moveLeft() {
        if (this.col > 0) {
            this.col--;
            this.lastMove = this.moveLeft;
        }
    }

    protected void moveRight() {
        if (this.col < MazeDigger.maze.getWidth() - 1) {
            this.col++;
            this.lastMove = this.moveRight;
        }
    }

    protected void moveUp() {
        if (this.row > 0) {
            this.row--;
            this.lastMove = this.moveUp;
        }
    }

    protected void moveDown() {
        if (this.row < MazeDigger.maze.getHeight() - 1) {
            this.row++;
            this.lastMove = this.moveDown;
        }
    }

    public void move() {

        if ((this.lastMove == null) || (this.allowedRandomMove())) {
            this.randomMove();
        } else {
            this.lastMove();
        }

        this.mark();
    }

    public bool allowedRandomMove() {
        return (MazeDigger.randomGenerator.Next(0, 100) < this.changeDirectionChance);
    }

    public void randomMove() {

        try {
            List<Action> movement = this.getPreferredDirection();

            int randInt = MazeDigger.randomGenerator.Next(movement.Count);
            Action randomMovement = movement[randInt];
            randomMovement();

        } catch (Exception e) {
            throw e;
        }
    }

    private List<Action> getPreferredDirection() {

        List<Action> moveDirection = new List<Action>();
        List<Action> possibleDirection = new List<Action>();

        if (this.col > 0) {
            if (!(this.hasVisited(this.row, this.col - 1))) {
                moveDirection.Add(this.moveLeft);
            }
            possibleDirection.Add(this.moveLeft);
        }
        if (this.col < MazeDigger.maze.getWidth() - 1) {
            if (!(this.hasVisited(this.row, this.col + 1))) {
                moveDirection.Add(this.moveRight);
            }
            possibleDirection.Add(this.moveRight);
        }
        if (this.row > 0) {
            if (!(this.hasVisited(this.row - 1, this.col))) {
                moveDirection.Add(this.moveUp);
            }
            possibleDirection.Add(this.moveUp);
        }
        if (this.row < MazeDigger.maze.getHeight() - 1) {
            if (!(this.hasVisited(this.row + 1, this.col))) {
                moveDirection.Add(this.moveDown);
            }
            possibleDirection.Add(this.moveDown);
        }
        if (moveDirection.Count == 0) {
            moveDirection = possibleDirection;
        }

        return moveDirection;
    }
}

public class PathDigger : MazeDigger {

    private int changeDirectionChance = MazeDigger.randomGenerator.Next(40, 80);

    public PathDigger(Maze maze) : base(maze) {

        this.setInitPosition();
        setChangeDirectionChance(this.changeDirectionChance);
    }

    public void setInitPosition() {
        int mazeHeight = MazeDigger.maze.getHeight();
        int mazeWidth = MazeDigger.maze.getWidth();

        // Randomly placed in the middle 80 % of the map
        int row = MazeDigger.randomGenerator.Next((int) (mazeHeight * 0.1), (int) (mazeHeight * 0.9));
        int col = MazeDigger.randomGenerator.Next((int) (mazeWidth * 0.1), (int) (mazeWidth * 0.9));
        this.setStartPosition(row, col);
    }
}

public class HallDigger : MazeDigger {

    private int changeDirectionChance = MazeDigger.randomGenerator.Next(80, 130);

    public HallDigger(Maze maze) : base(maze) {

        this.setInitPosition();
        setChangeDirectionChance(this.changeDirectionChance);
    }

    public void setInitPosition() {
        int mazeHeight = MazeDigger.maze.getHeight();
        int mazeWidth = MazeDigger.maze.getWidth();

        // Randomly placed in the middle 40 % of the map
        int row = MazeDigger.randomGenerator.Next((int) (mazeHeight * 0.3), (int) (mazeHeight * 0.7));
        int col = MazeDigger.randomGenerator.Next((int) (mazeWidth * 0.3), (int) (mazeWidth * 0.7));
        this.setStartPosition(row, col);
    }
}

public class EdgeDigger : MazeDigger {

    private int changeDirectionChance = MazeDigger.randomGenerator.Next(80, 130);

    public EdgeDigger(Maze maze) : base(maze) {

        this.setInitPosition();
        setChangeDirectionChance(this.changeDirectionChance);
    }

    public void setInitPosition() {
        int mazeHeight = MazeDigger.maze.getHeight();
        int mazeWidth = MazeDigger.maze.getWidth();

        int mazeEdge = MazeDigger.randomGenerator.Next(0, 3);
        int row = 0;
        int col = 0;

        // Top side
        if (mazeEdge == 0) { //Randomly placed in the top edge 10 % of the map
            row = MazeDigger.randomGenerator.Next(0, (int) (mazeHeight * 0.05));
            col = MazeDigger.randomGenerator.Next(0, mazeWidth - 1);
        }
        // Bottom side
        else if (mazeEdge == 1) { //Randomly placed in the bottom edge 10 % of the map
            row = MazeDigger.randomGenerator.Next((int) (mazeHeight * 0.95), mazeHeight - 1);
            col = MazeDigger.randomGenerator.Next(0, mazeWidth - 1);
        }
        // Left side
        else if (mazeEdge == 2) { //Randomly placed in the left edge 10 % of the map
            row = MazeDigger.randomGenerator.Next(0, mazeHeight - 1);
            col = MazeDigger.randomGenerator.Next(0, (int) (mazeWidth * 0.05));
        }
        // Right side
        else if (mazeEdge == 3) { //Randomly placed in the right edge 10 % of the map
            row = MazeDigger.randomGenerator.Next(0, mazeHeight - 1);
            col = MazeDigger.randomGenerator.Next((int) (mazeWidth * 0.95), mazeWidth - 1);
        }
        this.setStartPosition(row, col);
    }
}