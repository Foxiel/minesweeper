using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    // all variables for this file
    [SerializeField] // SerializeField makes it possible to change the value in unity
    private Difficulty diffi;
    [SerializeField]
    private NameOption usern;
    [SerializeField]
    private Image bg;
    [SerializeField]    
    private GameObject pausemenu;
    [SerializeField]
    private TMP_Text endgame;
    [SerializeField]
    private TMP_Text timertext;
    [SerializeField]
    private TMP_Text errorText;

    private int _width;
    private int _height;
    private int _mines;
    private int _minutes;
    private int _seconds;
    private bool _firstclick;

    private Board _board;
    private Cell[,] _state;
    private bool _gameover;
    private bool _pause;

    void Awake() // this will be called once the object with this script is enabled on unity
    {
        try
        {
            errorText = GameObject.Find("GeneralError").GetComponent<TMP_Text>();
            string user = usern.userName.text;
            string diff = diffi.chosen;
            Player player1 = new Player(user, diff, "", "");

            switch (player1.difficulty)
            {
                case "singleEasy":
                    _width = 15;
                    _height = 15;
                    _mines = 30;
                    Camera.main.orthographicSize = 9f;
                    break;

                case "singleHard":
                    _width = 20;
                    _height = 20;
                    _mines = 60;
                    Camera.main.orthographicSize = 12f;
                    break;

                case "multiEasy":
                    // make field bigger
                    break;

                case "multiHard":
                    // make field bigger
                    break;
            }

            _board = GetComponentInChildren<Board>(); // This gets the object with the Board component
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    void Start() // This will be called once after awake
    {
        try
        {
            NewGame();
            InvokeRepeating(nameof(ConvertTimeToText), 0, 1.0f);
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void NewGame() // This function starts a new game
    {
        try
        {
            errorText.text = "";
            _firstclick = false;
            if (_pause)
            {
                Paused(false);
            }
            
            _gameover = false;
            endgame.gameObject.SetActive(false);
            timertext.gameObject.SetActive(true);
            _state = new Cell[_width, _height];

            GenerateCells();
            GenerateMines();
            GenerateNumbers();

            Camera.main.transform.position =
                new Vector3(_width / 2f, _height / 2f, -10f); // sets the on the middle of the board
            _board.Draw(_state);

            _minutes = 0;
            _seconds = 0;
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void GenerateCells() // This function generates the board
    {
        try
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Cell cell = new Cell();
                    cell.position = new Vector3Int(x, y, 0);
                    cell.type = Cell.Type.Empty;
                    _state[x, y] = cell;
                }
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void GenerateMines()
    {
        try
        {
            for (int i = 0; i < _mines; i++) // This for loop generates a location for a mine
            {
                int x = Random.Range(0, _width);
                int y = Random.Range(0, _height);

                while (_state[x, y].type == Cell.Type.Mine) // This while loop checks if there already is a mine
                {
                    x++;

                    if (x >= _width)
                    {
                        x = 0;
                        y++;

                        if (y >= _height)
                        {
                            y = 0;
                        }
                    }
                }

                _state[x, y].type = Cell.Type.Mine; // This places a mine
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void GenerateNumbers() // This function generates the numbers around the mines
    {
        try
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Cell cell = _state[x, y];

                    if (cell.type == Cell.Type.Mine)
                    {
                        continue;
                    }

                    cell.number = CountMines(x, y);

                    if (cell.number > 0)
                    {
                        cell.type = Cell.Type.Number;
                    }

                    _state[x, y] = cell;
                }
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private int CountMines(int Cx, int Cy) // This function counts the mines around a cell
    {
        try
        {
            int count = 0;

            for (int adjx = -1; adjx <= 1; adjx++)
            {
                for (int adjy = -1; adjy <= 1; adjy++)
                {
                    if (adjx == 0 && adjy == 0)
                    {
                        continue;
                    }

                    int x = Cx + adjx;
                    int y = Cy + adjy;

                    if (GetCell(x, y).type == Cell.Type.Mine)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
            return 0;
        }
    }

    private void Update() // This gets called every frame
    {
        try
        {
            if (_pause) return;

            if (_gameover)
            {
                if (Input.GetKeyDown("escape"))
                    SceneManager
                        .LoadScene("Menu"); // This checks if the escape key is pressed and goes to menu if it is
                else if
                    (Input.anyKeyDown) // This checks if any key is pressed except for mouse buttons 1 and 0 and will start a new game if true
                {
                    if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) return;
                    NewGame();
                }
            }

            if (Input.GetMouseButtonDown(1)) // If you right click on a mine it will flag it
            {
                Flag();
            }
            else if (Input.GetMouseButtonDown(0)) // if you left click on a mine it will reveal the cell
            {
                Reveal();
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void Flag() // This function flags a cell
    {
        try
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = _board.tilemap.WorldToCell(worldPosition);

            Cell cell = GetCell(cellPosition.x, cellPosition.y);

            if (cell.type == Cell.Type.Invalid || cell.revealed)
            {
                return;
            }

            cell.flagged = !cell.flagged;
            _state[cellPosition.x, cellPosition.y] = cell;
            _board.Draw(_state);
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void Reveal() // This function reveals a cell and will create a new board if it is the first click is on a mine or number
    {
        try
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = _board.tilemap.WorldToCell(worldPosition);

            Cell cell = GetCell(cellPosition.x, cellPosition.y);

            if (!CheckEmpty(cell) && !_firstclick)
            {
                GenerateCells();
                GenerateMines();
                GenerateNumbers();
                _board.Draw(_state);
                Reveal();
            }
            else
            {
                _firstclick = true;

                if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
                {
                    return;
                }

                switch (cell.type)
                {
                    case Cell.Type.Mine:
                        Explode(cell);
                        break;

                    case Cell.Type.Empty:
                        Flood(cell);
                        CheckWinner();
                        break;

                    default:
                        cell.revealed = true;
                        _state[cellPosition.x, cellPosition.y] = cell;
                        CheckWinner();
                        break;
                }

                _board.Draw(_state);
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void Flood(Cell cell) // This will reveal all cells around the empty cell you clicked on
    {
        try
        {
            if (cell.revealed) return;
            if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;

            cell.revealed = true;
            _state[cell.position.x, cell.position.y] = cell;

            if (cell.type == Cell.Type.Empty)
            {
                Flood(GetCell(cell.position.x - 1, cell.position.y));
                Flood(GetCell(cell.position.x + 1, cell.position.y));
                Flood(GetCell(cell.position.x, cell.position.y - 1));
                Flood(GetCell(cell.position.x, cell.position.y + 1));
                Flood(GetCell(cell.position.x - 1, cell.position.y - 1));
                Flood(GetCell(cell.position.x + 1, cell.position.y - 1));
                Flood(GetCell(cell.position.x - 1, cell.position.y + 1));
                Flood(GetCell(cell.position.x + 1, cell.position.y + 1));
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void Explode(Cell cell) // This will be called if you click on a mine
    {
        try
        {
            _gameover = true;

            cell.revealed = true;
            cell.exploded = true;
            _state[cell.position.x, cell.position.y] = cell;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    cell = _state[x, y];

                    if (cell.type == Cell.Type.Mine)
                    {
                        cell.revealed = true;
                        _state[x, y] = cell;
                    }
                }
            }

            _gameover = true;
            endgame.gameObject.SetActive(true);
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void CheckWinner() // This will check if you have won the game
    {
        try{
        bool allNonMineCellsRevealed = true;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cell cell = _state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed)
                {
                    allNonMineCellsRevealed = false;
                }
            }
        }

        if (allNonMineCellsRevealed)
        {
            _gameover = true;
            endgame.gameObject.SetActive(true);
        }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }
    
    private Cell GetCell(int x, int y) // This function gets the cell at the given location
    {
        try
        {
            if (IsValid(x, y))
            {
                return _state[x, y];
            }
            else
            {
                return new Cell();
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
            return new Cell();
        }
    }

    private bool IsValid(int x, int y) // This function checks if the given location is valid
    {
        try
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
            return false;
        }
    }

    public void ToMainMenu() // This function goes to the main menu
    {
        try
        {
            SceneManager.LoadScene("Menu");
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void Paused(bool isPaused) // This function pauses the game
    {
        try
        {
            _pause = isPaused;
            bg.gameObject.SetActive(isPaused);
            pausemenu.gameObject.SetActive(isPaused);
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void ConvertTimeToText() // This function converts the time to text
    {
        try
        {
            if (_gameover || _pause) return;

            _seconds++;

            if (_seconds == 60)
            {
                _minutes++;
                _seconds = 0;
            }

            timertext.text = _minutes.ToString("D2") + ":" + _seconds.ToString("D2");
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private bool CheckEmpty(Cell cell) // This function checks if the cell is empty
    {
        try
        {
            if (cell.type == Cell.Type.Empty) return true;

            return false;
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
            return false;
        }
    }
}
