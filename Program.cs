using System;
using Impinj.OctaneSdk;

namespace Vigilant_Scanner;

class Program
{   

    static string READER_HOSTNAME = "";
    //this is fine
    static ImpinjReader reader = new ImpinjReader();


    static void ConnectToReader() {
        try {
            Console.WriteLine("Attempting to connect to {0}", READER_HOSTNAME);
            //time until timout
            reader.ConnectTimeout = 6000;
            //connect to the reader
            reader.Connect(READER_HOSTNAME);
            Console.WriteLine("Successfully connected.");


        } catch (OctaneSdkException e) {
            Console.WriteLine("Connection Failed.");
            throw e;
        }
    }

    //curently not doing anything, but this is where we'll query/save responses
    static void AskVars() {
        Console.WriteLine("Please enter your reader's hostname");
        READER_HOSTNAME = Console.ReadLine(); //this still allows bad imputs
        //this is a janky way of stopping no imput from going through
        while (READER_HOSTNAME == "") {
            Console.WriteLine("No imput recieved, try agian");
            READER_HOSTNAME = Console.ReadLine();
        }
    }

    //here we'll change the default settings of the reader
    static void SettingManage(){
        //aquire the default settings
        Settings settings = reader.QueryDefaultSettings();

        

    }

    static void Main(string[] args)
    {
        Console.WriteLine("Thank you for using our program!");
        AskVars();
        try {
            ConnectToReader();
            SettingManage();
        } catch (OctaneSdkException e) {
            Console.WriteLine("Octane SDK exeption: {0}", e.Message);
        } catch (Exception e) {
            Console.WriteLine("Exeption: {0}", e.Message);
        }
        
    }
}
