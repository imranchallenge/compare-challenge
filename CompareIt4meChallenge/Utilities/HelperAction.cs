using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using CompareIt4meChallenge.Models;
using Microsoft.VisualBasic.FileIO;
using System.Net;

namespace CompareIt4meChallenge.Utilities
{
    public class HelperAction
    {
        public static Messages ParseFile(string path)
        {
            List<Person> people = new List<Person>();
            string code;
            string name;
            int lineCount = 0;
            Messages messages = new Messages();

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(path);
            StreamReader reader = new StreamReader(stream);

            using (TextFieldParser csvParser = new TextFieldParser(reader))
            {
                csvParser.SetDelimiters(new string[] {","});
                // Skip the row with the column names
                //csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    lineCount++;
                    if (fields != null && fields.Length == 3)
                    {
                        if (fields[0].Length != 10)
                        {
                            messages.Body = string.Format("code length not correct on line : {0}", lineCount);
                            messages.Status = "0";

                            return messages;
                        }

                        double coin = 0;
                        if (!double.TryParse(fields[1], out coin))
                        {

                            messages.Body = string.Format("coin not correct format on line : {0}", lineCount);
                            messages.Status = "0";

                            return messages;
                        }
                        if (coin < 0)
                        {
                            messages.Body = string.Format("coin value should greater then zero on line: {0}", lineCount);
                            messages.Status = "0";

                            return messages;
                        }

                        code = fields[0];
                        name = fields[2];

                        people.Add(new Person {Code = code, Coin = coin, Name = name});
                    }
                    else
                    {
                        messages.Body = string.Format("record not correct on line: {0}", lineCount);
                        messages.Status = "0";

                        return messages;
                    }
                }
            }
            SavePerson(people);
            return new Messages() {Status = "1", Body = "Record save successfully"};
        }

        private static void SavePerson(List<Person> people)
        {
            try
            {
                ChallengeDb db = new ChallengeDb();

                foreach (Person t in people)
                {
                    db.People.Add(t);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("can not save people in db!");
            }
        }
    }
}