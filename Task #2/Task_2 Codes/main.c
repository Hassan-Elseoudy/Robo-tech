#include <stdio.h>
#include <limits.h>
#include <stdlib.h>
#include <ctype.h>
#include <stdbool.h>
#include <string.h>
#include <math.h>

int main()
{
    printf("//Write a C code to know the sizes of data types {1}\n");
    sizeOfDataTypes();
    //decoration
    decorationFunction();
    printf("//Check Endianess {2}\n");
    checkEndianess();
    //decoration
    decorationFunction();
    printf("//Print Alphabetics {3}\n");
    printAlpha();
    //decoration
    decorationFunction();
    printf("//Rotate {5}\n");
    rotateIt(0x12345678,4);
    //decoration
    decorationFunction();
    printf("//Checking for a char {6}\n");
    charCheck();
    //decoration
    decorationFunction();
    printf("//Checking for an int {7}\n");
    intCheck();
    //decoration
    decorationFunction();
    printf("//Checking for division remainder {8}\n");
    divisibilityCheck();
    //decoration
    decorationFunction();
    printf("//Reverse an array {1}\n");
    reverseArray();
    //decoration
    decorationFunction();
    printf("//Sort an array {2}\n");
    sortArray();
    //decoration
    decorationFunction();
    printf("//Search in an array {3}\n");
    searchArray();
    //decoration
    decorationFunction();
    printf("//Descending print exclusively! {4}\n");
    printDescending();
    //decoration
    decorationFunction();
    printf("//Vowel or not?! {1}\n");
    vowelOrNotIf();
    //decoration
    decorationFunction();
    printf("//Vowel or not?! {1'}\n");
    vowelOrNotSwitch();
    //decoration
    decorationFunction();
    printf("//OnesAndZeros? {2}\n");
    onesOrZeros();
    //decoration
    decorationFunction();
    return 0;
}

void sizeOfDataTypes()
{
    printf("Size of char -> %d\nSize of int ->  %d\nSize of float ->    %d\nSize of short int ->    %d\nSize of long int -> %d\nSize of double ->   %d",sizeof(char),sizeof(int),sizeof(float),sizeof(short),sizeof(int),sizeof(double));
}
void decorationFunction()
{
    printf("\n----------------\n");
}
void checkEndianess()
{
    unsigned int var = 1;
    // Character is storing only 1 byte of the integer (4 bytes)
    char ch = (char*) & var;
    if (ch)
        // if ch == 1 "true" then machine is little endian
        printf("Little endian");
    else
        // else ch == 0 "false"
        printf("Big endian");
}

void printAlpha()
{
    int i;
    for(i = 'A'; i <= 'Z'; i++)
        printf("%c\t",(char) i);
}

void rotateIt(int num,int numOfShifts)
{
    printf("Before Rotation -> %X\n",num);
    //if numOfShifts=4, num=0x12345678:
    //(num >> numOfShifts) = 0x12345678 >> 4 = 0x01234567
    //(num << (32-numOfShifts)) = (0x12345678 << 28) = 0x80000000
    //0x80000000 | 0x01234567 = 0x81234567
    printf("After Rotation -> %X",(num >> numOfShifts) | (num << (32-numOfShifts)));
}

void charCheck()
{
    int ch = 0;
    printf("Enter a character: ");
    //Asking user to enter character and validate it
    scanf(" %c", &ch);
    while(!isalpha(ch))
    {
        printf("Enter a correct character!\n");
        scanf(" %c", &ch);
    }
    //Checking if the letter is upperCase or LowerCase
    if (ch <= 'Z' && ch >= 'A')
        // Difference between lowerCase and UpperCase is a space
        printf("Lower case is %c",(ch +' '));
    else
        printf("Upper case is %c",(ch -' '));
}

void intCheck()
{
    int num = 0;
    printf("Enter an integer: ");
    //Asking user to enter character and validate it
    scanf("%d", &num);
    //Checking if the letter is upperCase or LowerCase
    if ((num <= 'Z' && num >= 'A') || (num <= 'z' && num >= 'A'))
        // built-in function in <ctype.h> library
        printf("Lower case is %c\ - Upper case is %c",tolower(num),toupper(num));
    else
        printf("Not in letters ASCII range");
}

