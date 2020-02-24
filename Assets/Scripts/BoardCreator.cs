using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public int cols;
    public int rows;
    private int tileSize = 1;
    public float seed = 20;
    public float seedChanger = 25;
    public float foiliageSeed = 25;
    public float foiliageSeedChanger = 250;
    public float caveSeed = 20;
    public float caveSeedChanger = 25;
    public static int GridSize = 256;
    public int playerCount;
    public GameObject[][] Grid = new GameObject[GridSize][];
    public GameObject[] Players = new GameObject[10];
    private Dictionary<TileType, GameObject> prefabs = new Dictionary<TileType, GameObject>();

    List<Transform> objects;

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            Grid[row] = new GameObject[GridSize]; 
            for (int col = 0; col < cols; col++)
            {
                Grid[row][col] = (GameObject) Instantiate(prefabs[GetTileFromPoint(row, col)], transform);
                Grid[row][col].transform.position = new Vector2(col, row);

                // Pass some data through to the Tile script attached to the prefabs.
                // This gives them references to the boardCreator (and through it the grid)
                // and their position inside it.  It's important to understand that row/col
                // are passed by value (copied) but boardCreator is passed by reference (pointer)
                // Go read: https://www.dotnetodyssey.com/2014/06/06/beginners-guide-value-reference-types-c/
                var tile = Grid[row][col].GetComponent<Tile>();
                tile.boardCreator = this;
                tile.Row = row;
                tile.Col = col;
                tile.Type = GetTileFromPoint(row, col);
            }
        }
    }
    void GeneratePlayers(int p) 
    {
        for(int i = 0; i < p; i++){
            Players[i]=(GameObject) Instantiate(Resources.Load("Player"));
            Players[i].transform.position = new Vector2(rows/2+i, cols/2+i);
        }
    }

    TileType GetTileFromPoint(float row, float col) {
        // Convert perlin noise to 0-1 value range
        float rowNoise = row / 20;
        float colNoise = col / 20;

        var noise = Mathf.PerlinNoise(rowNoise, colNoise);
        if (row == 0){
            return TileType.Barrier;
        }
        if (col == 0){
            return TileType.Barrier;
        }
        if(col == cols-1){
            return TileType.Barrier;
        }
        if (row == rows-1){
            return TileType.Barrier;
        }
        else if (noise < .10) {
            return TileType.Water;
        } 
        else if (noise < .15) {
            return TileType.WaterShallow;
        } 
        else if (noise < .20){
            return TileType.Sand;
        }
        else if (noise < .7 ) {
            return TileType.Grass;
        }
        else if (noise < .9) {
            return TileType.Dirt;
        }
        else {
            return TileType.Snow;
        }
    }

    void GenerateFoiliage()
    {
        for (int r = rows-1; r > 0; r--)
        {
            for (int c = cols-1; c > 0; c--)
            {
                var noise = Mathf.PerlinNoise((float)r/foiliageSeed, (float)c/foiliageSeed);
                var tile = Grid[r][c].GetComponent<Tile>();
                if(tile.AllowsPlants())
                {
                    if (noise > .8 )
                    {
                        var BerryBush = (GameObject)Instantiate(Resources.Load("BerryBush"));
                        BerryBush.transform.position = Grid[r][c].transform.position;
                    }
                    if (noise > .2 && noise < .3){
                        var PineTree = (GameObject) Instantiate(Resources.Load("PineTree"));
                        PineTree.transform.position = Grid[r][c].transform.position;
                    }
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

    void CreateReferenceResources() {
        prefabs[TileType.Sand] = (GameObject)Instantiate(Resources.Load("SandTile"));
        prefabs[TileType.Snow] = (GameObject)Instantiate(Resources.Load("SnowTile"));
        prefabs[TileType.Stone] = (GameObject)Instantiate(Resources.Load("StoneTile"));
        prefabs[TileType.Water] = (GameObject)Instantiate(Resources.Load("WaterTile"));
        prefabs[TileType.Dirt] = (GameObject)Instantiate(Resources.Load("DirtTile"));
        prefabs[TileType.Grass] = (GameObject)Instantiate(Resources.Load("FreshGrassTile"));
        prefabs[TileType.Berry] = (GameObject)Instantiate(Resources.Load("BerryBush"));
        prefabs[TileType.Barrier] = (GameObject)Instantiate(Resources.Load("Barrier"));
        prefabs[TileType.PineTree] = (GameObject)Instantiate(Resources.Load("PineTree"));
        prefabs[TileType.DirtTransition] = (GameObject)Instantiate(Resources.Load("DirtTileTurningGrassFirst"));
        prefabs[TileType.WaterShallow] = (GameObject)Instantiate(Resources.Load("WaterTileLightPixelTest"));
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
        DisableReferenceResources();
        GeneratePlayers(playerCount);
        GenerateFoiliage();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
