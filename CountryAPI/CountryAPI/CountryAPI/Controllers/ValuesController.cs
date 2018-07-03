using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CountryAPI.Context;
using System.Collections;
using System.Net;
using System.Web;
using CountryAPI.Models;
namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        /*
        [HttpGet("/api/login")]
        public Dictionary<string, Dictionary<string, string>> login([FromHeader] string user, [FromHeader] string password)
        {
            return CountryDatabase.Login(user, password);
        }
        */
        [HttpGet]
        [Route("/api/values/search")]
        public IActionResult GetSearch([FromQuery] string search) {
            if (search == null)
            {
                search = "";
            }
            CountryDatabase.connect();
            return Ok(CountryDatabase.getCountryCodeByName(search));
            
        }

        // GET api/values
        [HttpGet]
        public Object Get([FromHeader] string Authorization)
        {

            //CountryDatabase.connect();
            //return Ok(CountryDatabase.getAllCountryCode());

            
            string response = OAuth2Server.ValidateAccessToken(Authorization);
            if (response.Equals("Valid"))
            {
                CountryDatabase.connect();
                return Ok(CountryDatabase.getAllCountryCode());//new string[] { "value1", "value2" };}
            }
            else if (response.Equals("Missing access_token"))
            {
                errorModel err = new errorModel();
                err.error = new error();
                err.error.status = 401;
                err.error.message = "Missing access_token";
                return Ok(err);
            }
            else if (response.Equals("Expired access_token"))
            {
                HttpContext.Response.StatusCode = 401;
                errorModel err = new errorModel();
                err.error = new error();
                err.error.message = "Expired access_token";
                err.error.status = 401;
                return err;
            }
            else
            {
                return Unauthorized();
            }
        }

        //api/getHighest/?num=
        [HttpGet("/api/getHighest/")]
        //[Route("/api/getHighest/{id}")]//route for different url
        public ArrayList GetHighest([FromQuery] int num) //get num from ?num=X where x is num
        {
            return CountryDatabase.getHighest(num);
        }


        //get all the cities in given country
        ///api/getCities/nameOFCountry
        [HttpGet("/api/getCities/{name}")]
        public ArrayList getCities(string name)
        {
            
            return CountryDatabase.getCities(name);
        }

        //what country is x city in
        [HttpGet("/api/getCountry/{city}")]
        public ArrayList findCountry(string city)
        {
            return CountryDatabase.getAllCountryCode(city);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
