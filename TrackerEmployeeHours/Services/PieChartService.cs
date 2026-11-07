using ScottPlot;
using TrackerEmployeeHours.Models;

namespace TrackerEmployeeHours.Services
{
    public class PieChartService
    {
        public byte[] GenerateChart(List<EmployeeHoursSum> employees)
        {
            var validEmployees = employees
                .Where(e => e.TotalHours > 0)
                .ToList();

            double totalHours = validEmployees.Sum(e => e.TotalHours);

            if (totalHours == 0)
                throw new InvalidOperationException("No working hours.");

            double[] values = validEmployees
                .Select(e => e.TotalHours)
                .ToArray();

            ScottPlot.Plot plot = new();

            var pie = plot.Add.Pie(values);
            pie.SliceLabelDistance = 0.5;

            for (int i = 0; i < validEmployees.Count; i++)
            {
                double percentage = values[i] / totalHours * 100;

                pie.Slices[i].Label = $"{percentage:F1}%";
                pie.Slices[i].LegendText = validEmployees[i].EmployeeName;
            }

            plot.ShowLegend();
            plot.Axes.Frameless();
            plot.HideGrid();

            return plot.GetImageBytes(900, 600, ScottPlot.ImageFormat.Png);
        }
    }
}

