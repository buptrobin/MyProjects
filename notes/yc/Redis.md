
### Problem Resolve
+ sj-t1 not running, should use shadc-t1
+ The server port for sentinel is 26379

### How Redis instance get process
+ com.yosemitecloud.rts.main.server.MainServer.setup():63:        **BudgetFacilities**.setup(config, HibernateFactory.core(), executor);
+ com.yosemitecloud.rts.main.budget.BudgetFacilities.setup():53:            modules.add(new **BudgetManagerModule**());
+ com.yosemitecloud.rts.budgetmanager.inject.BudgetManagerModule.java:39:        bind(RedisAccess.class).to(RedisAccessImpl.class);
+ com.yosemitecloud.rts.budgetmanager.dao.redis.RedisAccessImpl()
```JAVA
pool = new JedisSentinelPool(master, ImmutableSet.copyOf(servers.split(",")));
```
+ JedisSentinelPool()
```JAVA
  public JedisSentinelPool(...) {
  	...
    HostAndPort master = initSentinels(sentinels, masterName);
    initPool(master);
  }

  ....
   private HostAndPort initSentinels(Set<String> sentinels, final String masterName) {

    HostAndPort master = null;
    boolean sentinelAvailable = false;

    for (String sentinel : sentinels) {
      final HostAndPort hap = toHostAndPort(Arrays.asList(sentinel.split(":")));

      Jedis jedis = null;
      try {
        jedis = new Jedis(hap.getHost(), hap.getPort());
        List<String> masterAddr = jedis.sentinelGetMasterAddrByName(masterName);
		...
        master = toHostAndPort(masterAddr);
      } catch (JedisException e) {
        log.warning("Cannot get master address from sentinel running. Trying next one.");
      } finally {
        if (jedis != null) {
          jedis.close();
        }
      }
    }

    if (master == null) {
      if (sentinelAvailable) {
        // can connect to sentinel, but master name seems to not monitored
        throw new JedisException("Can connect to sentinel, but " + masterName
            + " seems to be not monitored...");
      } else {
        throw new JedisConnectionException("All sentinels down, cannot determine where is master is running...");
      }
    }
    ...
    return master;
  }
```
+ com.yosemitecloud.rts.budgetmanager.inject.BudgetManagerModule.java:39:        bind(BudgetDao.class).to(**BudgetDaoRedis**.class);
+ BudgetDaoRedis.java:44:        debitor.**setRedis**(redis);

```JAVA
	Jedis getResource() throws IOException {
		return redis.getResource();
	}
```
+ RedisAccessImpl()
```JAVA
    public Jedis getResource() {
        return pool.getResource();
    }
```

### How RTS use redis

+ com.yosemitecloud.rts.main.server.jetty.JettyServerWrapper.java:308:                    "bidSimulator");
```JAVA
	this.register(new BidSimulatorServlet(contextSupplier),	"bidSimulator");
```

+ com.yosemitecloud.rts.main.server.eventhandler.BidSimulatorServlet

+ com.yosemitecloud.rts.budgetmanager.BudgetManagerImpl will reset the claims