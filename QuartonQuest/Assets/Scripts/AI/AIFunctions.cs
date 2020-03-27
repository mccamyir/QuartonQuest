﻿using System;
using HeuristicCalculator;
using System.Collections.Generic;
namespace AI
{
    public static class AIFunctions
    {

        // Copies a node's piece map to a string array
        public static void copyPieceMap(QuartoSearchTree.Node recievingNode, QuartoSearchTree.Node nodeToCopy, int pieceToFind)
        {
            string pieceString;
            bool piecePlayable;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                pieceString = nodeToCopy.pieces[i].getPiece();
                piecePlayable = nodeToCopy.pieces[i].getPlayablePiece();

                recievingNode.pieces[i].setValues(pieceString, piecePlayable);
            }
            recievingNode.pieces[pieceToFind].setPlayable(false);
        }

        public static void printBoard(QuartoSearchTree.Node current)
        {
            for (int j = 0; j < QuartoSearchTree.MAXGAMEBOARD; j++)
            {
                if (current.gameBoard[j] != null)
                {
                    Console.Write(current.gameBoard[j]);
                    Console.Write(" ");
                }
                else
                    Console.Write("XX ");

                if ((j + 1) % 4 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.Write("Piece to Play: ");
            Console.WriteLine(current.pieceToPlay);
            Console.WriteLine();
        }

        public static int countPiecesOnBoard(string[] gameBoard)
        {
            int pieceCount = 0;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] != null)
                    pieceCount += 1;
            }

            return pieceCount;
        }

        // Depths set to perform under 5 seconds in current state
        public static int setTreeDepth(int piecesOnBoard)
        {
            int newDepth = 1;
            if (piecesOnBoard == 1)
                newDepth = 4;
            else if (piecesOnBoard >= 2 && piecesOnBoard <= 5)
                newDepth = 5;
            else if (piecesOnBoard >= 6 && piecesOnBoard <= 7)
                newDepth = 6;
            else if (piecesOnBoard == 8)
                newDepth = 7;
            else if (piecesOnBoard == 9)
                newDepth = 8;
            else if (piecesOnBoard > 9)
                newDepth = 14;

            return newDepth;
        }

        // Checks if the game state that is being sent back to the oppenent will cause the opponent to win. If so find another solution,
        // if no solution or no win send that game state. 
        public static QuartoSearchTree.Node checkForOpponentWin(QuartoSearchTree.Node currentNode)
        {
            QuartoSearchTree.Node newWinNode = null;
            bool hasWon = true;
            string[] gameBoard;
            string piece;
            int move;
            for (int i = 0; hasWon && currentNode.parent.children[i] != null; i++)
            {
                gameBoard = currentNode.parent.children[i].gameBoard;
                piece = currentNode.parent.children[i].pieces[currentNode.parent.children[i].pieceToPlay].getPiece();
                move = currentNode.parent.children[i].moveOnBoard;

                hasWon = isWinOnBoard(gameBoard, piece);

                if (!hasWon)
                {
                    newWinNode = currentNode.parent.children[i];
                }
            }

            if (hasWon && newWinNode == null)
            {
                newWinNode = currentNode;
            }

            return newWinNode;
        }

        public static bool isWinOnBoard(string[] gameBoard, string pieceString)
        {
            bool hasWon = true;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] == null)
                {
                    hasWon = Heuristic.isWin(gameBoard, pieceString, i);
                    if (hasWon)
                        return hasWon;
                }
            }

            return hasWon;
        }

        public static List<int> makePlayablePiecesOnly(Piece[] pieces)
        {
            List<int> piecesPlayable = new List<int>();
            for(int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if(pieces[i].playable == true)
                {
                    piecesPlayable.Add(i);
                }
            }

            return piecesPlayable;
        }

        public static List<int> makePlayablePositionList(string[] gameBoard)
        {
            List<int> positionsPlayable = new List<int>();
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] == null)
                {
                    positionsPlayable.Add(i);
                }
            }
            return positionsPlayable;
        }
    }
}
