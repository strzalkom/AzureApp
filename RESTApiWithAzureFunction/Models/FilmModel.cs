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
        public string Description { get; set; }
        public int Rating { get; set; }

        public string PhotoUrl { get; set; }
    }
    public class CreateFilmModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class UpdateFilmModel
    {
        public string Description { get; set; }
     
    }
}
