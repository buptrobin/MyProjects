## Tencent Integration
### Preapring
+ Sandbox Account for the ADX
+ Formal Account for the ADX

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
