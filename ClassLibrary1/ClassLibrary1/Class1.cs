using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeoLibrary
{
    public class Class1
    {
        public class GeoJSONData
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("geometry")]
            public GeometryData Geometry { get; set; }
        }

        public class GeometryData
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("coordinates")]
            public object Coordinates { get; set; }
        }

        public static GeoJSONData LoadGeoJSON(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"GeoJSON file not found at {filePath}");
            }

            string json = File.ReadAllText(filePath);
            var geojsonData = JsonConvert.DeserializeObject<GeoJSONData>(json);
            return geojsonData;
        }

        public static void SaveGeoJSON(string filePath, GeoJSONData data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static string GetGeometryType(string geoJsonData)
        {
            JObject json = JObject.Parse(geoJsonData);
            return json["geometry"]["type"].ToString();
        }

        public static object GetCoordinates(GeoJSONData data)
        {
            return data.Geometry.Coordinates;
        }

        public static double CalculateArea(GeoJSONData data)
        {
            if (data.Geometry.Type != "Polygon")
            {
                throw new ArgumentException("Data does not represent a Polygon type");
            }

            var coordinates = ((JArray)data.Geometry.Coordinates).ToObject<List<List<double>>>();

            double area = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                int j = (i + 1) % coordinates.Count;
                area += coordinates[i][0] * coordinates[j][1];
                area -= coordinates[i][1] * coordinates[j][0];
            }

            return Math.Abs(area) / 2;
        }

        public static double CalculateDistance(GeoJSONData data1, GeoJSONData data2)
        {
            if (data1.Geometry.Type != "Point" || data2.Geometry.Type != "Point")
            {
                throw new ArgumentException("Data does not represent Point type");
            }

            var coordinates1 = ((JArray)data1.Geometry.Coordinates).ToObject<List<double>>();
            var coordinates2 = ((JArray)data2.Geometry.Coordinates).ToObject<List<double>>();

            double x1 = coordinates1[0];
            double y1 = coordinates1[1];
            double x2 = coordinates2[0];
            double y2 = coordinates2[1];

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}