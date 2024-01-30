public class Player // Player class to store player information
{
    public string username;
    public string difficulty;
    public string role;
    public string color;

    public Player(string user, string diff, string rol, string col)
    {
        this.username = user;
        this.difficulty = diff;
        this.role = rol;
        this.color = col;
    }
}
