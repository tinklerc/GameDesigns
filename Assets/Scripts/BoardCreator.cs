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

    public int playerCount;

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
        WaterShallow,
        Grass,
        DirtTransition,
        Berry,
        Barrier,
        PineTree
    
    }

    public Dictionary<Point, GameObject> Grid = new Dictionary<Point, GameObject>();
    public GameObject[][] Grid2 = new GameObject[256][];
    public GameObject[] Players = new GameObject[10];
    private Dictionary<Tile, GameObject> prefabs = new Dictionary<Tile, GameObject>();

    List<Transform> objects;

    void GenerateGrid()
    {
        if (rows > 256){
            rows = 256;
        }
        if (cols > 256){
            cols = 256;
        }
        
        for (int row = 0; row < rows; row++)
        {
           Grid2[row] = new GameObject[256]; 
            for (int col = 0; col < cols; col++)
            {
                var point = new Point(row * tileSize, col * tileSize);
                Grid2[row][col] = (GameObject) Instantiate(prefabs[GetTileFromPoint(point)], transform);
                Grid2[row][col].transform.position = new Vector2(point.row, point.col);
            }
        }
    }
    void GeneratePlayers(int p) 
    {
                for(int i = 0; i < p; i++){
                    Players[i]=(GameObject) Instantiate(Resources.Load("Tree"));
                    Players[i].transform.position = new Vector2(rows/2+i, cols/2+i);
                }

        

    }

    Tile GetTileFromPoint(Point p) {
        // Convert perlin noise to 0-1 value range
        float row = (float)p.row;
        float col = (float)p.col;
        float rowNoise = row / 20;
        float colNoise = col / 20;

        var noise = Mathf.PerlinNoise(rowNoise, colNoise);
        // Debug.Log(p.col);
        // Debug.Log(p.row);
        // Debug.Log(noise);
        if (row == 0){
            return Tile.Barrier;
        }
        if (col == 0){
            return Tile.Barrier;
        }
        if(col == cols-1){
            
            return Tile.Barrier;
                
        }
        
        if (row == rows-1){
            return Tile.Barrier;
        }

        
        else if (noise < .10) {
            return Tile.Water;
        } 
        else if (noise < .15) {
            return Tile.WaterShallow;
        } 
        else if (noise < .20){
            return Tile.Sand;
        }
        else if (noise < .7 ) {
            return Tile.Grass;
        }
        // else if (noise < .53){  
        //     return Tile.DirtTransition;

        // }
        else if (noise < .9) {
            return Tile.Dirt;
        }
        else {
            return Tile.Snow;
        }
    }

    void GenerateFoiliage()
    {
         if (rows > 256){
            rows = 256;
        }
        if (cols > 256){
            cols = 256;
        }
        for (int r = 1; r < rows-1; r++)
        {
            for (int co = cols-1; co > 1; co--)
            {
                float fRow = r;
                float fCol = co;
                
                float noise =
                    Mathf
                        .PerlinNoise(fRow/foiliageSeed, fCol/foiliageSeed);
                if (noise > .8 )
               
                {
                    if(Grid2[r][co].gameObject.name != "WaterTile(Clone)(Clone)" && Grid2[r][co].gameObject.name != "SnowTile(Clone)(Clone)" && Grid2[r][co].gameObject.name !="WaterTileLightPixelTest(Clone)(Clone)" && Grid2[r][co].gameObject.name != "SandTile(Clone)(Clone)"){
                    GameObject BerryBush =
                        (GameObject) Instantiate(Resources.Load("BerryBush"));
                    BerryBush.transform.position = Grid2[r][co].transform.position;
                     }
                }
                if (noise > .2 && noise < .3){
                    if(Grid2[r][co].gameObject.name != "WaterTile(Clone)(Clone)" && Grid2[r][co].gameObject.name != "SnowTile(Clone)(Clone)" && Grid2[r][co].gameObject.name !="WaterTileLightPixelTest(Clone)(Clone)"  && Grid2[r][co].gameObject.name != "SandTile(Clone)(Clone)"){
                    GameObject PineTree =
                        (GameObject) Instantiate(Resources.Load("PineTree"));
                    PineTree.transform.position = Grid2[r][co].transform.position;
                     }
                }
            }
        }
    }
    void changeTransitionTiles(){
        

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
                        // Debug.Log (noise);
                        // if (noise < .2)
                        // {
                        //     ChangeTileWater(co * cols + r);
                        // }
                        // if (noise > .2 && noise < .21)
                        // {
                        //     ChangeTileSand(co * cols + r);
                        // }
                        // if (noise > .5 && noise < .8)
                        // {
                        //     ChangeTileDirt(co * cols + r);
                        // }
                        // if (noise > .8)
                        // {
                        //     ChangeTileSnow(co * cols + r);
                        // }
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
        prefabs[Tile.Barrier] = (GameObject)Instantiate(Resources.Load("Barrier"));
        prefabs[Tile.PineTree] = (GameObject)Instantiate(Resources.Load("PineTree"));
        prefabs[Tile.DirtTransition] = (GameObject)Instantiate(Resources.Load("DirtTileTurningGrassFirst"));
        prefabs[Tile.WaterShallow] = (GameObject)Instantiate(Resources.Load("WaterTileLightPixelTest"));
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
        GeneratePlayers(playerCount);
        GenerateFoiliage();
        changeTransitionTiles();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
