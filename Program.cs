using System;
using Impinj.OctaneSdk;

namespace Vigilant_Scanner;

class Program
{   

    //need to be instantiated in its own function at some point to allow other readers
    const string READER_HOSTNAME = "1234"; //right now this will just be our one
    //this is fine
    static ImpinjReader reader = new ImpinjReader();


    static void ConnectToReader() {
        try {
            Console.WriteLine("Attempting to connect to {0}", READER_HOSTNAME);
            //max number of attempts
            //reader.MaxConnectionAttempts = 5;
            //time until timout
            reader.ConnectTimeout = 6000;
            //connect to the reader
            reader.Connect(READER_HOSTNAME);
            Console.WriteLine("Successfully connected.");
        } catch (OctaneSdkException e) {
            Console.WriteLine("Connection Failed.");
            throw (e);
        }
    }

    //curently not doing anything, but this is where we'll query/save responses
    static void AskVars() {

    }

    static void Main(string[] args)
    {
        
        ConnectToReader();
        
    }
}
