
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Helpers;
using Repository.Pattern;
using System.Data.Entity.Validation;

namespace WebApp.Identity
{
    public class UserStoreService : IUserStore<Users, long>,
        IUserPasswordStore<Users, long>,
        IUserEmailStore<Users, long>,

        IDisposable
    {
        IdentityDbContext _context;
        private bool _disposed;


        public UserStoreService()
        {
            //_context = new IdentityDbContext();
        }

        public UserStoreService(IdentityDbContext context)
        {
            _context = context;
        }

        #region IUserStore<TUser,TKey>

        public Task CreateAsync(Users user)
        {
            _context.User.Add(user);
            return _context.SaveChangesAsync();
        }
        public Task DeleteAsync(Users user)
        {
            throw new NotImplementedException();
        }
        public Task<Users> FindByIdAsync(long userId)
        {
            Task<Users> task =
            _context.User.Where(apu => apu.Id == userId && apu.IsActive)
            .FirstOrDefaultAsync();

            return task;
        }
        public Task<Users> FindByNameAsync(string userName)
        {
            Task<Users> task =
                _context.User.Where(apu => apu.UserName == userName && apu.IsActive)
            .FirstOrDefaultAsync();

            return task;
        }
        public Task UpdateAsync(Users user)
        {

            _context.User.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Task.FromResult(0);

        }

        public Result<long> Delete(long userId)
        {
            Result<long> result = new Result<long>();
            try
            {
                Users user = _context.User.Where(u => u.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.IsActive = false;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();

                }
                else
                {
                    result.success = false;
                    result.AddError("User does not exist in system");
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }

            return result;
        }




        //public IEnumerable<SuperAdminUser> GetAdminUser()
        //{


        //    try
        //    {
        //        var users = (from u in _context.Users
        //                    join p in _context.partners on u.partner_id equals p.id
        //                    where u.role != null && u.role.ToLower() == Common.Roles.ADMIN && u.is_deleted != true
        //                     select new SuperAdminUser
        //                     { 
        //                       Id = u.Id,
        //                       Name = u.UserName, 
        //                       FirstName = u.first_name ,
        //                       LastName = u.last_name,
        //                       Email = u.email,
        //                       PartnerName = p.name

        //                    });
        //        //_context.Users.Where(u => (u.Is_admin == true && u.IsDeleted != true));
        //        return users;

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;

        //}

        #endregion


        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(Users user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }
        public Task<bool> HasPasswordAsync(Users user)
        {
            return Task.FromResult(user.Password != null);
        }
        public Task SetPasswordHashAsync(
          Users user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }
        #endregion


        #region IUserEmailStore

        public Task SetEmailAsync(Users user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            user.Email = email;
            _context.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(Users user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(Users user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(Users user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            //    user.Email = true;
            _context.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<Users> FindByEmailAsync(string email)
        {
            return _context.User.FirstOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper());
        }

        public Users FindByEmail(string email)
        {
            return _context.User.Where(u => u.Email.ToUpper() == email.ToUpper() && u.IsActive == true).FirstOrDefault();
        }

        #endregion

        #region IUserClaimStore


        #endregion


        #region CustomMethod

        #endregion


        /*     #region CustomMethod

         * 
         * 
        public Result<int> UpdateUsersbyEmail(string email)
        {
            Result<int> result = new Result<int>();
            try
            {
              List<Users> users = _context.User.Where(u => u.email == email).ToList<User>();
              if(users != null && users.Count > 0)
              {

                  foreach(Users u in users)
                  {
                      u.is_password_reset_requested = true;
                   
                  }

                  _context.SaveChanges();
              }

            }
            catch(Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }
            return result;
        }

        public Result<int> UpdateUsersbyEmail(string email,int userId)
        {
            Result<int> result = new Result<int>();
            try
            {
                List<Users> users = _context.Users.Where(u => u.email == email).ToList<User>();
                if (users != null && users.Count > 0)
                {

                    foreach (Users u in users)
                    {
                        if (u.Id == userId)
                        {
                            u.is_password_updated = true;
                            u.temp_password = null;
                        }
                        u.is_password_reset_requested = false;

                    }

                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }
            return result;
        }

        #endregion*/
        public Result<List<Users>> GetAllUsers(long siteid, int pageId, int pageSize, ref int count)
        {
            Result<List<Users>> result = new Result<List<Users>>();
            try
            {
                List<Users> users = _context.User.Where(u => u.IsActive).OrderByDescending(i => i.DateCreated).ToList();
                if (users != null && users.Count > 0)
                    result.data = users;
                else
                {
                    result.success = false;
                    result.AddError("No user found");
                }


            }
            catch (Exception ex)
            {
                result.success = false;
                result.AddError(ex.Message);
            }
            return result;
        }


        public void Dispose()
        {
            //  _context.Dispose();
        }
    }
}