using System.Collections.Generic;
using DockerDemo.API.Models;

namespace DockerDemo.API.ViewModels
{
    public class GetSugarBabiesResp
    {
        public List<SugarBaby> Data { get; set; }
        
        public int Total { get; set; }
    }
}