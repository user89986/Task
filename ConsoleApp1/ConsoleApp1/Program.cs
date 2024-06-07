using System;
using GeoLibrary;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
               
                string filePath = @"file"; 

               
                Class1.GeoJSONData geoJsonData = Class1.LoadGeoJSON(filePath);
                Console.WriteLine("Загружены данные GeoJSON:");
                PrintGeoJSONData(geoJsonData);

                // Вычисление и вывод площади, если тип геометрии - Polygon
                if (geoJsonData.Geometry.Type == "Polygon")
                {
                    double area = Class1.CalculateArea(geoJsonData);
                    Console.WriteLine($"Площадь полигона: {area}");
                }

                // Если тип геометрии - Point, демонстрация вычисления расстояния до той же точки
                if (geoJsonData.Geometry.Type == "Point")
                {
                    double distance = Class1.CalculateDistance(geoJsonData, geoJsonData);
                    Console.WriteLine($"Расстояние до той же точки: {distance} (должно быть 0)");
                }

             
                string saveFilePath = @"file"; 
                Class1.SaveGeoJSON(saveFilePath, geoJsonData);
                Console.WriteLine($"Данные GeoJSON сохранены в {saveFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        static void PrintGeoJSONData(Class1.GeoJSONData data)
        {
            Console.WriteLine($"Тип: {data.Type}");
            Console.WriteLine($"Тип геометрии: {data.Geometry.Type}");
            Console.WriteLine("Координаты:");
            PrintCoordinates(data.Geometry.Coordinates);
        }

        static void PrintCoordinates(object coordinates)
        {
            string jsonCoordinates = JsonConvert.SerializeObject(coordinates, Formatting.Indented);
            Console.WriteLine(jsonCoordinates);
            Console.ReadLine();
        }
    }
}