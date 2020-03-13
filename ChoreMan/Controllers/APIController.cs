using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//APIController added
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.IO;

//namespace added
using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan.Controllers
{
    public class APIController : ApiController
    {
        private ChoreRepository ChoreRepository;
        public APIController()
        {
            try
            {
                this.ChoreRepository = new ChoreRepository();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        public APIController(ChoreRepository _ChoreRepository)
        {
            try
            {
                this.ChoreRepository = _ChoreRepository;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Chorelist CRUD

        //Create
        [HttpPost]
        [HttpOptions]
        [Route("api/createchorelist")]
        public HttpResponseMessage CreateChoreList()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }



        public HttpResponseMessage OKResponse(Object arg)
        {
            try
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, arg, Configuration.Formatters.JsonFormatter);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true
                };
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        public HttpResponseMessage ErrorResponse(Exception ex)
        {
            try
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message, Configuration.Formatters.JsonFormatter);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true
                };
                return response;
            }
            catch (Exception ex2)
            {
                throw new Exception(ex.Message + " " + ex2.Message);
            }
        }
    }
}
