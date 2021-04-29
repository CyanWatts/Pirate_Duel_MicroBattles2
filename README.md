# Pirate_Duel_MicroBattles2
A C#-based easy pixel version of the "Pirate Duel" game in Micro Battles2. </br>
I use Visual Studio 2019(WinForm) to write this game.

0. File Structure：
	The directory "Pirate Duel" is the source code, that is, the original project directory of Visual Studio.
	The file "PirateDuel_Setup1.0.msi" and "setup.exe" is for the setup of this game ( Windows )

1. Operating instructions：(Keyboard)
	Player1：A key ( if you choose PVE mode, then you will be Player1, and Player2 is controlled by the computer, which currently chooses action randomly )
  Player2: L key ( if you choose PVP mode )

2. Introduction to the game

  (1) Elements
	
	**********Pirate**********
	A 50*50 pixel block, which is controlled by the player/computer. A pirate can throw the knife and jump, and aims to kill the other one.

	**********Collimator**********
	When the pirate is holding a knife, the black pixel block that appears around the pirate, which helps to aim and throw.

	**********Knife**********
	Each pirate has a knife at the beginning / reset of the game. When the pirate is holding a knife, his action is to throw the knife towards his collimator. If the knife touches the other pirate, the other pirate will be judged dead. You can't kill yourself of course, which means the knife you thorws won't kill you even it touches you.

	**********Floor**********
	The light brown pixel block which represents the ground. When a pirate or knife is falling from the air, he/it will stop at the first floor he/it touches.

	**********Wall**********
	The Gray line at the very left and right side. When a pirate or knife touches the Wall, he/it will rebound to the reverse horizontal direction.

  (2) Beginning of the game
	Two pirates start in a symmetrical position, each with a knife in hands( though they don't seem to have hands )

    
    (3) Interaction of players ( rule for winning )
		The first pirate to win 5 points wins the game. ( Namely kills the other 5 times )
