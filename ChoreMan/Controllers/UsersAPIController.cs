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

//external added
using Nelibur.ObjectMapper;

namespace ChoreMan.Controllers
{
    public class UsersAPIController : ApiController
    {
        private UserRepository UserRepository;
        private ChoreRepository ChoreRepository;

        public UsersAPIController()
        {
            try
            {
                this.UserRepository = new UserRepository();
                this.ChoreRepository = new ChoreRepository();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        public UsersAPIController(UserRepository _UserRepository, ChoreRepository _ChoreRepository)
        {
            try
            {
                this.UserRepository = _UserRepository;
                this.ChoreRepository = _ChoreRepository;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Test
        [HttpGet]
        [Route("test")]
        public HttpResponseMessage Test()
        {
            try
            {
                if (HttpContext.Current.Session == null)
                {
                    _User User = new _User();
                    User.Username = "rcaigoy31";
                    HttpContext.Current.Session["User"] = User;
                }
                    
                return OKResponse("");
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Create User
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("usersapi/createuser")]
        public HttpResponseMessage CreateUser(string UserValues)
        {
            try
            {
                User UserObject = JsonConvert.DeserializeObject<User>(UserValues);
                var User = new _User(UserRepository.CreateUser(UserObject));
                HttpContext.Current.Session["User"] = User;

                return OKResponse(User);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read User
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("usersapi/getuser")]
        public HttpResponseMessage GetUser(string Username, string Password)
        {
            try
            {
                return OKResponse(new _User(UserRepository.Login(Username, Password)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Update User Info
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("usersapi/updateuserinfo")]
        public HttpResponseMessage UpdateUserInfo(int Id, string UserValues)
        {
            try
            {
                var UserObject = JsonConvert.DeserializeObject<User>(UserValues);
                return OKResponse(new _User(UserRepository.UpdateUserInfo(Id, UserObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Update User Password
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("usersapi/updateuserpassword")]
        public HttpResponseMessage UpdateUserPassword(string Username, string OldPassword, string NewPassword)
        {
            try
            {
                return OKResponse(new _User(UserRepository.ChangePassword(Username, OldPassword, NewPassword)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Delete
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("usersapi/deleteuser")]
        public HttpResponseMessage DeleteUser(int Id)
        {
            try
            {
                return OKResponse(new _User(UserRepository.DeleteUser(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //refactored api responses
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
