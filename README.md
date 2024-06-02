The trainer class performs the Evaluation on the model. Use the sampledata.csv to train the model and output the evaluation metrics.
Run using commandline arguments:  Train "D:\EmployeeAttrition\Data\sampledata.csv"
For this project, The used model metrics are:
1) Loss function
2) Mean Absolute Error
3) Mean Squared Error
4) RSquared
5) Root Mean Squared Error


After training the model, build a sample JSON file and save it as input.json
{
	"durationInMonths": 0.0,
	"isMarried": 0,
	"bsDegree": 1,
	"msDegree": 1,
	"yearsExperience": 2,
	"ageAtHire": 29,
	"hasKids": 0,
	"withinMonthOfVesting": 0,
	"deskDecorations": 1,
	"longCommute": 1


}
Run the prediction using commandline arguments on the model to find the duration the employee is gonna work for the company in months.
Predict "D:\EmployeeAttrition\Data\input.json"

---------------------------------------------------------------EXAMPLE OUTPUT---------------------------------------------------------------------


train "D:\EmployeeAttrition\Data\sampledata.csv"

Loss function:331.78
Mean Absolute Error: 14.91
Mean Squared Error:331.78
RSquared:0.08
Root Mean Squared Error:18.21


predict "D:\Machine Learning Projects\EmployeeAttrition\input.json"

The Employee is predicted to work  40.33months




