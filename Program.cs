// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
namespace MyFirstProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            float number = 50f;
            int age = (int) number;
            Console.WriteLine(age);

            // Keep console window open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

// ****************************************************************************************
//            int num = 2344;
//            //  = "Hello, World!";
//            Console.WriteLine(num);

//            if (num == 234)
//            {
//                Console.WriteLine("Yes, it is " + num);
//            }
//            else
//            {
//                Console.WriteLine("No, it is not " + num);
//            }

//            Console.WriteLine();
//            foreach (var s in args)
//            {
//                Console.Write(s);
//                Console.Write(' ');
//            }
//            Console.WriteLine(401);

//            string[] answers =
//                        {
//                "It is certain.",       "Reply hazy, try again.",     "Don’t count on it.",
//                "It is decidedly so.",  "Ask again later.",           "My reply is no.",
//                "Without a doubt.",     "Better not tell you now.",   "My sources say no.",
//                "Yes – definitely.",    "Cannot predict now.",        "Outlook not so good.",
//                "You may rely on it.",  "Concentrate and ask again.", "Very doubtful.",
//                "As I see it, yes.",
//                "Most likely.",
//                "Outlook good.",
//                "Yes.",
//                "Signs point to yes.",
//            };
//            Console.WriteLine(answers[new Random().Next(0, answers.Length)]);

//            // variables are declared once and can be changed later once
//            // Naming of variables 
//            // constants are declared and cannot be changed
//            // THE PRIMITIVE_TYPES ARE:
//            /*
//            INTERGER
//            FLOAT
//            DOUBLE
//            CHAR
//            BOOLEAN
//            */
//            Integers
//int myInt = 5;
//            int myInt2 = myInt + 5;
//            Console.WriteLine(myInt2);
//            // Floats
//            float myFloat = 5.5f;
//            float myFloat2 = myFloat + 5.5f;
//            Console.WriteLine(myFloat2);
//            // Doubles  
//            double myDouble = 5.5;
//            double myDouble2 = myDouble + 5.5;
//            Console.WriteLine(myDouble2);
//            // Chars
//            char myChar = 'a';
//            char myChar2 = myChar;
//            Console.WriteLine(myChar2);
//            // Booleans
//            bool myBool = true;
//            bool myBool2 = myBool;
//            Console.WriteLine(myBool2);


        }
    }

}
