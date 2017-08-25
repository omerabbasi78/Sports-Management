using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Security.Policy;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.HelperClass;
using Repository.Pattern;

namespace WebApp.Identity
{
    public class UsersManager
    {

        private readonly IOwinContext _iOwinContext = HttpContext.Current.GetOwinContext();
        UserStoreService userStoreService;
    
        private ApplicationUserManager _userManager;
        public ApplicationUserManager AppUserManager
        {
            get
            {
                return _userManager ?? _iOwinContext.Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UsersManager()
        {
            userStoreService = new UserStoreService();
        }

        public Result<Users> FindById(long Id)
        {
            Result<Users> result = new Result<Users>();
            Users user = AppUserManager.FindById(Id);
            if (user == null)
            {
                result.success = false;
                result.AddError("User dows not exist in system");
            }
            else
            {
                result.data = user;
            }
            return result;
        }


     

        public Result<long> Delete(long userId)
        {
            Result<long> result = new Result<long>();
            result = userStoreService.Delete(userId);
            return result;
        }


        public Result<long> CreateUser(Users newUser, ControllerBase controllerBase)
        {
            Result<long> result = new Result<long>();

            // result=userStoreService.FindUserByName(newUser.UserName);

            //newUser.created_by = int.Parse(HttpContext.Current.User.Identity.GetUserId());// == null ? 1 : int.Parse(HttpContext.Current.User.Identity.GetUserId());
            newUser.DateCreated = DateTime.Now;
            //newUser.TempPassword = RandomPassword.Generate();
            //newUser.Password = newUser.TempPassword;
            newUser.IsAccountVerified = false;

            var returnVal = AppUserManager.Create(newUser, newUser.Password);
            if (returnVal.Succeeded)
            {
                if (newUser.IsActive)
                {
                    SendWelcomeEmmail(newUser.Id, controllerBase);
                }
                result.success = true;
                result.data = newUser.Id;
            }
            else
            {
                result.AddErrors(returnVal.Errors.ToList<string>());
            }

            return result;

        }

        public List<Users> GetAllUsers(long siteid, int pageId, int pageSize, ref int count)
        {
            Result<List<Users>> result = userStoreService.GetAllUsers(siteid, pageId, pageSize, ref count);
            if (result.data == null)
            {
                result.success = false;
                result.AddError("No user found");
                return null;
            }
            return result.data;
        }



        public Result<Users> SignIn(LoginViewModel model)
        {
            Result<Users> result = new Result<Users>();
            Users user = AppUserManager.Find(model.UserName, model.Password);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>();
                    
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
                    claims.Add(new Claim("UserId", user.Id.ToString()));
                    claims.Add(new Claim("Name", user.Name));
                    claims.Add(new Claim("ProfilePic", user.ProfilePic));
                    claims.Add(new Claim("RoleName", user.Role.RoleName.ToString()));
                    claims.Add(new Claim("RoleId", user.RoleId.ToString()));

                    var id = new ClaimsIdentity(claims,
                                                DefaultAuthenticationTypes.ApplicationCookie);


                    AuthenticationManager.SignIn(id);

                    user.LastLogin = DateTime.Now;
                    AppUserManager.Update(user);
                    result.data = user;

                }
                else
                {
                    result.success = false;
                    result.AddError("Please Activate your account and try to login.");
                }


            }
            else
            {
                result.success = false;
                result.AddError("Invalid Username or Password.");
            }

            return result;

        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _iOwinContext.Authentication;
            }
        }

        public Result<int> ResetPassword(ResetPasswordViewModel model)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindByName(model.UserName);
                if (user == null)
                {
                    result.success = false;
                    result.AddError("User does not exist in system.");
                    return result;
                }

                if (user.IsPasswordResetRequested != true)
                {
                    result.success = false;
                    result.AddError("Your Password Reset Token has been expired, contact system Administrator.");
                    return result;
                }
                var res = AppUserManager.ResetPassword(user.Id, model.Code, model.Password);
                if (res.Succeeded)
                {
                    user.TempPassword = null;
                    AppUserManager.Update(user);
                }
                else
                {
                    result.success = false;
                    result.errors = res.Errors.ToList<string>();
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }

