using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public int cols = 256;

    public int rows = 256;

    private int tileSize = 1;

    public float seed = 1;

    public float seedChanger = 25;

    public float foiliageSeed = 20;

    public float foiliageSeedChanger = 250;

    public float caveSeed = 20;

    public float caveSeedChanger = 25;

    public struct Point {
        public int row, col;
        public Point(int r, int c) {
            row = r;
            col = c;
        }
    }

    public enum Tile {
        Sand,
        Stone,
        Snow,
        Dirt,
        Water,
        Grass,
        Berry
    }

    public Dictionary<Point, GameObject> Grid = new Dictionary<Point, GameObject>();
    private Dictionary<Tile, GameObject> prefabs = new Dictionary<Tile, GameObject>();

    List<Transform> objects;

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var point = new Point(row * tileSize, col * tileSize);
                Grid[point] = (GameObject) Instantiate(prefabs[GetTileFromPoint(point)], transform);
                Grid[point].transform.position = new Vector2(point.row, point.col);
            }
        }
    }

    Tile GetTileFromPoint(Point p) {
        // Convert perlin noise to 0-1 value range
        float row = (float)p.row / rows;
        float col = (float)p.col / cols;

        var noise = Mathf.PerlinNoise(row, col);
        Debug.Log(p.col);
        Debug.Log(p.row);
        Debug.Log(noise);
        if (noise < .2) {
            return Tile.Water;
        } 
        else if (noise < .21){
            return Tile.Sand;
        }
        else if (noise < .5 ) {
            return Tile.Grass;
        }
        else if (noise < .8) {
            return Tile.Dirt;
        }
        else {
            return Tile.Snow;
        }
    }

    void GenerateFoiliage()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int co = 0; co < cols; co++)
            {
                float poX =
                    objects[co * cols + r].gameObject.transform.position.x;
                float poY =
                    objects[co * cols + r].gameObject.transform.position.y;
                poX = poX * 1.0000f;
                poY = poY * 1.0000f;
                float noise =
                    Mathf
                        .PerlinNoise(poX / foiliageSeedChanger + foiliageSeed,
                        poY / foiliageSeedChanger + foiliageSeed);
                if (
                    noise > .7 &&
                    objects[co * cols + r].gameObject.name !=
                    "WaterTile(Clone)" &&
                    objects[co * cols + r].gameObject.name !=
                    "SandTile(Clone)" &&
                    objects[co * cols + r].gameObject.name !=
                    "WaterTile(Clone)" &&
                    objects[co * cols + r].gameObject.name != "DirtTile(Clone)"
                )
                {
                    GameObject BerryBush =
                        (GameObject) Instantiate(Resources.Load("BerryBush"));
                    BerryBush.transform.position = new Vector2(poX, poY);
                }
                if (
                    noise < .3 &&
                    objects[co * cols + r].gameObject.name !=
                    "WaterTile(Clone)" &&
                    objects[co * cols + r].gameObject.name !=
                    "SandTile(Clone)" &&
                    objects[co * cols + r].gameObject.name !=
                    "WaterTile(Clone)" &&
                    objects[co * cols + r].gameObject.name != "DirtTile(Clone)"
                )
                {
                    GameObject tree =
                        (GameObject) Instantiate(Resources.Load("Tree"));
                    tree.transform.position = new Vector2(poX, poY + .5f);
                }
            }
        }
    }

    void GenerateCaves()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int co = 0; co < cols; co++)
            {
                float poX =
                    objects[co * cols + r].gameObject.transform.position.x;
                float poY =
                    objects[co * cols + r].gameObject.transform.position.y;
                poX = poX * 1.0000f;
                poY = poY * 1.0000f;
                float noise =
                    Mathf
                        .PerlinNoise(poX / caveSeedChanger + caveSeed,
                        poY / caveSeedChanger + caveSeed);
                if (
                    noise < .3 &&
                    objects[co * cols + r].gameObject.name == "DirtTile(Clone)"
                )
                {
                    GameObject StoneTile =
                        (GameObject) Instantiate(Resources.Load("StoneTile"));
                    StoneTile.transform.position = new Vector2(poX, poY);
                }
            }
        }
    }

    void ChangeTheTiles()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int co = 0; co < cols; co++)
            {
                if (co < +cols && r < +rows)
                {
                    if (co > -1 && r > -1)
                    {
                        float poX =
                            objects[co * cols + r]
                                .gameObject
                                .transform
                                .position
                                .x;
                        float poY =
                            objects[co * cols + r]
                                .gameObject
                                .transform
                                .position
                                .y;
                        poX = poX * 1.0000f;
                        poY = poY * 1.0000f;
                        float noise =
                            Mathf
                                .PerlinNoise(poX / seedChanger + seed,
                                poY / seedChanger + seed);
                        Debug.Log (noise);
                        if (noise < .2)
                        {
                            ChangeTileWater(co * cols + r);
                        }
                        if (noise > .2 && noise < .21)
                        {
                            ChangeTileSand(co * cols + r);
                        }
                        if (noise > .5 && noise < .8)
                        {
                            ChangeTileDirt(co * cols + r);
                        }
                        if (noise > .8)
                        {
                            ChangeTileSnow(co * cols + r);
                        }
                    }
                }
            }
        }
    }

    void CreateReferenceResources() {
        prefabs[Tile.Sand] = (GameObject)Instantiate(Resources.Load("SandTile"));
        prefabs[Tile.Snow] = (GameObject)Instantiate(Resources.Load("SnowTile"));
        prefabs[Tile.Stone] = (GameObject)Instantiate(Resources.Load("StoneTile"));
        prefabs[Tile.Water] = (GameObject)Instantiate(Resources.Load("WaterTile"));
        prefabs[Tile.Dirt] = (GameObject)Instantiate(Resources.Load("DirtTile"));
        prefabs[Tile.Grass] = (GameObject)Instantiate(Resources.Load("FreshGrassTile"));
        prefabs[Tile.Berry] = (GameObject)Instantiate(Resources.Load("BerryBush"));
    }

    void DisableReferenceResources() {
        foreach(var entry in prefabs)
        {
            entry.Value.SetActive(false); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateReferenceResources();
        objects = new List<Transform>();
        GenerateGrid();
        //ChangeTheTiles();
        //GenerateFoiliage();
        DisableReferenceResources();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
