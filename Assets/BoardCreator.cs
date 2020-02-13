using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
  
    public int cols = 100;
    public int rows = 100;
    private int tileSize = 1;
    public float seed = 1;
    public float seedChanger = 25;
    public float foiliageSeed = 20;
    public float foiliageSeedChanger = 250;
    public float caveSeed = 20;
    public float caveSeedChanger = 25;


    List<Transform> objects;

    void ChangeTileDirt(int i)
    {
        Destroy(objects[i].gameObject);
        GameObject dirtTile = (GameObject)Instantiate(Resources.Load("DirtTile"));
        dirtTile.transform.position = new Vector2(objects[i].position.x, objects[i].position.y);
        Destroy(objects[i].gameObject);
        objects[i] = dirtTile.transform;


    }
    void ChangeTileWater(int i)
    {
        Destroy(objects[i].gameObject);
        GameObject dirtTile = (GameObject)Instantiate(Resources.Load("WaterTile"));
        dirtTile.transform.position = new Vector2(objects[i].position.x, objects[i].position.y);
        Destroy(objects[i].gameObject);
        objects[i] = dirtTile.transform;
    }
        void ChangeTileSnow(int i)
    {
        Destroy(objects[i].gameObject);
        GameObject dirtTile = (GameObject)Instantiate(Resources.Load("SnowTile"));
        dirtTile.transform.position = new Vector2(objects[i].position.x, objects[i].position.y);
        Destroy(objects[i].gameObject);
        objects[i] = dirtTile.transform;

    }
    void ChangeTileSand(int i)
    {
        Destroy(objects[i].gameObject);
        GameObject dirtTile = (GameObject)Instantiate(Resources.Load("SandTile"));
        dirtTile.transform.position = new Vector2(objects[i].position.x, objects[i].position.y);
        Destroy(objects[i].gameObject);
        objects[i] = dirtTile.transform;

    }
    void GenerateGrid()
    {
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("FreshGrassTile"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(referenceTile, transform);
                float posX = col * tileSize;
                float posY = row * tileSize;
                tile.transform.position = new Vector2(posX, posY);
                objects.Add(tile.transform);
                

            }
        }
        Destroy(referenceTile);
    }
    void GenerateFoiliage()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int co = 0; co < cols; co++)
            {
                float poX = objects[co * cols + r].gameObject.transform.position.x;
                float poY = objects[co * cols + r].gameObject.transform.position.y;
                poX = poX * 1.0000f;
                poY = poY * 1.0000f;
                float noise = Mathf.PerlinNoise(poX / foiliageSeedChanger + foiliageSeed, poY / foiliageSeedChanger + foiliageSeed);
                if (noise > .7 && objects[co * cols + r].gameObject.name != "WaterTile(Clone)" && objects[co * cols + r].gameObject.name != "SandTile(Clone)" && objects[co * cols + r].gameObject.name != "WaterTile(Clone)" && objects[co * cols + r].gameObject.name != "DirtTile(Clone)")
                {
                    GameObject BerryBush = (GameObject)Instantiate(Resources.Load("BerryBush"));
                    BerryBush.transform.position = new Vector2(poX, poY);
                }
                if (noise < .3 && objects[co * cols + r].gameObject.name != "WaterTile(Clone)" && objects[co * cols + r].gameObject.name != "SandTile(Clone)" && objects[co * cols + r].gameObject.name != "WaterTile(Clone)" && objects[co * cols + r].gameObject.name != "DirtTile(Clone)")
                {
                    GameObject tree = (GameObject)Instantiate(Resources.Load("Tree"));
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
                float poX = objects[co * cols + r].gameObject.transform.position.x;
                float poY = objects[co * cols + r].gameObject.transform.position.y;
                poX = poX * 1.0000f;
                poY = poY * 1.0000f;
                float noise = Mathf.PerlinNoise(poX / caveSeedChanger + caveSeed, poY / caveSeedChanger + caveSeed);
                if (noise < .3 && objects[co * cols + r].gameObject.name == "DirtTile(Clone)")
                {
                    GameObject StoneTile = (GameObject)Instantiate(Resources.Load("StoneTile"));
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
                        if (co <+ cols && r <+ rows)
                        {
                            if (co > -1 && r > -1)
                            {
                                float poX = objects[co * cols + r].gameObject.transform.position.x;
                                float poY = objects[co * cols + r].gameObject.transform.position.y;
                                poX = poX * 1.0000f;
                                poY = poY * 1.0000f;    
                                float noise = Mathf.PerlinNoise(poX/seedChanger + seed, poY/seedChanger + seed);
                                Debug.Log(noise);
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
    

            // Start is called before the first frame update
    void Start()
    {
        
        objects = new List<Transform>();
        GenerateGrid();
        ChangeTheTiles();
        GenerateFoiliage();
        GenerateCaves();




    }
  

    // Update is called once per frame
    void Update()
    {
        
    }

}
