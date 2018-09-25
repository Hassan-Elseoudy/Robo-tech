#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <ctype.h>
#include <stdbool.h>
#include <string.h>
#include <math.h>


int counter = 1;

void rotateIt(int,int);
void onesOrZeros();
void decorationFunction();
void changeTheArguments(short);
void checkEndianess();
void ageCalculator();
void interact(int);
int permuteDriver();
int withUser();
int yearCalc(int);
void swapP2(char*,char*);

int main()
{
    printf("//Rotate {Repeated [1]}\n");
    rotateIt(0x12345678,0);
    decorationFunction();

    printf("//OnesAndZeros? {Repeated [2]}\n");
    onesOrZeros();
    decorationFunction();

    printf("//ShortInteger {Pointers [1]}\n");
    changeTheArguments(0x00);
    decorationFunction();

    printf("//Endianess {Pointers [2]}\n");
    checkEndianess();
    decorationFunction();

    printf("//Permutations {Pointers [3]}\n");
    counter = permuteDriver() - 1;
    printf("Number of permutations is (number of characters)! = %d",counter);
    decorationFunction();

    printf("C code that interacts with user {Functions [1]}\n");
    interact(withUser());
    decorationFunction();

    printf("C code that calculates age {Functions [2]}\n");
    ageCalculator();
    decorationFunction();

    return 0;
}

void rotateIt(int num,int numOfShifts)
{
    printf("Enter number of shifts: ");
    scanf("%d",&numOfShifts);
    printf("Before Rotation -> %X\n",num);
    //if numOfShifts=4, num=0x12345678:
    //(num >> numOfShifts) = 0x12345678 >> 4 = 0x01234567
    //(num << (32-numOfShifts)) = (0x12345678 << 28) = 0x80000000
    //0x80000000 | 0x01234567 = 0x81234567
    printf("After Rotation -> %X",(num >> numOfShifts) | (num << (32-numOfShifts)));
}

void changeTheArguments(short shortInteger)
{
    printf("Enter your required integer (in hexa): ");
    scanf("%x",&shortInteger);
    printf("Before Rotation -> %X\n",shortInteger);
    shortInteger = ((shortInteger >> 8) | (shortInteger << 8));
    printf("After Rotation -> %X",shortInteger);
}
void decorationFunction()
{
    printf("\n----------------\n");
}

void checkEndianess()
{
    unsigned int var = 1;
    char *ch;
    // Character is storing only 1 byte of the integer (4 bytes)
    ch = &var;
    if (ch)
        // if ch == 1 "true" then machine is little Endian
        printf("Little Endian");
    else
        // else ch == 0 "false"
        printf("Big Endian");
}

// Doing the permutation (String itself, leftIndex, rightIndex)
int perP2(char *str, int leftPosition, int rightPosition)
{
    int i;
    if (leftPosition == rightPosition)      //Base case "can't permute"
    {
        printf("[%d] %s \n",counter, str);
        counter++;
    }
    else
    {
        for (i = leftPosition; i <= rightPosition; i++) // Actual program
        {
            swapP2((str+leftPosition), (str+i));    // Start swapping but it destroys string, need backtracking
            perP2(str, leftPosition+1, rightPosition); // Recursive function
            swapP2((str+leftPosition), (str+i));    // Backtracking
        }
    }
    return counter;
}

void swapP2(char *x, char *y)
{
    char temp = *x;
    *x = *y;
    *y = temp;
}

int permuteDriver()
{
    char str [100];
    printf("C code to print all permutations of a given string using pointers .\nEnter String: ");
    scanf("%s",str);
    return(perP2(str,0,strlen(str) - 1));
}

int withUser()
{
    int choice;
    printf("welcome to my code my name is smsm ,if you are a male enter 1 and if you are a female enter 2: ");
    scanf("%d",&choice);
    return(choice);

}

void interact(int choice)
{
    if(choice == 1)
        printf("welcome Mr!");
    else if(choice == 2)
        printf("welcome Miss!");
    else
        printf("Invalid choice!");
}

void ageCalculator()
{
    int yearBorn;
    printf("Enter the year you were born: ");
    scanf("%d",&yearBorn);
    printf("Your age is: %d",yearCalc(yearBorn));
}
int yearCalc(int yearBorn)
{
    if((2018 - yearBorn) > 100 || (2018 - yearBorn) <= 0)
        printf(":D\n");
    return (2018 - yearBorn);
}

void onesOrZeros()
{
    int n,i,j,sum,s;
    printf("Enter # of elements in the array: ");
    scanf("%d",&n);
    //SaveElements  //Save bits binaryNumber
    int arrInt[n], boolSum [n];
    //Save bits after 0b //Get rid of 0b
    char arrCh[n];
    printf("Enter elements in the array (spaced): ");
    for(i = 0; i < n; i++)
        scanf(" %d",&arrInt[i]);

    printf("Enter string of binary digits (e.g.10101010): ");
    scanf("%s",arrCh);

    //Get decimal value of string
    for(sum=0, j=0, s = strlen(arrCh)-1; s >= 0; s--, ++j)
        if(arrCh[s] == '1')
            sum += pow(2,j);

    //Extracting bits into boolSum Array
    for(i = 0; i < n; i++)
    {
        int mask =  1 << i;
        int masked_n = sum & mask;
        int thebit = masked_n >> i;
        boolSum[i] = thebit;
    }
    printf("Required data -> ");
    for(i = 0; i < n; i++)
    {
        if(boolSum[i])
            printf("%d\t",arrInt[i]);
    }
}
