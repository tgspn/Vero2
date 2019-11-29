using Hyperledger.Fabric.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VeroAPI;

namespace HyperledgerTest
{
    class VeroChain
    {
        private HFClient client;
        private Hyperledger.Fabric.SDK.Channels.Channel channel;
        private string chaincode = "vero";//"fabcar";

        public VeroChain(string username)
        {

            var user = SampleUser.Load(username, @"hfc-key-store/");
            client = Hyperledger.Fabric.SDK.HFClient.Create();
            var crypto = new Hyperledger.Fabric.SDK.Security.CryptoPrimitives();
            crypto.Store.AddCertificate(user.Enrollment.Cert);
            crypto.Init();
            client.CryptoSuite = crypto;

            client.UserContext = user;

            channel = client.NewChannel("mychannel");
            var peer = client.NewPeer("p1", $"grpc://{Startup.HyperleaderServer}:7051");
            channel.AddPeer(peer);

            var ordered = client.NewOrderer("o1", $"grpc://{Startup.HyperleaderServer}:7050");
            channel.AddOrderer(ordered);

            channel.Initialize();
        }

        public void SalvarInfo(string id, Dictionary<string, string> vero)
        {
            var tx_id = client.NewTransactionProposalRequest();
            tx_id.Args = new List<string>() { id, Newtonsoft.Json.JsonConvert.SerializeObject(vero) };
            tx_id.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            tx_id.Fcn = "salvarInfo";

            var result = channel.SendTransactionProposal(tx_id);
            if (result.Count > 0)
            {
                //  return Newtonsoft.Json.JsonConvert.DeserializeObject<FabCarCollections>(System.Text.Encoding.UTF8.GetString(result[0].ChaincodeActionResponsePayload));
                Console.WriteLine(result[0].TransactionID);

                var eventHub = client.NewEventHub("event1", $"grpc://{Startup.HyperleaderServer}:7053");
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
        public Dictionary<string, string> BuscarInfo(string id)
        {
            var tx_id = client.NewQueryProposalRequest();
            tx_id.Args = new List<string>() { id };
            tx_id.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            tx_id.Fcn = "BuscarInfo";
            //c3369ed2-0b93-42ef-aa93-c76fd962a448
            var response = channel.QueryByChaincode(tx_id);
            if (!response[0].IsInvalid)
            {
                var msgEnc = response[0].ChaincodeActionResponsePayload;
                var msg = System.Text.Encoding.UTF8.GetString(msgEnc);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(msg);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result[0]);
            }
            Console.WriteLine(response[0].Message);
            return null;
        }

        public void InutilizarRegistro(string key)
        {
            var t = client.NewQueryProposalRequest();
            var request = t;
            t.ChaincodeID = new Hyperledger.Fabric.SDK.ChaincodeID() { Name = chaincode };
            t.Fcn = "inutilizarRegistro";
            t.Args = new System.Collections.Generic.List<string>()
            {
                key
            };
            var response = channel.QueryByChaincode(t);
            if (!response[0].IsInvalid)
            {
                Console.WriteLine("OK");

            }
            Console.WriteLine(response[0].Message);
        }
    }
}
