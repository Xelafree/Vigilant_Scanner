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

    //this is where it queries/saves responses
    static void AskVars() {
        Console.WriteLine("Please enter your reader's hostname:");
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

        //autostart settings (lets the scanner run without client connected)
        settings.AutoStart.Mode = AutoStartMode.Immediate;
        settings.AutoStop.Mode = AutoStopMode.None;

        //GPO to GPO 1 when LLRP is connected
        settings.Gpos.GetGpo(1).Mode = GpoMode.LLRPConnectionStatus;

        //show timestamps
        settings.Report.IncludeFirstSeenTime = true;
        settings.Report.IncludeLastSeenTime = true;
        settings.Report.IncludeSeenCount = true;

        //hold reports for recconect
        settings.HoldReportsOnDisconnect = true;

        //keepalives
        settings.Keepalives.Enabled = true;
        settings.Keepalives.PeriodInMs = 5000;

        //close connection after 5 failed keepalives
        settings.Keepalives.EnableLinkMonitorMode = true;
        settings.Keepalives.LinkDownThreshold = 5;

        //assign event handlers
        reader.KeepaliveReceived += OnKeepaliveReceived;
        reader.ConnectionLost += OnConnectionLost;

        //temporarily apply settings
        reader.ApplySettings(settings);

        //apply settings to non-voilitile (currently not active for debugging)
        //reader.SaveSettings(settings);


    }

    static void Main(string[] args)
    {
        Console.WriteLine("Thank you for using our program!");
        AskVars();
        try {
            ConnectToReader();
            SettingManage();

            //asign tag event hadler
            reader.TagsReported += OnTagsReported;

            //escape
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();


            //stop and disconnect reader
            reader.Stop();
            reader.Disconnect();
        } catch (OctaneSdkException e) {
            Console.WriteLine("Octane SDK exeption: {0}", e.Message);
        } catch (Exception e) {
            Console.WriteLine("Exeption: {0}", e.Message);
        }
        
    }

    //called for disconnect (lost connection)
    static void OnConnectionLost(ImpinjReader reader) {
        //debug
        Console.WriteLine("Connections lost from {0} ({1})", reader.Name, reader.Address);

        //cleanup
        reader.Disconnect();

        //reconnection
        Console.WriteLine("Reconecting...");
        reader.Connect();
    }

    //function for keepalive messages
    static void OnKeepaliveReceived(ImpinjReader reader) {
        Console.WriteLine("Keep alive reaceived from {0} ({1})", reader.Name, reader.Address);
    }

    //event handler for tag reporting
    //currently baseline debugging
    //will expand later
    static void OnTagsReported(ImpinjReader sender, TagReport report) {
        //loop and print
        foreach(Tag tag in report) {
            Console.WriteLine("EPC: {0} Last Seen Time: {1}", tag.Epc, tag.LastSeenTime);
        }
    }
}
