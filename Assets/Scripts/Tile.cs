using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
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

public class Tile : MonoBehaviour
{
    public BoardCreator boardCreator;
    public int Row, Col;
    public TileType Type;

    enum Adj {
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Type == TileType.Grass) {
            var bot = GetAdjacent(Adj.Bottom);
            // Most languages (including C#) use short circuiting
            // for boolean checks (&& and ||) this means
            // that for && checks, the system returns false as soon as
            // it hits the first false value (because all of them have to be true
            // or the && condition is going to be false anyways).  For ||, it returns
            // true as soon as the first true value is found (because if any of them are true
            // then the || condition will be true).  It doesn't even bother running the later checks.
            // -----
            // Why this matters?  Because referencing a property of null (like bot.Type)
            // will blow up with a null reference exception if bot is null. So the order of
            // these checks matters - if written like
            // if(bot.Type == TileType.Dirt && bot != null)
            // then we might try to access Type on null and throw an exception.
            if(bot != null && bot.Type == TileType.Dirt) {
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GrassTopHalf");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Get the neighboring GameObject by named position
    **/
    Tile GetAdjacent(Adj pos) {
        var adjRow = Row;
        var adjCol = Col;
        if(pos == Adj.Left) {
            adjCol = Col - 1;
        }
        else if (pos == Adj.TopLeft) {
            adjCol = Col - 1;
            adjRow = Row + 1;
        }
        else if (pos == Adj.Top) {
            adjRow = Row + 1;
        }
        else if (pos == Adj.TopRight) {
            adjCol = Col + 1;
            adjRow = Row + 1;
        }
        else if (pos == Adj.Right) {
            adjCol = Col + 1;
        }
        else if (pos == Adj.BottomRight) {
            adjCol = Col + 1;
            adjRow = Row - 1;
        }
        else if (pos == Adj.Bottom) {
            adjRow = Row - 1;
        }
        else if (pos == Adj.BottomLeft) {
            adjRow = Row - 1;
            adjCol = Col - 1;
        }
        
        // Now that we have the right row/col for the desired neighbor
        // Make sure that it actually exists, otherwise return null
        if(valid(adjRow) && valid(adjCol)) {
            return boardCreator.Grid[adjRow][adjCol].GetComponent<Tile>();
        }
        else {
            return null;
        }
    }

    public bool AllowsPlants() {
        return (
            Type != TileType.Water &&
            Type != TileType.Snow &&
            Type != TileType.WaterShallow &&
            Type != TileType.Sand
        );
    }

    /**
     * Limit integer row and colum values to a safe range
     * between 0 and GridSize - 1 so we don't ever attempt
     * to access an index out of range.
    **/
    bool valid(int input) {
        if(input < 0) {
            return false;
        }
        else if (input >= BoardCreator.GridSize) {
            return false;
        } 
        else {
            return true;
        }
    }
}
