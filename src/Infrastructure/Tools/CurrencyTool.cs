using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Tools
{
    public class CurrencyTool
    {
        private readonly HttpClient _httpClient;

        public CurrencyTool(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Description("Get the exchange rate for given currency code.")]
        public async Task<string> GetExchangeRate([Description("Currency code like USD, EUR")]string currencyCode,CancellationToken cancellationToken)
        {
            var url="https://www.tcmb.gov.tr/kurlar/today.xml";

            try
            {
                var response= await _httpClient.GetStringAsync(url,cancellationToken);
                var doc = XDocument.Parse(response);
                var currency = doc.Descendants("Currency").FirstOrDefault(x=>x.Attribute("Kod")?.Value==currencyCode.ToUpper());
                if (currency == null) {
                    return $"There isn't any exchange rate information for {currencyCode}";
                }

                var name=currency.Element("Isim")?.Value;
                var buying=currency.Element("ForexBuying")?.Value??"bilinmiyor";
                var selling = currency.Element("ForexSelling")?.Value??"bilinmiyor";

                return $"{name} ({currencyCode}): alış = {buying}, satış = {selling}";


            }
            catch(OperationCanceledException) {
                throw;
            }
            catch (Exception ex) { 
                return ex.Message;
            
            }
        }



    }
}
