## ADX Integration
### Business
+ Contract
+ Money
+ Contact
  + Business Contact
  + Technical Contact

### Code Change
#### New Class
+ ADXAudit
+ ADXRtbAdaptor
+ ADXPriceDecoder
+ ADXRequestSimulator
#### Change Class
+ Bidding
  + ExchangeType
  + UserRequestParam
  + ExchangeAdaptorFactory
  + AbstractUserRequestInfo
+ Cookie Matching
  + ExchangeType, add the cm url
  + UserRequestParam: Make sure the ADX uid is correct

#### Database Change
+ versioned_org
+ add new placements

### Progress
1. Get the sandbox account and token
2. Finish the bidding code change
3. Test bidding offline
4. Sandbox Testing
5. Cookie matching Test
6. Get PROD account and token
