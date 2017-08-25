using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Pattern
{
    /// <summary>
    /// Response data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        bool _success = true;
        List<string> _errors;
        string _errorMessage;
        T _data;

        /// <summary>
        /// If result is successsful then the actual response will be in data. It will be null if request fails
        /// </summary>
        public T data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// A boolean variable which tells whether request was successful or there were any errors
        /// </summary>
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        /// <summary>
        /// If result is not successsful then there will be a list of errors telling what went wrong
        /// </summary>
        public List<string> errors { get { return _errors; } set { _errors = value; } }

        /// <summary>
        /// If result is not successsful then there will be an error message telling what went wrong
        /// </summary>
        public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }
        public Result()
        {
            _errors = new List<string>();
            _success = true;
        }

        public void AddErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                _success = false;
                _errors = errors;
            }
        }
        public void AddError(string errorMessage)
        {
            _success = false;
            _errorMessage = errorMessage;


        }

    }
}