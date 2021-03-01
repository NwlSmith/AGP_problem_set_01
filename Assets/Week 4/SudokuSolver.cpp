#include <iostream>
#include <fstream>
#include <cstring>

/*
 * Date: 2/24/2021
 * Description: Sudoku solver.
 * Unity crashed when I tried run this extremely inefficient code, so I decided to write in CPP instead.
 */

 /**
  * Integer to string.
  *
  * Converts a string to a char array string of the given base.
  * @param Value value to be converted to char array.
  * @param Base given base for integer conversion, either 8 (octal), 10 (decimal), or 16 (hex).
  * @return converted char array.
  */
char *itoa(int Value, int Base)
{
	// Check if valid base.
	if (Base != 8 && Base != 10 && Base != 16)
	{
		throw std::invalid_argument("Invalid base.");
		return nullptr;
	}

	// Calculate length
	int length = 0;

	// For Base 10, if Value is negative, make it positive and increase the length of the array.
	int valueSigned = Value;
	if (Base == 10 && Value < 0)
	{
		valueSigned *= -1;
		length++;
	}

	// Count number of times Base goes into Value. This is the number of digits.
	// Without the restriction on built-in functions, logarithms would make this more efficient.
	unsigned int valueUnsigned = valueSigned;
	do
	{
		length++;
		// Retrieve next digit.
		valueUnsigned /= Base;
	} while (valueUnsigned > 0); // Do-while loop for special case where Value = 0.


	char *str = (char *)malloc((length + 1) * sizeof(char));
	unsigned int digitIndex = length - 1;
	str[length] = '\0';

	// Set unsigned Value back to original
	valueUnsigned = valueSigned;

	char possibleChars[17] = "0123456789abcdef";

	// loop until Value is zero.
	do
	{
		// Set str array to corresponding char.
		str[digitIndex] = possibleChars[valueUnsigned % Base];
		digitIndex--;

		// Retrieve next digit.
		valueUnsigned /= Base;
	} while (valueUnsigned > 0); // Do-while loop for special case where Value = 0.

	// Accommodate negative numbers in base 10.
	if (Base == 10 && Value < 0)
		str[0] = '-';

	return str;
}

/**
 * Matrix to string in spiral order.
 *
 * Converts an integer matrix to a char array string by taking entries in clockwise order..
 * @param Matrix integer matrix.
 * @param NumRows number of rows, height of matrix.
 * @param NumColumns number of columns, width of matrix.
 * @param OutBuffer returned string buffer, assumed to be valid and large enough to hold all data.
 */
void BuildStringFromMatrix(int *Matrix, int NumRows, int NumColumns, char *OutBuffer)
{
	// 4 variables for array positions.
	int curLeft = 0;
	int curRight = NumColumns - 1;
	int curTop = 0;
	int curBot = NumRows - 1;

	int pos = 0;

	std::string str;

	while (curLeft <= curRight && curTop <= curBot)
	{
		// Move right from top-left-most position to top-right-most position.
		for (pos = curLeft; pos <= curRight; pos++)
		{
			str += std::to_string(Matrix[curTop * NumColumns + pos]); // Matrix[curTop][pos].
			str += ", ";
		}
		// Top row has been read, move down.
		curTop++;

		// Move down from top-right-most position to bottom-right-most position.
		for (pos = curTop; pos <= curBot; pos++)
		{
			str += std::to_string(Matrix[pos * NumColumns + curRight]); // Matrix[pos][curRight].
			str += ", ";
		}
		// Right row has been read, move left.
		curRight--;

		// Check that this row hasn't already been read.
		if (curTop <= curBot)
		{
			// Move left from bottom-right-most position to bottom-left-most position.
			for (pos = curRight; pos >= curLeft; pos--)
			{
				str += std::to_string(Matrix[curBot * NumColumns + pos]); // Matrix[curBot][pos].
				str += ", ";
			}
			// Bottom row has been read, move up.
			curBot--;
		}

		// Check that this column hasn't already been read.
		if (curLeft <= curRight)
		{
			// Move up from bottom-left-most position to top-left-most position.
			for (pos = curBot; pos >= curTop; pos--)
			{
				str += std::to_string(Matrix[pos * NumColumns + curLeft]); // Matrix[pos][curLeft]
				str += ", ";
			}
			// Left row has been read, move right.
			curLeft++;
		}
	}

	// If the matrix is not empty, delete the last 2 memory positions (the extra ", ").
	if (NumColumns > 0 && NumRows > 0)
	{
		str.pop_back();
		str.pop_back();
	}

	std::strcpy(OutBuffer, str.c_str());

	return;
}

