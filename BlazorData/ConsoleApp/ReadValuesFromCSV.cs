using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
    public string Nation { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // 1. Specify the exact CSV file path
        // string filePath = @"D:\Learning VS\ConsoleApp1\ConsoleApp1\Values.csv";
        string relativePath = @"..\..\..\Values.csv";
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));

          // 2. Read data from CSV
        List<Person> people = ReadPeopleFromCsv(filePath);

        // 3. Get Nation input from user
        Console.WriteLine("Enter a Nation:");
        string nationInput = Console.ReadLine();
        

        // 4. Filter people by Nation
        List<Person> filteredPeople = people.Where(p => p.Nation.Equals(nationInput, StringComparison.OrdinalIgnoreCase)).ToList();

        // 5. Display results
        if (filteredPeople.Count > 0)
        {
            Console.WriteLine("Details of people from " + nationInput + ":");
            foreach (var person in filteredPeople)
            {
                Console.WriteLine($"Name: {person.Name}, Age: {person.Age}, Sex: {person.Sex}, Nation: {person.Nation}");
            }
        }
        else
        {
            Console.WriteLine("No people found from " + nationInput + ".");
        }

        Console.ReadKey();
    }

    static List<Person> ReadPeopleFromCsv(string filePath)
    {
        List<Person> people = new List<Person>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                // Skip header row
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    people.Add(new Person
                    {
                        Name = values[0],
                        Age = int.Parse(values[1]),
                        Sex = values[2],
                        Nation = values[3]
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading CSV file: " + ex.Message);
        }

        return people;
    }
}
