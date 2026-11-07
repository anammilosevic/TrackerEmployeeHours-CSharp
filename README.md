
# Employee Working Hours Tracker

This is a simple ASP.NET Core MVC application that shows employee working hours using data from an API.

## Technologies Used:
- C#
- ASP.NET Core MVC
- ScottPlot
- Bootstrap

## Features:
- Gets employee data from an API
- Calculates total working hours for each employee
- Skips deleted and invalid entries
- Sorts employees by total hours
- Highlights employees with less than 100 hours
- Displays a pie chart and allows downloading it as PNG

## How to Run:
1. Open the solution in Visual Studio.
2. Right-click the project and select "Manage User Secrets".
3. Add the API key:

   ```json
   {
     "ApiSettings": {
       "Key": "YOUR_API_KEY"
     }
   }
   ```

4. Run the application!
