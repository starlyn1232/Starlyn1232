using System.Text;

namespace SecEfsEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Init Msg

            Console.WriteLine("\nWelcome to the Samsung Sec Efs Editor By Starlyn1232");

            //File path var

            string file = "";

            try
            {
                //Read File Path

                //Auto save if file is dropped to exe

                if (args.Length == 1)
                    file = args[0];

                else
                {
                    Console.Write($"\nPlease input the file path: ");
                    file = Console.ReadLine();
                }

                //Remove Quotes (Needed for Drop-In files)

                file = file.Replace("\"", "");

                //Open File

                Console.WriteLine($"\nOpening File: {file.Substring(file.LastIndexOf("\\")+1)}");

                var Sec = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

                //Use StreamReader to analyze file

                var reader = new StreamReader(Sec);

                //Save current sn

                Console.WriteLine("\nSuccess open file.");

                Console.Write("\nInput the current SN: ");

                string sn = Console.ReadLine();

                //Check New SN

                if (sn.Length != 11)
                    throw new IndexOutOfRangeException("Lenght error, SN must has 11 digits!!!");

                //Convert to hex string (Why? Because we can iterate null character with no problems)

                sn = Convert.ToHexString(Encoding.UTF8.GetBytes(sn));

                //Init Reader

                bool SN_Found = false;

                string line = "";
                var data = new byte[0];

                //Run analyzer

                while ((line = reader.ReadLine()) != null &&
                    (data = Encoding.UTF8.GetBytes(line)) != null)
                {
                    if ((line = Convert.ToHexString(data)) == null)
                        break;

                    if (line.Contains(sn))
                    {
                        SN_Found = true;

                        break;
                    }
                }

                //If current sn gets found, let's work

                if (SN_Found)
                {
                    Console.WriteLine($"\nData found! (SN : {sn})");

                    //Read New SN

                    Console.Write("\nInput the new SN: ");
                    var newSn = Console.ReadLine();

                    //Check New SN

                    if (newSn.Length != 11)
                        throw new IndexOutOfRangeException("Lenght error, SN must has 11 digits!!!");

                    Console.Write("\nApplying Changes: ");

                    //Applying changes

                    data = File.ReadAllBytes(file);

                    line = Convert.ToHexString(data);
                    line = line.Replace(sn, Convert.ToHexString(Encoding.UTF8.GetBytes(newSn)));

                    data = Convert.FromHexString(line);

                    //Saving new file

                    File.WriteAllBytes($"{file.Substring(0, file.LastIndexOf("\\") + 1)}sec_efs_new.img", data);

                    //Final Msg

                    Console.Write("Done!");

                    Console.WriteLine("\n\nNew file saved as: sec_efs_new.img");
                }

                //Current sn wasn't found

                else
                    Console.WriteLine("\nNot Data found!");

                //Close StreamReader

                reader.Close();

                //Close File Stream

                Sec.Close();

                //Show final Msg

                Console.WriteLine("\nPress Enter to close. Enjoy :)");
            }

            catch (Exception ex)
            {
                //Error viewer

                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
