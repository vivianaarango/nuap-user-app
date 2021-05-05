namespace nuap.ViewModels
{
    public class PaymentGatewayViewModel: BaseViewModel
    {
        private string url;

        public string Url
        {
            get { return this.url; }
            set { SetValue(ref this.url, value); }
        }

        public PaymentGatewayViewModel(string url)
        {
            this.Url = url;
        }
    }
}
