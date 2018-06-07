using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using libsvm;
using Microsoft.AspNetCore.Mvc;
using svm.Models;


namespace svm.Controllers
{
    public class SVMMainController : Controller
    {
        private static Dictionary<int, string> _predictionDictionary;


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public void MainSVM()
        {
            Random rnd = new Random();


            const string dataFilePath = @"spamdata.csv";
            var dataTable = DataTable.New.ReadCsv(dataFilePath);
            List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();

            double[] y = dataTable.Rows.Select(row => double.Parse(row["IsSpam"])).ToArray();

            var vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();
            var problemBuilder = new TextClassProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y, vocabulary.ToList());

            const double C = 1; 

            var model = new C_SVC(problem, KernelHelper.LinearKernel(), C);
            var accuracy = model.GetCrossValidationAccuracy(10);
            Console.Clear();
            Console.WriteLine(new string('=', 50));

            Console.WriteLine("Accuracy of the model is {0:P}", accuracy);
            model.Export(string.Format(@"model_{0}_accuracy.model", accuracy));

            Console.WriteLine(new string('=', 50));

            //This just takes the predicted value (-1 to 1) and translates to your categorization response

            _predictionDictionary = new Dictionary<int, string> { { -1, "Angry" }, { 1, "Happy" } };
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
        }

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
