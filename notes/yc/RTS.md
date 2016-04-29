## RTS
### REST Server
-----
#### MainServer
+ Logger
+ JettyServerWrapper
+ TimeZone
+ ConfigWrapper
+ HibernateFactory
+ Tracking, by COOKIE/DEVICE

#### Request handler
+ Bid request handler (define in ExchangeType enum)
  + ORTB, ORTB_RTB/ORTB_DEBUG/ORTB_SIMULATOR
  + BIDU, BIDU_RTB/BIDU_DEBUG
  + TENCENT, TENCENT_RTB/TENCENT_DEBUG
+ User request handler (define in UserRequestHandlerType)
  + JsToServeAdHandler, js/as.js
  + JsToServeBeaconHandler, js/bk.js
  + AD_SELECT, as
  + AD_CLICK, ac
  + BEACON, bk.gif/bk.html
  + COOKIE_MAPPING, cm
  + AD_CREATIVE, creatives
+ Win notification, WinNotification
+ User profile accessor, userProfile
+ Beacon generator request, beaconGenerator
+ Bid simulator, bidSimulator

### Flow
#### Cookie Matching
#### start the cm request after beacon
1. Beacon be accessed, BeaconHandler serving
2. initiateCookieMatch
    1. ExchangeType.getPossibleExchangesForCookieMatching()
      + Why return exchangesForCookieMatch.clone? This just to make sure
    2. UserProfileWrapper.getOrgToExternalIds()
    3. For each exchange info of this user profile
      + check this exchange support cookie matching or not
      + for the cookie match not expired (not check expiration by now), will do cm
    4. get redirect url for cm, only the first non-expired exchange will do CM
    5. httpResponse.sendRedirect(url);

#### CookieMatchHandler
#### Process
1. SHADC IP
2. HAPROXY TRAFFIC
#### TODO
+ AbstractUserRequestHandler.cookieMatchExpired not implemented
+ AbstractUserRequestHandler.getRedirectUrlForCookieMatch() only match the first exchange, if it fail...

### Auditing
Audit
#### Advertiser
#### Creative
+ public int AddCreative(int adId) // return the ret_code, log the whole return message,
+ public int GetAuditFailedCreatives()


#### TODO


#### Other Class
##### CacheWrapper,
##### Restriction


### Beacon
-----
### Bid
-----
#### AdSelector

#### Exchange Type
#### Exchange Adaptor
> adaptor for the pb and request/response
#### Bid Process
+ Bidding Request (BIDU BES--> load balance --> bid server)
  1. fast no (80%), for fraud traffic or no value traffic
  1. qualify ads campaign-ad
      1. check the restriction of the placement and the ads
      2. check the location, user profile
      3. check the policy
      4. check the budget
  1. HBase to get user profile
      + impression
      + click
      + beacon/active
  1. for each ads do
      1. CTR/CVR prediction
      2. pricing, here is the second bid pricing, and optimized aligorithm to get best price
      3. when submit, will return the max bid price, campaign-ad to BES
  1. log --> ETL pipeline --> Hadoop
+ Bid Win Request (BES-->bid server)
  1. update budget
  2. update user profile in HBase (add the bid/win record)
  3. log --> Hadoop
+ User click ad
  1. update user profile
  1. log --> Hadoop
+ User access adv website, beacon call
  1. update user profile
  1. log --> Hadoop
  1.  conversion

### BudgetMangement
-----
### UserStoreManager
-----
### AdSelect
-----
+ Flow
> When there is placement, will create adseel

+ RequestData

### AdClick
-----

### Model
-----
+ log --> Hadoop, and the Hadoop tables
  + bid
  + impression
  + click
  + beacon active
+ Hadoop bid log include
  +  timestamp, uid, URL, ref-URL, site category, IP/location, user agent, user profile ...
+ What the Hadoop log used for
  + report
    + performance for the logs
    + insights for the activities
  + model
