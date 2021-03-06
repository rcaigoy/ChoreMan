﻿using System;
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
    public class APIController : ApiController
    {
        private UserRepository UserRepository;
        private ChoreRepository ChoreRepository;
        public APIController()
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

        public APIController(UserRepository _UserRepository, ChoreRepository _ChoreRepository)
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


        #region CHORELIST
        //Chorelist CRUD

        //Create
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/createchorelist")]
        public HttpResponseMessage CreateChoreList(string AuthToken, string ChoreListValues)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));

                ChoreList ChoreListObject = JsonConvert.DeserializeObject<ChoreList>(ChoreListValues);

                //check if User Matches Id
                if (User.Id != ChoreListObject.UserId)
                    throw new Exception("Unathorized");

                return OKResponse(new _ChoreList(ChoreRepository.CreateChoreList(ChoreListObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //read single
        [HttpGet]
        [HttpOptions]
        [Route("api/getchorelist")]
        public HttpResponseMessage GetChoreList(int Id)
        {
            try
            {
                return OKResponse(new _ChoreList(ChoreRepository.GetChoreList(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Update
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/updatechorelist")]
        public HttpResponseMessage UpdateChoreList(string AuthToken, int ChoreListId, string Name, int StatusId)
        {
            try
            {
                ChoreList ChoreListObject = new ChoreList
                {
                    Id = ChoreListId,
                    Name = Name,
                    StatusId = StatusId
                };

                var User = UserRepository.RefreshAuthToken(AuthToken);

                //check if User Matches Id
                if (User.Id != ChoreRepository.GetChoreList(ChoreListId).UserId)
                    throw new Exception("Unathorized");

                return OKResponse(new _ChoreList(ChoreRepository.UpdateChoreList(ChoreListId, ChoreListObject)));
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
        [Route("api/deletechorelist")]
        public HttpResponseMessage DeleteChoreList(string AuthToken, int Id)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                if (!ChoreRepository.CanEditChoreList(User.Id, Id))
                    throw new Exception("Not Authorized");

                return OKResponse(new _ChoreList(ChoreRepository.DeleteChoreList(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        #endregion CHORELIST

        #region CHOREUSER


        //Create
        [HttpPost]
        [HttpOptions]
        [Route("api/addchoreuser")]
        public HttpResponseMessage AddChoreUser(string AuthToken, string ChoreUserValues)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));
                ChoreUser ChoreUserObject = JsonConvert.DeserializeObject<ChoreUser>(ChoreUserValues);

                //get chorelist from chore user
                var ChoreList = ChoreRepository.GetChoreList(ChoreUserObject.ChoreListId);

                //check if userid matches chore list object user
                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unathorized");

                //set Chore User as Active
                ChoreUserObject.IsActive = true;

                return OKResponse(new _ChoreUser(ChoreRepository.AddChoreUser(ChoreUserObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read single
        [HttpGet]
        [HttpOptions]
        [Route("api/getchoreuser")]
        public HttpResponseMessage GetChoreUser(string AuthToken, int Id)
        {
            try
            {
                //get user from auth token
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));

                //get chore user object from id
                var ChoreUserObject = new _ChoreUser(ChoreRepository.GetChoreUser(Id));

                //get chorelist from chore user object
                var ChoreList = ChoreRepository.GetChoreList(ChoreUserObject.ChoreListId);

                //check if userid matches chore list object user
                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unathorized");

                return OKResponse(ChoreUserObject);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read list
        [HttpGet]
        [HttpOptions]
        [Route("api/getchoreusers")]
        public HttpResponseMessage GetChoreUsers(int ChoreListId)
        {
            try
            {
                TinyMapper.Bind<List<ChoreUser>, List<_ChoreUser>>();
                var ChoreUsers = TinyMapper.Map<List<_ChoreUser>>(ChoreRepository.GetChoreUsers(ChoreListId));
                return OKResponse(ChoreUsers);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        [HttpPost]
        [HttpOptions]
        [Route("api/updatechoreuser")]
        public HttpResponseMessage UpdateChoreUser(string AuthToken, int Id, string ChoreUserValues)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));
                ChoreUser ChoreUserObject = JsonConvert.DeserializeObject<ChoreUser>(ChoreUserValues);

                //get chorelist from chore user
                var ChoreList = ChoreRepository.GetChoreList(ChoreUserObject.ChoreListId);

                //check if userid matches chore list object user
                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unathorized");

                //keep chore user as active
                ChoreUserObject.IsActive = true;

                return OKResponse(new _ChoreUser(ChoreRepository.UpdateChoreUser(Id, ChoreUserObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        [HttpPost]
        [HttpOptions]
        [Route("api/deletechoreuser")]
        public HttpResponseMessage DeleteChoreUser(string AuthToken, int Id)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                //get ChoreUser
                var ChoreUser = ChoreRepository.GetChoreUser(Id);

                //check if userid matches chore list object user
                if (!ChoreRepository.CanEditChoreList(User.Id, ChoreUser.ChoreListId))
                    throw new Exception("Unathorized");

                return OKResponse(new _ChoreUser(ChoreRepository.DeleteChoreUser(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        #endregion CHOREUSER


        //chores
        #region CHORES

        //Create
        [HttpPost]
        [HttpOptions]
        [Route("api/addchore")]
        public HttpResponseMessage AddChore(string AuthToken, string ChoreValues)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));
                Chore ChoreObject = JsonConvert.DeserializeObject<Chore>(ChoreValues);

                var ChoreList = new _ChoreList(ChoreRepository.GetChoreList((int)ChoreObject.ChoreListId));

                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unauthorized");

                ChoreObject.IsActive = true;

                return OKResponse(new _Chore(ChoreRepository.AddChore(ChoreObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read single
        [HttpGet]
        [HttpOptions]
        [Route("api/getchore")]
        public HttpResponseMessage GetChore(string AuthToken, int Id)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));

                //get chore from Id
                var ChoreObject = new _Chore(ChoreRepository.GetChore(Id));

                //get ChoreList from Choreobject
                var ChoreList = new _ChoreList(ChoreRepository.GetChoreList(ChoreObject.ChoreListId));

                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unauthorized");

                return OKResponse(ChoreObject);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read list
        [HttpGet]
        [HttpOptions]
        [Route("api/getchores")]
        public HttpResponseMessage GetChores(int ChoreListId)
        {
            try
            {
                TinyMapper.Bind<List<Chore>, List<_Chore>>();
                List<_Chore> Chores = TinyMapper.Map<List<_Chore>>(ChoreRepository.GetChores(ChoreListId));

                return OKResponse(Chores);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Update
        [HttpPost]
        [HttpOptions]
        [Route("api/updatechore")]
        public HttpResponseMessage UpdateChore(string AuthToken, int Id, string ChoreValues)
        {
            try
            {
                _User User = new _User(UserRepository.RefreshAuthToken(AuthToken));

                //get chore from Id
                Chore ChoreObject = JsonConvert.DeserializeObject<Chore>(ChoreValues);

                //get ChoreList from Choreobject
                var ChoreList = new _ChoreList(ChoreRepository.GetChoreList((int)ChoreObject.ChoreListId));

                if (User.Id != ChoreList.UserId)
                    throw new Exception("Unauthorized");

                ChoreObject.IsActive = true;

                return OKResponse(new _Chore(ChoreRepository.UpdateChore(Id, ChoreObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Delete
        [HttpPost]
        [HttpOptions]
        [Route("api/deletechore")]
        public HttpResponseMessage DeleteChore(string AuthToken, int Id)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                var Chore = ChoreRepository.GetChore(Id);

                if (!ChoreRepository.CanEditChoreList(User.Id, (int)Chore.ChoreListId))
                    throw new Exception("Not Authorized");

                return OKResponse(new _Chore(ChoreRepository.DeleteChore(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        #endregion CHORES

        //rotation intervals

        #region ROTATIONINTERVALS

        //Create
        [HttpPost]
        [HttpOptions]
        [Route("api/addrotationinterval")]
        public HttpResponseMessage AddRotationInterval(string AuthToken, string RotationIntervalValues)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                var RotationIntervalObject = JsonConvert.DeserializeObject<RotationInterval>(RotationIntervalValues);

                if (!ChoreRepository.CanEditChoreList(User.Id, RotationIntervalObject.ChoreListId))
                    throw new Exception("Not Authorized");

                RotationIntervalObject.IsActive = true;

                return OKResponse(new _RotationInterval(ChoreRepository.AddRotationInterval(RotationIntervalObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read single
        [HttpGet]
        [HttpOptions]
        [Route("api/getrotationinterval")]
        public HttpResponseMessage GetRotationInterval(string AuthToken, int Id)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                var RotationIntervalObject = new _RotationInterval(ChoreRepository.GetRotationInterval(Id));

                if (!ChoreRepository.CanEditChoreList(User.Id, RotationIntervalObject.ChoreListId))
                    throw new Exception("Not Authorized");

                return OKResponse(RotationIntervalObject);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Read List
        [HttpGet]
        [HttpOptions]
        [Route("api/getrotationintervals")]
        public HttpResponseMessage GetRotatioinIntervals(int ChoreListId)
        {
            try
            {
                TinyMapper.Bind<List<RotationInterval>, List<_RotationInterval>>();
                var RotationIntervals = TinyMapper.Map<List<_RotationInterval>>(ChoreRepository.GetRotationIntervals(ChoreListId));

                return OKResponse(RotationIntervals);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Update
        [HttpPost]
        [HttpOptions]
        [Route("api/updaterotationinterval")]
        public HttpResponseMessage UpdateRotationInterval(string AuthToken, int Id, string RotationIntervalValues)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                var RotationIntervalObject = JsonConvert.DeserializeObject<RotationInterval>(RotationIntervalValues);

                if (!ChoreRepository.CanEditChoreList(User.Id, RotationIntervalObject.ChoreListId))
                    throw new Exception("Not Authorized");

                RotationIntervalObject.IsActive = true;

                return OKResponse(new _RotationInterval(ChoreRepository.UpdateRotationInterval(Id, RotationIntervalObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Delete
        [HttpPost]
        [HttpOptions]
        [Route("api/deleterotationinterval")]
        public HttpResponseMessage DeleteRotationInterval(string AuthToken, int Id)
        {
            try
            {
                var User = UserRepository.RefreshAuthToken(AuthToken);

                var RotationInterval = ChoreRepository.GetRotationInterval(Id);

                if (!ChoreRepository.CanEditChoreList(User.Id, RotationInterval.ChoreListId))
                    throw new Exception("Not Authorized");

                return OKResponse(new _RotationInterval(ChoreRepository.DeleteRotationInterval(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        #endregion ROTATIONINTERVALS

        private HttpResponseMessage OKResponse(Object arg)
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


        private HttpResponseMessage ErrorResponse(Exception ex)
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
