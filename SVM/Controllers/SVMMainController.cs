using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using libsvm;
using Microsoft.AspNetCore.Mvc;
using svm.Models;
using Newtonsoft.Json;


namespace svm.Controllers
{
    public class SVMMainController : Controller
    {
        private static Dictionary<int, string> _predictionDictionary = new Dictionary<int, string> { { -1, "Angry" }, { 1, "Happy" } };
        List<string> vocabulary;
        TextClassProblemBuilder problemBuilder;
        svm_problem problem;
        C_SVC model;
        Random rnd = new Random();


        public SVMMainController()
        {
            const string dataFilePath = @"spamdata.csv";
            var dataTable = DataTable.New.ReadCsv(dataFilePath);
            List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();

            double[] y = dataTable.Rows.Select(row => double.Parse(row["IsSpam"])).ToArray();

            this.vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();
            this.problemBuilder = new TextClassProblemBuilder();
            this.problem = this.problemBuilder.CreateProblem(x, y, this.vocabulary.ToList());

            const double C = 1;
            this.model = new C_SVC(this.problem, KernelHelper.LinearKernel(), C);
            var accuracy = model.GetCrossValidationAccuracy(10);
            Console.Clear();
            Console.WriteLine(new string('=', 50));

            Console.WriteLine("Accuracy of the model is {0:P}", accuracy);
            model.Export(string.Format(@"model_{0}_accuracy.model", accuracy));

            Console.WriteLine(new string('=', 50));
        }

        [HttpGet]
        public double Predict(string jsonString)
        {
            //var list = JsonConvert.DeserializeObject(jsonString);
            //TODO json string omzetten naar en lijst en meegeven aan string[] userInput


            double inc = 0;
            string[] userInput = { };

            for (int i = 0; i < userInput.Length; i++)
            {
                var newX = TextClassProblemBuilder.CreateNode(userInput[i], vocabulary);

                var predictedY = model.Predict(newX);

                inc = (inc + predictedY);
                Console.WriteLine("The user input is {0}", userInput[i]);
                Console.WriteLine("The prediction is {0}  value is {1} ", _predictionDictionary[(int)predictedY], predictedY);
                Console.WriteLine(new string('=', 50));

            }
            Console.WriteLine("End result = {0}", (inc / userInput.Length));
            Console.WriteLine("");
            return (inc / userInput.Length);
        }

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
