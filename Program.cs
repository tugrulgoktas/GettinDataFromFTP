using CSV.Models;
using CSV.Models.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CSV
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Student> students = new List<Student>();
            List<string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);

            string jsonSerialized = "{\"StudentId\":\"200443399\",\"FirstName\":\"Tugrul\",\"LastName\":\"Goktas\",\"ImagePath\":null,\"MyRecord\":false}";

            //Deserialize the JSON string
            Student studentDeserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Student>(jsonSerialized);

            //Output the Student using it's ToString() function
            Console.WriteLine(studentDeserialized);






            try
            {
                int numberOfStudent = directories.Count();
                Console.WriteLine(
                    "There are {0} student inside class",
                    numberOfStudent);

                double average = directories.Average(s => s.Length);

                Console.WriteLine("The average Ages of class are {0}.", average);

                double maxAge = directories.Max(s => s.Length);

                Console.WriteLine("The max Age is {0}.", maxAge);


                double minAge = directories.Min(s => s.Length);

                Console.WriteLine("The min Age is {0}.", minAge);



            }
            catch (OverflowException)
            {
                Console.WriteLine("The count is too large to store as an Int32.");
                Console.WriteLine("Try using the LongCount() method instead.");
            }









            foreach (var directory in directories)
           {
                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);



                Console.WriteLine(student);

                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InfoFile;

                bool fileExists = FTP.FileExists(infoFilePath);
                if (fileExists == true)
                {
                    string csvPath = $@"/Users/tugrulgoktas/Desktop/Students File{directory}.csv";



                    byte[] bytes = FTP.DownloadFileBytes(infoFilePath);
                    string csvData = Encoding.Default.GetString(bytes);

                    string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    if(csvlines.Length != 2)
                    {
                        Console.WriteLine("Error in CSV Format");
                    }
                    else
                    {
                        student.FromCSV(csvlines[1]);
                    }
                    

                    Console.WriteLine("Found info file:");
                }
                else
                {
                    Console.WriteLine("Could not find info file:");
                }

                Console.WriteLine("\t" + infoFilePath);

                string imageFilePath = student.FullPathUrl + "/" + Constants.Locations.ImageFile;

                bool imageFileExists = FTP.FileExists(imageFilePath);

                if (imageFileExists == true)
                {
                    Console.WriteLine("Found image file:");
                }
                else
                {
                    Console.WriteLine("Could not find image file:");
                }

                



                Console.WriteLine("/", imageFileExists);

                students.Add(student);
                

                //Console.WriteLine(directory);
            }

            string studentsCSVPath = $"{Constants.Locations.DataFolder}\\students.csv";

            //Establish a file stream to collect data from the response
            using (StreamWriter fs = new StreamWriter(studentsCSVPath))
            {
                foreach (var student in students)
                {
                    fs.WriteLine(student.ToCSV());
                }
            }

            File.WriteAllText(@"students.json", JsonConvert.SerializeObject(directories));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"students.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, students);
            }



            XmlSerializer serialiser = new XmlSerializer(typeof(List<string>));

            // Create the TextWriter for the serialiser to use
            TextWriter filestream = new StreamWriter(@"student.xml");

            //write to the file
            serialiser.Serialize(filestream, directories);

            // Close the file
            filestream.Close();






            return;


        }

    }
}
