using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    [SerializeField]
    private Tile tileUnknown;
    [SerializeField]
    private Tile tileEmpty;
    [SerializeField]
    private Tile tileMine;
    [SerializeField]
    private Tile tileExploded;
    [SerializeField]
    private Tile tileFlag;
    [SerializeField]
    private Tile tileNum1;
    [SerializeField]
    private Tile tileNum2;
    [SerializeField]
    private Tile tileNum3;
    [SerializeField]
    private Tile tileNum4;
    [SerializeField]
    private Tile tileNum5;
    [SerializeField]
    private Tile tileNum6;
    [SerializeField]
    private Tile tileNum7;
    [SerializeField]
    private Tile tileNum8;
    private TMP_Text errorText;
    
    private void Awake()
    {
        errorText = GameObject.Find("GeneralError").GetComponent<TMP_Text>();
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state) // Puts the cell on the tilemap
    {
        try
        {
            int width = state.GetLength(0);
            int height = state.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell cell = state[x, y];
                    tilemap.SetTile(cell.position, getTile(cell));
                }
            }
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private Tile getTile(Cell cell) // Gets the tile for the cell
    {
        try
        {
            if (cell.revealed)
            {
                return getRevealed(cell);
            }
            else if (cell.flagged)
            {
                return tileFlag;
            }
            else
            {
                return tileUnknown;
            }
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
            return null;
        }
    }

    private Tile getRevealed(Cell cell) // Gets the tile for the revealed cell
    {
        try
        {
            switch (cell.type)
            {
                case Cell.Type.Empty: return tileEmpty;
                case Cell.Type.Mine: return cell.exploded ? tileExploded : tileMine;
                case Cell.Type.Number: return getNumber(cell);
                default: return null;
            }
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
            return null;
        }
    }

    private Tile getNumber(Cell cell) // Gets the tile for the number cell
    {
        try
        {
            switch (cell.number)
            {
                case 1: return tileNum1;
                case 2: return tileNum2;
                case 3: return tileNum3;
                case 4: return tileNum4;
                case 5: return tileNum5;
                case 6: return tileNum6;
                case 7: return tileNum7;
                case 8: return tileNum8;
                default: return null;
            }
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
            return null;
        }
    }
}