void divisibilityCheck()
{
    int a,b;
    printf("Enter 2 numbers (spaced): ");
    scanf("%d %d",&a,&b);
    //Get the maximum betn 2 nums and divide it by the minimum
    printf(((a)>(b)?(a):(b)) % ((a)<(b)?(a):(b)) == 0 ? "Divisible" : "Not Divisible");
}

void reverseArray()
{
    int n;
    printf("Enter # of elements in the array: ");
    scanf(" %d",&n);
    int arr[n];
    int i,j;
    printf("Enter elements in the array (spaced) : ");
    for(i = 0; i < n; i++)
        scanf(" %d",&arr[i]);
    for(i = 0,j = n-1; i < n / 2; i++,j--)
    {
        // Swapping
        arr[i] += arr[j];
        arr[j] = arr[i] - arr[j];
        arr[i] -= arr[j];
    }
    printf("Array in a reversed order\n");
    for(i = 0; i <n; i++)
        printf("%d\t",arr[i]);
}

void sortArray()
{
    int n;
    printf("Enter # of elements in the array: ");
    scanf(" %d",&n);
    int arr[n];
    int i,j;
    printf("Enter elements in the array (spaced) : ");
    for(i = 0; i < n; i++)
        scanf(" %d",&arr[i]);
    // Let's do the oldie old bubble sort O(N^2) :D
    for(i = 0; i < n - 1; i++)
    {
        for(j = 0; j < n - i - 1; j++)
        {
            if(arr[j] > arr[j+1])
            {
                arr[j+1] += arr[j];
                arr[j] = arr[j+1] - arr[j];
                arr[j+1] -= arr[j];
            }
        }
    }
    printf("Array in a sorted order\n");
    for(i = 0; i <n; i++)
        printf("%d\t",arr[i]);
}

void searchArray()
{
    int n,i,j,key;
    bool flag = false;
    printf("Enter # of elements in the array: ");
    scanf(" %d",&n);
    int arr[n];
    printf("Enter elements in the array (spaced) : ");
    for(i = 0; i < n; i++)
        scanf(" %d",&arr[i]);
    printf("Enter the element to be searched: ");
    scanf("%d",&key);
    //Search for the key in the array
    for(i = 0; i < n; i++)
        if(arr[i] == key)
        {
            printf("Key is @ index = %d",i);
            flag = true;
            break;
        }
    if(!flag)
        printf("Not in the array!");
}
void printDescending()
{
    int a,b,i;
    printf("Enter 2 numbers (spaced) (lower then upper): ");
    scanf("%d %d",&a,&b);
    printf("Numbers exclusively!\n");
    for(i = b - 1; i > a; i--)
        printf("%d\t",i);
}

void vowelOrNotIf()
{
    char ch;
    printf("Enter character (lower case): ");
    scanf(" %c",&ch);
    printf((ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u' || ch == 'y') ? "Vowel!" : "Consonant!");
}

void vowelOrNotSwitch()
{
    char ch;
    printf("Enter character (lower case): ");
    scanf(" %c",&ch);
    switch(ch)
    {
    case('a'):
    case('e'):
    case('i'):
    case('o'):
    case('u'):
    case('y'):
        printf("Vowel!");
        break;
    default:
        printf("Consonant!");
    }
}

void onesOrZeros()
{
    int n,i,j,sum,s;
    printf("Enter # of elements in the array: ");
    scanf(" %d",&n);
    //SaveElements  //Save bits binaryNumber
    int arrInt[n], boolSum [n];
    //Save bits after 0b //Get rid of 0b
    char arrCh[n], test[2];
    printf("Enter elements in the array (spaced): ");
    for(i = 0; i < n; i++)
        scanf(" %d",&arrInt[i]);

    printf("Enter string (ex.0b10101010): ");
    scanf("%2s%s",test,arrCh);

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
        if(boolSum[i])
            printf("%d\t",arrInt[i]);
}
