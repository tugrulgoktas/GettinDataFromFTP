using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static CSV.Models.Constants;

namespace CSV.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

      




        private string _DateOfBirth;
        public string DateOfBirth
        {
            get {
              
                return _DateOfBirth;
            }
            set
            {
                _DateOfBirth = value;

                //Convert DateOfBirth to DateTime
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DateOfBirthDT = dtOut;
                int age = DateTime.Now.Year - DateOfBirthDT.Year;
                if (DateTime.Now.DayOfYear < DateOfBirthDT.DayOfYear)
                    _ = age - 1;
            }
        }

        public DateTime DateOfBirthDT { get; internal set; }
        public string ImageData { get; set; }

        public string AbsoluteUrl { get; set; }
        public string Directory { get; set; }
        public string FullPathUrl
        {
            get
            {
                return AbsoluteUrl + "/" + Directory;
            }
        }

        public List<String> Exceptions { get; set; } = new List<string>();

        public void FromCSV(string csvdata)
        {
            string[] data = csvdata.Split(",", StringSplitOptions.None);

            try
            {
                StudentId = data[0];
                FirstName = data[1];
                LastName = data[2];
                DateOfBirth = data[3];
                ImageData = data[4];
            }
            catch(Exception e)
            {
                Exceptions.Add(e.Message);

            }
        }

        public void FromDirectory(string directory)
        {
            Directory = directory;

            if(String.IsNullOrEmpty(directory.Trim()))
            {
                return;
            }

            string[] data = directory.Trim().Split(" ", StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
        }

        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DateOfBirthDT.ToShortDateString()},{ImageData}";
            return result;
        }

        public override string ToString()
        {
            string result = $"{StudentId} {FirstName} {LastName}";
            return result;
        }


       


    }
}
