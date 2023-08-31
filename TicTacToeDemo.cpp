#include <iostream>
#include <vector>

class TicTacToe {
public:
    TicTacToe() : board(3, std::vector<char>(3, ' ')) {}

    void printBoard() {
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                std::cout << board[i][j];
                if (j < 2) std::cout << '|';
            }
            std::cout << std::endl;
            if (i < 2) std::cout << "-----" << std::endl;
        }
    }

    bool isGameOver() {
        return isWin('X') || isWin('O') || getAvailableMoves().empty();
    }

    bool isWin(char player) {
        for (int i = 0; i < 3; ++i) {
            if (board[i][0] == player && board[i][1] == player && board[i][2] == player)
                return true;
            if (board[0][i] == player && board[1][i] == player && board[2][i] == player)
                return true;
        }
        if (board[0][0] == player && board[1][1] == player && board[2][2] == player)
            return true;
        if (board[0][2] == player && board[1][1] == player && board[2][0] == player)
            return true;
        return false;
    }

    std::vector<std::pair<int, int>> getAvailableMoves() {
        std::vector<std::pair<int, int>> moves;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (board[i][j] == ' ')
                    moves.emplace_back(i, j);
            }
        }
        return moves;
    }

    int evaluate() {
        if (isWin('X')) return 1;
        if (isWin('O')) return -1;
        return 0;
    }

    int minimax(int depth, bool isMaximizing) {
        if (isGameOver())
            return evaluate();

        if (isMaximizing) {
            int maxEval = INT_MIN;
            for (const auto& move : getAvailableMoves()) {
                board[move.first][move.second] = 'X';
                maxEval = std::max(maxEval, minimax(depth + 1, !isMaximizing));
                board[move.first][move.second] = ' ';
            }
            return maxEval;
        } else {
            int minEval = INT_MAX;
            for (const auto& move : getAvailableMoves()) {
                board[move.first][move.second] = 'O';
                minEval = std::min(minEval, minimax(depth + 1, !isMaximizing));
                board[move.first][move.second] = ' ';
            }
            return minEval;
        }
    }

    std::pair<int, int> findBestMove() {
        int bestEval = INT_MIN;
        std::pair<int, int> bestMove;
        for (const auto& move : getAvailableMoves()) {
            board[move.first][move.second] = 'X';
            int moveEval = minimax(0, false);
            board[move.first][move.second] = ' ';
            if (moveEval > bestEval) {
                bestEval = moveEval;
                bestMove = move;
            }
        }
        return bestMove;
    }

    void play() {
        while (!isGameOver()) {
            printBoard();
            int row, col;
            std::cout << "Enter row (0-2) and column (0-2): ";
            std::cin >> row >> col;
            if (row < 0 || row > 2 || col < 0 || col > 2 || board[row][col] != ' ') {
                std::cout << "Invalid move! Try again." << std::endl;
                continue;
            }
            board[row][col] = 'O';

            if (isGameOver()) {
                printBoard();
                break;
            }

            auto bestMove = findBestMove();
            board[bestMove.first][bestMove.second] = 'X';
        }

        printBoard();
        if (isWin('X'))
            std::cout << "AI wins!" << std::endl;
        else if (isWin('O'))
            std::cout << "You win!" << std::endl;
        else
            std::cout << "It's a draw!" << std::endl;
    }

private:
    std::vector<std::vector<char>> board;
};

int main() {
    TicTacToe game;
    game.play();
    return 0;
}