# PE-Nation assessment
This assessment includes 2 parts:
- The APIMocker: A simple application which based on the configuration defined in its AppSettings simulates an API
- The PENation.API: An API application which connects to an external API. In this configuration it's connecting to the mocked API.

## The APIMocker 
### configuration
The configuration is only one tag *APIDetail* which contains following elements:
- Path
- QueryString
- StatusCode
- ResponseBody

To create an API serving /order?customerNo=12345 we need to configure as:

``` json
  "APIDetail": [
  {
    "Path": "/Order",
    "QueryString": "?customerNo=12345",
    "StatusCode": 200,
    "ResponseBody": "[{\"orderNumber\":\"23\",\"orderDate\":\"2020-07-01T06:57:53.917141+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"10\",\"orderDate\":\"2020-07-14T06:57:53.917435+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"7\",\"orderDate\":\"2020-07-08T06:57:53.917443+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"-9\",\"orderDate\":\"2020-07-14T06:57:53.917444+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"9\",\"orderDate\":\"2020-06-27T06:57:53.917468+10:00\",\"orderStatus\":\"Order\"}]"
  }],
```

The QueryString can be even replaced with 
```json
    "QueryString": "?customerNo={0}",
```
to support other values of customerNo.

### multiple API
If we have more than one APIDetail they should be ordered based on more specific first.
```json
  "APIDetail": [
    {
      "Path": "/Order",
      "QueryString": "?customerNo={0}&date={1}",
      "StatusCode": 200,
      "ResponseBody": "[{\"orderNumber\":\"23\",\"orderDate\":\"2020-07-01T06:57:53.917141+10:00\",\"orderStatus\":\"Order\"}]"
    },
    {
      "Path": "/Order",
      "QueryString": "?customerNo={0}",
      "StatusCode": 200,
      "ResponseBody": "[{\"orderNumber\":\"23\",\"orderDate\":\"2020-07-01T06:57:53.917141+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"10\",\"orderDate\":\"2020-07-14T06:57:53.917435+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"7\",\"orderDate\":\"2020-07-08T06:57:53.917443+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"-9\",\"orderDate\":\"2020-07-14T06:57:53.917444+10:00\",\"orderStatus\":\"Order\"},{\"orderNumber\":\"9\",\"orderDate\":\"2020-06-27T06:57:53.917468+10:00\",\"orderStatus\":\"Order\"}]"
    }
  ],
```

Then the APIMocker needs to be executed.
![Running the API mocker](https://github.com/mkokabi/pe-nation/blob/master/images/Running%20the%20APIMocker.png?raw=true)

It's how it looks:
MockAPI Welcome page
![API mocker](https://github.com/mkokabi/pe-nation/blob/master/images/MockAPI%20Welcome%20page.png?raw=true)
![API mocker](https://github.com/mkokabi/pe-nation/blob/master/images/APIMocker%20at%20work.png?raw=true)

## API Consumer
The Second part is just a simple API application which is going to consume this application. It's an API application itself but can be a Web application or a background service or anything else.

In it's configuration it has the URL which is currently pointint to the mock application
``` json
  "OrderApiUrl": "https://localhost:46609/order?customerNo=12345",
```

The code is simply getting this URL from the config and making a call.
``` Csharp
var client = _clientFactory.CreateClient();
var orderApiUrl = _configuration["OrderApiUrl"];

client.DefaultRequestHeaders
  .Accept
  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

var request = new HttpRequestMessage(HttpMethod.Get, orderApiUrl);
var response = await client.SendAsync(request);

if (response.IsSuccessStatusCode)
{
    var responseContent = await response.Content.ReadAsStringAsync();
    var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(responseContent);
    return orders;
}

```
Then we can run the API application
![Running the API consumer](https://github.com/mkokabi/pe-nation/blob/master/images/Running%20the%20API%20consumer.png?raw=true)

It's how it looks:
![API Consumer](https://github.com/mkokabi/pe-nation/blob/master/images/API%20application%20consuming%20the%20Mock.png?raw=true)

