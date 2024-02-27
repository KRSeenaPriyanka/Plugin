//C# Defining an Enum

// To access an element of an enum, we write EnumName.ElementName, where EnumName is the name of the enum and ElementName is the name of the element.
//To get the integer value, we need to cast it into integer - (int)EnumName.ElementName

using System;

enum Season
{
    Summer,
    Spring,
    Winter,
    Autumn
}

class Test
{
    public static void Main(string[] args)
    {
        int a = (int)Season.Summer;
        int b = (int)Season.Spring;
        int c = (int)Season.Winter;
        int d = (int)Season.Autumn;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
    }
}

//You can see that by default, values started from 0.

enum Season1
{
    Summer = 5,
    Spring,
    Winter,
    Autumn
}

class Test1
{
    static void Main(string[] args)
    {
        int a = (int)Season.Summer;
        int b = (int)Season.Spring;
        int c = (int)Season.Winter;
        int d = (int)Season.Autumn;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
    }
}

//Here, we have given Summer a value of 5. So, the values started from 5.

enum Season2
{
    Summer = 5,
    Spring = 10,
    Winter = 15,
    Autumn = 20
}

class Test2
{
    static void Main(string[] args)
    {
        int a = (int)Season.Summer;
        int b = (int)Season.Spring;
        int c = (int)Season.Winter;
        int d = (int)Season.Autumn;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
    }
}

//C# GetNames


enum Season3
{
    Summer,
    Spring,
    Winter,
    Autumn
}

class Test3
{
    static void Main(string[] args)
    {
        foreach (string i in Enum.GetNames(typeof(Season)))
            Console.WriteLine(i);
    }
}

// C# GetValues

enum Season4
{
    Summer,
    Spring,
    Winter,
    Autumn
}

class Test4
{
    static void Main(string[] args)
    {
        foreach (int i in Enum.GetValues(typeof(Season)))
            Console.WriteLine(i);
    }
}

//C# GetName

enum Season5
{
    Summer,
    Spring,
    Winter,
    Autumn
}

class Test5
{
    static void Main(string[] args)
    {
        Console.WriteLine(Enum.GetName(typeof(Season), 3));
    }
}