int boardInts [81];
bool boardOriginal[81];

void PopulateBoard()
{
	//char inputFile [82] = "58..6.2.............3.......28.....51.......7.............76...36...9...875......";
	char inputFile [82] = "..............3.85..1.2.......5.7.....4...1...9.......5......73..2.1........4...9";


	for (int i = 0; i < sizeof(inputFile)/sizeof(*inputFile); i++)
	{
		if (inputFile[i] != ('.'))
		{
			boardInts[i] = (inputFile[i] - 48);
			boardOriginal[i] = true;
		}
		else
		{
			boardInts[i] = 0;
		}
	}
}

void PrintBoard()
{
	for (int y = 0; y < 3; y++)
	{
		for (int x = 0; x < 3; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 3; x < 6; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 6; x < 9; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << std::endl;
	}
	std::cout << std::endl;
	
	for (int y = 3; y < 6; y++)
	{
		for (int x = 0; x < 3; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 3; x < 6; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 6; x < 9; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << std::endl;
	}
	std::cout << std::endl;

	for (int y = 6; y < 9; y++)
	{
		for (int x = 0; x < 3; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 3; x < 6; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << " ";
		for (int x = 6; x < 9; x++)
		{
			std::cout << boardInts[y * 9 + x] << " ";
		}
		std::cout << std::endl;
	}
	std::cout << std::endl;

	
	std::cout << std::endl;
	std::cout << std::endl;
}

bool Conflict(int curIndex, int lookIndex)
{
	return boardInts[lookIndex] == boardInts[curIndex] && lookIndex != curIndex;
}

bool ValidPlacementLine(int curIndex)
{
	
	// From the left of the row to the right.
	// This loop has good locality.
	int left = curIndex - (curIndex % 9);
	int right = left + 9;
	for (int i = left; i < right; i++)
	{
		// If this square is the same as this number, AND this square is not looking at itself, return false;
		if (Conflict(curIndex, i))
		{
			return false;
		}
	}

	// This loop has HORRIBLE locality.
	// From the top of the column to the bottom.
	for (int i = curIndex % 9; i < 81; i += 9)
	{
		// If this square is the same as this number, AND this square is not looking at itself, return false;
		if (Conflict(curIndex, i))
			return false;
	}
	return true;
}

bool ValidPlacementSq(int curIndex)
{
	int xComponent = ((curIndex % 9) / 3) * 3;
	int yComponent = (curIndex / 27) * 3;

	int upperLeft = yComponent * 9 + xComponent;
	int lowerRight = upperLeft + 18;
	for (int y = upperLeft; y <= lowerRight; y += 9)
	{
		for (int x = 0; x < 3; x++)
		{
			int i = y + x;

			if (Conflict(curIndex, i))
				return false;
		}
	}

	return true;
}

bool ValidPlacement(int curIndex)
{
	return ValidPlacementLine(curIndex) && ValidPlacementSq(curIndex);
}

// This must be done recursively, otherwise I would 81 nested for loops
bool SolverRecurse(int index)
{
	if (index >= 81) return true;

	// If this square is an original, just move onto the next.
	if (boardOriginal[index])
	{
		return SolverRecurse(index + 1);
	}

	for (int num = 1; num <= 9; num++)
	{
		boardInts[index] = num;
		if (ValidPlacement(index))
		{
			if (SolverRecurse(index + 1))
				return true;
		}
	}
	boardInts[index] = 0;

	return false;
}

int main()
{
	PopulateBoard();
	PrintBoard();
	SolverRecurse(0);
	PrintBoard();

	//std::cout << itoa(1, 11) << std::endl;
	/*
	std::cout << "0, 10 answer should be 0 \n";
	char* c1 = itoa(0, 10);
	std::cout << c1 << std::endl;
	free(c1);
	std::cout << "-0, 10 answer should be 0 \n";
	std::cout << itoa(-0, 10) << std::endl;
	std::cout << "2147483647, 10 answer should be 2147483647 \n";
	std::cout << itoa(2147483647, 10) << std::endl;
	std::cout << "9, 10 answer should be 9 \n";
	std::cout << itoa(9, 10) << std::endl;
	std::cout << "-9, 10 answer should be -9 \n";
	std::cout << itoa(-9, 10) << std::endl;
	std::cout << "123, 10 answer should be 123 \n";
	std::cout << itoa(123, 10) << std::endl;
	std::cout << "-123, 10 answer should be -123 \n";
	std::cout << itoa(-123, 10) << std::endl;
	std::cout << "10, 10 answer should be 10 \n";
	std::cout << itoa(10, 10) << std::endl;
	std::cout << "-10, 10 answer should be -10 \n";
	std::cout << itoa(-10, 10) << std::endl;
	std::cout << "123456789, 10 answer should be 123456789 \n";
	std::cout << itoa(123456789, 10) << std::endl;
	std::cout << "-123456789, 10 answer should be -123456789 \n";
	std::cout << itoa(-123456789, 10) << std::endl
		 << std::endl;

	std::cout << "0, 8 answer should be 0 \n";
	std::cout << itoa(0, 8) << std::endl;
	std::cout << "7, 8 answer should be 7 \n";
	std::cout << itoa(7, 8) << std::endl;
	std::cout << "-7, 8 answer should be 37777777771 \n";
	std::cout << itoa(-7, 8) << std::endl;
	std::cout << "8, 8 answer should be 10 \n";
	std::cout << itoa(8, 8) << std::endl;
	std::cout << "-8, 8 answer should be 37777777770 \n";
	std::cout << itoa(-8, 8) << std::endl;
	std::cout << "10, 8 answer should be 12 \n";
	std::cout << itoa(10, 8) << std::endl;
	std::cout << "0xff ff ff, 8 answer should be 77 77 77 77\n";
	std::cout << itoa(0xFFFFFF, 8) << std::endl
		 << std::endl;

	std::cout << "0, 16 answer should be 0 \n";
	std::cout << itoa(0, 16) << std::endl;
	std::cout << "9, 16 answer should be 9 \n";
	std::cout << itoa(9, 16) << std::endl;
	std::cout << "15, 16 answer should be F \n";
	std::cout << itoa(15, 16) << std::endl;
	std::cout << "16, 16 answer should be 10 \n";
	std::cout << itoa(16, 16) << std::endl;
	std::cout << "-7, 16 answer should be idk what this is \n";
	std::cout << itoa(-7, 16) << std::endl;
	std::cout << "0xffff, 16 answer should be FFFF \n";
	std::cout << itoa(0xFFFF, 16) << std::endl;
	std::cout << "0xffffffff, 16 answer should be FFFF FFFF \n";
	std::cout << itoa(4294967295, 16) << std::endl;
	std::cout << "0x1000, 16 answer should be 1000 \n";
	std::cout << itoa(0x1000, 16) << std::endl;

	std::cout << "\n\n0xffffffff, 10 answer should be -1 \n";
	std::cout << itoa(0xffffffff, 10) << std::endl;
	std::cout << "\n\n0xffffffff, 8 answer should be 37777777777 \n";
	std::cout << itoa(0xffffffff, 8) << std::endl;


	char outBuffer[100];
	//int mat[12] = {2, 3, 4, 8, 5, 7, 9, 12, 1, 0, 6, 10};
	int mat[12] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
	//int matrix[3][4] = {{2, 3, 4, 8}, {5, 7, 9, 12}, {1, 0, 6, 10}};
	BuildStringFromMatrix(mat, 3, 4, outBuffer);

	std::cout << "3, 4: " << outBuffer << std::endl;

	char outBuffer1[100];

	BuildStringFromMatrix(mat, 2, 6, outBuffer1);

	std::cout << "2, 6: " << outBuffer1 << std::endl;

	char outBuffer2[100];

	BuildStringFromMatrix(mat, 1, 12, outBuffer2);

	std::cout << "1, 12: " << outBuffer2 << std::endl;

	char outBuffer3[100];

	BuildStringFromMatrix(mat, 0, 12, outBuffer3);

	std::cout << "0, 12: " << outBuffer3 << std::endl;

	char outBuffer4[100];

	BuildStringFromMatrix(mat, 4, 3, outBuffer4);

	std::cout << "4, 3: " << outBuffer4 << std::endl;

	char outBuffer5[100];

	BuildStringFromMatrix(mat, 6, 2, outBuffer5);

	std::cout << "6, 2: " << outBuffer5 << std::endl;

	char outBuffer6[100];

	BuildStringFromMatrix(mat, 12, 1, outBuffer6);

	std::cout << "12, 1: " << outBuffer6 << std::endl;

	char outBuffer7[100];

	BuildStringFromMatrix(mat, 1, 1, outBuffer7);

	std::cout << "1, 1: " << outBuffer7 << std::endl;*/
}