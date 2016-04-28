using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace WpfApplication3
{
    public partial class GameWindow : Window
    {
        //String for database connection
        private const string sqlconnection = "Data Source=TTTdatabase.sqlite;Version=3;";

        private User player1;
        private User player2;
        private int difficulty;
        private string difficultyStr;

        // playerOneTurn player 1 = True and player 2 = False
        //Player1 will be 'X' and player2 will be 'O'
        private bool playerOneTurn;

        //when moveCounter = 36 game ends
        private int moveCounter;


        
        //list of buttons to check score
        Button[] buttons;

        //User Score board
        private bool[] ROWA;
        private bool[] ROWB;
        private bool[] ROWC;
        private bool[] ROWD;
        private bool[] ROWE;
        private bool[] ROWF;

        private bool[] COL1;
        private bool[] COL2;
        private bool[] COL3;
        private bool[] COL4;
        private bool[] COL5;
        private bool[] COL6;

        private bool[] DI1;
        private bool[] DI2;
        private bool[] DI3;
        private bool[] DI4;
        private bool[] DI5;
        private bool[] DI6;

        //AI Scoreboard
        private bool[] AIROWA;
        private bool[] AIROWB;
        private bool[] AIROWC;
        private bool[] AIROWD;
        private bool[] AIROWE;
        private bool[] AIROWF;

        private bool[] AICOL1;
        private bool[] AICOL2;
        private bool[] AICOL3;
        private bool[] AICOL4;
        private bool[] AICOL5;
        private bool[] AICOL6;

        private bool[] AIDI1;
        private bool[] AIDI2;
        private bool[] AIDI3;
        private bool[] AIDI4;
        private bool[] AIDI5;
        private bool[] AIDI6;

        //Empty game
        public GameWindow()
        {
            InitializeComponent();

        }

        //Single player game
        public GameWindow(User player, int difficultyLevel)
        {
            //Create object for player 
            player1 = new User(player);
            player2 = new User("CPU");

            //Set difficulty
            difficulty = difficultyLevel;

            //Initilize components in the window
            InitializeComponent();

            //Change the Player label to respective names if computer then label is "CPU"
            player1Label.Content = player1.getUsername() + ":";
            player2Label.Content = player2.getUsername() + ":";

            selectPlayer1Label.Content = player1.getUsername();
            selectPlayer2Label.Content = player2.getUsername();

            //set difficultyStr for Top Label
            if (difficulty == 1)
            {
                difficultyStr = "Easy";
            }
            else if (difficulty == 2)
            {
                difficultyStr = "Medium";
            }
            else
            {
                difficultyStr = "Hard";
            }

            //Change the top label to Game Mode + Game Difficulty
            gameModeLabel.Content = "Single Player: " + difficultyStr;

            //change score label to 0
            score1Label.Content = player1.getScore();
            score2Label.Content = player2.getScore();


            //set move counter to 1
            moveCounter = 0;

            //Set properties
            setProperties();


        }

        //Multiplayer game
        public GameWindow(User playerOne, User playerTwo)
        {
            //create user objects for each player
            player1 = new User(playerOne);
            player2 = new User(playerTwo);

            InitializeComponent();

            //Change the Player label to respective names if computer then label is "CPU"
            player1Label.Content = player1.getUsername() + ":";
            player2Label.Content = player2.getUsername() + ":";

            selectPlayer1Label.Content = player1.getUsername();
            selectPlayer2Label.Content = player2.getUsername();

            //Difficulty set to multiplayer (No AI)
            difficultyStr = "Multiplayer";

            //Change label to 'Multilayer'
            gameModeLabel.Content = "Multiplayer";

            //change score label to 0
            score1Label.Content = player1.getScore();
            score2Label.Content = player2.getScore();

            //set move counter to 1
            moveCounter = 0;

            //Set properties
            setProperties();
        }

        //Method to set properties for the game
        private void setProperties()
        {

            //Array for checking scores
            ROWA = new bool[3];
            ROWB = new bool[3];
            ROWC = new bool[3];
            ROWD = new bool[3];
            ROWE = new bool[3];
            ROWF = new bool[3];

            COL1 = new bool[3];
            COL2 = new bool[3];
            COL3 = new bool[3];
            COL4 = new bool[3];
            COL5 = new bool[3];
            COL6 = new bool[3];

            DI1 = new bool[3];
            DI2 = new bool[3];
            DI3 = new bool[3];
            DI4 = new bool[3];
            DI5 = new bool[3];
            DI6 = new bool[3];

            //SET arrays
            for (int i = 0; i < 3; i++)
            {
                //set row wins comb
                ROWA[i] = false;
                ROWB[i] = false;
                ROWC[i] = false;
                ROWD[i] = false;
                ROWE[i] = false;
                ROWF[i] = false;

                //set col wins comb
                COL1[i] = false;
                COL2[i] = false;
                COL3[i] = false;
                COL4[i] = false;
                COL5[i] = false;
                COL6[i] = false;
            }

            setAIArrays();

            setButtonsArray();

        }

        //Array for GameBoard buttons
        private void setButtonsArray()
        {
            buttons = new Button[36];

            buttons[0] = A1;
            buttons[1] = A2;
            buttons[2] = A3;
            buttons[3] = A4;
            buttons[4] = A5;
            buttons[5] = A6;
            buttons[6] = B1;
            buttons[7] = B2;
            buttons[8] = B3;
            buttons[9] = B4;
            buttons[10] = B5;
            buttons[11] = B6;
            buttons[12] = C1;
            buttons[13] = C2;
            buttons[14] = C3;
            buttons[15] = C4;
            buttons[16] = C5;
            buttons[17] = C6;
            buttons[18] = D1;
            buttons[19] = D2;
            buttons[20] = D3;
            buttons[21] = D4;
            buttons[22] = D5;
            buttons[23] = D6;
            buttons[24] = E1;
            buttons[25] = E2;
            buttons[26] = E3;
            buttons[27] = E4;
            buttons[28] = E5;
            buttons[29] = E6;
            buttons[30] = F1;
            buttons[31] = F2;
            buttons[32] = F3;
            buttons[33] = F4;
            buttons[34] = F5;
            buttons[35] = F6;

        }

        //Method to update the buttons array
        private void updateButtons()
        {
            //Set counter to see how many empty buttons are left
            int counter = 0;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Content == "")
                {
                    counter++;
                }
            }

            //Algorithm to resize button Array
            /*
            //Store in temporary array buttons left
            Button[] temp = new Button[counter];

            int j = 0;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Content == "")
                {
                    temp[j] = buttons[i];
                    j++;
                }
            }

            buttons = new Button[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                buttons[i] = temp[i];
            }

*/
        }

        //Method to set AI Arrays
        private void setAIArrays()
        {
            AIROWA = new bool[3];
            AIROWB = new bool[3];
            AIROWC = new bool[3];
            AIROWD = new bool[3];
            AIROWE = new bool[3];
            AIROWF = new bool[3];

            AICOL1 = new bool[3];
            AICOL2 = new bool[3];
            AICOL3 = new bool[3];
            AICOL4 = new bool[3];
            AICOL5 = new bool[3];
            AICOL6 = new bool[3];

            AIDI1 = new bool[3];
            AIDI2 = new bool[3];
            AIDI3 = new bool[3];
            AIDI4 = new bool[3];
            AIDI5 = new bool[3];
            AIDI6 = new bool[3];

            //SET arrays
            for (int i = 0; i < 3; i++)
            {
                //seT AIROW
                AIROWA[i] = false;
                AIROWB[i] = false;
                AIROWC[i] = false;
                AIROWD[i] = false;
                AIROWE[i] = false;
                AIROWF[i] = false;

                //set AICOL wins comb
                AICOL1[i] = false;
                AICOL2[i] = false;
                AICOL3[i] = false;
                AICOL4[i] = false;
                AICOL5[i] = false;
                AICOL6[i] = false;

                //set AIDIagonal win comb
                AIDI1[i] = false;
                AIDI2[i] = false;
                AIDI3[i] = false;
                AIDI4[i] = false;
                AIDI5[i] = false;
                AIDI6[i] = false;

            }

            for (int i = 0; i < 36; i++)
            {
                checkDiagonal(true);
                checkVerticalScore(true);
                checkHorizontal(true);
            }

        }

        //function to highlight labels with mouse hovering over it
        private void highlightLabel(object sender, MouseEventArgs e)
        {
            Label selectedLabel = sender as Label;
            selectedLabel.Foreground = new SolidColorBrush(Colors.Blue);
            Mouse.OverrideCursor = Cursors.Hand;
            selectedLabel.FontSize += 10;
            selectedLabel.FontWeight = FontWeights.SemiBold;

        }

        //function to unhighlight labels when mouse leaves them
        private void unhighlightLabel(object sender, MouseEventArgs e)
        {
            Label selectedLabel = sender as Label;
            selectedLabel.Foreground = new SolidColorBrush(Colors.White);
            Mouse.OverrideCursor = Cursors.Arrow;
            selectedLabel.FontSize -= 10;
            selectedLabel.FontWeight = FontWeights.Regular;

        }

        private void selectPlayer1Action(object sender, MouseButtonEventArgs e)
        {
            playerOneTurn = true;
            selectWhoStartsPanel.Visibility = Visibility.Hidden;
            playerOneIndicator.Visibility = Visibility.Visible;

        }

        private void selectPlayer2Action(object sender, MouseButtonEventArgs e)
        {
            playerOneTurn = false;
            selectWhoStartsPanel.Visibility = Visibility.Hidden;
            playerTwoIndicatior.Visibility = Visibility.Visible;

            if (player2.getUsername() == "CPU")
                AI();
        }

        //Exit game button prompts message and options
        private void exitGameMessage(object sender, MouseButtonEventArgs e)
        {
            if (moveCounter < 36)
            {
                //If it is press when player 1 turn is going on
                if (playerOneTurn)
                {
                    //If player 1 is a guest
                    if (player1.isGuest())
                    {
                        //If player 2 is a not guest and it is not CPU display that it will add a win point to player 2
                        if (!player2.isGuest() && player2.getUsername() != "CPU")
                        {
                            exitMessLabel2.Content = "A winning point will be added to " + player2.getUsername() + ".";
                        }
                        //If player 2 is a guest or CPU no message needed
                        else
                        {
                            exitMessLabel2.Visibility = Visibility.Hidden;
                        }
                    }
                    //if player 1 is logged in
                    else
                    {
                        //If player 2 is not a guest and it is not the cpu display it will add a lose point to player 1 and that it will add a win point to player 2
                        if (!player2.isGuest() && player2.getUsername() != "CPU")
                        {
                            exitMessLabel2.Content = "A losing point will be added to " + player1.getUsername() + ".\nA winning point will be added to " + player2.getUsername() + ".";
                        }
                        //If player 2 is a guest or the CPU it will add a losing point to player 1
                        else
                        {
                            exitMessLabel2.Content = "A losing point will be added to " + player1.getUsername() + ".";
                        }

                    }
                }
                //If it press when player 2 turn is going on
                else
                {
                    //if player 2 is cpu
                    if (player2.getUsername() == "CPU")
                    {

                        if (!player1.isGuest())
                        {
                            exitMessLabel2.Content = "A losing point will be added to " + player1.getUsername() + ".";
                        }
                        else
                        {
                            exitMessLabel2.Visibility = Visibility.Hidden;
                        }
                    }
                    //if player 2 is guest
                    else if (player2.isGuest())
                    {
                        if (player1.isGuest())
                        {
                            exitMessLabel2.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            exitMessLabel2.Content = "A winning point will be added to " + player1.getUsername() + ".";
                        }
                    }
                    //if player 2 is logged in
                    else
                    {
                        //If player 1 is not a guest it will add a lose point to player 2 and add a win point to player 1
                        if (!player1.isGuest())
                        {
                            exitMessLabel2.Content = "A losing point will be added to " + player2.getUsername() + ".\nA winning point will be added to " + player1.getUsername() + ".";
                        }
                        else
                        {
                            exitMessLabel2.Content = "A losing point will be added to " + player2.getUsername() + ".";
                        }
                    }

                }

                exitGamePrompt.Visibility = Visibility.Visible;
                exitButton.Visibility = Visibility.Hidden;
            }
            else
            {
                displayPlayerStats.Visibility = Visibility.Visible;
                exitButton.Visibility = Visibility.Hidden;
            }

        }

        //Function to exit the game window
        private void exitGame(object sender, MouseButtonEventArgs e)
        {
            endGame();
            exitGamePrompt.Visibility = Visibility.Hidden;
            moveCounter = 36;
        }

        //End game method when game is not over and exited in the middle of the game
        private void endGame()
        {

            //If it is pressed when player 1 turn is going on
            if (playerOneTurn)
            {
                //If player 1 is a guest
                if (player1.isGuest())
                {
                    //If player 2 is a not guest and it is not CPU it will add a win point to player 2
                    if (!player2.isGuest() && player2.getUsername() != "CPU")
                    {
                        //add winning point to player 2
                        addWinningPoint(player2);

                        //set stats for player 2
                        setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);

                        guestPlayerCpuStats();
                        setWinner();

                        //display everything
                        displayPlayerStats.Visibility = Visibility.Visible;

                    }
                    //If player 2 is a guest or CPU 
                    else
                    {
                        guestPlayerCpuStats();
                        setWinner();

                        //exit back to main menu since no information is display
                        displayPlayerStats.Visibility = Visibility.Visible;


                    }

                }
                //if player 1 is logged in
                else
                {
                    //If player 2 is not a guest and it is not the cpu display it will add a lose point to player 1 and that it will add a win point to player 2
                    if (!player2.isGuest() && player2.getUsername() != "CPU")
                    {
                        //A losing point will be added to player1
                        addLosingPoint(player1);

                        //A winning point will be added to player2
                        addWinningPoint(player2);

                        //set labels
                        setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);
                        setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);
                        setWinner();

                        //display panel and stats
                        displayPlayerStats.Visibility = Visibility.Visible;


                    }
                    //If player 2 is a guest or the CPU it will add a losing point to player 1
                    else
                    {
                        //A losing point will be added to player1
                        addLosingPoint(player1);

                        //set labels
                        setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);
                        guestPlayerCpuStats();
                        setWinner();

                        //display panel and stats
                        displayPlayerStats.Visibility = Visibility.Visible;

                    }

                }
            }
            //If its pressed when player 2 turn is going on
            else
            {
                //if player 2 is cpu
                if (player2.getUsername() == "CPU")
                {
                    //if player 1 is not a guest
                    if (!player1.isGuest())
                    {
                        //A losing point will be added to player1
                        addLosingPoint(player1);

                        //sets the stats labels 
                        setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);
                        guestPlayerCpuStats();
                        setWinner();

                        //display everything
                        displayPlayerStats.Visibility = Visibility.Visible;


                    }
                    else
                    {
                        guestPlayerCpuStats();
                        setWinner();

                        //exit the gamewindow and open the main Meny window
                        displayPlayerStats.Visibility = Visibility.Visible;

                    }
                }
                //if player 2 is guest
                else if (player2.isGuest())
                {
                    //if player 1 is a guest
                    if (player1.isGuest())
                    {
                        guestPlayerCpuStats();
                        setWinner();

                        //exit the gamewindow and open the main Meny window
                        displayPlayerStats.Visibility = Visibility.Visible;
                    }
                    //if player 1 is not a guest
                    else
                    {
                        //A winning point will be added to player1
                        addWinningPoint(player1);

                        //set labels
                        setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);
                        guestPlayerCpuStats();
                        setWinner();

                        //display everything
                        displayPlayerStats.Visibility = Visibility.Visible;

                    }
                }
                //if player 2 is logged in
                else
                {
                    //If player 1 is not a guest it will add a lose point to player 2 and add a win point to player 1
                    if (!player1.isGuest())
                    {
                        //A losing point will be added to player2
                        addLosingPoint(player2);

                        //A winning point will be added to player1
                        addWinningPoint(player1);

                        //set stats label
                        setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);
                        setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);
                        setWinner();

                        //display panel and stats
                        displayPlayerStats.Visibility = Visibility.Visible;


                    }
                    //If player 1 is guest
                    else
                    {
                        //A losing point will be added to player2
                        addLosingPoint(player2);

                        //set stats
                        setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);
                        guestPlayerCpuStats();
                        setWinner();

                        //display everything
                        displayPlayerStats.Visibility = Visibility.Visible;

                    }
                }

            }
        }

        //End of game when all moves are done and the board is full GameOver
        private void gameOver()
        {

            //if player 1 won
            if (player1.getScore() > player2.getScore())
            {

                //if player 1 is not a guest
                if (!player1.isGuest())
                {
                    //add winning point to player 1
                    addWinningPoint(player1);

                    //set stats for player 1
                    setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);

                }
                //else if player 1 is a guest
                else
                {
                    //set guest player stats
                    guestPlayerCpuStats();
                }

                //if player 2 is not a guest and is not CPU
                if (!player2.isGuest() && player2.getUsername() != "CPU")
                {
                    //add losing point to player 2
                    addLosingPoint(player2);

                    //set the stats
                    setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);

                }
                //if player 2 is a guest or cpu
                else
                {
                    //set guest player or cpu stats
                    guestPlayerCpuStats();
                }
            }
            //if player 2 won
            else if (player2.getScore() > player1.getScore())
            {
                //if player 1 is not a guest
                if (!player1.isGuest())
                {
                    //add losing point to player 1
                    addLosingPoint(player1);

                    //set player 1 stats
                    setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);


                }
                //else if player 1 is a guest
                else
                {
                    //set guest player or cpu stats
                    guestPlayerCpuStats();
                }

                //if player 2 is not a guest and is not CPU
                if (!player2.isGuest() && player2.getUsername() != "CPU")
                {
                    //add winning plint to player 2
                    addWinningPoint(player2);

                    //set the stats
                    setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);

                }
                //if player 2 is a guest or cpu
                else
                {
                    //set guest player or cpu stats
                    guestPlayerCpuStats();
                }
            }
            //if it was a tie game
            else
            {
                //if player 1 is not a guest
                if (!player1.isGuest())
                {
                    //add tie point to player1
                    addTiePoint(player1);

                    //set player 1 stats
                    setStats(player1, player1StatsLabel, player1StatsScoreLabel, player1StatsWinsLabel, player1StatsTiesLabel, player1StatsLosesLabel, player1StatsHighestScoreLabel);

                }
                //else if player 1 is a guest
                else
                {
                    //set guest player or cpu stats
                    guestPlayerCpuStats();
                }

                //if player 2 is not a guest and is not CPU
                if (!player2.isGuest() && player2.getUsername() != "CPU")
                {
                    //add tie ppint to player 2
                    addTiePoint(player2);

                    //set the stats
                    setStats(player2, player2StatsLabel, player2StatsScoreLabel, player2StatsWinsLabel, player2StatsTiesLabel, player2StatsLosesLabel, player2StatsHighestScoreLabel);

                }
                //if player 2 is a guest or cpu
                else
                {
                    //set guest player or cpu stats
                    guestPlayerCpuStats();
                }
            }

            //set winner();
            setWinner();

            //display everything
            displayPlayerStats.Visibility = Visibility.Visible;
            exitButton.Visibility = Visibility.Hidden;
        }

        //Set winner in stats label
        private void setWinner()
        {
            if (moveCounter == 36)
            {
                if (player1.getScore() > player2.getScore())
                {
                    player1StatsLabel.Content += " Wins!";
                    player2StatsLabel.Content += " Loses!";
                }
                else if (player2.getScore() > player1.getScore())
                {
                    player1StatsLabel.Content += " Loses!";
                    player2StatsLabel.Content += " Wins!";
                }
                else if (player1.getScore() == player2.getScore())
                {
                    player1StatsLabel.Content += " Tie";
                    player2StatsLabel.Content += " Tie";
                }
            }
            else
            {
                if (playerOneTurn)
                {
                    player1StatsLabel.Content += " Loses!";
                    player2StatsLabel.Content += " Wins!";
                }
                else
                {
                    if (player2.getUsername() != "CPU")
                    {
                        player1StatsLabel.Content += " Wins!";
                        player2StatsLabel.Content += " Loses!";
                    }
                    else
                    {
                        player1StatsLabel.Content += " Loses!";
                        player2StatsLabel.Content += " Wins!";
                    }
                }
            }


        }

        //Set Guest player stats
        private void guestPlayerCpuStats()
        {
            if (player1.isGuest())
            {
                player1StatsLabel.Content = player1.getUsername();
                player1StatsScoreLabel.Content = "Score: " + player1.getScore();
                player1StatsWinsLabel.Visibility = Visibility.Hidden;
                player1StatsLosesLabel.Visibility = Visibility.Hidden;
                player1StatsTiesLabel.Visibility = Visibility.Hidden;
                player1StatsHighestScoreLabel.Visibility = Visibility.Hidden;


            }

            if (player2.isGuest() || player2.getUsername() == "CPU")
            {
                player2StatsLabel.Content = player2.getUsername();
                player2StatsScoreLabel.Content = "Score: " + player2.getScore();
                player2StatsWinsLabel.Visibility = Visibility.Hidden;
                player2StatsTiesLabel.Visibility = Visibility.Hidden;
                player2StatsLosesLabel.Visibility = Visibility.Hidden;
                player2StatsHighestScoreLabel.Visibility = Visibility.Hidden;
            }
        }

        //Function to add a winning point to the database
        private void addWinningPoint(User player)
        {
            //Connect to the database
            SQLiteConnection conn = new SQLiteConnection(sqlconnection);

            //open connection
            conn.Open();

            //sql query to be perform
            string sql = "SELECT * FROM " + difficultyStr + "Score WHERE username='" + player.getUsername() + "'";

            //create a command
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            //execute command
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool found = reader.Read();//true if found false if not

            conn.Close();

            if (found)
            {
                //open connection
                conn.Open();

                //Update the winning column for the user
                sql = "UPDATE " + difficultyStr + "Score SET wins = wins + 1 WHERE username='" + player.getUsername() + "'";

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                //Now lets check if the highest score needs to be updated for that table
                sql = "UPDATE " + difficultyStr + "Score SET highestScore = " + player.getScore() + " WHERE username='" +
                    player.getUsername() + "' AND highestScore<" + player.getScore();

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            else
            {
                conn.Open();

                //insert the user into table
                sql = "INSERT INTO " + difficultyStr + "Score (username, wins, loses, highestScore, ties) VALUES('" +
                    player.getUsername() + "', 1, 0, " + player.getScore() + ", 0)";

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();
            }


        }

        //Function to add tie point to the database
        private void addTiePoint(User player)
        {
            //Connect to the database
            SQLiteConnection conn = new SQLiteConnection(sqlconnection);

            //open connection
            conn.Open();

            //sql query to be perform
            string sql = "SELECT * FROM " + difficultyStr + "Score WHERE username='" + player.getUsername() + "'";

            //create a command
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            //execute command
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool found = reader.Read();//true if found false if not

            conn.Close();

            if (found)
            {
                //open connection
                conn.Open();

                //Update the winning column for the user
                sql = "UPDATE " + difficultyStr + "Score SET ties = ties + 1 WHERE username='" + player.getUsername() + "'";

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                //Now lets check if the highest score needs to be updated for that table
                sql = "UPDATE " + difficultyStr + "Score SET highestScore = " + player.getScore() + " WHERE username='" +
                    player.getUsername() + "' AND highestScore<" + player.getScore();

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            else
            {
                conn.Open();

                //insert the user into table
                sql = "INSERT INTO " + difficultyStr + "Score (username, wins, loses, highestScore, ties) VALUES('" +
                    player.getUsername() + "', 0, 0, " + player.getScore() + ", 1)";

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        //Function to add a losing point to the database
        private void addLosingPoint(User player)
        {
            //Connect to the database
            SQLiteConnection conn = new SQLiteConnection(sqlconnection);

            //open connection
            conn.Open();

            //sql query to be perform
            string sql = "SELECT * FROM " + difficultyStr + "Score WHERE username='" + player.getUsername() + "'";

            //create a command
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            //execute command           
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool found = reader.Read();

            conn.Close();

            if (found)
            {
                //Open connection
                conn.Open();

                //Update the winning column for the user
                sql = "UPDATE " + difficultyStr + "Score SET loses = loses + 1 WHERE username='" + player.getUsername() + "'";

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                //Now lets check if the highest score needs to be updated for that table
                sql = "UPDATE " + difficultyStr + "Score SET highestScore = " + player.getScore() + " WHERE username='" +
                    player.getUsername() + "' AND highestScore<" + player.getScore();

                //create command
                cmd = new SQLiteCommand(sql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            else
            {
                //insert the user into table
                conn.Open();

                //create sql
                string addsql = "INSERT INTO " + difficultyStr + "Score (username, wins, loses, highestScore, ties) VALUES('" +
                    player.getUsername() + "', 0, 1, " + player.getScore().ToString() + ", 0)";

                //create command
                cmd = new SQLiteCommand(addsql, conn);

                //execute command
                cmd.ExecuteNonQuery();

                conn.Close();

            }

        }

        //Function for button to go back to Main Menu
        private void backToMainMenu(object sender, MouseButtonEventArgs e)
        {
            MainWindow menuWindow = new MainWindow(true);
            menuWindow.Show();
            this.Close();
        }

        //If no option is selected when exit game menu is prompted
        private void backToGame(object sender, MouseButtonEventArgs e)
        {
            exitGamePrompt.Visibility = Visibility.Hidden;
            exitButton.Visibility = Visibility.Visible;
        }

        //Set the stats for player
        private void setStats(User player, object playerSender, object scoreSender, object winsSender, object tiesSender, object loseSender, object highestScoreSender)
        {
            Label playerLabel = playerSender as Label;
            Label scoreLabel = scoreSender as Label;
            Label winsLabel = winsSender as Label;
            Label tiesLabel = tiesSender as Label;
            Label loseLabel = loseSender as Label;
            Label highScoreLabel = highestScoreSender as Label;

            //Connect to the database
            SQLiteConnection conn = new SQLiteConnection(sqlconnection);

            //open connection
            conn.Open();

            //sql query to be perform
            string sql = "SELECT * FROM " + difficultyStr + "Score WHERE username='" + player.getUsername() + "'";

            //create a command
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            //execute command
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                playerLabel.Content = reader["username"] + ":";
                scoreLabel.Content = "Score: " + player.getScore();
                winsLabel.Content = "Wins: " + reader["wins"];
                tiesLabel.Content = "Ties: " + reader["ties"];
                loseLabel.Content = "Loses: " + reader["loses"];
                highScoreLabel.Content = "Highest Score: " + reader["highestScore"];
            }

            conn.Close();
        }

        //Place mark on board "Button click action"
        private void placeMark(object sender, RoutedEventArgs e)
        {
            Button selectedButton = sender as Button;

            if (playerOneTurn && selectedButton.Content.ToString() == "")
            {
                selectedButton.Content = "X";
                selectedButton.FontSize = 60;
                selectedButton.FontWeight = FontWeights.SemiBold;
                selectedButton.Foreground = new SolidColorBrush(Colors.DarkGray);

                //Change players
                changePlayers();

                if (player2.getUsername() == "CPU" && moveCounter < 36)
                {
                    AI();
                }
            }
            else if (!playerOneTurn && selectedButton.Content.ToString() == "")
            {
                //If player2 is not the CPU
                if (player2.getUsername() != "CPU")
                {
                    selectedButton.Content = "O";
                    selectedButton.FontSize = 60;
                    selectedButton.Foreground = new SolidColorBrush(Colors.LightGray);
                    selectedButton.FontWeight = FontWeights.SemiBold;

                    //Change Players
                    changePlayers();
                }

            }


            //Check if the game is over
            if (moveCounter == 36)
            {
                gameOver();
            }

        }

        //Change player indicator
        private void changePlayerIndicator()
        {
            if (playerOneTurn)
            {
                playerTwoIndicatior.Visibility = Visibility.Hidden;
                playerOneIndicator.Visibility = Visibility.Visible;
            }
            else
            {
                playerTwoIndicatior.Visibility = Visibility.Visible;
                playerOneIndicator.Visibility = Visibility.Hidden;
            }
        }

        //Change players
        private void changePlayers()
        {
            //Increment move counter
            moveCounter++;

            //Check if the user score
            checkScore();

            //Update buttons array
            updateButtons();

            //Set the AI arrays
            setAIArrays();

            //Switch players
            if (playerOneTurn)
            {
                playerOneTurn = false;
            }
            else
            {
                playerOneTurn = true;
            }

            //Change player indicator
            changePlayerIndicator();
        }

        //Check score of game
        private void checkScore()
        {
            for (int i = 0; i < 18; i++)
            {
                checkIfUserScore(checkHorizontal(false));
                checkIfUserScore(checkVerticalScore(false));
                checkIfUserScore(checkDiagonal(false));
            }
        }

        //Check if user scored
        private void checkIfUserScore(bool score)
        {
            //If somebody scores then add a point to the player
            if (score)
            {
                if (playerOneTurn)
                {
                    player1.addScore();
                    score1Label.Content = player1.getScore();
                }
                else
                {
                    player2.addScore();
                    score2Label.Content = player2.getScore();
                }
            }
        }



        //Check for vertical score
        private bool checkVerticalScore(bool AICheck)
        {
            //VERTICAL CHECK
            // COL1
            if (A1.Content != "" && B1.Content != "" && C1.Content != "" && D1.Content != "")
            {
                if ((A1.Content == B1.Content) && (B1.Content == C1.Content) && (C1.Content == D1.Content) && !COL1[0])
                {
                    if (AICheck)
                        return AICOL1[0] = true;
                    else
                        return COL1[0] = true;
                }
            }
            if (E1.Content != "" && B1.Content != "" && C1.Content != "" && D1.Content != "")
            {
                if ((B1.Content == C1.Content) && (C1.Content == D1.Content) && (D1.Content == E1.Content) && !COL1[1])
                {
                    if (AICheck)
                        return AICOL1[1] = true;
                    else
                        return COL1[1] = true;
                }
            }
            if (E1.Content != "" && F1.Content != "" && C1.Content != "" && D1.Content != "")
            {
                if ((C1.Content == D1.Content) && (D1.Content == E1.Content) && (E1.Content == F1.Content) && !COL1[2])
                {
                    if (AICheck)
                        return AICOL1[2] = true;
                    else
                        return COL1[2] = true;
                }
            }

            // COL2
            if (A2.Content != "" && B2.Content != "" && C2.Content != "" && D2.Content != "")
            {
                if ((A2.Content == B2.Content) && (B2.Content == C2.Content) && (C2.Content == D2.Content) && !COL2[0])
                {
                    if (AICheck)
                        return AICOL2[0] = true;
                    else
                        return COL2[0] = true;
                }
            }
            if (E2.Content != "" && B2.Content != "" && C2.Content != "" && D2.Content != "")
            {
                if ((B2.Content == C2.Content) && (C2.Content == D2.Content) && (D2.Content == E2.Content) && !COL2[1])
                {
                    if (AICheck)
                        return AICOL2[1] = true;
                    else
                        return COL2[1] = true;
                }
            }
            if (E2.Content != "" && F2.Content != "" && C2.Content != "" && D2.Content != "")
            {
                if ((C2.Content == D2.Content) && (D2.Content == E2.Content) && (E2.Content == F2.Content) && !COL2[2])
                {
                    if (AICheck)
                        return AICOL2[2] = true;
                    else
                        return COL2[2] = true;
                }
            }

            // COL3
            if (A3.Content != "" && B3.Content != "" && C3.Content != "" && D3.Content != "")
            {
                if ((A3.Content == B3.Content) && (B3.Content == C3.Content) && (C3.Content == D3.Content) && !COL3[0])
                {
                    if (AICheck)
                        return AICOL3[0] = true;
                    else
                        return COL3[0] = true;
                }
            }
            if (E3.Content != "" && B3.Content != "" && C3.Content != "" && D3.Content != "")
            {
                if ((B3.Content == C3.Content) && (C3.Content == D3.Content) && (D3.Content == E3.Content) && !COL3[1])
                {
                    if (AICheck)
                        return AICOL3[1] = true;
                    else
                        return COL3[1] = true;
                }
            }
            if (E3.Content != "" && F3.Content != "" && C3.Content != "" && D3.Content != "")
            {
                if ((C3.Content == D3.Content) && (D3.Content == E3.Content) && (E3.Content == F3.Content) && !COL3[2])
                {
                    if (AICheck)
                        return AICOL3[2] = true;
                    else
                        return COL3[2] = true;
                }
            }

            //COL4
            if (A4.Content != "" && B4.Content != "" && C4.Content != "" && D4.Content != "")
            {
                if ((A4.Content == B4.Content) && (B4.Content == C4.Content) && (C4.Content == D4.Content) && !COL4[0])
                {
                    if (AICheck)
                        return AICOL4[0] = true;
                    else
                        return COL4[0] = true;
                }
            }
            if (E4.Content != "" && B4.Content != "" && C4.Content != "" && D4.Content != "")
            {
                if ((B4.Content == C4.Content) && (C4.Content == D4.Content) && (D4.Content == E4.Content) && !COL4[1])
                {
                    if (AICheck)
                        return AICOL4[1] = true;
                    else
                        return COL4[1] = true;
                }
            }
            if (E4.Content != "" && F4.Content != "" && C4.Content != "" && D4.Content != "")
            {
                if ((C4.Content == D4.Content) && (D4.Content == E4.Content) && (E4.Content == F4.Content) && !COL4[2])
                {
                    if (AICheck)
                        return AICOL4[2] = true;
                    else
                        return COL4[2] = true;
                }
            }

            //COL5
            if (A5.Content != "" && B5.Content != "" && C5.Content != "" && D5.Content != "")
            {
                if ((A5.Content == B5.Content) && (B5.Content == C5.Content) && (C5.Content == D5.Content) && !COL5[0])
                {
                    if (AICheck)
                        return AICOL5[0] = true;
                    else
                        return COL5[0] = true;
                }
            }
            if (E5.Content != "" && B5.Content != "" && C5.Content != "" && D5.Content != "")
            {
                if ((B5.Content == C5.Content) && (C5.Content == D5.Content) && (D5.Content == E5.Content) && !COL5[1])
                {
                    if (AICheck)
                        return AICOL5[1] = true;
                    else
                        return COL5[1] = true;
                }
            }
            if (E5.Content != "" && F5.Content != "" && C5.Content != "" && D5.Content != "")
            {
                if ((C5.Content == D5.Content) && (D5.Content == E5.Content) && (E5.Content == F5.Content) && !COL5[2])
                {
                    if (AICheck)
                        return AICOL5[2] = true;
                    else
                        return COL5[2] = true;
                }
            }

            //COL6
            if (A6.Content != "" && B6.Content != "" && C6.Content != "" && D6.Content != "")
            {
                if ((A6.Content == B6.Content) && (B6.Content == C6.Content) && (C6.Content == D6.Content) && !COL6[0])
                {
                    if (AICheck)
                        return AICOL6[0] = true;
                    else
                        return COL6[0] = true;
                }
            }
            if (E6.Content != "" && B6.Content != "" && C6.Content != "" && D6.Content != "")
            {
                if ((B6.Content == C6.Content) && (C6.Content == D6.Content) && (D6.Content == E6.Content) && !COL6[1])
                {
                    if (AICheck)
                        return AICOL6[1] = true;
                    else
                        return COL6[1] = true;
                }
            }
            if (E6.Content != "" && F6.Content != "" && C6.Content != "" && D6.Content != "")
            {
                if ((C6.Content == D6.Content) && (D6.Content == E6.Content) && (E6.Content == F6.Content) && !COL6[2])
                {
                    if (AICheck)
                        return AICOL6[2] = true;
                    else
                        return COL6[2] = true;
                }
            }

            return false;
        }

        //Check for horizontal score
        private bool checkHorizontal(bool AICheck)
        {
            //HORIZONTAL CHECK

            //ROW A
            if (A1.Content != "" && A2.Content != "" && A3.Content != "" && A4.Content != "")
            {
                if ((A1.Content == A2.Content) && (A2.Content == A3.Content) && (A3.Content == A4.Content) && !ROWA[0])
                {
                    if (AICheck)
                        return AIROWA[0] = true;
                    else
                        return ROWA[0] = true;
                }
            }

            if (A2.Content != "" && A3.Content != "" && A4.Content != "" && A5.Content != "")
            {
                if ((A2.Content == A3.Content) && (A3.Content == A4.Content) && (A4.Content == A5.Content) && !ROWA[1])
                {
                    if (AICheck)
                        return AIROWA[1] = true;
                    else
                        return ROWA[1] = true;
                }
            }

            if (A3.Content != "" && A4.Content != "" && A5.Content != "" && A6.Content != "")
            {
                if ((A3.Content == A4.Content) && (A4.Content == A5.Content) && (A5.Content == A6.Content) && !ROWA[2])
                {
                    if (AICheck)
                        return AIROWA[2] = true;
                    else
                        return ROWA[2] = true;
                }
            }

            // ROW B
            if (B1.Content != "" && B2.Content != "" && B3.Content != "" && B4.Content != "")
            {
                if ((B1.Content == B2.Content) && (B2.Content == B3.Content) && (B3.Content == B4.Content) && !ROWB[0])
                {
                    if (AICheck)
                        return AIROWB[0] = true;
                    else
                        return ROWB[0] = true;
                }
            }

            if (B5.Content != "" && B2.Content != "" && B3.Content != "" && B4.Content != "")
            {
                if ((B2.Content == B3.Content) && (B3.Content == B4.Content) && (B4.Content == B5.Content) && !ROWB[1])
                {
                    if (AICheck)
                        return AIROWB[1] = true;
                    else
                        return ROWB[1] = true;
                }
            }

            if (B6.Content != "" && B5.Content != "" && B3.Content != "" && B4.Content != "")
            {
                if ((B3.Content == B4.Content) && (B4.Content == B5.Content) && (B5.Content == B6.Content) && !ROWB[2])
                {
                    if (AICheck)
                        return AIROWB[2] = true;
                    else
                        return ROWB[2] = true;
                }
            }

            // ROW C
            if (C1.Content != "" && C2.Content != "" && C3.Content != "" && C4.Content != "")
            {
                if ((C1.Content == C2.Content) && (C2.Content == C3.Content) && (C3.Content == C4.Content) && !ROWC[0])
                {
                    if (AICheck)
                        return AIROWC[0] = true;
                    else
                        return ROWC[0] = true;
                }
            }

            if (C5.Content != "" && C2.Content != "" && C3.Content != "" && C4.Content != "")
            {
                if ((C2.Content == C3.Content) && (C3.Content == C4.Content) && (C4.Content == C5.Content) && !ROWC[1])
                {
                    if (AICheck)
                        return AIROWC[1] = true;
                    else
                        return ROWC[1] = true;
                }
            }

            if (C5.Content != "" && C6.Content != "" && C3.Content != "" && C4.Content != "")
            {
                if ((C3.Content == C4.Content) && (C4.Content == C5.Content) && (C5.Content == C6.Content) && !ROWC[2])
                {
                    if (AICheck)
                        return AIROWC[2] = true;
                    else
                        return ROWC[2] = true;
                }
            }

            // ROW D
            if (D1.Content != "" && D2.Content != "" && D3.Content != "" && D4.Content != "")
            {
                if ((D1.Content == D2.Content) && (D2.Content == D3.Content) && (D3.Content == D4.Content) && !ROWD[0])
                {
                    if (AICheck)
                        return AIROWD[0] = true;
                    else
                        return ROWD[0] = true;
                }
            }

            if (D5.Content != "" && D2.Content != "" && D3.Content != "" && D4.Content != "")
            {
                if ((D2.Content == D3.Content) && (D3.Content == D4.Content) && (D4.Content == D5.Content) && !ROWD[1])
                {
                    if (AICheck)
                        return AIROWD[1] = true;
                    else
                        return ROWD[1] = true;
                }
            }

            if (D5.Content != "" && D6.Content != "" && D3.Content != "" && D4.Content != "")
            {
                if ((D3.Content == D4.Content) && (D4.Content == D5.Content) && (D5.Content == D6.Content) && !ROWD[2])
                {
                    if (AICheck)
                        return AIROWD[2] = true;
                    else
                        return ROWD[2] = true;
                }
            }

            // ROW E
            if (E1.Content != "" && E2.Content != "" && E3.Content != "" && E4.Content != "")
            {
                if ((E1.Content == E2.Content) && (E2.Content == E3.Content) && (E3.Content == E4.Content) && !ROWE[0])
                {
                    if (AICheck)
                        return AIROWE[0] = true;
                    else
                        return ROWE[0] = true;
                }
            }

            if (E5.Content != "" && E2.Content != "" && E3.Content != "" && E4.Content != "")
            {
                if ((E2.Content == E3.Content) && (E3.Content == E4.Content) && (E4.Content == E5.Content) && !ROWE[1])
                {
                    if (AICheck)
                        return AIROWE[1] = true;
                    else
                        return ROWE[1] = true;
                }
            }

            if (E5.Content != "" && E6.Content != "" && E3.Content != "" && E4.Content != "")
            {
                if ((E3.Content == E4.Content) && (E4.Content == E5.Content) && (E5.Content == E6.Content) && !ROWE[2])
                {
                    if (AICheck)
                        return AIROWE[2] = true;
                    else
                        return ROWE[2] = true;
                }
            }

            // ROW F
            if (F1.Content != "" && F2.Content != "" && F3.Content != "" && F4.Content != "")
            {
                if ((F1.Content == F2.Content) && (F2.Content == F3.Content) && (F3.Content == F4.Content) && !ROWF[0])
                {
                    if (AICheck)
                        return AIROWF[0] = true;
                    else
                        return ROWF[0] = true;
                }
            }

            if (F5.Content != "" && F2.Content != "" && F3.Content != "" && F4.Content != "")
            {
                if ((F2.Content == F3.Content) && (F3.Content == F4.Content) && (F4.Content == F5.Content) && !ROWF[1])
                {
                    if (AICheck)
                        return AIROWF[1] = true;
                    else
                        return ROWF[1] = true;
                }
            }

            if (F5.Content != "" && F6.Content != "" && F3.Content != "" && F4.Content != "")
            {
                if ((F3.Content == F4.Content) && (F4.Content == F5.Content) && (F5.Content == F6.Content) && !ROWF[2])
                {
                    if (AICheck)
                        return AIROWF[2] = true;
                    else
                        return ROWF[2] = true;
                }
            }

            return false;

        }

        //Check for diagonal score
        private bool checkDiagonal(bool AICheck)
        {
            //Diagonal
            //A
            if (A1.Content != "" && B2.Content != "" && C3.Content != "" && D4.Content != "")
            {
                if ((A1.Content == B2.Content) && (B2.Content == C3.Content) && (C3.Content == D4.Content) && !DI1[0])
                {
                    if (AICheck)
                        return AIDI1[0] = true;
                    else
                        return DI1[0] = true;
                }
            }
            if (A2.Content != "" && B3.Content != "" && C4.Content != "" && D5.Content != "")
            {
                if ((A2.Content == B3.Content) && (B3.Content == C4.Content) && (C4.Content == D5.Content) && !DI1[1])
                {
                    if (AICheck)
                        return AIDI1[1] = true;
                    else
                        return DI1[1] = true;
                }
            }
            if (A3.Content != "" && B4.Content != "" && C5.Content != "" && D6.Content != "")
            {
                if ((A3.Content == B4.Content) && (B4.Content == C5.Content) && (C5.Content == D6.Content) && !DI1[2])
                {
                    if (AICheck)
                        return AIDI1[2] = true;
                    else
                        return DI1[2] = true;
                }
            }

            // B
            if (B1.Content != "" && C2.Content != "" && D3.Content != "" && E4.Content != "")
            {
                if ((B1.Content == C2.Content) && (C2.Content == D3.Content) && (D3.Content == E4.Content) && !DI2[0])
                {
                    if (AICheck)
                        return AIDI2[0] = true;
                    else
                        return DI2[0] = true;
                }
            }
            if (B2.Content != "" && C3.Content != "" && D4.Content != "" && E5.Content != "")
            {
                if ((B2.Content == C3.Content) && (C3.Content == D4.Content) && (D4.Content == E5.Content) && !DI2[1])
                {
                    if (AICheck)
                        return AIDI2[1] = true;
                    else
                        return DI2[1] = true;
                }
            }
            if (B3.Content != "" && C4.Content != "" && D5.Content != "" && E6.Content != "")
            {
                if ((B3.Content == C4.Content) && (C4.Content == D5.Content) && (D5.Content == E6.Content) && !DI2[2])
                {
                    if (AICheck)
                        return AIDI2[2] = true;
                    else
                        return DI2[2] = true;
                }
            }

            // C
            if (C1.Content != "" && D2.Content != "" && E3.Content != "" && F4.Content != "")
            {
                if ((C1.Content == D2.Content) && (D2.Content == E3.Content) && (E3.Content == F4.Content) && !DI3[0])
                {
                    if (AICheck)
                        return AIDI3[0] = true;
                    else
                        return DI3[0] = true;
                }
            }
            if (C2.Content != "" && D3.Content != "" && E4.Content != "" && F5.Content != "")
            {
                if ((C2.Content == D3.Content) && (D3.Content == E4.Content) && (E4.Content == F5.Content) && !DI3[1])
                {
                    if (AICheck)
                        return AIDI3[1] = true;
                    else
                        return DI3[1] = true;
                }
            }
            if (C3.Content != "" && D4.Content != "" && E5.Content != "" && F6.Content != "")
            {
                if ((C3.Content == D4.Content) && (D4.Content == E5.Content) && (E5.Content == F6.Content) && !DI3[2])
                {
                    if (AICheck)
                        return AIDI3[2] = true;
                    else
                        return DI3[2] = true;
                }
            }

            //D
            if (D1.Content != "" && C2.Content != "" && B3.Content != "" && A4.Content != "")
            {
                if ((D1.Content == C2.Content) && (C2.Content == B3.Content) && (B3.Content == A4.Content) && !DI4[0])
                {
                    if (AICheck)
                        return AIDI4[0] = true;
                    else
                        return DI4[0] = true;
                }
            }
            if (D2.Content != "" && C3.Content != "" && B4.Content != "" && A5.Content != "")
            {
                if ((D2.Content == C3.Content) && (C3.Content == B4.Content) && (B4.Content == A5.Content) && !DI4[1])
                {
                    if (AICheck)
                        return AIDI4[1] = true;
                    else
                        return DI4[1] = true;
                }
            }
            if (D3.Content != "" && C4.Content != "" && B5.Content != "" && A6.Content != "")
            {
                if ((D3.Content == C4.Content) && (C4.Content == B5.Content) && (B5.Content == A6.Content) && !DI4[2])
                {
                    if (AICheck)
                        return AIDI4[2] = true;
                    else
                        return DI4[2] = true;
                }
            }

            // E
            if (E1.Content != "" && D2.Content != "" && C3.Content != "" && B4.Content != "")
            {
                if ((E1.Content == D2.Content) && (D2.Content == C3.Content) && (C3.Content == B4.Content) && !DI5[0])
                {
                    if (AICheck)
                        return AIDI5[0] = true;
                    else
                        return DI5[0] = true;
                }
            }
            if (E2.Content != "" && D3.Content != "" && C4.Content != "" && B5.Content != "")
            {
                if ((E2.Content == D3.Content) && (D3.Content == C4.Content) && (C4.Content == B5.Content) && !DI5[1])
                {
                    if (AICheck)
                        return AIDI5[1] = true;
                    else
                        return DI5[1] = true;
                }
            }
            if (E3.Content != "" && D4.Content != "" && C5.Content != "" && B6.Content != "")
            {
                if ((E3.Content == D4.Content) && (D4.Content == C5.Content) && (C5.Content == B6.Content) && !DI5[2])
                {
                    if (AICheck)
                        return AIDI5[2] = true;
                    else
                        return DI5[2] = true;
                }
            }

            // F
            if (F1.Content != "" && E2.Content != "" && D3.Content != "" && C4.Content != "")
            {
                if ((F1.Content == E2.Content) && (E2.Content == D3.Content) && (D3.Content == C4.Content) && !DI6[0])
                {
                    if (AICheck)
                        return AIDI6[0] = true;
                    else
                        return DI6[0] = true;
                }
            }
            if (F2.Content != "" && E3.Content != "" && D4.Content != "" && C5.Content != "")
            {
                if ((F2.Content == E3.Content) && (E3.Content == D4.Content) && (D4.Content == C5.Content) && !DI6[1])
                {
                    if (AICheck)
                        return AIDI6[1] = true;
                    else
                        return DI6[1] = true;
                }
            }
            if (F3.Content != "" && E4.Content != "" && D5.Content != "" && C6.Content != "")
            {
                if ((F3.Content == E4.Content) && (E4.Content == D5.Content) && (D5.Content == C6.Content) && !DI6[2])
                {
                    if (AICheck)
                        return AIDI6[2] = true;
                    else
                        return DI6[2] = true;
                }
            }

            return false;
        }

        //AI method
        private void AI()
        {
            //Check if the game is over
            if (moveCounter == 36)
            {
                gameOver();
            }
            else
            {


                if (difficulty == 1)
                {

                    //Execute easy AI
                    easyAI();

                    //Change players
                    changePlayers();
                }
                else if (difficulty == 2)
                {
                    int num;

                    Random rand = new Random();

                    num = rand.Next(9);

                    if (num < 3)
                    {
                        easyAI();
                    }
                    else
                    {
                        hardAI();
                    }

                    //Change Players
                    changePlayers();
                }
                else if (difficulty == 3)
                {
                    //Execute hard AI
                    hardAI();

                    //Change Players
                    changePlayers();
                }
            }
        }

        //Easy AI method
        private void easyAI()
        {
            int buttonNum = 0;
            bool empty = false;
            int size = buttons.Length;

            do
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i].Content == "")
                    {
                        buttonNum = i;
                    }

                }
                /*
            Random rand = new Random();
            if (buttons.Length > 1)
            {
                buttonNum = rand.Next(buttons.Length - 1);

            }
            else
                buttonNum = 0;
                */

                //Double Check
                if (buttons[buttonNum].Content == "")
                {
                    buttons[buttonNum].Content = "O";
                    buttons[buttonNum].FontSize = 60;
                    buttons[buttonNum].Foreground = new SolidColorBrush(Colors.LightGray);
                    buttons[buttonNum].FontWeight = FontWeights.SemiBold;

                    empty = true;

                }

                //Algorithim for resizing array
                /*
                //Store in temporary array buttons left
                Button[] temp = new Button[size - 1];
                int j = 0;
                for (int i = 0; i < size; i++)
                {
                    if (i != buttonNum)
                    {
                        temp[j] = buttons[i];
                        j++;
                    }
                }
                //Set new buttons array with new length and buttons from temporary buttons array
                buttons = new Button[size - 1];
                for (int i = 0; i < size - 1; i++)
                {
                    buttons[i] = temp[i];
                }
                //Reduce size by one
                size--;
                */

            } while (!empty);


        }

        //Hard Ai method
        private void hardAI()
        {
            computerMakeMove();
        }

        // Hard mode prioritizes what moves should be more releavant      
        private void computerMakeMove()
        {
            //AI for Hard difficulties is going to have different priorities if the first one succeeds
            //That is the one it is going to use

            //This bool will control the priorities
            // bool found = false;

            //this will store the location of the button in the array
            int location = 0;

            //this is the size of the buttons array
            int size = buttons.Length;

            bool empty = false;

            int moveCounter = 0;

            //priority 1: Score a Point
            //Priority 2: Block X from scoring 2 points
            //priority 3: Block X From scoring 1 point
            //priority 4: Secure Center squares
            //priority 5: Secure Corner Squares
            //priority 6: Open Space

            do
            {
                Button move = null;

                move = WinOrBlockTwoPoints("O"); //Block Two Point Possibilities
                if (move == null)
                {
                    move = WinOrBlockTwoPoints("X");//Score Two Point Possibilities
                    if (move == null)
                    {
                        move = WinOrBlock("O"); //Block One Point Possibilities
                        if (move == null)
                        {
                            move = WinOrBlock("X"); //Score One Point Possibilities
                            if (move == null)
                            {
                                move = ScoreOrBlockTriples("O");// Start blocking or getting XX_ or X_X
                                if (move == null)
                                {
                                    move = ScoreOrBlockTriples("X");
                                    if (move == null)
                                    {
                                        move = Center();//Secure Center Squares
                                        if (move == null)
                                        {
                                            move = Corner();//Secure corner
                                            if (move == null)
                                            {
                                                // move = Open();//Random

                                                // int buttonNum;

                                                do
                                                {

                                                    for (int i = 0; i < buttons.Length; i++)
                                                    {

                                                        if (buttons[i].Content == "")
                                                        {
                                                            move = buttons[i];
                                                            empty = true;
                                                        }
                                                    }                                   /*
                                                    Random rand = new Random();
                                                    if (moveCounter > buttons.Length)
                                                    {
                                                        buttonNum = rand.Next(buttons.Length - 1);

                                                    }
                                                    else
                                                        buttonNum = 0;

                                                    if (buttons[buttonNum].Content == "")
                                                    {
                                                        buttons[buttonNum].Content = "O";
                                                        buttons[buttonNum].FontSize = 60;
                                                        buttons[buttonNum].Foreground = new SolidColorBrush(Colors.LightGray);
                                                        buttons[buttonNum].FontWeight = FontWeights.SemiBold;

                                                        empty = true;

                                                    }
                                                        */

                                                } while (!empty);
                                            }//end if
                                        }//end if
                                    }//end if
                                }//end if
                            }//end if
                        }//end if
                    }//end if
                }


                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] == move)
                    {
                        location = i;
                        moveCounter++;
                    }
                    // else
                    //  {
                    //    MessageBox.Show("Error placing marker restart game;");
                    //  endGame();
                    //}

                }

                if (buttons[location].Content == "")
                {
                    buttons[location].Content = "O";
                    buttons[location].FontSize = 60;
                    buttons[location].Foreground = new SolidColorBrush(Colors.LightGray);
                    buttons[location].FontWeight = FontWeights.SemiBold;

                    // Debug 
                    //MessageBox.Show("buttons.Length is " + buttons.Length);
                    empty = true;
                }


                // algorithm to shift Button array
                /*
                //Store in temporary array buttons left
                Button[] temp = new Button[size - 1];
                int j = 0;
                for (int i = 0; i < size; i++)
                {
                    if (i != location)
                    {
                        temp[j] = buttons[i];
                        j++;
                    }
                }
                //Set new buttons array with new length and buttons from temporary buttons array
                buttons = new Button[size - 1];
                for (int i = 0; i < size - 1; i++)
                {
                    buttons[i] = temp[i];
                }
                //Reduce size by one
                size--;
*/
            } while (!empty);

        }//end computer move

        private Button WinOrBlockTwoPoints(string mark)
        {
            Console.WriteLine("Blocking or Scoring: 2 points");
            //1-5 Horizontal 
            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == ""))
                return A5;
            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == "") && (buttons[4].Content == mark))
                return A4;
            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == "") && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A3;
            if ((buttons[0].Content == mark) && (buttons[1].Content == "") && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A2;
            if ((buttons[0].Content == "") && (buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A1;
            //End row A1-A5
            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == ""))
                return B5;
            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == "") && (buttons[10].Content == mark))
                return B4;
            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == "") && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B3;
            if ((buttons[6].Content == mark) && (buttons[7].Content == "") && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B2;
            if ((buttons[6].Content == "") && (buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B1;
            //End row B1-B5

            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == ""))
                return C5;
            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == "") && (buttons[16].Content == mark))
                return C4;
            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == "") && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C3;
            if ((buttons[12].Content == mark) && (buttons[13].Content == "") && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C2;
            if ((buttons[12].Content == "") && (buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C1;
            //End row C1-C5

            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == "") && (buttons[22].Content == mark))
                return D4;
            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == "") && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D3;
            if ((buttons[18].Content == mark) && (buttons[19].Content == "") && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D2;
            if ((buttons[18].Content == "") && (buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D1;
            //End row D1-D5

            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == "") && (buttons[28].Content == mark))
                return E4;
            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == "") && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E3;
            if ((buttons[24].Content == mark) && (buttons[25].Content == "") && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E2;
            if ((buttons[24].Content == "") && (buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E1;
            //End row E1-E5

            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == "") && (buttons[34].Content == mark))
                return F4;
            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == "") && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F3;
            if ((buttons[30].Content == mark) && (buttons[31].Content == "") && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F2;
            if ((buttons[30].Content == "") && (buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F1;
            //End row F1-F4
            //end 1-5 Horizontal

            // 2-6 Horizontal
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == ""))
                return A6;
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == "") && (buttons[5].Content == mark))
                return A5;
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == "") && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A4;
            if ((buttons[1].Content == mark) && (buttons[2].Content == "") && (buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A3;
            if ((buttons[1].Content == "") && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A2;
            //End row A2-A6

            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == ""))
                return B6;
            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == "") && (buttons[11].Content == mark))
                return B5;
            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == "") && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B4;
            if ((buttons[7].Content == mark) && (buttons[8].Content == "") && (buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B3;
            if ((buttons[7].Content == "") && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B2;
            //End row B2-B6

            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == ""))
                return C6;
            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == "") && (buttons[17].Content == mark))
                return C5;
            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == "") && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C4;
            if ((buttons[13].Content == mark) && (buttons[14].Content == "") && (buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C3;
            if ((buttons[13].Content == "") && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C2;
            //End row C2-C6

            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == "") && (buttons[23].Content == mark))
                return D5;
            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == "") && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D4;
            if ((buttons[19].Content == mark) && (buttons[20].Content == "") && (buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D3;
            if ((buttons[19].Content == "") && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D2;
            //End row D2-D5

            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == "") && (buttons[29].Content == mark))
                return E5;
            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == "") && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E4;
            if ((buttons[25].Content == mark) && (buttons[26].Content == "") && (buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E3;
            if ((buttons[25].Content == "") && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E2;
            //End row E2-E6

            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == "") && (buttons[35].Content == mark))
                return F5;
            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == "") && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F4;
            if ((buttons[31].Content == mark) && (buttons[32].Content == "") && (buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F3;
            if ((buttons[31].Content == "") && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F2;
            //End row F2-f5
            //end 2-6 Horizontal

            //A-E Vertical
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == "") && (buttons[24].Content == mark))
                return D1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == "") && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return C1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == "") && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return B1;
            if ((buttons[0].Content == "") && (buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return A1;
            //End vertical A1,B1, C1, D1 and E1

            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == "") && (buttons[25].Content == mark))
                return D2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == "") && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return C2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == "") && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return B2;
            if ((buttons[1].Content == "") && (buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return A2;
            //End vertical A2,B2, C2, D2 and E2

            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[2].Content == "") && (buttons[26].Content == mark))
                return D3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return C3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == "") && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return B3;
            if ((buttons[2].Content == "") && (buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return A3;
            //End vertical A3,B3, C3, D3 and E3

            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == "") && (buttons[27].Content == mark))
                return D4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == "") && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return C4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == "") && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return B4;
            if ((buttons[3].Content == "") && (buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return A4;
            //End vertical A4,B4, C4, D4 and E4

            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == "") && (buttons[28].Content == mark))
                return D5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == "") && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return C5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == "") && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return B5;
            if ((buttons[4].Content == "") && (buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return A5;
            //End vertical A5,B5, C5, D5 and E5

            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == "") && (buttons[29].Content == mark))
                return D6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == "") && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return C6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == "") && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return B6;
            if ((buttons[5].Content == "") && (buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return A6;
            //End vertical A6,B6, C6, D6 and E6
            //end A-E Vertical

            //B-F Vertical
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == "") && (buttons[30].Content == mark))
                return E1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == "") && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return D1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == "") && (buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return C1;
            if ((buttons[6].Content == "") && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return B1;
            //End vertical B1, C1, D1, E1 and F1

            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == "") && (buttons[31].Content == mark))
                return E2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == "") && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return D2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == "") && (buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return C2;
            if ((buttons[7].Content == "") && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return B2;
            //End vertical B2, C2, D2, E2 and F2

            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == "") && (buttons[32].Content == mark))
                return E3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == "") && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return D3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return C3;
            if ((buttons[8].Content == "") && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return B3;
            //End vertical B3, C3, D3, E3 and F3

            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == "") && (buttons[33].Content == mark))
                return E4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == "") && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return D4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == "") && (buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return C4;
            if ((buttons[9].Content == "") && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return B4;
            //End vertical B4, C4, D4, E4 and F4

            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == "") && (buttons[34].Content == mark))
                return E5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == "") && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return D5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == "") && (buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return C5;
            if ((buttons[10].Content == "") && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return B5;
            //End vertical B5, C5, D5, E5 and F5

            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == "") && (buttons[35].Content == mark))
                return E6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[2].Content == "") && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return D6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == "") && (buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return C6;
            if ((buttons[11].Content == "") && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return B6;
            //End vertical B6, C6, D6, E6 and F6
            // End Vertical B-F

            //Primary Diagonal - \ center
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == "") && (buttons[28].Content == mark))
                return D4;
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == "") && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return C3;
            if ((buttons[0].Content == mark) && (buttons[7].Content == "") && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return B2;
            if ((buttons[0].Content == "") && (buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return A1;
            //End diagonal A1,B2,C3, D4 and E4

            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == "") && (buttons[35].Content == mark))
                return E5;
            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == "") && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return D4;
            if ((buttons[7].Content == mark) && (buttons[14].Content == "") && (buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return C3;
            if ((buttons[7].Content == "") && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return B2;
            //End Diagonal B2,,C3, D4, E4 and F6
            //end primary diagonal \

            //Primary Diagonal - / center
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == "") && (buttons[25].Content == mark))
                return D3;
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == "") && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return C4;
            if ((buttons[5].Content == mark) && (buttons[10].Content == "") && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return B5;
            if ((buttons[5].Content == "") && (buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return A6;
            //End diagonal A6, B5, C4, D3 and E2

            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == "") && (buttons[30].Content == mark))
                return E2;
            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == "") && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return D3;
            if ((buttons[10].Content == mark) && (buttons[15].Content == "") && (buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return C4;
            if ((buttons[10].Content == "") && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return B5;
            //End diagonal B5, C4, D3, E2 and F1
            //end Primary diagonal /

            //Secondary Diagonal - \ \ Beside Center
            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == "") && (buttons[34].Content == mark))
                return E4;
            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == "") && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return D3;
            if ((buttons[6].Content == mark) && (buttons[13].Content == "") && (buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return C2;
            if ((buttons[6].Content == "") && (buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return B1;
            //End diagonal B1, C2, D3, E4 and F5

            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == "") && (buttons[29].Content == mark))
                return D5;
            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == "") && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return C4;
            if ((buttons[1].Content == mark) && (buttons[8].Content == "") && (buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return B3;
            if ((buttons[1].Content == "") && (buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return A2;
            //End diagonal A2, B3, C4, D5 and E6
            //end secondary Diagonal \ \

            //Secondary Diagonal - / / Beside Center
            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == "") && (buttons[24].Content == mark))
                return D2;
            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == "") && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return C3;
            if ((buttons[4].Content == mark) && (buttons[9].Content == "") && (buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return B4;
            if ((buttons[4].Content == "") && (buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return A5;
            //End diagonal A5, B4, C3, D2 and E1

            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == "") && (buttons[31].Content == mark))
                return E3;
            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == "") && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return D4;
            if ((buttons[11].Content == mark) && (buttons[16].Content == "") && (buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return C5;
            if ((buttons[11].Content == "") && (buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return B6;
            //End diagonal B6, C5, D4, E3 and F2
            // End Diagonal  / /
            return null;
        }
        private Button WinOrBlock(string mark)
        {
            Console.WriteLine("Blocking or Scoring: 1 Point");
            // 1-4 Horizontal

            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == ""))
                return A4;
            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == "") && (buttons[3].Content == mark))
                return A3;
            if ((buttons[0].Content == mark) && (buttons[1].Content == "") && (buttons[2].Content == mark) && (buttons[3].Content == mark))
                return A2;
            if ((buttons[0].Content == "") && (buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark))
                return A1;
            //End row A1-A4

            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == ""))
                return B4;
            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == "") && (buttons[9].Content == mark))
                return B3;
            if ((buttons[6].Content == mark) && (buttons[7].Content == "") && (buttons[8].Content == mark) && (buttons[9].Content == mark))
                return B2;
            if ((buttons[6].Content == "") && (buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark))
                return B1;
            //End row B1-B4

            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == ""))
                return C4;
            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == "") && (buttons[15].Content == mark))
                return C3;
            if ((buttons[12].Content == mark) && (buttons[13].Content == "") && (buttons[14].Content == mark) && (buttons[15].Content == mark))
                return C2;
            if ((buttons[12].Content == "") && (buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark))
                return C1;
            //End row C1-C4

            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == "") && (buttons[21].Content == mark))
                return D3;
            if ((buttons[18].Content == mark) && (buttons[19].Content == "") && (buttons[20].Content == mark) && (buttons[21].Content == mark))
                return D2;
            if ((buttons[18].Content == "") && (buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark))
                return D1;
            //End row D1-D4

            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == "") && (buttons[27].Content == mark))
                return E3;
            if ((buttons[24].Content == mark) && (buttons[25].Content == "") && (buttons[26].Content == mark) && (buttons[27].Content == mark))
                return E2;
            if ((buttons[24].Content == "") && (buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark))
                return E1;
            //End row E1-E4

            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == "") && (buttons[33].Content == mark))
                return F3;
            if ((buttons[30].Content == mark) && (buttons[31].Content == "") && (buttons[32].Content == mark) && (buttons[33].Content == mark))
                return F2;
            if ((buttons[30].Content == "") && (buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark))
                return F1;
            //End row F1-F4
            //end 1-4 Horizontal

            // 2-5 Horizontal
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == ""))
                return A5;
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == "") && (buttons[4].Content == mark))
                return A4;
            if ((buttons[1].Content == mark) && (buttons[2].Content == "") && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A3;
            if ((buttons[1].Content == "") && (buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A2;
            //End row A2-A5

            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == ""))
                return B5;
            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == "") && (buttons[10].Content == mark))
                return B4;
            if ((buttons[7].Content == mark) && (buttons[8].Content == "") && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B3;
            if ((buttons[7].Content == "") && (buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B2;
            //End row B2-B5

            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == ""))
                return C5;
            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == "") && (buttons[16].Content == mark))
                return C4;
            if ((buttons[13].Content == mark) && (buttons[14].Content == "") && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C3;
            if ((buttons[13].Content == "") && (buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C2;
            //End row C2-C5

            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == "") && (buttons[22].Content == mark))
                return D4;
            if ((buttons[19].Content == mark) && (buttons[20].Content == "") && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D3;
            if ((buttons[19].Content == "") && (buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D2;
            //End row D2-D5

            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == "") && (buttons[28].Content == mark))
                return E4;
            if ((buttons[25].Content == mark) && (buttons[26].Content == "") && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E3;
            if ((buttons[25].Content == "") && (buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E2;
            //End row E2-E5

            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == "") && (buttons[34].Content == mark))
                return F4;
            if ((buttons[31].Content == mark) && (buttons[32].Content == "") && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F3;
            if ((buttons[31].Content == "") && (buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F2;
            //End row F2-F5
            //end 2-5 Horizontal

            // 3-6 Horizontal
            if ((buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == ""))
                return A6;
            if ((buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == "") && (buttons[5].Content == mark))
                return A5;
            if ((buttons[2].Content == mark) && (buttons[3].Content == "") && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A4;
            if ((buttons[2].Content == "") && (buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A3;
            //End row A3-A6

            if ((buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == ""))
                return B6;
            if ((buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == "") && (buttons[11].Content == mark))
                return B5;
            if ((buttons[8].Content == mark) && (buttons[9].Content == "") && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B4;
            if ((buttons[8].Content == "") && (buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B3;
            //End row B3-B6

            if ((buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == ""))
                return C6;
            if ((buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == "") && (buttons[17].Content == mark))
                return C5;
            if ((buttons[14].Content == mark) && (buttons[15].Content == "") && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C4;
            if ((buttons[14].Content == "") && (buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C3;
            //End row C3-C6

            if ((buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == "") && (buttons[23].Content == mark))
                return D5;
            if ((buttons[20].Content == mark) && (buttons[21].Content == "") && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D4;
            if ((buttons[20].Content == "") && (buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D3;
            //End row D3-D6

            if ((buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == "") && (buttons[29].Content == mark))
                return E5;
            if ((buttons[26].Content == mark) && (buttons[27].Content == "") && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E4;
            if ((buttons[26].Content == "") && (buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E3;
            //End row D3-D6

            if ((buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == "") && (buttons[35].Content == mark))
                return F5;
            if ((buttons[32].Content == mark) && (buttons[33].Content == "") && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F4;
            if ((buttons[32].Content == "") && (buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F3;
            //End row F3-F6

            //A-D Vertical
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == ""))
                return D1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == "") && (buttons[18].Content == mark))
                return C1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == "") && (buttons[12].Content == mark) && (buttons[18].Content == mark))
                return B1;
            if ((buttons[0].Content == "") && (buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark))
                return A1;
            //End vertical A1,B1, C1 and D1

            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == ""))
                return D2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == "") && (buttons[19].Content == mark))
                return C2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == "") && (buttons[13].Content == mark) && (buttons[19].Content == mark))
                return B2;
            if ((buttons[1].Content == "") && (buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark))
                return A2;
            //End vertical A2, B2, C2 and D2

            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == "") && (buttons[20].Content == mark))
                return C3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == "") && (buttons[14].Content == mark) && (buttons[20].Content == mark))
                return B3;
            if ((buttons[2].Content == "") && (buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark))
                return A3;
            //End vertical A3, B3, C3 and C3

            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == "") && (buttons[21].Content == mark))
                return C4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == "") && (buttons[15].Content == mark) && (buttons[21].Content == mark))
                return B4;
            if ((buttons[3].Content == "") && (buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark))
                return A4;
            //End vertical A4, B4, C4 and D4

            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == "") && (buttons[22].Content == mark))
                return C5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == "") && (buttons[16].Content == mark) && (buttons[22].Content == mark))
                return B5;
            if ((buttons[4].Content == "") && (buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark))
                return A5;
            //End vetical A5,B5,C5 and D5

            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == "") && (buttons[23].Content == mark))
                return C6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == "") && (buttons[17].Content == mark) && (buttons[23].Content == mark))
                return B6;
            if ((buttons[5].Content == "") && (buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark))
                return A6;
            //End vertical A6, B6, C6 and D6

            //B-E Vertical
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == "") && (buttons[24].Content == mark))
                return D1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == "") && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return C1;
            if ((buttons[6].Content == "") && (buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return B1;
            //End vertical B1, C1, D1 and E1

            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == "") && (buttons[25].Content == mark))
                return D2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == "") && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return C2;
            if ((buttons[7].Content == "") && (buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return B2;
            //End vertical B2, C2, D2 and E2

            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == "") && (buttons[26].Content == mark))
                return D3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return C3;
            if ((buttons[8].Content == "") && (buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return B3;
            //End vertical B3, C3, D3 and E3

            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == "") && (buttons[27].Content == mark))
                return D4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == "") && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return C4;
            if ((buttons[9].Content == "") && (buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return B4;
            //End vertical B4, C4, D4 and E4

            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == "") && (buttons[28].Content == mark))
                return D5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == "") && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return C5;
            if ((buttons[10].Content == "") && (buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return B5;
            //End vertical B5, C5, D5 and E5

            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == "") && (buttons[29].Content == mark))
                return D6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == "") && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return C6;
            if ((buttons[11].Content == "") && (buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return B6;
            //End vertical B6, C6, D6 and E6

            //C-F Vertical
            if ((buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == "") && (buttons[30].Content == mark))
                return E1;
            if ((buttons[12].Content == mark) && (buttons[18].Content == "") && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return D1;
            if ((buttons[12].Content == "") && (buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return C1;
            //End vertical C1,D1,E1 and F1

            if ((buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == "") && (buttons[31].Content == mark))
                return E2;
            if ((buttons[13].Content == mark) && (buttons[19].Content == "") && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return D2;
            if ((buttons[13].Content == "") && (buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return C2;
            //End vertical C2,D2,E2 and F2

            if ((buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == "") && (buttons[32].Content == mark))
                return E3;
            if ((buttons[14].Content == mark) && (buttons[20].Content == "") && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return D3;
            if ((buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return C3;
            //End vertical C3,D3,E3 and F3

            if ((buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == "") && (buttons[33].Content == mark))
                return E4;
            if ((buttons[15].Content == mark) && (buttons[21].Content == "") && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return D4;
            if ((buttons[15].Content == "") && (buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return C4;
            //End vertical C4,D4,E4 and F4

            if ((buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == "") && (buttons[34].Content == mark))
                return E5;
            if ((buttons[16].Content == mark) && (buttons[22].Content == "") && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return D5;
            if ((buttons[16].Content == "") && (buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return C5;
            //End vertical C5,D5,E5 and F5

            if ((buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == "") && (buttons[35].Content == mark))
                return E6;
            if ((buttons[17].Content == mark) && (buttons[23].Content == "") && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return D6;
            if ((buttons[17].Content == "") && (buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return C6;
            //End vertical C6,D6,E6 and F6

            //Primary Diagonal - \ center
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == "") && (buttons[21].Content == mark))
                return C3;
            if ((buttons[0].Content == mark) && (buttons[7].Content == "") && (buttons[14].Content == mark) && (buttons[21].Content == mark))
                return B2;
            if ((buttons[0].Content == "") && (buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark))
                return A1;
            //End diagonal A1,B2,C3 and D4

            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == "") && (buttons[28].Content == mark))
                return D4;
            if ((buttons[7].Content == mark) && (buttons[14].Content == "") && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return C3;
            if ((buttons[7].Content == "") && (buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return B2;
            //End diagonal B2,C3,D4 and E5

            if ((buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == "") && (buttons[35].Content == mark))
                return E5;
            if ((buttons[14].Content == mark) && (buttons[21].Content == "") && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return D4;
            if ((buttons[14].Content == "") && (buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return C3;
            //End diagonal C3,D4,E5 and F6

            //Primary Diagonal - / center
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == "") && (buttons[20].Content == mark))
                return C4;
            if ((buttons[5].Content == mark) && (buttons[10].Content == "") && (buttons[15].Content == mark) && (buttons[20].Content == mark))
                return B5;
            if ((buttons[5].Content == "") && (buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark))
                return A6;
            //End diagonal A6,B5, C4 and D3

            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == "") && (buttons[25].Content == mark))
                return D3;
            if ((buttons[10].Content == mark) && (buttons[15].Content == "") && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return C4;
            if ((buttons[10].Content == "") && (buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return B5;
            //End diagonal B5,C4,D3 and E2

            if ((buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == "") && (buttons[30].Content == mark))
                return E2;
            if ((buttons[15].Content == mark) && (buttons[20].Content == "") && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return D3;
            if ((buttons[15].Content == "") && (buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return C4;
            //End diagonal C4,D3,E2 and F1

            //Secondary Diagonal - \ \ Beside Center
            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == "") && (buttons[27].Content == mark))
                return D3;
            if ((buttons[6].Content == mark) && (buttons[13].Content == "") && (buttons[20].Content == mark) && (buttons[27].Content == mark))
                return C2;
            if ((buttons[6].Content == "") && (buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == mark))
                return B1;
            //End diagonal B1,C2,D3 AND E4

            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == "") && (buttons[22].Content == mark))
                return C4;
            if ((buttons[1].Content == mark) && (buttons[8].Content == "") && (buttons[15].Content == mark) && (buttons[22].Content == mark))
                return B3;
            if ((buttons[1].Content == "") && (buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == mark))
                return A2;
            //End diagonal A2,B3,C4 and D5

            if ((buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == "") && (buttons[34].Content == mark))
                return E4;
            if ((buttons[14].Content == mark) && (buttons[20].Content == "") && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return D3;
            if ((buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return C2;
            //End diagonal C2,D3,E4 and F5

            if ((buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == "") && (buttons[29].Content == mark))
                return D5;
            if ((buttons[8].Content == mark) && (buttons[15].Content == "") && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return C4;
            if ((buttons[8].Content == "") && (buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return B3;
            //End diagonal B3,C4,D5 and E6

            //Secondary Diagonal - / / Beside Center
            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == ""))
                return D2;
            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == "") && (buttons[19].Content == mark))
                return C3;
            if ((buttons[4].Content == mark) && (buttons[9].Content == "") && (buttons[14].Content == mark) && (buttons[19].Content == mark))
                return B4;
            if ((buttons[4].Content == "") && (buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == mark))
                return A5;
            //End diagonal A5,B4,C3 and D2

            if ((buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == "") && (buttons[24].Content == mark))
                return D2;
            if ((buttons[9].Content == mark) && (buttons[14].Content == "") && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return C3;
            if ((buttons[9].Content == "") && (buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return B4;
            //End diagonal B4,C3,D2 and E1

            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == "") && (buttons[26].Content == mark))
                return D4;
            if ((buttons[11].Content == mark) && (buttons[16].Content == "") && (buttons[21].Content == mark) && (buttons[26].Content == mark))
                return C5;
            if ((buttons[11].Content == "") && (buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == mark))
                return B6;
            //End diagonal B6,C5,D4 and E3

            if ((buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == "") && (buttons[31].Content == mark))
                return E3;
            if ((buttons[16].Content == mark) && (buttons[21].Content == "") && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return D4;
            if ((buttons[16].Content == "") && (buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return C5;
            //End diagonal C5,D4,E3 and F2

            //Tertiary Diagonal - \  \  \
            if ((buttons[2].Content == mark) && (buttons[9].Content == mark) && (buttons[16].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[2].Content == mark) && (buttons[9].Content == mark) && (buttons[16].Content == "") && (buttons[23].Content == mark))
                return C5;
            if ((buttons[2].Content == mark) && (buttons[9].Content == "") && (buttons[16].Content == mark) && (buttons[23].Content == mark))
                return B4;
            if ((buttons[2].Content == "") && (buttons[9].Content == mark) && (buttons[16].Content == mark) && (buttons[23].Content == mark))
                return A3;
            //End Tertiary Diagonal A3, B4, C5 and D6

            if ((buttons[12].Content == mark) && (buttons[19].Content == mark) && (buttons[26].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[12].Content == mark) && (buttons[19].Content == mark) && (buttons[26].Content == "") && (buttons[33].Content == mark))
                return E3;
            if ((buttons[12].Content == mark) && (buttons[19].Content == "") && (buttons[26].Content == mark) && (buttons[33].Content == mark))
                return D2;
            if ((buttons[12].Content == "") && (buttons[19].Content == mark) && (buttons[26].Content == mark) && (buttons[33].Content == mark))
                return C1;
            //End Tertiary Diagonal C1, D2, E3 and F4

            //Tertiary Diagonal - /  /  /
            if ((buttons[3].Content == mark) && (buttons[8].Content == mark) && (buttons[13].Content == mark) && (buttons[18].Content == ""))
                return D1;
            if ((buttons[3].Content == mark) && (buttons[8].Content == mark) && (buttons[13].Content == "") && (buttons[18].Content == mark))
                return C2;
            if ((buttons[3].Content == mark) && (buttons[8].Content == "") && (buttons[13].Content == mark) && (buttons[18].Content == mark))
                return B3;
            if ((buttons[3].Content == "") && (buttons[8].Content == mark) && (buttons[13].Content == mark) && (buttons[18].Content == mark))
                return A4;
            //End Tertiary Diagonal A4, B3, C2 and D1

            if ((buttons[17].Content == mark) && (buttons[22].Content == mark) && (buttons[27].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[17].Content == mark) && (buttons[22].Content == mark) && (buttons[27].Content == "") && (buttons[32].Content == mark))
                return E4;
            if ((buttons[17].Content == mark) && (buttons[22].Content == "") && (buttons[27].Content == mark) && (buttons[32].Content == mark))
                return D5;
            if ((buttons[17].Content == "") && (buttons[22].Content == mark) && (buttons[27].Content == mark) && (buttons[32].Content == mark))
                return C6;
            //End Tertiary Diagonal C6, D5, E4 and F3

            return null;
        }
        private Button ScoreOrBlockTriples(string mark)
        {
            Console.WriteLine("Looking for Triples");
            // 1-3 Horizontal
            if ((buttons[0].Content == mark) && (buttons[1].Content == mark) && (buttons[2].Content == ""))
                return A3;
            if ((buttons[0].Content == mark) && (buttons[1].Content == "") && (buttons[2].Content == mark))
                return A2;
            if ((buttons[0].Content == "") && (buttons[1].Content == mark) && (buttons[2].Content == mark))
                return A1;
            //end A1-A3

            if ((buttons[6].Content == mark) && (buttons[7].Content == mark) && (buttons[8].Content == ""))
                return B3;
            if ((buttons[6].Content == mark) && (buttons[7].Content == "") && (buttons[8].Content == mark))
                return B2;
            if ((buttons[6].Content == "") && (buttons[7].Content == mark) && (buttons[8].Content == mark))
                return B1;
            //End row B1-B3

            if ((buttons[12].Content == mark) && (buttons[13].Content == mark) && (buttons[14].Content == ""))
                return C3;
            if ((buttons[12].Content == mark) && (buttons[13].Content == "") && (buttons[14].Content == mark))
                return C2;
            if ((buttons[12].Content == "") && (buttons[13].Content == mark) && (buttons[14].Content == mark))
                return C1;
            //End row C1-C3

            if ((buttons[18].Content == mark) && (buttons[19].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[18].Content == mark) && (buttons[19].Content == "") && (buttons[20].Content == mark))
                return D2;
            if ((buttons[18].Content == "") && (buttons[19].Content == mark) && (buttons[20].Content == mark))
                return D1;
            //End row D1-D3

            if ((buttons[24].Content == mark) && (buttons[25].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[24].Content == mark) && (buttons[25].Content == "") && (buttons[26].Content == mark))
                return E2;
            if ((buttons[24].Content == "") && (buttons[25].Content == mark) && (buttons[26].Content == mark))
                return E1;
            //End row E1-E3

            if ((buttons[30].Content == mark) && (buttons[31].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[30].Content == mark) && (buttons[31].Content == "") && (buttons[32].Content == mark))
                return F2;
            if ((buttons[30].Content == "") && (buttons[31].Content == mark) && (buttons[32].Content == mark))
                return F1;
            //End row F1-F3
            //end 1-3

            // 2-4 Horizontal
            if ((buttons[1].Content == mark) && (buttons[2].Content == mark) && (buttons[3].Content == ""))
                return A4;
            if ((buttons[1].Content == mark) && (buttons[2].Content == "") && (buttons[3].Content == mark))
                return A3;
            if ((buttons[1].Content == "") && (buttons[2].Content == mark) && (buttons[3].Content == mark))
                return A2;
            //end A2-A4

            if ((buttons[7].Content == mark) && (buttons[8].Content == mark) && (buttons[9].Content == ""))
                return B4;
            if ((buttons[7].Content == mark) && (buttons[8].Content == "") && (buttons[9].Content == mark))
                return B3;
            if ((buttons[7].Content == "") && (buttons[8].Content == mark) && (buttons[9].Content == mark))
                return B2;
            //End row B2-B4

            if ((buttons[13].Content == mark) && (buttons[14].Content == mark) && (buttons[15].Content == ""))
                return C4;
            if ((buttons[13].Content == mark) && (buttons[14].Content == "") && (buttons[15].Content == mark))
                return C3;
            if ((buttons[13].Content == "") && (buttons[14].Content == mark) && (buttons[15].Content == mark))
                return C2;
            //End row C2-C4

            if ((buttons[19].Content == mark) && (buttons[20].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[19].Content == mark) && (buttons[20].Content == "") && (buttons[21].Content == mark))
                return D3;
            if ((buttons[19].Content == "") && (buttons[20].Content == mark) && (buttons[21].Content == mark))
                return D2;
            //End row D1-D4

            if ((buttons[25].Content == mark) && (buttons[26].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[25].Content == mark) && (buttons[26].Content == "") && (buttons[27].Content == mark))
                return E3;
            if ((buttons[25].Content == "") && (buttons[26].Content == mark) && (buttons[27].Content == mark))
                return E2;
            //End row E2-E4

            if ((buttons[31].Content == mark) && (buttons[32].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[31].Content == mark) && (buttons[32].Content == "") && (buttons[33].Content == mark))
                return F3;
            if ((buttons[31].Content == "") && (buttons[32].Content == mark) && (buttons[33].Content == mark))
                return F2;
            //End row F2-F4
            //end 2-4

            //3-5 Horizontal
            if ((buttons[2].Content == mark) && (buttons[3].Content == mark) && (buttons[4].Content == ""))
                return A5;
            if ((buttons[2].Content == mark) && (buttons[3].Content == "") && (buttons[4].Content == mark))
                return A4;
            if ((buttons[2].Content == "") && (buttons[3].Content == mark) && (buttons[4].Content == mark))
                return A3;
            //End row A3-A5

            if ((buttons[8].Content == mark) && (buttons[9].Content == mark) && (buttons[10].Content == ""))
                return B5;
            if ((buttons[8].Content == mark) && (buttons[9].Content == "") && (buttons[10].Content == mark))
                return B4;
            if ((buttons[8].Content == "") && (buttons[9].Content == mark) && (buttons[10].Content == mark))
                return B3;
            //End row B3-B5

            if ((buttons[14].Content == mark) && (buttons[15].Content == mark) && (buttons[16].Content == ""))
                return C5;
            if ((buttons[14].Content == mark) && (buttons[15].Content == "") && (buttons[16].Content == mark))
                return C4;
            if ((buttons[14].Content == "") && (buttons[15].Content == mark) && (buttons[16].Content == mark))
                return C3;
            //End row C3-C5

            if ((buttons[20].Content == mark) && (buttons[21].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[20].Content == mark) && (buttons[21].Content == "") && (buttons[22].Content == mark))
                return D4;
            if ((buttons[20].Content == "") && (buttons[21].Content == mark) && (buttons[22].Content == mark))
                return D3;
            //End row D3-D5

            if ((buttons[26].Content == mark) && (buttons[27].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[26].Content == mark) && (buttons[27].Content == "") && (buttons[28].Content == mark))
                return E4;
            if ((buttons[26].Content == "") && (buttons[27].Content == mark) && (buttons[28].Content == mark))
                return E3;
            //End row D3-D5

            if ((buttons[32].Content == mark) && (buttons[33].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[32].Content == mark) && (buttons[33].Content == "") && (buttons[34].Content == mark))
                return F4;
            if ((buttons[32].Content == "") && (buttons[33].Content == mark) && (buttons[34].Content == mark))
                return F3;
            //End row F3-F5
            //End 3-5

            // 4-6 Horizontal
            if ((buttons[3].Content == mark) && (buttons[4].Content == mark) && (buttons[5].Content == ""))
                return A6;
            if ((buttons[3].Content == mark) && (buttons[4].Content == "") && (buttons[5].Content == mark))
                return A5;
            if ((buttons[3].Content == "") && (buttons[4].Content == mark) && (buttons[5].Content == mark))
                return A4;
            //End row A4-A6

            if ((buttons[9].Content == mark) && (buttons[10].Content == mark) && (buttons[11].Content == ""))
                return B6;
            if ((buttons[9].Content == mark) && (buttons[10].Content == "") && (buttons[11].Content == mark))
                return B5;
            if ((buttons[9].Content == "") && (buttons[10].Content == mark) && (buttons[11].Content == mark))
                return B4;
            //End row B4-B6

            if ((buttons[15].Content == mark) && (buttons[16].Content == mark) && (buttons[17].Content == ""))
                return C6;
            if ((buttons[15].Content == mark) && (buttons[16].Content == "") && (buttons[17].Content == mark))
                return C5;
            if ((buttons[15].Content == "") && (buttons[16].Content == mark) && (buttons[17].Content == mark))
                return C4;
            //End row C4-C6

            if ((buttons[21].Content == mark) && (buttons[22].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[21].Content == mark) && (buttons[22].Content == "") && (buttons[23].Content == mark))
                return D5;
            if ((buttons[21].Content == "") && (buttons[22].Content == mark) && (buttons[23].Content == mark))
                return D4;
            //End row D4-D6

            if ((buttons[27].Content == mark) && (buttons[28].Content == mark) && (buttons[19].Content == ""))
                return E6;
            if ((buttons[27].Content == mark) && (buttons[28].Content == "") && (buttons[29].Content == mark))
                return E5;
            if ((buttons[27].Content == "") && (buttons[28].Content == mark) && (buttons[29].Content == mark))
                return E4;
            //End row D4-D6

            if ((buttons[33].Content == mark) && (buttons[34].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[33].Content == mark) && (buttons[34].Content == "") && (buttons[35].Content == mark))
                return F5;
            if ((buttons[33].Content == "") && (buttons[34].Content == mark) && (buttons[35].Content == mark))
                return F4;
            //End row F4-F6
            //end 4-6

            //A-C Vertical
            if ((buttons[0].Content == mark) && (buttons[6].Content == mark) && (buttons[12].Content == ""))
                return C1;
            if ((buttons[0].Content == mark) && (buttons[6].Content == "") && (buttons[12].Content == mark))
                return B1;
            if ((buttons[0].Content == "") && (buttons[6].Content == mark) && (buttons[12].Content == mark))
                return A1;
            //End vertical A1,B1, C1 

            if ((buttons[1].Content == mark) && (buttons[7].Content == mark) && (buttons[13].Content == ""))
                return C2;
            if ((buttons[1].Content == mark) && (buttons[7].Content == "") && (buttons[13].Content == mark))
                return B2;
            if ((buttons[1].Content == "") && (buttons[7].Content == mark) && (buttons[13].Content == mark))
                return A2;
            //End vertical A2, B2, C2 

            if ((buttons[2].Content == mark) && (buttons[8].Content == mark) && (buttons[14].Content == ""))
                return C3;
            if ((buttons[2].Content == mark) && (buttons[8].Content == "") && (buttons[14].Content == mark))
                return B3;
            if ((buttons[2].Content == "") && (buttons[8].Content == mark) && (buttons[14].Content == mark))
                return A3;
            //End vertical A3, B3, C3 

            if ((buttons[3].Content == mark) && (buttons[9].Content == mark) && (buttons[15].Content == ""))
                return C4;
            if ((buttons[3].Content == mark) && (buttons[9].Content == "") && (buttons[15].Content == mark))
                return B4;
            if ((buttons[3].Content == "") && (buttons[9].Content == mark) && (buttons[15].Content == mark))
                return A4;
            //End vertical A4, B4, C4 

            if ((buttons[4].Content == mark) && (buttons[10].Content == mark) && (buttons[16].Content == ""))
                return C5;
            if ((buttons[4].Content == mark) && (buttons[10].Content == "") && (buttons[16].Content == mark))
                return B5;
            if ((buttons[4].Content == "") && (buttons[10].Content == mark) && (buttons[16].Content == mark))
                return A5;
            //End vetical A5,B5,C5

            if ((buttons[5].Content == mark) && (buttons[11].Content == mark) && (buttons[17].Content == ""))
                return C6;
            if ((buttons[5].Content == mark) && (buttons[11].Content == "") && (buttons[17].Content == mark))
                return B6;
            if ((buttons[5].Content == "") && (buttons[11].Content == mark) && (buttons[17].Content == mark))
                return A6;
            //End vertical A6, B6, C6 
            //end A-C

            //B-D Vertical
            if ((buttons[6].Content == mark) && (buttons[12].Content == mark) && (buttons[18].Content == ""))
                return D1;
            if ((buttons[6].Content == mark) && (buttons[12].Content == "") && (buttons[18].Content == mark))
                return C1;
            if ((buttons[6].Content == "") && (buttons[12].Content == mark) && (buttons[18].Content == mark))
                return B1;
            //End vertical B1, C1 and D1

            if ((buttons[7].Content == mark) && (buttons[13].Content == mark) && (buttons[19].Content == ""))
                return D2;
            if ((buttons[7].Content == mark) && (buttons[13].Content == "") && (buttons[19].Content == mark))
                return C2;
            if ((buttons[7].Content == "") && (buttons[13].Content == mark) && (buttons[19].Content == mark))
                return B2;
            //End vertical  B2, C2 and D2

            if ((buttons[8].Content == mark) && (buttons[14].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[8].Content == mark) && (buttons[14].Content == "") && (buttons[20].Content == mark))
                return C3;
            if ((buttons[8].Content == "") && (buttons[14].Content == mark) && (buttons[20].Content == mark))
                return B3;
            //End vertical B3, C3 and C3

            if ((buttons[9].Content == mark) && (buttons[15].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[9].Content == mark) && (buttons[15].Content == "") && (buttons[21].Content == mark))
                return C4;
            if ((buttons[9].Content == "") && (buttons[15].Content == mark) && (buttons[21].Content == mark))
                return B4;
            //End vertical  B4, C4 and D4

            if ((buttons[10].Content == mark) && (buttons[16].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[10].Content == mark) && (buttons[16].Content == "") && (buttons[22].Content == mark))
                return C5;
            if ((buttons[10].Content == "") && (buttons[16].Content == mark) && (buttons[22].Content == mark))
                return B5;
            //End vetical B5,C5 and D5

            if ((buttons[11].Content == mark) && (buttons[17].Content == mark) && (buttons[23].Content == ""))
                return D6;
            if ((buttons[11].Content == mark) && (buttons[17].Content == "") && (buttons[23].Content == mark))
                return C6;
            if ((buttons[11].Content == "") && (buttons[17].Content == mark) && (buttons[23].Content == mark))
                return B6;
            //End vertical B6, C6 and D6
            //end B-D

            //C-E
            //C-F Vertical
            if ((buttons[12].Content == mark) && (buttons[18].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[12].Content == mark) && (buttons[18].Content == "") && (buttons[24].Content == mark))
                return D1;
            if ((buttons[12].Content == "") && (buttons[18].Content == mark) && (buttons[24].Content == mark))
                return C1;
            //End vertical C1,D1,E1

            if ((buttons[13].Content == mark) && (buttons[19].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[13].Content == mark) && (buttons[19].Content == "") && (buttons[25].Content == mark))
                return D2;
            if ((buttons[13].Content == "") && (buttons[19].Content == mark) && (buttons[25].Content == mark))
                return C2;
            //End vertical C2,D2,E2

            if ((buttons[14].Content == mark) && (buttons[20].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[14].Content == mark) && (buttons[20].Content == "") && (buttons[26].Content == mark))
                return D3;
            if ((buttons[14].Content == "") && (buttons[20].Content == mark) && (buttons[26].Content == mark))
                return C3;
            //End vertical C3,D3,E3

            if ((buttons[15].Content == mark) && (buttons[21].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[15].Content == mark) && (buttons[21].Content == "") && (buttons[27].Content == mark))
                return D4;
            if ((buttons[15].Content == "") && (buttons[21].Content == mark) && (buttons[27].Content == mark))
                return C4;
            //End vertical C4,D4,E4

            if ((buttons[16].Content == mark) && (buttons[22].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[16].Content == mark) && (buttons[22].Content == "") && (buttons[28].Content == mark))
                return D5;
            if ((buttons[16].Content == "") && (buttons[22].Content == mark) && (buttons[28].Content == mark))
                return C5;
            //End vertical C5,D5,E5

            if ((buttons[17].Content == mark) && (buttons[23].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[17].Content == mark) && (buttons[23].Content == "") && (buttons[29].Content == mark))
                return D6;
            if ((buttons[17].Content == "") && (buttons[23].Content == mark) && (buttons[29].Content == mark))
                return C6;
            //End vertical C6,D6,E6


            //D-F Vertical


            if ((buttons[18].Content == mark) && (buttons[24].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[18].Content == mark) && (buttons[24].Content == "") && (buttons[30].Content == mark))
                return E1;
            if ((buttons[18].Content == "") && (buttons[24].Content == mark) && (buttons[30].Content == mark))
                return D1;
            //End vertical D1,E1 and F1

            if ((buttons[19].Content == mark) && (buttons[25].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[19].Content == mark) && (buttons[25].Content == "") && (buttons[31].Content == mark))
                return E2;
            if ((buttons[19].Content == "") && (buttons[25].Content == mark) && (buttons[31].Content == mark))
                return D2;
            //End vertical D2,E2 and F2

            if ((buttons[20].Content == mark) && (buttons[26].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[20].Content == mark) && (buttons[26].Content == "") && (buttons[32].Content == mark))
                return E3;
            if ((buttons[20].Content == "") && (buttons[26].Content == mark) && (buttons[32].Content == mark))
                return D3;
            //End vertical D3,E3 and F3

            if ((buttons[21].Content == mark) && (buttons[27].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[21].Content == mark) && (buttons[27].Content == "") && (buttons[33].Content == mark))
                return E4;
            if ((buttons[21].Content == "") && (buttons[27].Content == mark) && (buttons[33].Content == mark))
                return D4;
            //End vertical D4,E4 and F4

            if ((buttons[22].Content == mark) && (buttons[28].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[22].Content == mark) && (buttons[28].Content == "") && (buttons[34].Content == mark))
                return E5;
            if ((buttons[22].Content == "") && (buttons[28].Content == mark) && (buttons[34].Content == mark))
                return D5;
            //End vertical D5,E5 and F5

            if ((buttons[23].Content == mark) && (buttons[29].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[23].Content == mark) && (buttons[29].Content == "") && (buttons[35].Content == mark))
                return E6;
            if ((buttons[23].Content == "") && (buttons[29].Content == mark) && (buttons[35].Content == mark))
                return D6;
            //End vertical D6,E6 and F6

            //Diagonal tripples
            //Primary Diagonal - \ center
            if ((buttons[0].Content == mark) && (buttons[7].Content == mark) && (buttons[14].Content == ""))
                return C3;
            if ((buttons[0].Content == mark) && (buttons[7].Content == "") && (buttons[14].Content == mark))
                return B2;
            if ((buttons[0].Content == "") && (buttons[7].Content == mark) && (buttons[14].Content == mark))
                return A1;
            //End diagonal A1,B2,C3

            if ((buttons[7].Content == mark) && (buttons[14].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[7].Content == mark) && (buttons[14].Content == "") && (buttons[21].Content == mark))
                return C3;
            if ((buttons[7].Content == "") && (buttons[14].Content == mark) && (buttons[21].Content == mark))
                return B2;
            //End diagonal B2,C3,D4

            if ((buttons[14].Content == mark) && (buttons[21].Content == mark) && (buttons[28].Content == ""))
                return E5;
            if ((buttons[14].Content == mark) && (buttons[21].Content == "") && (buttons[28].Content == mark))
                return D4;
            if ((buttons[14].Content == "") && (buttons[21].Content == mark) && (buttons[28].Content == mark))
                return C3;
            //End diagonal C3,D4,E5

            if ((buttons[21].Content == mark) && (buttons[28].Content == mark) && (buttons[35].Content == ""))
                return F6;
            if ((buttons[21].Content == mark) && (buttons[28].Content == "") && (buttons[35].Content == mark))
                return E5;
            if ((buttons[21].Content == "") && (buttons[28].Content == mark) && (buttons[35].Content == mark))
                return D4;
            //End diagonal D4,E5 and F6
            //end diagonal \

            //Primary Diagonal - / center
            if ((buttons[5].Content == mark) && (buttons[10].Content == mark) && (buttons[15].Content == ""))
                return C4;
            if ((buttons[5].Content == mark) && (buttons[10].Content == "") && (buttons[15].Content == mark))
                return B5;
            if ((buttons[5].Content == "") && (buttons[10].Content == mark) && (buttons[15].Content == mark))
                return A6;
            //End diagonal A6,B5, C4 

            if ((buttons[10].Content == mark) && (buttons[15].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[10].Content == mark) && (buttons[15].Content == "") && (buttons[20].Content == mark))
                return C4;
            if ((buttons[10].Content == "") && (buttons[15].Content == mark) && (buttons[20].Content == mark))
                return B5;
            //End diagonal B5,C4,D3

            if ((buttons[15].Content == mark) && (buttons[20].Content == mark) && (buttons[25].Content == ""))
                return E2;
            if ((buttons[15].Content == mark) && (buttons[20].Content == "") && (buttons[25].Content == mark))
                return D3;
            if ((buttons[15].Content == "") && (buttons[20].Content == mark) && (buttons[25].Content == mark))
                return C4;
            //End diagonal C4,D3,E2

            if ((buttons[20].Content == mark) && (buttons[25].Content == mark) && (buttons[30].Content == ""))
                return F1;
            if ((buttons[20].Content == mark) && (buttons[25].Content == "") && (buttons[30].Content == mark))
                return E2;
            if ((buttons[20].Content == "") && (buttons[25].Content == mark) && (buttons[30].Content == mark))
                return D3;
            //End diagonal ,D3,E2 and F1
            //end diagonal /

            //Secondary Diagonal - \ \ Beside Center

            if ((buttons[6].Content == mark) && (buttons[13].Content == mark) && (buttons[20].Content == ""))
                return D3;
            if ((buttons[6].Content == mark) && (buttons[13].Content == "") && (buttons[20].Content == mark))
                return C2;
            if ((buttons[6].Content == "") && (buttons[13].Content == mark) && (buttons[20].Content == mark))
                return B1;
            //End diagonal B1,C2,D3 

            if ((buttons[1].Content == mark) && (buttons[8].Content == mark) && (buttons[15].Content == ""))
                return C4;
            if ((buttons[1].Content == mark) && (buttons[8].Content == "") && (buttons[15].Content == mark))
                return B3;
            if ((buttons[1].Content == "") && (buttons[8].Content == mark) && (buttons[15].Content == mark))
                return A2;
            //End diagonal A2,B3,C4

            if ((buttons[13].Content == mark) && (buttons[20].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[13].Content == mark) && (buttons[20].Content == "") && (buttons[27].Content == mark))
                return D3;
            if ((buttons[13].Content == "") && (buttons[20].Content == mark) && (buttons[27].Content == mark))
                return C2;
            //End diagonal C2,D3,E4 

            if ((buttons[20].Content == mark) && (buttons[27].Content == mark) && (buttons[34].Content == ""))
                return F5;
            if ((buttons[20].Content == mark) && (buttons[27].Content == "") && (buttons[34].Content == mark))
                return E4;
            if ((buttons[20].Content == "") && (buttons[27].Content == mark) && (buttons[34].Content == mark))
                return D3;
            //End diagonal D3,E4 and F5

            if ((buttons[8].Content == mark) && (buttons[15].Content == mark) && (buttons[22].Content == ""))
                return D5;
            if ((buttons[8].Content == mark) && (buttons[15].Content == "") && (buttons[22].Content == mark))
                return C4;
            if ((buttons[8].Content == "") && (buttons[15].Content == mark) && (buttons[22].Content == mark))
                return B3;
            //End diagonal B3,C4,D5

            if ((buttons[15].Content == mark) && (buttons[22].Content == mark) && (buttons[29].Content == ""))
                return E6;
            if ((buttons[15].Content == mark) && (buttons[22].Content == "") && (buttons[29].Content == mark))
                return D5;
            if ((buttons[15].Content == "") && (buttons[22].Content == mark) && (buttons[29].Content == mark))
                return C4;
            //End diagonalC4,D5 and E6
            //end diag \ \ 

            //Secondary Diagonal - / / Beside Center

            if ((buttons[4].Content == mark) && (buttons[9].Content == mark) && (buttons[14].Content == ""))
                return C3;
            if ((buttons[4].Content == mark) && (buttons[9].Content == "") && (buttons[14].Content == mark))
                return B4;
            if ((buttons[4].Content == "") && (buttons[9].Content == mark) && (buttons[14].Content == mark))
                return A5;
            //End diagonal A5,B4,C3

            if ((buttons[9].Content == mark) && (buttons[14].Content == mark) && (buttons[19].Content == ""))
                return D2;
            if ((buttons[9].Content == mark) && (buttons[14].Content == "") && (buttons[19].Content == mark))
                return C3;
            if ((buttons[9].Content == "") && (buttons[14].Content == mark) && (buttons[19].Content == mark))
                return B4;
            //End diagonal B4,C3,D2

            if ((buttons[14].Content == mark) && (buttons[19].Content == mark) && (buttons[24].Content == ""))
                return E1;
            if ((buttons[14].Content == mark) && (buttons[19].Content == "") && (buttons[24].Content == mark))
                return D2;
            if ((buttons[14].Content == "") && (buttons[19].Content == mark) && (buttons[24].Content == mark))
                return C3;
            //End diagonal C3,D2 and E1

            if ((buttons[11].Content == mark) && (buttons[16].Content == mark) && (buttons[21].Content == ""))
                return D4;
            if ((buttons[11].Content == mark) && (buttons[16].Content == "") && (buttons[21].Content == mark))
                return C5;
            if ((buttons[11].Content == "") && (buttons[16].Content == mark) && (buttons[21].Content == mark))
                return B6;
            //End diagonal B6,C5,D4

            if ((buttons[16].Content == mark) && (buttons[21].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[16].Content == mark) && (buttons[21].Content == "") && (buttons[26].Content == mark))
                return D4;
            if ((buttons[16].Content == "") && (buttons[21].Content == mark) && (buttons[26].Content == mark))
                return C5;
            //End diagonal C5,D4,E3

            if ((buttons[21].Content == mark) && (buttons[26].Content == mark) && (buttons[31].Content == ""))
                return F2;
            if ((buttons[21].Content == mark) && (buttons[26].Content == "") && (buttons[31].Content == mark))
                return E3;
            if ((buttons[21].Content == "") && (buttons[26].Content == mark) && (buttons[31].Content == mark))
                return D4;
            //End diagonal D4,E3 and F2
            //end diag / /


            //Diagonal \ \ \

            //Tertiary Diagonal - \  \  \

            if ((buttons[2].Content == mark) && (buttons[9].Content == mark) && (buttons[16].Content == ""))
                return C5;
            if ((buttons[2].Content == mark) && (buttons[9].Content == "") && (buttons[16].Content == mark))
                return B4;
            if ((buttons[2].Content == "") && (buttons[9].Content == mark) && (buttons[16].Content == mark))
                return A3;
            //End Tertiary Diagonal A3, B4, C5 

            if ((buttons[12].Content == mark) && (buttons[19].Content == mark) && (buttons[26].Content == ""))
                return E3;
            if ((buttons[12].Content == mark) && (buttons[19].Content == "") && (buttons[26].Content == mark))
                return D2;
            if ((buttons[12].Content == "") && (buttons[19].Content == mark) && (buttons[26].Content == mark))
                return C1;
            //End Tertiary Diagonal C1, D2, E3

            if ((buttons[19].Content == mark) && (buttons[26].Content == mark) && (buttons[33].Content == ""))
                return F4;
            if ((buttons[19].Content == mark) && (buttons[26].Content == "") && (buttons[33].Content == mark))
                return E3;
            if ((buttons[19].Content == "") && (buttons[26].Content == mark) && (buttons[33].Content == mark))
                return D2;
            //End Tertiary Diagonal D2, E3 and F4
            //end \ \ \ 

            //Tertiary Diagonal - /  /  /

            if ((buttons[3].Content == mark) && (buttons[8].Content == mark) && (buttons[13].Content == ""))
                return C2;
            if ((buttons[3].Content == mark) && (buttons[8].Content == "") && (buttons[13].Content == mark))
                return B3;
            if ((buttons[3].Content == "") && (buttons[8].Content == mark) && (buttons[13].Content == mark))
                return A4;
            //End Tertiary Diagonal A4, B3, C2 

            if ((buttons[17].Content == mark) && (buttons[22].Content == mark) && (buttons[27].Content == ""))
                return E4;
            if ((buttons[17].Content == mark) && (buttons[22].Content == "") && (buttons[27].Content == mark))
                return D5;
            if ((buttons[17].Content == "") && (buttons[22].Content == mark) && (buttons[27].Content == mark))
                return C6;
            //End Tertiary Diagonal C6, D5, E4 

            if ((buttons[22].Content == mark) && (buttons[27].Content == mark) && (buttons[32].Content == ""))
                return F3;
            if ((buttons[22].Content == mark) && (buttons[27].Content == "") && (buttons[32].Content == mark))
                return E4;
            if ((buttons[22].Content == "") && (buttons[27].Content == mark) && (buttons[32].Content == mark))
                return D5;
            //End Tertiary Diagonal  D5, E4 and F3
            //end / / /

            return null;
        }
        private Button Center()
        {
            Console.WriteLine("Securing Center Square");

            if (buttons[14].Content == "X")
            {
                if (buttons[15].Content == "")
                    return C4;
                if (buttons[20].Content == "")
                    return D3;
                if (buttons[21].Content == "")
                    return D4;
            }

            if (buttons[15].Content == "X")
            {
                if (buttons[14].Content == "")
                    return C3;
                if (buttons[20].Content == "")
                    return D3;
                if (buttons[21].Content == "")
                    return D4;
            }

            if (buttons[20].Content == "X")
            {
                if (buttons[14].Content == "")
                    return C3;
                if (buttons[15].Content == "")
                    return C4;
                if (buttons[21].Content == "")
                    return D4;
            }

            if (buttons[21].Content == "X")
            {
                if (buttons[14].Content == "")
                    return C3;
                if (buttons[15].Content == "")
                    return C4;
                if (buttons[20].Content == "")
                    return D3;
            }

            if (buttons[14].Content == "")
                return C3;
            if (buttons[15].Content == "")
                return C4;
            if (buttons[20].Content == "")
                return D3;
            if (buttons[21].Content == "")
                return D4;

            return null;
        }
        private Button Corner()
        {
            Console.WriteLine("Securing Corner Square");

            if (buttons[0].Content == "X")
            {
                if (buttons[5].Content == "")
                    return A6;
                if (buttons[30].Content == "")
                    return F1;
                if (buttons[35].Content == "")
                    return F6;
            }

            if (buttons[5].Content == "X")
            {
                if (buttons[0].Content == "")
                    return A1;
                if (buttons[30].Content == "")
                    return F1;
                if (buttons[35].Content == "")
                    return F6;
            }

            if (buttons[35].Content == "O")
            {
                if (buttons[0].Content == "")
                    return A1;
                if (buttons[5].Content == "")
                    return A6;
                if (buttons[30].Content == "")
                    return F1;
            }

            if (buttons[30].Content == "O")
            {
                if (buttons[0].Content == "")
                    return A1;
                if (buttons[5].Content == "")
                    return A6;
                if (buttons[35].Content == "")
                    return F6;
            }

            if (buttons[0].Content == "")
                return A1;
            if (buttons[5].Content == "")
                return A6;
            if (buttons[30].Content == "")
                return F1;
            if (buttons[35].Content == "")
                return F6;

            return null;
        }

        /*
        private Button Open()
        {
                easyAI();
        }

        */

        //Display the gameboard at the end if user wants to
        private void displayGameBoardStats(object sender, MouseButtonEventArgs e)
        {
            setButtonsArray();

            for (int i = 0; i < 36; i++)
            {
                buttons[i].IsEnabled = false;

            }

            exitButton.Content = "Back";

            exitButton.Visibility = Visibility.Visible;

            displayPlayerStats.Visibility = Visibility.Hidden;

        }

        private void playAgainButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveCounter = 0;

            for (int i = 0; i <36; i++)
            {
                buttons[i].Content = "";
            }

            score1Label.Content = 0;
            score2Label.Content = 0;

           
            player1.resetScore();
            player2.resetScore();

            setAIArrays();
            setButtonsArray();
            setProperties();

            displayPlayerStats.Visibility = Visibility.Hidden;
            exitButton.Visibility = Visibility.Visible;
        }
    }


}
