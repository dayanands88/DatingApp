namespace API.Helpers
{
    public class PagenationParams 
    {
         private const int MaxPageSize = 50;
        public int PageNumber {get;set;} = 5;
        private int _PageSize = 10;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}