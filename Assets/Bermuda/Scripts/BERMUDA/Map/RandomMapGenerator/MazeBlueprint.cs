using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MazeBlueprint {

    [SerializeField] private int mazeHeight;
    [SerializeField] private int mazeWidth;
    
    private List<List<String>> playAreaBlueprint = new List<List<String>>();
    [SerializeField] private List<String> playAreaBlueprintSerialized = new List<String>();

    public MazeBlueprint(int mazeHeight, int mazeWidth) {
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;
    }

    public String serialize() {
        serializePlayAreaBlueprint();
        return JsonUtility.ToJson(this, false);
    }

    public void deserialize(String data) {
        JsonUtility.FromJsonOverwrite(data, this);
        this.deserializePlayAreaBlueprint();
    }

    /* =================================================
                        Getter Setter
    ================================================= */

    public int getMazeHeight() {
        return this.mazeHeight;
    }

    public int getMazeWidth() {
        return this.mazeWidth;
    }

    public String getTileType(int row, int col) {
        return this.playAreaBlueprint[row][col];
    }

    public Vector2Int getRandomTile(String type){
        
        int spawnX;
        int spawnY;

        while (true) {
            spawnX = UnityEngine.Random.Range(0, this.getMazeWidth());
            spawnY = UnityEngine.Random.Range(0, this.getMazeHeight());

            if (this.getTileType(spawnY, spawnX) == type) {
                return new Vector2Int(spawnX, spawnY);
            }
        }
    }

    public Vector2Int getRandomTile(List<String> types){
        int spawnX;
        int spawnY;

        while (true) {
            spawnX = UnityEngine.Random.Range(0, this.getMazeWidth());
            spawnY = UnityEngine.Random.Range(0, this.getMazeHeight());
            
            if (types.Contains(this.getTileType(spawnY, spawnX))) {
                return new Vector2Int(spawnX, spawnY);
            }
        }
    }

    public void setPlayAreaBlueprint(List<List<String>> playAreaBlueprint) {
        this.playAreaBlueprint = playAreaBlueprint;
    }

    private void deserializePlayAreaBlueprint(){
        
        this.playAreaBlueprint = new List<List<String>>();
        List<String> serializedData = this.playAreaBlueprintSerialized;

        for (int i = 0; i < this.mazeHeight; i++) {

            List<String> newRow = new List<String>();
            for (int j = 0; j < this.mazeWidth; j++) {
                newRow.Add(serializedData[i*this.mazeWidth + j]);
            }
            this.playAreaBlueprint.Add(newRow);
        }
    }

    private void serializePlayAreaBlueprint(){

        this.playAreaBlueprintSerialized = new List<String>();
        foreach(List<String> row in this.playAreaBlueprint){
            this.playAreaBlueprintSerialized.AddRange(row);
        }
    }

    /* =================================================
                        Debug Area
    ================================================= */

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


