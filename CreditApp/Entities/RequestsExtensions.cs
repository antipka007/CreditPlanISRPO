namespace CreditApp.Entities
{
    public partial class Requests
    {
        public string StatusText
        {
            get
            {
                switch (status_req)
                {
                    case "Waiting":
                        return "На рассмотрении";
                    case "Approved":
                        return "Одобрено";
                    case "Canceled":
                        return "Отклонено";
                    default:
                        return status_req;
                }
            }
        }
    }
}
