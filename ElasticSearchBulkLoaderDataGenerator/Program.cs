using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchBulkLoaderDataGenerator
{
    class Program
    {
        private static System.Threading.CancellationToken cancelToken = new System.Threading.CancellationToken();
        static void Main(string[] args)
        {
            var totalThreads = 4;
            var bulkRequests = 500;

            var node = new Uri("http://myserver:9200");

            var token = cancelToken;



            for (int i = 0; i < totalThreads; i++)
            {
                Task.Run(() => {

                    while (!token.IsCancellationRequested)
                    {
                        var settings = new ConnectionSettings(node);
                        var client = new ElasticClient(settings);
                        var bulkRequest = new BulkRequest()
                        {
                            Operations = GetOperations(bulkRequests)
                        };
                        var result = client.Bulk(bulkRequest);
                        Console.WriteLine("Thread {0} publushed {1} requests with a status of {2}", i, result.IsValid, bulkRequests);
                    }
                }
                , cancelToken);

            }
             

        }

        private static IList<IBulkOperation> GetOperations(int TotalOperations)
        {
            var returnOperations = new List<IBulkOperation>(TotalOperations);
            for (int i = 0; i < TotalOperations; i++)
            {
                returnOperations.Add(new BulkCreateOperation<CreditCardTransaction>(new CreditCardTransaction()));
            }
            return returnOperations;
        }
    }
}
