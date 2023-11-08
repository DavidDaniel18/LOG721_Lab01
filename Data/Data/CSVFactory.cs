using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Data
{
    public class CSVFactory
    {
        public static List<Group> CreateGroupFromCSVFile(string filePath)
        {
            List<Group> groups = new List<Group>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length != 2)
                    {
                        throw new Exception("Fields array length does not meet the expected length.");
                    }
                    if (fields[0] == "group_id" && fields[1] == "barycenter")
                    {
                        continue;
                    }


                    int groupId = int.Parse(fields[0].Replace("#", ""));
                    string[] barycenters = fields[1].Replace("<", "").Replace(">", "").Split(',');
                    if (barycenters.Length != 2)
                    {
                        throw new Exception("Barycenters array length does not meet the expected length.");
                    }

                    int width = int.Parse(barycenters[0]);
                    int price = int.Parse(barycenters[1]);
                    Data data = new Data(width, price);


                    Group group = new Group(groupId, data);
                    groups.Add(group);

                }
            }

            return groups;
        }


        public static List<Data> CreateDataFromCSVFile(string filePath)
        {
            List<Data> datas = new List<Data>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length != 2)
                    {
                        throw new Exception("Fields array length does not meet the expected length.");
                    }
                    if (fields[0] == "width(pixel)" && fields[1] == "price($)")
                    {
                        continue;
                    }

                    int width = int.Parse(fields[0]);
                    int price = int.Parse(fields[1]);

                    Data data = new Data(width, price);
                    datas.Add(data);

                }
            }

            return datas;
        }

    }
}