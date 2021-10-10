using PaymentSystem.Repo.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Client.API
{
    public class ApiAccess
    {
        private  HttpClient _client;

        internal  HttpClient Client
        {
            get
            {
                if(_client==null)
                {
                    _client = new HttpClient();
                    _client.BaseAddress = new Uri(Constants.BASE_URL);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(Constants.CONTENT_TYPE));
                }
                return _client;
            }
        }

        internal async Task RunAsync()
        {
            try
            {
                //BalanceDto balance = await GetBalanceAsync("api/Pay/ee26f298-2d5f-4ea1-9219-a5f1573e48e9");
            }
            catch (Exception ee)
            {

            }
        }

        internal async Task<UserDto> LoginUserAsync(string userName)
        {
            string path = Constants.LOGIN_URL+ userName;
            HttpResponseMessage response = await Client.PostAsync(path, null);
            UserDto user = null;
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserDto>();
            }
            return user;

        }

        internal async Task<bool> TopUpAsync(Guid userID, decimal topupAmount)
        {
            string path = Constants.TOPUP_URL;
            HttpResponseMessage response = await Client.PostAsJsonAsync(path, 
                new TopupDto() 
                { 
                    UserId = userID,
                    TopupAmount = topupAmount
                });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        internal async Task<BalanceDto> TransferUpAsync(Guid userID, string payeeUserName,decimal transferAmount)
        {
            string path = Constants.TRANSFER_URL;
            BalanceDto balance = null;
            HttpResponseMessage response = await Client.PostAsJsonAsync(path,
                new TransferDto()
                {
                    Id=userID,
                    PayeeUserName = payeeUserName,
                    TransferAmount = transferAmount
                });
            
            if (response.IsSuccessStatusCode)
            {
                balance = await response.Content.ReadAsAsync<BalanceDto>();
            }
            return balance;
        }

        //'https://localhost:5001/api/Login/Authenticate/authenticate?UserName=sanka' \
        //        {
        //          "userId": "96d41f7d-23bf-432a-9406-e2abce76bd64",
        //          "username": "sanka"
        //          }

        //        {
        //  "userId": "96d41f7d-23bf-432a-9406-e2abce76bd64",
        //  "topupAmount": 10
        //}'
        internal async Task<BalanceDto> GetBalanceAsync(string userGuid)
        {
                string path = Constants.BALANCE_URL+userGuid;
                BalanceDto balance = null;
                HttpResponseMessage response = await Client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    balance = await response.Content.ReadAsAsync<BalanceDto>();
                }
                return balance;
        }
    }
}
