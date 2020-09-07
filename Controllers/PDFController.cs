using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PDF_Extractor_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PDFController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/")]
        public List<string> Index()
        {
            string conString = _configuration.GetConnectionString("TestCon");
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM users", con);
                var reader = cmd.ExecuteReader();
                List<string> ret = new List<string>();
                while (reader.Read())
                {
                    ret.Add(reader["username"].ToString());
                }
                con.Close();
                return ret;
            }
        }
    }
}