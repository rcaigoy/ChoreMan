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
    [Authorize]
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
        public HttpResponseMessage CreateChoreList(string ChoreListValues)
        {
            try
            {
                ChoreList ChoreListObject = JsonConvert.DeserializeObject<ChoreList>(ChoreListValues);
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
        public HttpResponseMessage UpdateChoreList(int Id, string ChoreListValues)
        {
            try
            {
                ChoreList ChoreListObject = JsonConvert.DeserializeObject<ChoreList>(ChoreListValues);
                return OKResponse(new _ChoreList(ChoreRepository.UpdateChoreList(Id, ChoreListObject)));
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
        public HttpResponseMessage DeleteChoreList(int Id)
        {
            try
            {
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
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/addchoreuser")]
        public HttpResponseMessage AddChoreUser(string ChoreUserValues)
        {
            try
            {
                ChoreUser ChoreUserObject = JsonConvert.DeserializeObject<ChoreUser>(ChoreUserValues);
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
        public HttpResponseMessage GetChoreUser(int Id)
        {
            try
            {
                return OKResponse(new _ChoreUser(ChoreRepository.GetChoreUser(Id)));
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


        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/updatechoreuser")]
        public HttpResponseMessage UpdateChoreUser(int Id, string ChoreUserValues)
        {
            try
            {
                ChoreUser ChoreUserObject = JsonConvert.DeserializeObject<ChoreUser>(ChoreUserValues);
                return OKResponse(new _ChoreUser(ChoreRepository.UpdateChoreUser(Id, ChoreUserObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/deletechoreuser")]
        public HttpResponseMessage DeleteChoreUser(int Id)
        {
            try
            {
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
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/addchore")]
        public HttpResponseMessage AddChore(string ChoreValues)
        {
            try
            {
                var ChoreObject = JsonConvert.DeserializeObject<Chore>(ChoreValues);

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
        public HttpResponseMessage GetChore(int Id)
        {
            try
            {
                return OKResponse(new _Chore(ChoreRepository.GetChore(Id)));
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
        [HttpGet]
        [HttpPost]
        [Route("api/updatechore")]
        public HttpResponseMessage UpdateChore(int Id, string ChoreValues)
        {
            try
            {
                Chore ChoreObject = JsonConvert.DeserializeObject<Chore>(ChoreValues);
                return OKResponse(new _Chore(ChoreRepository.UpdateChore(Id, ChoreObject)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Delete
        [HttpGet]
        [HttpPost]
        [Route("api/deletechore")]
        public HttpResponseMessage DeleteChore(int Id)
        {
            try
            {
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
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/addrotationinterval")]
        public HttpResponseMessage AddRotationInterval(string RotationIntervalValues)
        {
            try
            {
                var RotationIntervalObject = JsonConvert.DeserializeObject<RotationInterval>(RotationIntervalValues);
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
        public HttpResponseMessage GetRotationInterval(int Id)
        {
            try
            {
                return OKResponse(new _RotationInterval(ChoreRepository.GetRotationInterval(Id)));
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
        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("api/updaterotationinterval")]
        public HttpResponseMessage UpdateRotationInterval(int Id, string RotationIntervalValues)
        {
            try
            {
                var RotationIntervalObject = JsonConvert.DeserializeObject<RotationInterval>(RotationIntervalValues);
                return OKResponse(new _RotationInterval(ChoreRepository.UpdateRotationInterval(Id, RotationIntervalObject)));
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
        [Route("api/deleterotationinterval")]
        public HttpResponseMessage DeleteRotationInterval(int Id)
        {
            try
            {
                return OKResponse(new _RotationInterval(ChoreRepository.DeleteRotationInterval(Id)));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        #endregion ROTATIONINTERVALS

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
