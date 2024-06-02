using EmployeeAttrition.ML.Base;
using EmployeeAttrition.ML.Objects;
using Microsoft.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttrition.ML
{
    public class Predictor : BaseML//provides prediction support in the project
    {
        public void Predict(string inputDataFile)
        {
            MLContext mLContext = new MLContext();
            if (!File.Exists(ModelPath))//Verifying if the model exists prior to reading it
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");
                return;
            }
            if (!File.Exists(inputDataFile))// //Verifying if the input file exists before making predictions on it 
            {
                Console.WriteLine($"Failed to find input data at {inputDataFile}");
                return;


            }

            /*Loading the model  */
            //Then we define the ITransformer Object
            ITransformer mlModel;
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = mLContext.Model.Load(stream, out _);//Stream is disposed as a result of Using statement
            }
            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");
                return;
            }

            // Create a prediction engine
            var predictionEngine = mLContext.Model.CreatePredictionEngine<EmploymentHistory, EmploymentHistoryPrediction>(mlModel);
          

            //Read in the file as Text and deserialize the JSON into our Employment History Object
            var json = File.ReadAllText(inputDataFile);

            //Call predict model on prediction engine class

            var prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<EmploymentHistory>(json));

            Console.WriteLine(
                $"Based on input json:{System.Environment.NewLine}" +
                $"{json}{System.Environment.NewLine}" +
                $"The Employee is predicted to work {prediction.DurationInMonths: #.##}months");
                
             
        }
    }
}
