using System;
using PaymentSystem.Client.Business;

namespace PaymentSystem.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine cli = new CommandLine();
            cli.Run();
            
        }
    }
}
