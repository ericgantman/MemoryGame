# Memory Game

## Overview
A simple Memory Game in C#, where players match pairs of cards on a board. The game supports both human and computer players.

## Structure
- **Board.cs**: Manages the game board and card positions.
- **Card.cs**: Represents an individual card with `IsFaceUp` and `IsMatched` properties.
- **ComputerPlayer.cs**: Implements AI logic for the computer player.
- **HumanPlayer.cs**: Handles human player input.
- **GameEngine.cs**: Manages game flow, player turns, and checks for matches.
- **MemoryGame.cs**: Sets up the game and players.
- **Program.cs**: Entry point for running the game.

## How to Play
Players take turns flipping two cards. If the cards match, they stay face up. The game ends when all pairs are matched. The player with the most matches wins.