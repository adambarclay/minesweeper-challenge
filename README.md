# Minesweeper Challenge

**Brief**

Write a game which follows the rules of “minesweeper” (see below). This should display the board to the user in the Windows console and give the player the chance to choose a location to “sweep”.

**Summary**

In Minesweeper, the objective is to find mines hidden on a board. The computer game will generate a board (in our case 8x8) and randomly place 8 mines somewhere on the board.

The player progressively reveals the board by “sweeping” a location. When the player does this, they are informed of the number of mines surrounding the selected location. For our game we will use the following key:

 - \* for a location which has yet to be revealed.
 - A number (0-8) which will identify the number of mines surrounding a location.
    - *Note: A 0 would indicate all the surrounding locations are mine free.*
- A letter (m) to indicate a mine.

Below is an example of a fully explored board:
*(for clarity this example contains only 3 mines, rather than 8)*

				m 1 0 0 0 0 0 0
				1 1 0 1 1 1 0 0
				0 0 0 1 m 1 0 0
				0 1 1 2 1 1 0 0
				0 1 m 1 0 0 0 0
				0 1 1 1 0 0 0 0
				0 0 0 0 0 0 0 0
				0 0 0 0 0 0 0 0

**Rules to generate the above**

- Where we see “2” this indicates there are 2 mines surrounding that location.
- Where we see “1” this indicates there is 1 mine surrounding to that location.
- Where we see “0” this indicates there are no surrounding mines, so every adjacent location is safe.

**Objective**

Write a console app which will:
- Construct an 8 x 8 board with 8 randomly distributed mines. Keep this hidden from the player.
- Via the console print out the 8 x 8 board to the player using an asterisk (\*) to indicate a hidden location. At the start all locations are hidden. Make sure to show the player the number of adjacent mines for the squares they have already revealed. The grid should have row and column identifiers *(see example sequence below)*.
- Ask the player for the location they wish to reveal.
- If the selected location is empty: 
    - Print the entire board to the console again andreveal the number of adjacent mines to the player.
    - If all the unrevealed locations contain mines: tell the player they won and end the program.
    - If there are still hidden mines: go back to the beginning and ask the player for the coordinates of another location to reveal.
- If the selected location contains a mine:
    - Print out the entire board and end the program. *(note: if the player is unlucky, this could be their first turn!)*

**Example Sequence**

For the following example assume the board hidden from the player is:

				m 1 0 0 0 0 0 0
				1 1 0 1 1 1 0 0
				0 0 0 1 m 1 0 0
				0 1 1 2 1 1 0 0
				0 1 m 1 0 0 0 0
				0 1 1 1 0 0 0 0
				0 0 0 0 0 0 0 0
				0 0 0 0 0 0 0 0

The first board shown to the player is:

				  A B C D E F G H
				1 * * * * * * * *
				2 * * * * * * * *
				3 * * * * * * * *
				4 * * * * * * * *
				5 * * * * * * * *
				6 * * * * * * * *
				7 * * * * * * * *
				8 * * * * * * * *

				Please enter a column and row (e.g. A8):

If the player types H8 the board is revealed as:

				  A B C D E F G H
				1 * * * * * * * *
				2 * * * * * * * *
				3 * * * * * * * *
				4 * * * * * * * *
				5 * * * * * * * *
				6 * * * * * * * *
				7 * * * * * * * *
				8 * * * * * * * 0

				Please enter a column and row (e.g. A8):

If the player types D4 the board is revealed as:

				  A B C D E F G H
				1 * * * * * * * *
				2 * * * * * * * *
				3 * * * * * * * *
				4 * * * 2 * * * *
				5 * * * * * * * *
				6 * * * * * * * *
				7 * * * * * * * *
				8 * * * * * * * 0
				
				Please enter a column and row (e.g. A8):

If the player types A1 (and a mine is hit) the complete board is revealed:

				  A B C D E F G H
				1 m 1 0 0 0 0 0 0
				2 1 1 0 1 1 1 0 0
				3 0 0 0 1 m 1 0 0
				4 0 1 1 2 1 1 0 0
				5 0 1 m 1 0 0 0 0
				6 0 1 1 1 0 0 0 0
				7 0 0 0 0 0 0 0 0
				8 0 0 0 0 0 0 0 0
				
				**Sorry you hit a mine**
				
				--Press any key to play again--
