using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added project
using ChoreMan.Entities;
using ChoreMan.Models;

//added internal
using System.Reflection;

//added external
using Nelibur.ObjectMapper;

namespace ChoreMan.Services
{
    public class UserRepository
    {
        private ChoremanEntities db;
        public UserRepository()
        {
            try
            {
                this.db = new ChoremanEntities("ChoremanEntities2");
                
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        public UserRepository(ChoremanEntities _db)
        {
            try
            {
                this.db = _db;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //CRUD
        
        //Create
        public User CreateUser(User Value)
        {
            try
            {
                //lowercase all values;
                Value.Username = Value.Username.ToLower();
                Value.Email = Value.Username.ToLower();

                if (VerifyUniqueUser(Value)) ;

                //create responseMessage out variable
                var ResponseMessage = new System.Data.Entity.Core.Objects.ObjectParameter("responseMessage", "");

                //use User class into Stored Procedure
                db.dbo_AddUser(pUsername: Value.Username, pEmail: Value.Email, pPhone: Value.Phone, pFirstName: Value.FirstName, pLastName: Value.LastName, pPassword: Value.Password, responseMessage: ResponseMessage);

                //get result
                string result = ResponseMessage.Value.ToString();

                if (result == "Success")
                {
                    db.SaveChanges();
                    return Value;
                }
                
                throw new Exception(result);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Read
        public User Login(string Username, string Password)
        {
            try
            {
                //lowercase username
                Username = Username.ToLower();
                //check if Username string is email
                if (Username.Contains('@') && Username.Contains('.'))
                {
                    //get username from email
                    var UsernameQuery = db.Users.Where(x => x.Email == Username).ToList();

                    //
                    if (UsernameQuery.Count > 0)
                    {
                        Username = UsernameQuery.FirstOrDefault().Username;
                    }
                    else
                    {
                        throw new Exception("Incorrect Username, Email, or Password");
                    }
                }

                //login with stored procedure
                //create responseMessage out variable
                var ResponseMessage = new System.Data.Entity.Core.Objects.ObjectParameter("responseMessage", "");

                //use stored procedure to authenticate
                db.Authenticate(pUsername: Username, pPassword: Password, responseMessage: ResponseMessage);

                //get result
                string result = ResponseMessage.Value.ToString();

                if (result == "Success")
                {
                    return db.Users.SingleOrDefault(x => x.Username == Username);
                }

                throw new Exception("Incorrect Username, Email, or Password");
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Update User Info
        public User UpdateUserInfo(int Id, User NewUser)
        {
            try
            {
                //lowercase username and email
                NewUser.Username = NewUser.Username.ToLower();
                NewUser.Email = NewUser.Email.ToLower();

                var OldUser = db.Users.SingleOrDefault(x => x.Id == Id);

                if (VerifyUniqueUser(NewUser, OldUser)) ;

                //iterate through all properties
                //except Id
                //and except PasswordHash
                //and except Salt
                foreach (var property in NewUser
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id" 
                                                && x.Name != "PasswordHash" 
                                                && x.Name != "Salt"))
                {
                    //get the value of the iterated property
                    var value = property.GetValue(NewUser);

                    if (value != null)
                    {
                        Type type = NewUser.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(OldUser, value, null);
                    }
                }

                db.SaveChanges();

                return OldUser;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Update UserPassword
        public User ChangePassword(string Username, string OldPassword, string NewPassword)
        {
            try
            {
                //lowercase username
                Username = Username.ToLower();

                //verify user with login;
                User User = Login(Username, OldPassword);

                //create stored procedure output variable
                var ResponseMessage = new System.Data.Entity.Core.Objects.ObjectParameter("responseMessage", "");

                //use stored procedure to change password
                db.UpdatePassword(pUsername: Username, pPassword: NewPassword, responseMessage: ResponseMessage);

                //get result
                string result = ResponseMessage.Value.ToString();

                if (result == "Success")
                {
                    //save changes
                    db.SaveChanges();

                    //return user
                    return db.Users.SingleOrDefault(x => x.Id == User.Id);
                }

                //if not successful, state db exception
                throw new Exception(result);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Delete User
        public User DeleteUser(int Id)
        {
            try
            {
                var User = db.Users.SingleOrDefault(x => x.Id == Id);
                User.IsActive = false;
                User.IsVerified = false;
                db.SaveChanges();

                return User;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Verify Account
        public User VerifyAccount(int Id)
        {
            try
            {
                var User = db.Users.SingleOrDefault(x => x.Id == Id);
                User.IsVerified = true;
                db.SaveChanges();

                return User;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //global use method
        public bool VerifyUniqueUser(User NewUser, User OldUser = null)
        {
            try
            {
                //if username exists
                if (db.Users.Count(x => x.Username == NewUser.Username) > 0)
                {
                    //if old user is null or old user and new user don't have same username
                    if (OldUser == null || NewUser.Username != OldUser.Username)
                        throw new Exception("Username already in use");
                }

                //if email exists
                if (db.Users.Count(x => x.Email == NewUser.Email) > 0)
                {
                    //if old user is null or old user and new user don't have same email
                    if (OldUser == null || NewUser.Email == NewUser.Email)
                        throw new Exception("Email already in use");
                }

                //if phone exists
                if (db.Users.Count(x => x.Phone == NewUser.Phone) > 0)
                {
                    //if old user is null or old user and new user don't have same phone
                    if (OldUser == null || NewUser.Phone == OldUser.Phone)
                        throw new Exception("Phone number already in use");
                }

                //check if New User info has special characters.
                if (!NewUser.Username.All(char.IsLetterOrDigit))
                    throw new Exception("Username cannot consist of special characters or spaces");


                //if email does not have @ or . characters
                if (!NewUser.Email.Contains('@') || !NewUser.Email.Contains('.'))
                    throw new Exception("Invalid Email Address");

                return true;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}