        public Result<int> ForgotPassword(ForgotPasswordViewModel model, ControllerBase controllerBase)
        {
            Result<int> result = new Result<int>();
            Users user = AppUserManager.FindByName(model.UserName);
            if (user != null && user.Id > 0 && user.IsActive)
            {
                ForgotPasswordViewModel info = new ForgotPasswordViewModel();
                string code = AppUserManager.GeneratePasswordResetToken(user.Id);

                var callbackUrl = Common.GetUrlHelper().Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
                info.Name = user.Name;
                info.Url = callbackUrl;
                info.UserName = user.UserName;

                info.Email = user.Email;

                string defaultPath = "~/Views/Templates/Default/ForgotPassword.cshtml";

                string emailBody = Common.RenderRazorViewToString(defaultPath, info, controllerBase);
                AppUserManager.SendEmail(user.Id, "Reset Password", emailBody);
              
                user.IsPasswordResetRequested = true;
                AppUserManager.Update(user);

             
            }
            else
            {

                result.success = false;
                result.AddError("User does not exist in system");
            }

            

            
           
            
            return result;



        }

        public Result<ResetPasswordViewModel> GetResetPasswordModel(long userId)
        {
            ResetPasswordViewModel model;

            Result<ResetPasswordViewModel> result = new Result<ResetPasswordViewModel>();
            Result<Users> userResult = new Result<Users>();
            userResult = FindById(userId);
            string code = AppUserManager.GeneratePasswordResetToken(userId);
            if (userResult.success)
            {
                model = new ResetPasswordViewModel();
                model.Email = userResult.data.Email;
                model.UserName = userResult.data.UserName;
                model.Code = code;
                result.data = model;

            }
            else
            {
                result.success = false;
                result.AddError(userResult.ErrorMessage);
            }
            return result;

        }

