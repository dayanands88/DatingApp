namespace API.Helpers
{
    public class LikesParams : PagenationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}