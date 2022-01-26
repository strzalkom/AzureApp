using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RESTApiWithAzureFunction.Models;

namespace RESTApiWithAzureFunction
{
    public static class FilmListFunction
    {
        [FunctionName("CreateFilm")]
        public static async Task<IActionResult> CreateFilm(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "film")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateFilmModel>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
                {
                    connection.Open();
                    if (!String.IsNullOrEmpty(input.Description))
                    {
                        //var query = $"INSERT INTO FilmList (Description) VALUES('{input.Description}')";
                        var query = $"INSERT INTO [FilmList] (Title,Description,Rating,PhotoUrl) VALUES('{input.Title}' ,'{input.Description}', '{input.Rating}' '{input.PhotoUrl}')";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [FunctionName("GetFilms")]
        public static async Task<IActionResult> GetFilms(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "film")] HttpRequest req, ILogger log)
        {
            List<FilmModel> filmList = new List<FilmModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
                {
                    connection.Open();
                    var query = @"Select * from FilmList";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        FilmModel film = new FilmModel()
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Rating = (int)reader["Rating"],
                            PhotoUrl = reader["PhotoUrl"].ToString()
                           
                        };
                        filmList.Add(film);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (filmList.Count > 0)
            {
                return new OkObjectResult(filmList);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("GetFilmById")]
        public static IActionResult GetFilmById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "film/{id}")] HttpRequest req, ILogger log, int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
                {
                    connection.Open();
                    var query = @"Select * from FilmList Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (dt.Rows.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(dt);
        }

        [FunctionName("DeleteFilm")]
        public static IActionResult DeleteFilm(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "film/{id}")] HttpRequest req, ILogger log, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
                {
                    connection.Open();
                    var query = @"Delete from FilmList Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [FunctionName("UpdateFilm")]
        public static async Task<IActionResult> UpdateFilm(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "film/{id}")] HttpRequest req, ILogger log, int id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<UpdateFilmModel>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
                {
                    connection.Open();
                    var query = @"Update FilmList Set Description = @Description Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Description", input.Description);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
    }
}