using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hyperledger.Fabric.SDK;
using Grpc;
using Grpc.Core;
using Hyperledger.Fabric.Protos.Msp.MspConfig;
using Hyperledger.Fabric.SDK.AMCL;
using Hyperledger.Fabric.SDK.AMCL.FP256BN;
using Hyperledger.Fabric.SDK.Idemix;
using Hyperledger.Fabric.SDK.Security;
using Newtonsoft.Json;
using Hyperledger.Fabric.SDK.Channels;

namespace HyperledgerTest
{
    public class FabCar
    {
        private HFClient client;
        private Hyperledger.Fabric.SDK.Channels.Channel channel;
        private string chaincode = "fabcar";
        public FabCar(string username)
        {

            var user = SampleUser.Load(username, @"D:\Projetos\Hyperledger\fabcar\hfc-key-store\");
            client = Hyperledger.Fabric.SDK.HFClient.Create();
            var crypto = new Hyperledger.Fabric.SDK.Security.CryptoPrimitives();            
            crypto.Store.AddCertificate(user.Enrollment.Cert);
            crypto.Init();
            client.CryptoSuite = crypto;
                       
            client.UserContext = user;
            
            channel = client.NewChannel("mychannel");
            var peer = client.NewPeer("p1", $"grpc://{Program.ServerIP}:7051");
            channel.AddPeer(peer);

            var ordered = client.NewOrderer("o1", $"grpc://{Program.ServerIP}:7050");
            channel.AddOrderer(ordered);

            channel.Initialize();

        }
        public FabCarCollections QueryAllCars()
        {
            var t = client.NewQueryProposalRequest();
            var request = t;
            t.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            t.Fcn = "queryAllCars";
            t.Args = new System.Collections.Generic.List<string>();
            var response = channel.QueryByChaincode(t);
            if (response.Count > 0)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<FabCarCollections>(System.Text.Encoding.UTF8.GetString(response[0].ChaincodeActionResponsePayload));

            return null;

        }
        public void CreateCar(FabCarItem carItem)
        {
            // createCar chaincode function - requires 5 args, ex: args: ['CAR12', 'Honda', 'Accord', 'Black', 'Tom'],

            var tx_id = client.NewTransactionProposalRequest();
            tx_id.Args = new List<string>()
            {
                carItem.Key,
                carItem.Record.make,
                carItem.Record.model,
                carItem.Record.color,
                carItem.Record.owner,

            };
            tx_id.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            tx_id.Fcn = "createCar";

            var result = channel.SendTransactionProposal(tx_id);
            if (result.Count > 0)
            {
                //  return Newtonsoft.Json.JsonConvert.DeserializeObject<FabCarCollections>(System.Text.Encoding.UTF8.GetString(result[0].ChaincodeActionResponsePayload));
                Console.WriteLine(result[0].TransactionID);

                var eventHub = client.NewEventHub("event1", $"grpc://{Program.ServerIP}:7053");
                channel.AddEventHub(eventHub);

                eventHub.ConnectAsync(result[0].TransactionContext).Wait();
                var result2 = channel.SendTransaction(result);
                while (true)
                {
                    Console.Write($"\r{eventHub.Status}");
                    break;
                }


            }

            //return null;
        }
        public void CreateCar2(FabCarItem carItem)
        {
            // createCar chaincode function - requires 5 args, ex: args: ['CAR12', 'Honda', 'Accord', 'Black', 'Tom'],

            var tx_id = client.NewTransactionProposalRequest();
            tx_id.Args = new List<string>()
            {
                carItem.Key,
                carItem.Record.make,
                carItem.Record.model,
                carItem.Record.color,
                carItem.Record.owner,

            };
            tx_id.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            tx_id.Fcn = "salvarInfo";

            var result = channel.SendTransactionProposal(tx_id);
            if (result.Count > 0)
            {
                //  return Newtonsoft.Json.JsonConvert.DeserializeObject<FabCarCollections>(System.Text.Encoding.UTF8.GetString(result[0].ChaincodeActionResponsePayload));
                Console.WriteLine(result[0].TransactionID);

                var eventHub = client.NewEventHub("event1", $"grpc://{Program.ServerIP}:7053");
                channel.AddEventHub(eventHub);

                eventHub.ConnectAsync(result[0].TransactionContext).Wait();
                var result2 = channel.SendTransaction(result);
                while (true)
                {
                    Console.Write($"\r{eventHub.Status}");
                    break;
                }


            }

            //return null;
        }
        public FabCarItem QueryCar(string key)
        {
            var t = client.NewQueryProposalRequest();
            var request = t;
            t.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            t.Fcn = "queryCar";
            t.Args = new System.Collections.Generic.List<string>()
            {
                key
            };
            var response = channel.QueryByChaincode(t);
            if (!response[0].IsInvalid)
            {
                var item = new FabCarItem()
                {
                    Key = key,
                    Record = Newtonsoft.Json.JsonConvert.DeserializeObject<Record>(System.Text.Encoding.UTF8.GetString(response[0].ChaincodeActionResponsePayload))
                };
                return item;
            }
            Console.WriteLine(response[0].Message);
            return null;

        }
        public FabCarItem ChangeCarOwner(string key,string newOwerner)
        {
            // changeCarOwner chaincode function - requires 2 args , ex: args: ['CAR10', 'Dave'],
            var t = client.NewQueryProposalRequest();
            var request = t;
            t.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            t.Fcn = "changeCarOwner";
            t.Args = new System.Collections.Generic.List<string>()
            {
                key,
                newOwerner
            };
            var response = channel.QueryByChaincode(t);
            if (!response[0].IsInvalid)
            {
                var item = new FabCarItem()
                {
                    Key = key,
                    Record = Newtonsoft.Json.JsonConvert.DeserializeObject<Record>(System.Text.Encoding.UTF8.GetString(response[0].ChaincodeActionResponsePayload))
                };
                return item;
            }
            Console.WriteLine(response[0].Message);
            return null;
        }
    }


    public class FabCarCollections : List<FabCarItem>
    {
    }

    public class FabCarItem
    {
        public string Key { get; set; }
        public Record Record { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==================================");
            sb.Append("Key: ");
            sb.AppendLine(Key);

            sb.AppendLine("----------------------------------");
            sb.AppendLine(Record?.ToString());
            sb.AppendLine("----------------------------------");
            sb.AppendLine("==================================");
            return sb.ToString();
        }
    }

    public class Record
    {
        public string color { get; set; }
        public string docType { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string owner { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\t");
            sb.Append("Color: ");
            sb.AppendLine(color);

            sb.Append("\t");
            sb.Append("DocType: ");
            sb.AppendLine(docType);

            sb.Append("\t");
            sb.Append("Make: ");
            sb.AppendLine(make);

            sb.Append("\t");
            sb.Append("Model: ");
            sb.AppendLine(model);

            sb.Append("\t");
            sb.Append("Owner: ");
            sb.AppendLine(owner);

            return sb.ToString();
        }

    }

}

