using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinApp
{
    class Program
    {
        private static readonly ICoinService CoinService = new BitcoinService(useTestnet: true);
        static void Main(string[] args)
        {
            var running = true;
            Help();
            while (running)
            {
                Console.WriteLine();
                Console.Write(">");
                var command = Console.ReadLine();
                Console.WriteLine();
                var commandSplit = command.Split(' ');
                switch (commandSplit.First())
                {
                    case "checkBalance":
                        GetBalance();
                        break;
                    case "createAddress":
                        CreateAddress();
                        break;
                    case "sendCoins":
                        if (commandSplit[1] == null || commandSplit[2] == null)
                        {
                            Console.WriteLine("Unknown parameters, try again");
                            break;
                        }
                        try
                        {
                            var amount = Decimal.Parse(commandSplit[2]);
                            SendCoinsTo(commandSplit[1], amount);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unknown amount, try again");
                        }
                        break;
                    case "listUnspent":
                        GetUnspendTransactions();
                        break;
                    case "help":
                        Help();
                        break;
                    case "quit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command, try again");
                        break;
                }
            }
        }

        static void Help()
        {
            Console.WriteLine("You have following options:");
            Console.WriteLine("checkBalance - Check your Balance");
            Console.WriteLine("createAddress - Create a new address");
            Console.WriteLine("sendCoins [address] [amount] - Send coins");
            Console.WriteLine("listUnspent - List unspent transactions");
            Console.WriteLine("help - This help menu");
            Console.WriteLine("quit - Exit application");
        }

        static void GetBalance()
        {
            var balance = CoinService.GetBalance();
            Console.WriteLine("My balance: " + balance);
        }

        static void CreateAddress()
        {
            var newAddress = CoinService.GetNewAddress();
            Console.WriteLine("New Address created: " + newAddress);
        }

        static void SendCoinsTo(string address, decimal amount)
        {
            CoinService.SendToAddress(address, amount);
            Console.WriteLine(amount + " has been sent to: " + address);
        }

        static void GetUnspendTransactions()
        {
            var unspendTransactions = CoinService.ListUnspent();
            foreach (var transaction in unspendTransactions)
            {
                Console.WriteLine("TransactionID: " + transaction.TxId);
                Console.WriteLine("Address: " + transaction.Address);
                Console.WriteLine("Amount: " + transaction.Amount);
                Console.WriteLine("Confirmations: " + transaction.Confirmations);
                Console.WriteLine("Spendable: " + transaction.Spendable);
                Console.WriteLine("-------------------");
            }
        }
    }
}
