﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HelloWorld
{
    class Game
    {
        struct Player
        {
            public int health;
            public float speed;
            public bool isAlive;
            public string name;
            public int defense;
            public int damage;
        }
        bool _gameOver = false;
        string _playerName = "Hero";
        int _playerHealth = 120;
        int _playerDamage = 200000;
        int _playerDefense = 10;
        Player player1;
        int levelScaleMax = 5;
        //Run the game
        public void Run()
        {
            Start();

            while(_gameOver == false)
            {
                Update();
            }

            End();
        }
        //This function handles the battles for our ladder. roomNum is used to update the our opponent to be the enemy in the current room. 
        //turnCount is used to keep track of how many turns it took the player to beat the enemy
        bool StartBattle(int roomNum, ref int turnCount)
        {
            //initialize default enemy stats
            int enemyHealth = 0;
            int enemyAttack = 0;
            int enemyDefense = 0;
            string enemyName = "";
            //Changes the enemy's default stats based on our current room number. 
            //This is how we make it seem as if the player is fighting different enemies
            switch (roomNum)
            {
                case 0:
                    {
                        enemyHealth = 100;
                        enemyAttack = 20;
                        enemyDefense = 5;
                        enemyName = "Wizard";
                        break;
                    }
                case 1:
                    {
                        enemyHealth = 80;
                        enemyAttack = 30;
                        enemyDefense = 5;
                        enemyName = "Troll";
                        break;
                    }
                case 2:
                    {
                        
                        enemyHealth = 200;
                        enemyAttack = 40;
                        enemyDefense = 10;
                        enemyName = "Giant";
                        break;
                    }
            }


            //Loops until the player or the enemy is dead
            while(_playerHealth > 0 && enemyHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                PrintStats(enemyName, enemyHealth, enemyAttack, enemyDefense);

                //Get input from the player
                char input;
                GetInput(out input,"Attack", "Defend");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if(input == '1')
                {
                    BlockAttack(ref enemyHealth, _playerDamage, enemyDefense);
                    Console.Clear();
                    Console.WriteLine("You dealt " + _playerDamage + " damage.");
                    enemyHealth -= _playerDamage;
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();

                    //After the player attacks, the enemy takes its turn. Since the player decided not to defend, the block attack function is not called.
                    _playerHealth -= enemyAttack;
                    Console.WriteLine(enemyName + " dealt " + enemyAttack + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else
                {
                    BlockAttack(ref _playerHealth, enemyAttack, _playerDefense);
                    Console.WriteLine(enemyName + " dealt " + enemyAttack + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                    Console.Clear();
                }
                Console.Clear();

            }
            //Return whether or not our player died
            return _playerHealth != 0;

        }
        //Decrements the health of a character. The attack value is subtracted by that character's defense
        void BlockAttack(ref int opponentHealth, int attackVal, int opponentDefense)
        {
            int damage = attackVal - opponentDefense;
            if(damage < 0)
            {
                damage = 0;
            }
            opponentHealth -= damage;   
        }
        //Scales up the player's stats based on the amount of turns it took in the last battle

        //Create an area for the player to manually upgrade their stats.
        //This could be in the form of a shop or some other context.
        //Create a function that overloads the function UpgradeStats. This should allow
        //the player to manually upgrade their individual stats.

        void UpgradeStats(int turnCount)
        {
            //Subtract the amount of turns from our maximum level scale to get our current level scale
            int scale = levelScaleMax - turnCount;
            if (scale <= 0)
            {
                scale = 1;
            }
            _playerHealth += 10 * scale;
            _playerDamage *= scale;
            _playerDefense *= scale;
        }

        public void UpgradeStats(int turncount, ref char input)
        {
            int scale = levelScaleMax - turncount;
            if(scale <= 0)
            {
                scale = 1;
            }
            while(input != '1' && input != '2' && input != '3')
            {
                //After each battle message is displayed
                Console.WriteLine("\nYou have leveled up! Where would you like to put this experience?");
                Console.WriteLine("1. Health");
                Console.WriteLine("2. Damage");
                Console.WriteLine("3. Defense");
                Console.Write(">");
                //Waits for user input before gifting desired upgrade
                input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1':
                        {
                            //Health option increases current health rather than maximum health
                            Console.WriteLine("You chose Health!");
                            Console.WriteLine("Your health has risen by 20");
                            _playerHealth = _playerHealth + 20;
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                    case '2':
                        {
                            Console.WriteLine("You chose Damage!");
                            Console.WriteLine("Your damage has risen by 5");
                            _playerDamage = _playerDamage + 5;
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                    case '3':
                        {
                            Console.WriteLine("You chose Defense!");
                            Console.WriteLine("Your defense has risen by 5");
                            _playerDefense = _playerDefense + 5;
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                }
            }



        }
        //Gets input from the player
        //Out's the char variable given. This variables stores the player's input choice.
        //The parameters option1 and option 2 displays the players current chpices to the screen
        void GetInput(out char input,string option1, string option2, string query)
        {
            Console.WriteLine(query);
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2' && input != '3')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }

        }

        void GetInput(out char input, string option1, string option2)
        {
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }


        }

        //Prints the stats given in the parameter list to the console
        void PrintStats(string name, int health, int damage, int defense)
        {
            Console.WriteLine("\n" + name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Damage: " + damage);
            Console.WriteLine("Defense: " + defense);
        }

        //This is used to progress through our game. A recursive function meant to switch the rooms and start the battles inside them.
        void ClimbLadder(int roomNum)
        {
            //Displays context based on which room the player is in
            switch (roomNum)
            {
                case 0:
                    {
                        Console.WriteLine("A wizard blocks your path");
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("A troll stands before you");
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("A giant has appeared!");
                        break;
                    }
                default:
                    {
                        _gameOver = true;
                        return;
                    }
            }

            int turnCount = 0;
            //Starts a battle. If the player survived the battle, level them up and then proceed to the next room.
            char input = ' ';
            if(StartBattle(roomNum, ref turnCount))
            {
                UpgradeStats(turnCount, ref input);
                ClimbLadder(roomNum + 1);
                Console.Clear();
            }
            _gameOver = true;

        }

        //Displays the character selection menu. 
        void SelectCharacter()
        {
            char input = ' ';
            //Loops until a valid option is choosen
            while(input != '1' && input != '2' && input != '3')
            {
                //Prints options
                Console.WriteLine("Welcome! Please select a character.");
                Console.WriteLine("1.Sir Kibble");
                Console.WriteLine("2.Gnojoel");
                Console.WriteLine("3.Joedazz");
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                //Sets the players default stats based on which character was picked
                switch (input)
                {
                    case '1':
                        {
                            player1.name = "Sir Kibble";
                            player1.health = 120;
                            player1.defense = 10;
                            player1.damage = 1000;
                            break;
                        }
                    case '2':
                        {
                            player1.name = "Gnojoel";
                            player1.health = 40;
                            player1.defense = 2;
                            player1.damage = 70;
                            break;
                        }
                    case '3':
                        {
                            player1.name = "Joedazz";
                            player1.health = 200;
                            player1.defense = 5;
                            player1.damage = 25;
                            break;
                        }
                        //If an invalid input is selected display and input message and input over again.
                    default:
                        {
                            Console.WriteLine("Invalid input. Press any key to continue.");
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                }
                Console.Clear();
            }
            //Prints the stats of the choosen character to the screen before the game begins to give the player visual feedback
            PrintStats(_playerName,_playerHealth,_playerDamage,_playerDefense);
            Console.WriteLine("Press any key to continue.");
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }
        //Performed once when the game begins
        public void Start()
        {
            SelectCharacter();
        }

        //Repeated until the game ends
        public void Update()
        {
            ClimbLadder(0);   
        }

        //Performed once when the game ends
        public void End()
        {
            //If the player died print death message
            if(_playerHealth <= 0)
            {
                Console.WriteLine("Failure");
                return;
            }
            //Print game over message
            Console.WriteLine("Congrats");
        }
    }
}
