using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO
{
    class Program
    {
        /// <summary>
        /// main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string textFilePath = @"Data\Data.txt";

            SimpleTextReadWrite(textFilePath);
            //StructuredTextReadWrite(textFilePath);
            //FileStreamReadWrite(textFilePath);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// a method that demonstrates writing strings to a text file
        /// </summary>
        /// <param name="dataFile"></param>
        static void SimpleTextReadWrite(string dataFile)
        {
            string address01;
            string address02;

            string dataFileContents = "";

            // initialize strings with addresses
            address01 = "1,Flintstone,Fred,301 Cobblestone Way,Bedrock,70777\n";
            address02 = "2,Rubble,Barney,303 Cobblestone Way,Bedrock,70777\n";

            Console.WriteLine("The following addresses will be added to the text file.\n");
            Console.WriteLine(address01 + address02);

            Console.WriteLine("\nAdd addresses. Press any key to continue.\n");
            Console.ReadKey();

            // add address strings to the end of the text file
            File.AppendAllText(dataFile, address01);
            File.AppendAllText(dataFile, address02);

            Console.WriteLine("Addresses added successfully.\n");

            Console.WriteLine("Read and display the addresses from the text file. Press any key to continue.\n");
            Console.ReadKey();

            // read all of the data file info into a single string
            dataFileContents = File.ReadAllText(dataFile);

            Console.WriteLine(dataFileContents);
        }

        /// <summary>
        /// method that demonstrates a text file where rows have multiple properties
        /// </summary>
        /// <param name="dataFile"></param>
        static void StructuredTextReadWrite(string dataFile)
        {
            string dataString;

            // initialize a string with all of the addresses
            dataString = BuildDataString();

            Console.WriteLine("The following addresses will be added to text file.\n");
            Console.WriteLine(dataString);

            Console.WriteLine("\nAdd addresses. Press any key to continue.\n");
            Console.ReadKey();

            // empty the text file and add the addresses
            File.WriteAllText(dataFile, dataString);

            Console.WriteLine("Read and display the addresses from the text file. Press any key to continue.\n");
            Console.ReadKey();

            // split the text file string into individual properties and display
            DisplayDataByProperty(dataFile);
        }

        /// <summary>
        /// method to initialize and build out the addresses string
        /// </summary>
        /// <returns></returns>
        static string BuildDataString()
        {
            StringBuilder dataStringBuilder = new StringBuilder();

            string id;
            string lastName;
            string firstName;
            string address;
            string city;
            string state;
            string zip;

            // declare a property delineator
            const char delineator = ',';

            string dataString;

            // use the StringBuilder class to build the string of addresses
            id = "1";
            lastName = "Flintstone";
            firstName = "Fred";
            address = "301 Cobblestone Way";
            city = "Bedrock";
            state = "MI";
            zip = "70777";

            dataStringBuilder.AppendLine(
                id + delineator +
                lastName + delineator +
                firstName + delineator +
                address + delineator +
                city + delineator +
                state + delineator +
                zip);

            id = "2";
            lastName = "Rubble";
            firstName = "Barney";
            address = "303 Cobblestone Way";
            city = "Bedrock";
            state = "MI";
            zip = "70777";

            dataStringBuilder.AppendLine(
                id + delineator +
                lastName + delineator +
                firstName + delineator +
                address + delineator +
                city + delineator +
                state + delineator +
                zip);

            dataString = dataStringBuilder.ToString();

            return dataString;
        }

        /// <summary>
        /// method to display the addresses properties
        /// </summary>
        /// <param name="dataFile"></param>
        static void DisplayDataByProperty(string dataFile)
        {
            const char delineator = ',';

            // read each line of addresses as a string element in an array
            string[] addresses = File.ReadAllLines(dataFile);

            // iterate through the address array and display each property
            for (int index = 0; index < addresses.Length; index++)
            {
                // use the Split method and the delineator on the array to separate each property into an array of properties
                string[] properties = addresses[index].Split(delineator);

                Console.WriteLine("Id: {0}", properties[0]);
                Console.WriteLine("Last Name: {0}", properties[1]);
                Console.WriteLine("First Name: {0}", properties[2]);
                Console.WriteLine("Address: {0}", properties[3]);
                Console.WriteLine("City: {0}", properties[4]);
                Console.WriteLine("State: {0}", properties[5]);
                Console.WriteLine("Zip: {0}", properties[6]);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// a method that demonstrates writing strings to a text file using the StreamWriter/StreamReader objects
        /// </summary>
        /// <param name="dataFile"></param>
        static void FileStreamReadWrite(string dataFile)
        {
            string address01;
            string address02;

            // initialize a FileStream object for writing
            FileStream wfileStream = File.OpenWrite(dataFile);

            // wrap the FieldStream object in a using statement to ensure of the dispose
            using (wfileStream)
            {
                // wrap the FileStream object in a StreamWriter object to simplify writing strings
                StreamWriter sWriter = new StreamWriter(wfileStream);

                // initialize strings with addresses 
                address01 = "1,Flintstone,Fred,301 Cobblestone Way,Bedrock,70777\n";
                address02 = "2,Rubble,Barney,303 Cobblestone Way,Bedrock,70777\n";

                Console.WriteLine("The following addresses will be added to the text file.\n");
                Console.WriteLine(address01 + address02);

                Console.WriteLine("\nAdd addresses. Press any key to continue.\n");
                Console.ReadKey();

                // add address strings to the end of the text file
                sWriter.Write(address01);
                sWriter.Write(address02);

                sWriter.Close();

                Console.WriteLine("Addresses added successfully.\n");
            }

            // initialize a FileStream object for reading
            FileStream rFileStream = File.OpenRead(dataFile);

            // wrap the FieldStream object in a using statement to ensure of the dispose
            using (rFileStream)
            {
                // initialize a FileStream object for reading
                StreamReader sReader = new StreamReader(rFileStream);

                Console.WriteLine("Read and display the addresses from the text file. Press any key to continue.\n");
                Console.ReadKey();

                // keep reading lines of text until the end of the file is reached
                while (!sReader.EndOfStream)
                {
                    Console.WriteLine(sReader.ReadLine());
                }
            }
        }
    }
}
