/*
* Name: Connor M Fitzgerald
* Course: CSCI 1250, Section 002
* Assignment: Lab 08, Game Night
* Date: November 19, 2026
* Description: Keeps a game night leaderboard in three text files. Shows the
* season so far, records tonight's points, saves the new totals,
* and adds tonight's result to a history that grows every week.
*/

const string PLAYERS_FILE = "players.txt";
const string POINTS_FILE = "points.txt";
const string HISTORY_FILE = "history.txt";
const int MAX_POINTS = 20;

// Look for the data files next to Program.cs, however the program is started.
string projectFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
Directory.SetCurrentDirectory(projectFolder);

string[] players = File.ReadAllLines(PLAYERS_FILE);
string[] points = File.ReadAllLines(POINTS_FILE);
string[] nightsHistory = File.ReadAllLines(HISTORY_FILE);

//Part 3
int night = nightsHistory.Length;
System.Console.WriteLine("=== GAME NIGHT LEADERBOARD ===");
if(night == 0)
{
    System.Console.WriteLine("No nights played yet.");
}
else
{
    System.Console.WriteLine($"Nights played: {night}");
    //  foreach: only need to output every string
    foreach(string nightStr in nightsHistory)
    {
        System.Console.WriteLine("  " + nightStr);
    }
}
System.Console.WriteLine($"Tonight is Night {++night}\n");

//Part 1
System.Console.WriteLine("PLAYER".PadRight(11)+"POINTS");
//  for: two arrays are used
//  needed to output a specific position from each array.
for(int i = 0; i < players.Length; i++)
{
    System.Console.WriteLine(players[i].PadRight(11)+points[i]);
}
System.Console.WriteLine($"Leader: {players[LeaderIndex(points)]} with {points[LeaderIndex(points)]}");
System.Console.WriteLine($"Season Total: {TotalPoints(points)}\n");

//Part 2
int tonightWinner = 0;
int tonightWinnerPoints = 0;

//  for: two arrays are used
//  needed to output a specific position from each array
//  also need i position to alter other arrays, and extract info from another.
for(int i=0; i < players.Length; i++)
{
    System.Console.Write($"Points tonight for {players[i]}: ");
    string pointsInput = Console.ReadLine();
    bool isNumber = int.TryParse(pointsInput, out int tonight);
    //  while: to prompt user for new input *every* time they give
    //  an un-acceptable input only if the first input is bad
    //  until they give a satisfactory response.
    while(tonight > MAX_POINTS || tonight < 0 || !isNumber)
    {
        System.Console.WriteLine($"Points must be from 0 to {MAX_POINTS}.");
        System.Console.Write($"Points tonight for {players[i]}: ");
        pointsInput = Console.ReadLine();
        isNumber = int.TryParse(pointsInput, out tonight);
    }
    if(tonight > tonightWinnerPoints)
    {
        tonightWinner = i;
        tonightWinnerPoints = tonight;
    }
    int season = Convert.ToInt32(points[i]) + tonight;
    points[i] = season.ToString();
}
System.Console.WriteLine($"Tonight's winner: {players[tonightWinner]} with {tonightWinnerPoints}");
File.WriteAllLines(POINTS_FILE, points);
System.Console.WriteLine($"\nSaved {POINTS_FILE}.");

//Part 3
string[] entry = {HistoryEntry(night, players[tonightWinner], tonightWinnerPoints) };
File.AppendAllLines(HISTORY_FILE, entry);
System.Console.WriteLine($"Added to {HISTORY_FILE}: {entry[0]}");

static int LeaderIndex(string[] points)
{
    int leaderPosition = 0;
    int leaderPoints = 0;
    //for: to be able to save and return an array position
    // and also do math for each.
    for(int i=0; i < points.Length; i++)
    {
        if(Convert.ToInt32(points[i]) > leaderPoints)
        {
        leaderPoints = Convert.ToInt32(points[i]);
        leaderPosition = i;
        };
    }
    return leaderPosition;
}
static int TotalPoints(string[] points)
{
    int totalPoints = 0;
    //  foreach:
    //  because saving/knowing the array position is not necessary
    //  I just need to do math for each value in the array.
    foreach(string point in points)
    {
        totalPoints += Convert.ToInt32(point);
    }
    return totalPoints;
}

static string HistoryEntry(int night, string winner, int points)
{
    string winnerOutput = $"Night {night}: {winner} won with {points}";
    return winnerOutput;
}
