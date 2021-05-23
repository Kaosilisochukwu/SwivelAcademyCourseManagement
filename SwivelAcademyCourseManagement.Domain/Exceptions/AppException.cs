using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Domain.Exceptions
{
    [Serializable]
    public class AppUserException : Exception
    {
        public AppUserException()
        {
        }

        public AppUserException(string message)
            : base(message)
        {
        }

        public AppUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class AppServerException : Exception
    {
        public AppServerException()
        {
        }

        public AppServerException(string message)
            : base(message)
        {
        }

        public AppServerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class CoursesException : Exception
    {
        public CoursesException()
        {
        }

        public CoursesException(string message)
            : base(message)
        {
        }

        public CoursesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
