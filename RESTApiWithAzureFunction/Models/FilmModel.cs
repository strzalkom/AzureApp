using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTApiWithAzureFunction.Models
{
    public class FilmModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Vote_average { get; set; }

        public string Poster_path { get; set; }
    }
    public class CreateFilmModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Vote_average { get; set; }
        public string Poster_path { get; set; }
    }
    public class UpdateFilmModel
    {
        public string Overview { get; set; }
     
    }
}