        public Result<int> UpdateResult(ResetPasswordViewModel model)
        {
            Result<int> result = new Result<int>();
            try
            {
                AppUserManager.RemovePassword(Common.CurrentUser.Id);
                AppUserManager.AddPassword(Common.CurrentUser.Id, model.Password);
                Users user = AppUserManager.FindById(Common.CurrentUser.Id);
                user.TempPassword = null;
                AppUserManager.Update(user);
                var identity = new ClaimsIdentity(HttpContext.Current.User.Identity);
                var ctx = _iOwinContext;

                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant
                (new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }
            return result;

        }

        public Result<int> SendWelcomeEmmail(long userId, ControllerBase controllerBase)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindById(userId);
                if (user == null)
                {

                    result.success = false;
                    result.AddError("User does not exist in system");

                }
                else
                {

                   

                    string defaultPath = "~/Views/Templates/Default/WelcomeEmail.cshtml";
                   

                    string emailBody = Common.RenderRazorViewToString(defaultPath, user, controllerBase);
                    AppUserManager.SendEmail(user.Id, "Welcome Email", emailBody);
                    AppUserManager.Update(user);

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }
     /*   public Result<ResetPasswordViewModel> GetResetPasswordModel(int userId)
        {
            ResetPasswordViewModel model;

            Result<ResetPasswordViewModel> result = new Result<ResetPasswordViewModel>();
            Result<Users> userResult = new Result<Users>();
            userResult = FindById(userId);
            string code = AppUserManager.GeneratePasswordResetToken(userId);
            if (userResult.success)
            {
                model = new ResetPasswordViewModel();
                model.Email = userResult.data.email;
                model.UserName = userResult.data.UserName;
                model.Code = code;
                result.data = model;

            }
            else
            {
                result.success = false;
                result.AddError(userResult.ErrorMessage);
            }
            return result;

        }

        public Result<Users> SignIn(LoginViewModel model)
        {
            Result<Users> result = new Result<Users>();
            Users user = AppUserManager.Find(model.Email, model.Password);
            bool is_admin = false;
            bool is_super_admin = false;
            if (user != null)
            {
                if (user.is_active)
                {
                    if ((user.role == null || user.role.ToLower() == Common.Roles.ADMIN.ToLower() || user.role.ToLower() == Common.Roles.USER.ToLower()) && user.partner_id != null)
                    {
                        Partners partner = new PartnerManager().FindById((int)user.partner_id);
                        if(partner == null || partner.disabled == true)
                        {
                            result.success = false;
                            result.AddError("Invalid Username or Password.");
                            return result;
                        }
                    }

                    var claims = new List<Claim>();
                   
                    claims.Add(new Claim(ClaimTypes.Name, user.first_name + " " + user.last_name));
                    claims.Add(new Claim(ClaimTypes.Email, user.email));
                    claims.Add(new Claim("password", user.password));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim("created_by", user.created_by.ToString()));
                    claims.Add(new Claim("system_id", user.system_id == null ? "0" : user.system_id.ToString()));
                    claims.Add(new Claim("is_password_updated", user.is_password_updated.ToString()));
                    claims.Add(new Claim("partner_id", user.partner_id == null ? "0" : user.partner_id.ToString()));
                    
                    if(user.role != null && user.role.ToLower() == Common.Roles.ADMIN)
                    {
                        is_admin = true;
                    }

                    if (user.role != null && user.role.ToLower() == Common.Roles.SUPER_ADMIN)
                    {
                        is_super_admin = true;
                    }

                    claims.Add(new Claim("is_admin", is_admin.ToString()));
                    claims.Add(new Claim("is_super_admin", is_super_admin.ToString()));

                    var id = new ClaimsIdentity(claims,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                  
                
                    AuthenticationManager.SignIn(id);

                    user.last_login = DateTime.Now;
                    AppUserManager.Update(user);
                    result.data = user;


                }
                else
                {
                    result.success = false;
                    result.AddError("Please Activate your account and try to login.");
                }


            }
            else
            {
                result.success = false;
                result.AddError("Invalid Username or Password.");
            }

            return result;



        }

        public Result<int> ForgotPassword(ForgotPasswordViewModel model, ControllerBase controllerBase)
        {
            Result<IEnumerable<SystemInfo>> systemInfo = new Result<IEnumerable<SystemInfo>>();
            systemInfo = customerStoreService.FindByEmail(model.Email);
            Result<int> result = new Result<int>();
            if (systemInfo.success)
            {

                List<ForgotPasswordViewModel> forgotPasswordViewModel = new List<ForgotPasswordViewModel>();

                foreach (var u in systemInfo.data)
                {
                    ForgotPasswordViewModel info = new ForgotPasswordViewModel();
                    string code = AppUserManager.GeneratePasswordResetToken(u.user_id);

                    var callbackUrl = Common.GetUrlHelper().Action("ResetPassword", "Account", new { userId = u.user_id, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
                    string fullname = u.name;
                    info.Name = fullname;
                    info.Url = callbackUrl;
                    info.Username = u.UserName;
                
                    info.Email = model.Email;


                    forgotPasswordViewModel.Add(info);
                }
                var currentUser = systemInfo.data.FirstOrDefault();


                int? partner = currentUser.partner_id;

                string defaultPath = "~/Views/Templates/Default/ForgotPassword.cshtml";
                if (partner != null)
                {
                    string partnerPath = "~/Views/Templates/Partners/" + partner.ToString() + "/ForgotPassword.cshtml";
                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(partnerPath)))
                    {
                        defaultPath = partnerPath;
                    }
                }



                string emailBody = Common.RenderRazorViewToString(defaultPath, forgotPasswordViewModel, controllerBase);
                AppUserManager.SendEmail(currentUser.user_id, "Reset Password", emailBody);
                Result<int> updateuser = new Result<int>();
                updateuser = userStoreService.UpdateUsersbyEmail(model.Email);


            }
            else
            {
                result.success = false;
                result.AddError(systemInfo.ErrorMessage);


            }

            return result;



        }

        public Result<int> SendWelcomeEmmail(int userId, ControllerBase controllerBase)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindById(userId);
                if (user == null)
                {

                    result.success = false;
                    result.AddError("User does not exist in system");

                }
                else
                {
                   
                    int? partner = user.partner_id;

                    string defaultPath = "~/Views/Templates/Default/WelcomeEmail.cshtml";
                    if (partner != null)
                    {
                        string partnerPath = "~/Views/Templates/Partners/" + partner.ToString() + "/WelcomeEmail.cshtml";
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(partnerPath)))
                        {
                            defaultPath = partnerPath;
                        }
                    }

                    string emailBody = Common.RenderRazorViewToString(defaultPath, user, controllerBase);
                    AppUserManager.SendEmail(user.Id, "Welcome Email", emailBody);

                    user.is_welcome_email_sent = true;
                    AppUserManager.Update(user);

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }

        public Result<int> ResendSendWelcomeEmmail(int userId, ControllerBase controllerBase)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindById(userId);
                if (user == null)
                {

                    result.success = false;
                    result.AddError("User does not exist in system");

                }
                else
                {
                    user.temp_password = RandomPassword.Generate();
                    user.password = user.temp_password;
                    AppUserManager.RemovePassword(userId);
                    AppUserManager.AddPassword(userId, user.temp_password);
                    user.is_password_updated = false;
                    AppUserManager.Update(user);

                   
                    int? partner = user.partner_id;

                    string defaultPath = "~/Views/Templates/Default/WelcomeEmail.cshtml";
                    if (partner != null)
                    {
                        string partnerPath = "~/Views/Templates/Partners/" + partner.ToString() + "/WelcomeEmail.cshtml";
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(partnerPath)))
                        {
                            defaultPath = partnerPath;
                        }
                    }

                    string emailBody = Common.RenderRazorViewToString(defaultPath, user, controllerBase);
                    AppUserManager.SendEmail(user.Id, "Welcome Email", emailBody);

                    user.is_welcome_email_sent = true;
                    AppUserManager.Update(user);

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }

        public Result<int> ResetPassword(ResetPasswordViewModel model)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindByName(model.UserName);
                if (user == null)
                {
                    result.success = false;
                    result.AddError("User does not exist in system.");
                    return result;
                }

                if (user.is_password_reset_requested != true)
                {
                    result.success = false;
                    result.AddError("Your Password Reset Token has been expired, contact system Administrator.");
                    return result;
                }
                var res = AppUserManager.ResetPassword(user.Id, model.Code, model.Password);
                if (res.Succeeded)
                {
                    userStoreService.UpdateUsersbyEmail(user.email, user.Id);
                }
                else
                {
                    result.success = false;
                    result.errors = res.Errors.ToList<string>();
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }

        public Result<int> UpdateResult(ResetPasswordViewModel model)
        {
            Result<int> result = new Result<int>();
            try
            {
                AppUserManager.RemovePassword(Common.CurrentUser.user_id);
                AppUserManager.AddPassword(Common.CurrentUser.user_id, model.Password);
                Users user = AppUserManager.FindById(Common.CurrentUser.user_id);
                user.temp_password = null;
                user.is_password_updated = true;
                AppUserManager.Update(user);
                var identity = new ClaimsIdentity(HttpContext.Current.User.Identity);
                identity.RemoveClaim(identity.FindFirst("is_password_updated"));
                identity.AddClaim(new Claim("is_password_updated", user.is_password_updated.ToString()));
                var ctx = _iOwinContext;
           
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant
                (new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }
            return result;

        }

        public Result<int> ResetPasswordAdmin(ResetPasswordViewModel model)
        {
            Result<int> result = new Result<int>();
            try
            {
                Users user = AppUserManager.FindByName(model.UserName);
                if (user == null)
                {
                    result.success = false;
                    result.AddError("User does not exist in system.");
                    return result;
                }

               
                var res = AppUserManager.ResetPassword(user.Id, model.Code, model.Password);
                if (!res.Succeeded)
                {
                    result.success = false;
                    result.errors = res.Errors.ToList<string>();
                    return result;
                }
                result.data = user.Id;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
                return result;
            }
          
            return result;
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _iOwinContext.Authentication;
            }
        }

        public IEnumerable<SuperAdminUser> GetAdminUsers(int? pageId, string searchTerm = "")
        {

            
            try
            {

                IEnumerable<SuperAdminUser> users = userStoreService.GetAdminUser();

                if (users != null && users.Count() > 0)
                {
                    if (!String.IsNullOrEmpty(searchTerm))
                    {
                        searchTerm = searchTerm.ToUpper();
                        users = users.Where(s => s.Name.ToUpper().Contains(searchTerm)
                                               || s.FirstName.ToUpper().Contains(searchTerm)
                                               || s.LastName.ToUpper().Contains(searchTerm)
                                               || s.Email.ToUpper().Contains(searchTerm)).ToList<SuperAdminUser>();
                    }

                    users = users.OrderByDescending(s => s.Id);
                    int pageSize = 15;
                    int pageNumber = (pageId ?? 1);
                    return users.ToPagedList(pageNumber, pageSize);
                }
            }
            catch(Exception ex)
            {
             return null;
            }

            return null;
        } */

    }
}