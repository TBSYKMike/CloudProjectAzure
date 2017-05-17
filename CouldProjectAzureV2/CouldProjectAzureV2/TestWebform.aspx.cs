using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace CouldProjectAzureV2
{
    public partial class TestWebform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             AzureTableConnector azureTableConnector = new AzureTableConnector();
             List<Entity> sensorData = azureTableConnector.RetriveDataFromSensors("people");

            string xSerie = "x";
            string ySerie = "y";
            string zSerie = "z";

            addAcceleroMeterDataToGraph(xSerie);
            addAcceleroMeterDataToGraph(ySerie);
            addAcceleroMeterDataToGraph(zSerie);

            DataChart.Series[0].Color = System.Drawing.Color.Green;
            DataChart.Series[1].Color = System.Drawing.Color.Black;
            DataChart.Series[2].Color = System.Drawing.Color.CornflowerBlue;

            for (int i = 0; i < sensorData.Count(); i++)
            {
                if (sensorData[i].SensorAccelerometerX != null && sensorData[i].SensorAccelerometerY != null && sensorData[i].SensorAccelerometerZ != null)
                {
                    DataChart.Series[xSerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerX, CultureInfo.InvariantCulture));
                    DataChart.Series[ySerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerY, CultureInfo.InvariantCulture));
                    DataChart.Series[zSerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerZ, CultureInfo.InvariantCulture));
                }
            }
        }
       

        private void addAcceleroMeterDataToGraph(string serie)
        {
            DataChart.Series.Add(serie);
            DataChart.Series[serie].ChartType = SeriesChartType.FastLine;
            // DataChart.Series[xSeries].BorderWidth = 2;      
        }
    }
}