## Tencent Integration
### Preapring
+ Sandbox Account for the ADX
+ Formal Account for the ADX

### Advertiser
+ name
+ url
+ license
+ IOException
+ check_status
+ memo



### Cookie Mapping
#### Process
1. User->DSP CM Server
2. DSP 302 redirect to TCMS with dspid,dspuid,gettuid
3. TCMS dspuid, redirect to DSP CM Server
4. DSP store tuid, response to USer

### RTB

#### Process
1. User->Publisher
2. User->ADX
3. ADX->DSP (bid request)
4. DSP->ADX (bid response
5. ADX->DSP (win notice)
6. ADX->User

### API
> 您的dsp_id ：213，您的token ：38ef2b8ef25865d4fe6cc5a8d07ca3f7

> 沙箱环境:http://opentest.adx.qq.com/location/list


### Report

### Deploy
`目前腾讯广告流量分布在天津(联通)、上海(电信)、深圳(电信)三个 IDC, Ad Server
与 AdExchange 服务器都采取就近接入的方式,要求 DSP 就近部署,进行网络测试 Ping 值
最好在 40ms 以内,流量的处理能力在 10000 QPS 以上,超时率控制在 10%以内
`

## Tencent RTB implementation
#### JettyServerWrapper.AddHandlersAndFilters
```JAVA
JettyServerWrapper.AddHandlersAndFilters
    this.createAndRegisterBidRequestHandlers();
    this.registerUserRequestHandlers(contextSupplier);
        //Here will loop UserRequestHandlerType, and register below:
          as.js->JsToServeAdHandler
          as->adSelectHandler
          ac->AdClickHandler
          bk.gif,bk.html->BeaconHandler
          cratives->AdCreativeHandler
    this.register(new BeaconGeneratorServlet(contextSupplier), "beaconGenerator");
    this.register(new BidSimulatorServlet(contextSupplier), "bidSimulator");
```

#### createAndRegisterBidRequestHandlers()
1. loop ExchangeType.values
2. get exchange adaptor

### Need to Change
+ ExchangeType enum
To add TENCENT type

+ ExchangeAdaptorFactory.java

+ TencentRtbAdaptor.java
Add new adaptor

### Possible need to change
+ GenderInfo.java //Bidu

### Bid Pricing
```
bid.price.fixed=0.00001

```

### Validating
```
./rtbclient -s http://yinhao.yosemitecloud.cn/TENCENT_RTB_DEBUG -r req.json -f pb
./rtbclient -s http://127.0.0.1:8080/TENCENT_RTB_DEBUG -r req.json -f pb


ip-10-129-0-13.shadc.yosemitecloud.com
refs/changes/80/70580/21

http://yinhao.yosemitecloud.cn/WinNotification?db=2015
http://127.0.0.1:8080/WinNotification?db=2015

git fetch ssh://robinwang@git.yosemitecloud.com:29418/rts/main refs/changes/26/70526/20 && git checkout FETCH_HEAD

git fetch ssh://robinwang@git.yosemitecloud.com:29418/rts/main refs/changes/49/70649/5 && git checkout FETCH_HEAD


ip-10-129-0-14.shadc.yosemitecloud.com
refs/changes/55/70655/1
```
### URLs
```
http://jinjunmei.yosemitecloud.cn/creatives?cf=test_300_250.jpg#yc00032

http://127.0.0.1:8080/as?ai=32&pl=32&ti=17&pp=afkdjasf&mo=1948&ri=32
http://jinjunmei.yosemitecloud.cn/as?ri=32&pp=afkdjasf&ai=32&pl=32&ti=17&mo=1948

http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}&${EXT2}
http://jinjunmei.yosemitecloud.cn/ac?${EXT}
```
### Upload
```
order_info=[{"dsp_order_id": "yc0001","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/www.yosemitecloud.cn\/ads/phiads1.png"}],"targeting_url": "http://www.yosemitecloud.com" ,"monitor_url": []}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7


order_info=[{"dsp_order_id": "3","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/jinjunmei.yosemitecloud.cn\/creatives?cf=yci300_250.jpg"}],"targeting_url": "http://jinjunmei.yosemitecloud.cn/ac?${EXT}" ,"monitor_url": ["http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}%26${EXT2}"]}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7

order_info=[{"dsp_order_id": "32","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/jinjunmei.yosemitecloud.cn\/creatives?cf=test_300_250.jpg"}],"targeting_url": "http://jinjunmei.yosemitecloud.cn/ac?${EXT}" ,"monitor_url": ["http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}%26${EXT2}"]}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7

order_info=[{"dsp_order_id": "44","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/jinjunmei.yosemitecloud.cn\/creatives?cf=test_300_600.jpg"}],"targeting_url": "http://jinjunmei.yosemitecloud.cn/ac?${EXT}" ,"monitor_url": ["http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}%26${EXT2}"]}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7

order_info=[{"dsp_order_id": "47","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/jinjunmei.yosemitecloud.cn\/creatives?cf=test_1000_90.jpg"}],"targeting_url": "http://jinjunmei.yosemitecloud.cn/ac?${EXT}" ,"monitor_url": ["http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}%26${EXT2}"]}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7

order_info=[{"dsp_order_id": "yc00032","client_name": "上海优闪" ,"file_info":[{"file_url": "http:\/\/jinjunmei.yosemitecloud.cn\/as?pp=${AUCTION_ENCRYPT_PRICE}&${EXT2}"}],"targeting_url": "http://jinjunmei.yosemitecloud.cn/ac?${EXT}" ,"monitor_url": ["http://jinjunmei.yosemitecloud.cn/as?pp=${AUCTION_ENCRYPT_PRICE}%26${EXT2}"]}]&dsp_id=213&token=38ef2b8ef25865d4fe6cc5a8d07ca3f7
```
### Insert Data
#### Insert placement
```sql
START TRANSACTION;
insert into  
versioned_placement_idgen
values();

INSERT INTO `versioned_placement` ( `id`, `is_latest`, `is_enabled`, `is_deleted`, `revision_user_id`, `revision_time`, `revision_comment`, `name`, `org_id`, `external_id`, `max_daily_budget`, `max_daily_impressions`, `width`, `height`, `placement_type`, `render_format`, `click_redirect`)
VALUES
( last_insert_id(), 1, 1, 0, 1, now(), 'create', 'BES-960x90', 3, NULL, NULL, NULL, 960, 90, 0, 0, 1);

COMMIT;
```
#### Insert to advertisement
```sql
START TRANSACTION;
insert into  
versioned_advertisement_idgen
values();

INSERT INTO `yc_core`.`versioned_advertisement`
(`id`,`is_latest`,`is_enabled`,`is_deleted`,`revision_user_id`,
`revision_time`,`revision_comment`,`advertiser_id`,`ad_type`,`name`,`width`,
`height`,`has_iframe`,`max_daily_budget`,`content_url`,`landing_url`,`visible_url`,`click_through_url`,`storage`)
VALUES
( last_insert_id(), 1, 1, 0, 1, now(), 'create', 1, 1, 'yosemite_rect', 960, 90, 1, NULL, 'BES_960_90.jpg', 'www.yosemitecloud.cn', 'www.yosemitecloud.cn', '', 1);

COMMIT;
```

#### Insert to assigned ad
```sql
START TRANSACTION;
insert into  
versionedassoc_assigned_ad_idgen
values();

INSERT INTO `yc_core`.`versionedassoc_assigned_ad`
(`id`,`is_latest`,`is_enabled`,`is_deleted`,`revision_user_id`,`revision_time`,`revision_comment`,`ad_id`,`item_id`,`item_type`,`max_daily_budget`)
VALUES
(last_insert_id(), 1, 1, 0, 1, now(), 'create', 51, 8, 2, '100000');
COMMIT;
```

### Response Record
```
2016-03-24 20:20:10,311 [Jetty Thread Pool-57] INFO  com.yosemitecloud.rts.main.server.eventhandler.BidRequestHandler - Bid request received:
com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@ea3ca63[
  exchange=TENCENT,b=2147483647,id=requestid100,exchangeProvidedGeo=<null>,user=com.yosemitecloud.rts.main.rtb.abstractinfo.BidUserInfo@233c3117[exchangeUserId=tuid100,exchangeProvidedOwnIdStr=<null>,exchangeProvidedOwnId=-2,a=<null>,detectedLanguage=UNKNOWN,acceptedLanguage=UNKNOWN,ipAddress=192.168.0.1,geoInfoFromIp=com.yosemitecloud.common.geoip.info.GeoIPRecord@44baef13,userAgent=mozilla, firefox,exchangeProvidedGender=UNKNOWN,b={},a=<null>,b=false],device=<null>,appOrSite=com.yosemitecloud.rts.main.rtb.abstractinfo.SiteInfo@4ca5eba5[url=http://www.qq.com,referrerUrl=,exchangeSiteCategory=<null>,a=<null>,b=0,c=0,d=<null>,e=0,f=<null>],isApp=false,isSite=true,isSecure=false,c=<null>,d=[com.yosemitecloud.rts.main.rtb.abstractinfo.BidSlotInfo@2111c4f3[bidRequest=com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@ea3ca63,impId=impid101,minimumBidCpi=100.0,placement=com.yosemitecloud.rts.main.server.data.PlacementUnit@77c59f90,impression=com.yosemitecloud.rts.main.rtb.abstractinfo.BannerInfo@15993e7b[isExpandable=false,isTopFrame=false,width=170,height=282,fold=<null>],a={},isSecure=false,b=<null>,c=TENCENTimpid101,d=<null>]],e=false,f=-1,g=<null>,h=-1,i=false,j=<null>]
  ___Read from userstore___
  applicable ads for placement:4___Best ad {AdUnit(ad: 1, tactic: 11, level: CAMPAIGN)} bestScore {1.876710136987935E-4} bestPrice {1.0E-5}___AdSelector returns no bid___Bid response sent with processing time 6___

2016-03-24 21:10:20,522 [Jetty Thread Pool-55] INFO  com.yosemitecloud.rts.main.server.eventhandler.BidRequestHandler - Bid request received: com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@5dc01aaa[exchange=TENCENT,bidderTimeoutMs=2147483647,id=requestid100,exchangeProvidedGeo=<null>,user=com.yosemitecloud.rts.main.rtb.abstractinfo.BidUserInfo@3274dbed[exchangeUserId=tuid100,exchangeProvidedOwnIdStr=<null>,exchangeProvidedOwnId=-2,ownId=<null>,detectedLanguage=UNKNOWN,acceptedLanguage=UNKNOWN,ipAddress=192.168.0.1,geoInfoFromIp=com.yosemitecloud.common.geoip.info.GeoIPRecord@4fccc742,userAgent=mozilla, firefox,exchangeProvidedGender=UNKNOWN,exchangeUserCategories={},ownUserProfile=<null>,setProfile=false],device=<null>,appOrSite=com.yosemitecloud.rts.main.rtb.abstractinfo.SiteInfo@548397ab[url=http://www.qq.com,referrerUrl=,exchangeSiteCategory=<null>,excludedProducts=<null>,exchangeSiteQuality=0,exchangePageType=0,exchangePageKeywords=<null>,exchangePageQuality=0,exchangePageCategories=<null>],isApp=false,isSite=true,isSecure=false,rejectionReasons=<null>,bidSlots=[com.yosemitecloud.rts.main.rtb.abstractinfo.BidSlotInfo@295d10e4[bidRequest=com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@5dc01aaa,impId=impid101,minimumBidCpi=0.0,placement=com.yosemitecloud.rts.main.server.data.PlacementUnit@6f7dece1,impression=com.yosemitecloud.rts.main.rtb.abstractinfo.BannerInfo@344ae0f6[isExpandable=false,isTopFrame=false,width=170,height=282,fold=<null>],slotDetails={},isSecure=false,bidInfo=<null>,slotId=TENCENTimpid101,excludedUrls=<null>]],isMobile=false,processingTime=-1,protocolVersion=<null>,sellingMemberId=-1,isNonIframeOnly=false,requestTime=<null>]___Read from userstore___applicable ads for placement:4___Best ad {AdUnit(ad: 1, tactic: 14, level: CAMPAIGN)} bestScore {1.876710136987935E-4} bestPrice {1.0E-5}___Bid response sent with processing time 80492___


2016-03-24 21:43:10,227 [Jetty Thread Pool-67] INFO  com.yosemitecloud.rts.main.server.eventhandler.BidRequestHandler - Bid request received: com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@3017f1f9[exchange=TENCENT,b=2147483647,id=requestid100,exchangeProvidedGeo=<null>,user=com.yosemitecloud.rts.main.rtb.abstractinfo.BidUserInfo@1440ba4f[exchangeUserId=tuid100,exchangeProvidedOwnIdStr=<null>,exchangeProvidedOwnId=-2,a=<null>,detectedLanguage=UNKNOWN,acceptedLanguage=UNKNOWN,ipAddress=192.168.0.1,geoInfoFromIp=com.yosemitecloud.common.geoip.info.GeoIPRecord@47f3ed6,userAgent=mozilla, firefox,exchangeProvidedGender=UNKNOWN,b={},a=<null>,b=false],device=<null>,appOrSite=com.yosemitecloud.rts.main.rtb.abstractinfo.SiteInfo@3735d265[url=http://www.qq.com,referrerUrl=,exchangeSiteCategory=<null>,a=<null>,b=0,c=0,d=<null>,e=0,f=<null>],isApp=false,isSite=true,isSecure=false,c=<null>,d=[com.yosemitecloud.rts.main.rtb.abstractinfo.BidSlotInfo@516cbb42[bidRequest=com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@3017f1f9,impId=impid101,minimumBidCpi=0.0,placement=com.yosemitecloud.rts.main.server.data.PlacementUnit@61c8a4cb,impression=com.yosemitecloud.rts.main.rtb.abstractinfo.BannerInfo@3bbb3d94[isExpandable=false,isTopFrame=false,width=170,height=282,fold=<null>],a={},isSecure=false,b=<null>,c=TENCENTimpid101,d=<null>]],e=false,f=-1,g=<null>,h=-1,i=false,j=<null>]___Read from userstore___applicable ads for placement:4___Best ad {AdUnit(ad: 1, tactic: 11, level: CAMPAIGN)} bestScore {1.876710136987935E-4} bestPrice {1.0E-5}___Bid response sent with processing time 8734___

2016-03-25 16:09:32,027 [Jetty Thread Pool-66] INFO  com.yosemitecloud.rts.main.server.eventhandler.BidRequestHandler - Bid request received: com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@65424aa2[exchange=TENCENT,b=2147483647,id=6A02CD62002656F4F162159324,exchangeProvidedGeo=<null>,user=com.yosemitecloud.rts.main.rtb.abstractinfo.BidUserInfo@6332059d[exchangeUserId=AQEBoYA8le9mJM-yiXAZobaUl_8F2m0pzBKZ,exchangeProvidedOwnIdStr=<null>,exchangeProvidedOwnId=-2,a=<null>,detectedLanguage=UNKNOWN,acceptedLanguage=UNKNOWN,ipAddress=106.2.205.98,geoInfoFromIp=com.yosemitecloud.common.geoip.info.GeoIPRecord@690eb94b,userAgent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.80 Safari/537.36 QQBrowser/9.3.6872.400,exchangeProvidedGender=UNKNOWN,b={},exchangeType=TENCENT,a=<null>,b=false],device=<null>,appOrSite=com.yosemitecloud.rts.main.rtb.abstractinfo.SiteInfo@45a720cf[url=http://news.qq.com/a/20160325/000874.htm,referrerUrl=,exchangeSiteCategory=<null>,a=<null>,b=0,c=0,d=<null>,e=0,f=<null>],isApp=false,isSite=true,isSecure=false,c=<null>,d=[com.yosemitecloud.rts.main.rtb.abstractinfo.BidSlotInfo@5f5b19c2[bidRequest=com.yosemitecloud.rts.main.rtb.abstractinfo.BidRequestInfo@65424aa2,impId=6A02CD62002656F4F16215932400,minimumBidCpi=0.005,placement=<null>,impression=com.yosemitecloud.rts.main.rtb.abstractinfo.VideoInfo@55f84076[minDuration=0,maxDuration=15000,startDelay=0,width=640,height=480,fold=<null>],a={},isSecure=false,b=<null>,c=TENCENT6A02CD62002656F4F16215932400,d=<null>]],e=false,f=-1,g=<null>,h=-1,i=false,j=<null>]___Read from userstore, user:-1___No applicable placement___Bid response sent with processing time 4___


5
bid {
  request_id: "033D5A71517156F23E630CB5F6"
  ip: "113.90.61.3"
  user_id: -1
  exchange_id: 5
  exchange_user_id: "AQEBuOkalLWtZfp_sKiYb3RJsSELSnbmQlDN"
  server_timestamp: 1458716259696
  user_time_zone: 0
  user_agent: "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.59 QQ/7.3.14258.201 Safari/537.36"
  language: 0
  user_categories: ""
  location: 156440300
  geo_info: ""
  url: "http://mini2015.qq.com/mini2015_gray.htm?geo_location=%E5%B9%BF%E4%B8%9C%E7%9C%81,%E6%B7%B1%E5%9C%B3%E5%B8%82&time=false&Width=750&Height=540"
  referrer_url: ""
  site_category: ""
  site_quality: 0
  page_type: 0
  page_keywords: ""
  page_quality: 0
  page_category: ""
  excluded_ad_categories: ""
  placement_vid: 38
  placement_id: 29
}

5
bid {
  request_id: "B651F73A517156F243A60ED630"
  ip: "58.247.81.182"
  user_id: -1
  exchange_id: 5
  exchange_user_id: "AQEBWggxAfpWocuW3dxnRC0893kGwcHbw1pZ"
  server_timestamp: 1458717607069
  user_time_zone: 0
  user_agent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36"
  language: 0
  user_categories: ""
  location: 156310100
  geo_info: ""
  url: "http://news.qq.com/a/20160323/030582.htm"
  referrer_url: ""
  site_category: ""
  site_quality: 0
  page_type: 0
  page_keywords: ""
  page_quality: 0
  page_category: ""
  excluded_ad_categories: ""
  placement_vid: 4
  placement_id: 4
}
```

### Notes
* AdSelector new ModelBidcache looks slow
