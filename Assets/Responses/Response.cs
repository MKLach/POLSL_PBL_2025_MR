using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oculus.Platform.Models;

namespace Assets.Responses
{
    [System.Serializable]
    public class User
    {
        public string color;
        public string username;
    }

    [System.Serializable]
    public class UserResponse
    {
        public string status;
        public User user;
    }


    [System.Serializable]
    public class PlaneResponse
    {
        public string status;
        public List<string> planes;
    }

    [Serializable]
    public class ChecklistPerPlaneDataResponse
    {
        public List<string> checklist_titles;
        public string plane;
        public string status;
    }
    [Serializable]
    public class MDFileResponse
    {
        public string status;
        public string plane;
        public string checklist_id;
        public string content;
    }
}
