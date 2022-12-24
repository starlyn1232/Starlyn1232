//Record class
    internal static class Record
    {
        //Record path
        internal static string dir = Directory.GetCurrentDirectory();

        //Record File
        internal static string record_file = $"{dir}\\record.diaz";

        //Check Key
        internal static bool CheckKey(string keyName)
        {
            bool result = false;

            //Read Stream String data
            using (var reader = new StreamReader(record_file))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    //keyName found
                    if (line == $"[{keyName}]")
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        //Remove Key
        internal static string RemoveKey(string keyName)
        {
            //New empty record
            string new_record = "";
            //Apply changes flag
            bool apply_changes = false;

            //Let's read stream data using StreamReader
            using (var reader = new StreamReader(record_file))
            {
                //Temp line holder
                string line = "";

                //Needed flag to skip keyValue data
                bool skip = false;

                //We'll read each record lines
                while ((line = reader.ReadLine()) != null)
                {
                    //KeyName Found
                    if (line == $"[{keyName}]")
                    {
                        apply_changes = true;
                        skip = true;
                        continue;
                    }

                    //Skip = 'We are skipping this keyValue data'
                    else if (skip)
                    {
                        //Stop skipping when found another keyName, it's kind of tricky because
                        //we should have '[Some value]' at keyValue, but surely you can help me
                        //to improve it
                        if (line.Contains('[') && line.Contains(']'))
                        {
                            new_record += ((new_record == "") ? (line) : ($"\n{line}"));
                            skip = false;
                        }
                    }

                    //Otherwise we save the content to the new_record
                    else
                        new_record += ((new_record == "") ? (line) : ($"\n{line}"));
                }
            }

            //keyName found? We save changes at record file
            if (apply_changes)
            {
                Manager.Manager_File.CreateFile(record_file, true);

                using (var writer = new StreamWriter(record_file))
                    writer.Write(new_record);
            }

            //Return updated record content
            return new_record;
        }

        //Write Key
        internal static void WriteKey(string keyName, string keyValue)
        {
            //Remove old keyName,keyValue content (if exits)
            //and save new content data
            string temp = RemoveKey(keyName);

            //StreamWriter for the updated record file
            using (var writer = new StreamWriter(record_file))
            {
                //So we here write the old + new content
                writer.WriteLine($"{((temp == "") ? temp : $"{temp}\n")}[{keyName}]\n{keyValue}");
            }
        }

        //Read Key
        internal static string ReadKey(string keyName)
        {
            string read = "NOT FOUND";
            string line = "";

            //StreamReader to read the keyValue
            using (var reader = new StreamReader(record_file))
            {
                //Found flag
                bool found = false;

                //Read the stream content
                while ((line = reader.ReadLine()) != null)
                {
                    //Found keyName
                    if (line == $"[{keyName}]")
                    {
                        //Init read
                        read = "";
                        found = true;
                        continue;
                    }

                    else if (found)
                    {
                        //Skip we keyName change
                        if (line.Contains('[') && line.Contains(']'))
                            break;

                        //Otherwise keep reading keyValue content
                        read += ((read == "") ? (line) : ($"\n{line}"));
                    }
                }
            }

            //Return keyValue
            return read;
        }

        //Set string with key
        internal static void DirectRead(ref string str, string keyName)
        {
            string temp = ReadKey(keyName);

            str = (temp == "NOT FOUND") ? str : temp;
        }
