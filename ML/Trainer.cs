using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using EmployeeAttrition.ML.Base;
using Microsoft.ML.Data;
using EmployeeAttrition.ML.Objects;
using Microsoft.ML.Transforms.Text;
using Microsoft.ML.Trainers;
using Microsoft.ML.Calibrators;
using EmployeeAttrition.Common;

namespace EmployeeAttrition.ML
{
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))//Check if training data exists
            {
                Console.WriteLine($"Failed to find training data file({trainingFileName})");
                return;
            }
            //MLContext mLContext = new MLContext();//Since object reference is required for to access Data Field

            IDataView trainingDataView = mLContext.Data.LoadFromTextFile<EmploymentHistory>(trainingFileName, ',');// Use of comma to separate the data as opposed to default tab like
            //Loads Text file into an IDataViewObject
            DataOperationsCatalog.TrainTestData dataSplit = mLContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.4);
            //Creates Test Set from main Training Data
            //The parameter testFraction specifies the percentage of the dataset to hold back for testing in our case by 20%

            /*Creating pipeline*/

            var dataProcessPipeline = mLContext.Transforms.CopyColumns("Label", nameof(EmploymentHistory.DurationInMonths))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.IsMarried)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.BSDegree)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.MSDegree)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.YearsExperience))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.AgeAtHire)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.HasKids)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.WithinMonthOfVesting)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.DeskDecorations)))
                .Append(mLContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.LongCommute)))
                .Append(mLContext.Transforms.Concatenate("Features",
                    typeof(EmploymentHistory).ToPropertyList<EmploymentHistory>(nameof(EmploymentHistory.DurationInMonths)))));


            //Create SDCA Trainer using the default paramters ("Label" and "Features")
            var trainer = mLContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");


            //Complete our pipeline by appending the trainer we instantiated

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            //Train the model with the data set created Earlier
            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);


            //Save created model to the filename specified matching training set's schema
            mLContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);//Model path is supplied from BaseML

            // Transform Model with the test set created Earlier
            IDataView testSetTransform = trainedModel.Transform(dataSplit.TestSet);


            //Call the Regression.Evaluate method to provide regression specific metrics 
            var modelMetrics = mLContext.Regression.Evaluate(testSetTransform);

            //Log the metrics to Console Output

            Console.Write($"Loss function:{modelMetrics.LossFunction:0.##}{Environment.NewLine}" +
                $"Mean Absolute Error: {modelMetrics.MeanAbsoluteError:#.##}{Environment.NewLine}" +
                $"Mean Squared Error:{modelMetrics.MeanSquaredError:#.##}{Environment.NewLine}" +
                $"RSquared:{modelMetrics.RSquared:0.##}{Environment.NewLine}" +
                $"Root Mean Squared Error:{modelMetrics.RootMeanSquaredError:#.##}");
















        }
    }
}
