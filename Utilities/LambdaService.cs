using Amazon.Lambda;
using Amazon.Lambda.Model;
using BookManagement.Models;
using Newtonsoft.Json;

namespace BookManagement.Utilities
{
    public class LambdaService
    {
        private readonly IAmazonLambda _lambdaClient;

        public LambdaService()
        {
            _lambdaClient = new AmazonLambdaClient();
        }

        public async Task InvokeLambdaFunctionAsync(Book book)
        {
            var request = new InvokeRequest
            {
                FunctionName = "BookNotificationLambda",
                Payload = JsonConvert.SerializeObject(book),
                InvocationType = InvocationType.Event
            };

            try
            {
                var response = await _lambdaClient.InvokeAsync(request);
                if (response.StatusCode != 200)
                {
                    Console.WriteLine($"Error invoking Lambda function: {response.FunctionError}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error invoking Lambda function: {ex.Message}");
            }

        }
    }

}
