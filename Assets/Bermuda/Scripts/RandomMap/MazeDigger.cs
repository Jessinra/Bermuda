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
    
    public void move() {

        if ((this.lastMove == null) || (this.allowedRandomMove())) {
            this.randomMove();
        } else {
            this.lastMove();
        }

        this.mark();
    }

    /* =================================================
                        Setter
    ================================================= */

    protected void setStartPosition(int row, int col) {
        this.row = row;
        this.col = col;
    }

    protected void setChangeDirectionChance(int chance) {
        this.changeDirectionChance = chance;
    }

    /* =================================================
                        Member Method
    ================================================= */

    private void mark() {
        List<List<String>> playArea = MazeDigger.maze.getPlayArea();
        playArea[this.row][this.col] = MazeDigger.visitedMark;
    }

    private bool hasVisited(int row, int col) {
        List<List<String>> playArea = MazeDigger.maze.getPlayArea();
        return playArea[row][col] == MazeDigger.visitedMark;
    }

    private void moveLeft() {
        if (ableToMoveLeft()) {
            this.col--;
            this.lastMove = this.moveLeft;
        }
    }

    private void moveRight() {
        if (ableToMoveRight()) {
            this.col++;
            this.lastMove = this.moveRight;
        }
    }

    private void moveUp() {
        if (ableToMoveUp()) {
            this.row--;
            this.lastMove = this.moveUp;
        }
    }

    private void moveDown() {
        if (ableToMoveDown()) {
            this.row++;
            this.lastMove = this.moveDown;
        }
    }

    private bool allowedRandomMove() {
        return (MazeDigger.randomGenerator.Next(0, 100) < this.changeDirectionChance);
    }

    private void randomMove() {

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

        if (ableToMoveLeft()) {
            if (hasNotVisitedLeft()) {
                moveDirection.Add(this.moveLeft);
            }
            possibleDirection.Add(this.moveLeft);
        }
        if (ableToMoveRight()) {
            if (hasNotVisitedRight()) {
                moveDirection.Add(this.moveRight);
            }
            possibleDirection.Add(this.moveRight);
        }
        if (ableToMoveUp()) {
            if (hasNotVisitedUp()) {
                moveDirection.Add(this.moveUp);
            }
            possibleDirection.Add(this.moveUp);
        }
        if (ableToMoveDown()) {
            if (hasNotVisitedDown()) {
                moveDirection.Add(this.moveDown);
            }
            possibleDirection.Add(this.moveDown);
        }
        if (moveDirection.Count == 0) {
            moveDirection = possibleDirection;
        }

        return moveDirection;
    }

    private bool ableToMoveLeft() {
        return this.col > 0;
    }

    private bool ableToMoveRight() {
        return this.col < MazeDigger.maze.getWidth() - 1;
    }

    private bool ableToMoveUp() {
        return this.row > 0;
    }

    private bool ableToMoveDown() {
        return this.row < MazeDigger.maze.getHeight() - 1;
    }

    private bool hasNotVisitedLeft() {
        return !(this.hasVisited(this.row, this.col - 1));
    }

    private bool hasNotVisitedRight() {
        return !(this.hasVisited(this.row, this.col + 1));
    }

    private bool hasNotVisitedUp() {
        return !(this.hasVisited(this.row - 1, this.col));
    }

    private bool hasNotVisitedDown() {
        return !(this.hasVisited(this.row + 1, this.col));
    }

}

/* =================================================
                    Child Classes
================================================= */

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