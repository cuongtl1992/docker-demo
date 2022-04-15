namespace DockerDemo.API.ViewModels
{
    public class GetSugarBabiesReq
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 15;
        
        public string Keyword { get; set; }
    }
}